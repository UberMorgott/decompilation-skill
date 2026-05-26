<#
.SYNOPSIS
    ConfuserEx deobfuscation pipeline for .NET assemblies.
.DESCRIPTION
    Runs the canonical ConfuserEx deobfuscation chain:
    unpack -> string decrypt -> control flow -> proxy removal -> rename -> extract embedded -> decompile -> build indexes.
    Each step is logged with timing and exit codes. Failures are non-fatal where possible.
.PARAMETER Target
    Path to the ConfuserEx-protected DLL.
.PARAMETER OutputDir
    Root output directory for the pipeline results.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$Target,

    [Parameter(Mandatory)]
    [string]$OutputDir
)

$ErrorActionPreference = 'Continue'
$script:PipelineLog = Join-Path $OutputDir 'pipeline.log'
$script:StepNumber = 0
$script:StepResults = @()
$script:LogTag = 'confuserex'

. (Join-Path $PSScriptRoot '_common.ps1')

if (-not (Test-Path $Target)) {
    Log "ERROR: Target not found: $Target"
    exit 1
}

# ── Create output folder structure ───────────────────────────────────────────

$dirs = @('original', 'intermediate', 'src', 'extracted', 'strings', 'metadata')
foreach ($d in $dirs) {
    $p = Join-Path $OutputDir $d
    if (-not (Test-Path $p)) { New-Item -ItemType Directory -Path $p -Force | Out-Null }
}

Log "Pipeline started for: $Target"
Log "Output directory: $OutputDir"

# ── Copy original ────────────────────────────────────────────────────────────

$originalCopy = Join-Path $OutputDir 'original' (Split-Path $Target -Leaf)
Copy-Item -Path $Target -Destination $originalCopy -Force
Log "Original preserved at $originalCopy"

$currentDll = $Target
$skipToRename = $false

try {

# ── Fast-path: try NoFuserEx all-in-one first ───────────────────────────────

if (Test-ToolAvailable 'NoFuserEx') {
    Log "Trying NoFuserEx as fast-path all-in-one deobfuscator..."
    $nofuserOutput = Join-Path $OutputDir 'intermediate' '00_nofuserex.dll'
    try {
        & NoFuserEx $currentDll $nofuserOutput 2>&1 | ForEach-Object { Log "  $_" }
        if ($LASTEXITCODE -eq 0 -and (Test-Path $nofuserOutput)) {
            # Validate: check the output is a valid .NET assembly (has BSJB metadata)
            $nfBytes = [System.IO.File]::ReadAllBytes($nofuserOutput)
            $nfValid = $false
            if ($nfBytes.Length -gt 512) {
                $searchLimit = [Math]::Min($nfBytes.Length - 4, 4096 * 64)
                for ($i = 0; $i -lt $searchLimit; $i++) {
                    if ($nfBytes[$i] -eq 0x42 -and $nfBytes[$i+1] -eq 0x53 -and $nfBytes[$i+2] -eq 0x4A -and $nfBytes[$i+3] -eq 0x42) {
                        $nfValid = $true
                        break
                    }
                }
            }
            if ($nfValid) {
                Log "NoFuserEx succeeded — skipping staged pipeline, jumping to rename step"
                $currentDll = $nofuserOutput
                $skipToRename = $true
            } else {
                Log "NoFuserEx output is not a valid .NET assembly, falling back to staged pipeline"
            }
        } else {
            Log "NoFuserEx failed (exit $LASTEXITCODE or no output), falling back to staged pipeline"
        }
    } catch {
        Log "NoFuserEx threw exception: $_ — falling back to staged pipeline"
    }
} else {
    Log "NoFuserEx not available, proceeding with staged pipeline"
}

if (-not $skipToRename) {
# ── Step 1: Unpack ───────────────────────────────────────────────────────────

$unpackedDll = Join-Path $OutputDir 'intermediate' '01_unpacked.dll'

Invoke-PipelineStep -Name 'Unpack (ConfuserEx-Unpacker-2)' -Action {
    if (Test-ToolAvailable 'ConfuserEx-Unpacker-2') {
        & 'ConfuserEx-Unpacker-2' $currentDll $unpackedDll
        if ($LASTEXITCODE -ne 0) { throw "ConfuserEx-Unpacker-2 exited with code $LASTEXITCODE" }
    } elseif (Test-ToolAvailable 'NoFuserEx') {
        Log "Trying NoFuserEx as all-in-one fallback..."
        & NoFuserEx $currentDll $unpackedDll
        if ($LASTEXITCODE -ne 0) { throw "NoFuserEx exited with code $LASTEXITCODE" }
    } else {
        Log "INSTALL_REQUIRED:ConfuserEx-Unpacker-2"
        Write-Output "INSTALL_REQUIRED:ConfuserEx-Unpacker-2"
        Log "No unpacker available, copying original as-is"
        Copy-Item -Path $currentDll -Destination $unpackedDll -Force
    }
}

if (Test-Path $unpackedDll) { $currentDll = $unpackedDll }

# ── Step 2: String decryption ────────────────────────────────────────────────

$stringsDecDll = Join-Path $OutputDir 'intermediate' '02_strings_decrypted.dll'

Invoke-PipelineStep -Name 'String decryption (static then dynamic fallback)' -Action {
    $done = $false

    # Try static decryptor first
    if (Test-ToolAvailable 'ConfuserEx-Static-String-Decryptor') {
        Log "Trying static string decryptor..."
        & 'ConfuserEx-Static-String-Decryptor' $currentDll $stringsDecDll 2>&1 | ForEach-Object { Log "  $_" }
        if ($LASTEXITCODE -eq 0 -and (Test-Path $stringsDecDll)) {
            $done = $true
            Log "Static string decryption succeeded"
        } else {
            Log "Static string decryption failed, trying dynamic..."
        }
    }

    # Dynamic fallback
    if (-not $done -and (Test-ToolAvailable 'ConfuserEx2_String_Decryptor')) {
        Log "WARNING: Dynamic string decryptor executes code from target binary."
        Log "WARNING: Ensure you are running in a sandboxed environment."
        & 'ConfuserEx2_String_Decryptor' $currentDll $stringsDecDll 2>&1 | ForEach-Object { Log "  $_" }
        if ($LASTEXITCODE -eq 0 -and (Test-Path $stringsDecDll)) {
            $done = $true
            Log "Dynamic string decryption succeeded"
        }
    }

    # Built-in runtime reflection decryptor (requires dotnet + AsmResolver)
    if (-not $done) {
        Log "WARNING: Built-in runtime decryptor EXECUTES CODE from target binary."
        Log "WARNING: Run only in sandboxed environment (VM recommended)."
        Write-Output "CONSENT_REQUIRED:dynamic-string-decrypt"

        # Check if dotnet SDK available
        if (Get-Command dotnet -ErrorAction SilentlyContinue) {
            $decryptorDir = Join-Path $OutputDir 'intermediate' 'runtime-decryptor'
            if (-not (Test-Path $decryptorDir)) { New-Item -ItemType Directory -Path $decryptorDir -Force | Out-Null }

            # Write C# project
            $csprojContent = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="AsmResolver.DotNet" Version="6.*" />
  </ItemGroup>
</Project>
'@
            $csprojContent | Out-File -FilePath (Join-Path $decryptorDir 'RuntimeDecrypt.csproj') -Encoding utf8

            # Write C# source — universal ConfuserEx string decryptor via Reflection + AsmResolver
            $csContent = @'
// dynamic-string-decrypt.cs — Universal ConfuserEx string decryptor via Reflection
// WARNING: This EXECUTES code from the target binary. Run in a VM/sandbox.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

var targetPath = args[0];
var outputPath = args.Length > 1 ? args[1] : Path.ChangeExtension(targetPath, ".decrypted.dll");

// 1. Load via Reflection (executes module ctor — sandbox!)
var asm = Assembly.LoadFrom(Path.GetFullPath(targetPath));

// 2. Load via AsmResolver for IL rewriting
var module = ModuleDefinition.FromFile(targetPath);

// 3. Locate decryption method: single int param, returns string, many call sites
var candidates = module.GetAllTypes()
    .SelectMany(t => t.Methods)
    .Where(m => m.Signature?.ReturnType.FullName == "System.String"
             && m.Signature.GetParameterCount() == 1
             && m.CilMethodBody != null)
    .OrderByDescending(m => CountCallSites(module, m))
    .ToList();

var decryptorDef = candidates.FirstOrDefault()
    ?? throw new Exception("Cannot locate decryption method heuristically");

Console.WriteLine($"[*] Decryptor: {decryptorDef.FullName}");

// 4. Resolve via Reflection
var decryptorInfo = asm.GetType(decryptorDef.DeclaringType!.FullName)!
    .GetMethod(decryptorDef.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)!;

// 5. Walk call sites, invoke, collect table
var table = new Dictionary<int, string>();
foreach (var type in module.GetAllTypes())
foreach (var method in type.Methods.Where(m => m.CilMethodBody != null))
{
    var instrs = method.CilMethodBody!.Instructions;
    for (int i = 1; i < instrs.Count; i++)
    {
        if (instrs[i].OpCode == CilOpCodes.Call
            && instrs[i].Operand is IMethodDescriptor md
            && md.FullName == decryptorDef.FullName
            && instrs[i - 1].IsLdcI4())
        {
            int token = instrs[i - 1].GetLdcI4Constant();
            if (!table.ContainsKey(token))
            {
                try { table[token] = (string)decryptorInfo.Invoke(null, new object[] { token })!; }
                catch { /* skip failed tokens */ }
            }
            // 6. Rewrite IL: replace ldc.i4 + call with ldstr
            instrs[i - 1].OpCode = CilOpCodes.Nop;
            instrs[i].OpCode = CilOpCodes.Ldstr;
            instrs[i].Operand = table[token];
        }
    }
}

// 7. Save
module.Write(outputPath);
Console.WriteLine($"[+] Decrypted {table.Count} strings → {outputPath}");

// Dump TSV for strings/decrypted.tsv
File.WriteAllLines(
    Path.ChangeExtension(outputPath, ".tsv"),
    table.Select(kv => $"{kv.Key}\t{kv.Value}"));

static int CountCallSites(ModuleDefinition mod, MethodDefinition target)
{
    return mod.GetAllTypes()
        .SelectMany(t => t.Methods)
        .Where(m => m.CilMethodBody != null)
        .SelectMany(m => m.CilMethodBody!.Instructions)
        .Count(i => i.OpCode == CilOpCodes.Call
                  && i.Operand is IMethodDescriptor md
                  && md.FullName == target.FullName);
}
'@
            $csContent | Out-File -FilePath (Join-Path $decryptorDir 'Program.cs') -Encoding utf8

            Log "Building runtime decryptor..."
            $buildOutput = & dotnet build $decryptorDir --configuration Release 2>&1
            $buildOutput | ForEach-Object { Log "  $_" }

            if ($LASTEXITCODE -eq 0) {
                Log "Running runtime decryptor on $currentDll..."
                $runOutput = & dotnet run --project $decryptorDir --configuration Release -- $currentDll $stringsDecDll 2>&1
                $runOutput | ForEach-Object { Log "  $_" }

                if ($LASTEXITCODE -eq 0 -and (Test-Path $stringsDecDll)) {
                    $done = $true
                    Log "Runtime reflection decryptor succeeded"

                    # Copy TSV if generated
                    $runtimeTsv = [System.IO.Path]::ChangeExtension($stringsDecDll, '.tsv')
                    if (Test-Path $runtimeTsv) {
                        $decryptedTsv = Join-Path $OutputDir 'strings' 'decrypted.tsv'
                        Copy-Item -Path $runtimeTsv -Destination $decryptedTsv -Force
                        Log "Decrypted strings table copied to $decryptedTsv"
                    }
                } else {
                    Log "Runtime decryptor failed (exit $LASTEXITCODE)"
                }
            } else {
                Log "Runtime decryptor build failed"
            }
        } else {
            Log "dotnet SDK not available, skipping runtime decryptor"
        }
    }

    if (-not $done) {
        Log "INSTALL_REQUIRED:ConfuserEx-Static-String-Decryptor or ConfuserEx2_String_Decryptor"
        Write-Output "INSTALL_REQUIRED:ConfuserEx-String-Decryptor"
        Log "No string decryptor succeeded (static, dynamic, runtime), copying previous stage"
        Copy-Item -Path $currentDll -Destination $stringsDecDll -Force
    }

    # Extract decrypted strings table if available
    $decryptedTsv = Join-Path $OutputDir 'strings' 'decrypted.tsv'
    if (-not (Test-Path $decryptedTsv)) {
        "token`tdecoded_string" | Out-File -FilePath $decryptedTsv -Encoding utf8
        Log "Created empty decrypted.tsv placeholder"
    }
}

if (Test-Path $stringsDecDll) { $currentDll = $stringsDecDll }

# ── Step 3: Control flow deobfuscation ───────────────────────────────────────

$cflowDll = Join-Path $OutputDir 'intermediate' '03_cflow_clean.dll'

Invoke-PipelineStep -Name 'Control flow (de4dot-cex -p crx)' -Action {
    $de4dotCmd = $null
    foreach ($name in @('de4dot-cex', 'de4dot-x64', 'de4dot')) {
        if (Test-ToolAvailable $name) { $de4dotCmd = $name; break }
    }

    if ($de4dotCmd) {
        Log "Using $de4dotCmd for control flow cleanup"
        & $de4dotCmd $currentDll -p crx -o $cflowDll 2>&1 | ForEach-Object { Log "  $_" }
        if ($LASTEXITCODE -ne 0) { throw "$de4dotCmd exited with code $LASTEXITCODE" }
    } else {
        Log "INSTALL_REQUIRED:de4dot-cex"
        Write-Output "INSTALL_REQUIRED:de4dot-cex"
        Copy-Item -Path $currentDll -Destination $cflowDll -Force
    }
}

if (Test-Path $cflowDll) { $currentDll = $cflowDll }

# ── Step 4: Proxy call removal ───────────────────────────────────────────────

$proxyDll = Join-Path $OutputDir 'intermediate' '04_proxies_removed.dll'

Invoke-PipelineStep -Name 'Proxy call removal (ProxyCall-Remover)' -Action {
    if (Test-ToolAvailable 'ProxyCall-Remover') {
        & 'ProxyCall-Remover' $currentDll $proxyDll 2>&1 | ForEach-Object { Log "  $_" }
        if ($LASTEXITCODE -ne 0) { throw "ProxyCall-Remover exited with code $LASTEXITCODE" }
    } else {
        Log "INSTALL_REQUIRED:ProxyCall-Remover"
        Write-Output "INSTALL_REQUIRED:ProxyCall-Remover"
        Copy-Item -Path $currentDll -Destination $proxyDll -Force
    }
}

if (Test-Path $proxyDll) { $currentDll = $proxyDll }

} # end if (-not $skipToRename)

# ── Step 5: Rename symbols ──────────────────────────────────────────────────
# NOTE: de4dot-cex with -p crx already renames symbols during control flow cleanup (step 3).
# A separate rename pass is only needed if step 3 was skipped or used a different tool.
# We copy the previous output to maintain the intermediate file numbering convention.

$renamedDll = Join-Path $OutputDir 'intermediate' '05_renamed.dll'

Invoke-PipelineStep -Name 'Symbol rename (already done by de4dot-cex -p crx)' -Action {
    Copy-Item -Path $currentDll -Destination $renamedDll -Force
    Log "Symbols already renamed in step 3 (de4dot-cex -p crx includes rename). Copied forward."
}

if (Test-Path $renamedDll) { $currentDll = $renamedDll }

# ── Step 6: Extract embedded assemblies ──────────────────────────────────────

Invoke-PipelineStep -Name 'Extract embedded assemblies' -Action {
    $extractScript = Join-Path $PSScriptRoot 'extract-embedded.ps1'
    $extractedDir = Join-Path $OutputDir 'extracted'

    if (Test-Path $extractScript) {
        & $extractScript -Target $currentDll -OutputDir $extractedDir 2>&1 | ForEach-Object { Log "  $_" }
    } else {
        Log "WARNING: extract-embedded.ps1 not found at $extractScript"
    }
}

# ── Step 7: Decompile main assembly ─────────────────────────────────────────

Invoke-PipelineStep -Name 'Decompile main (ilspycmd)' -Action {
    $srcDir = Join-Path $OutputDir 'src'

    if (Test-ToolAvailable 'ilspycmd') {
        & ilspycmd $currentDll -p -o $srcDir 2>&1 | ForEach-Object { Log "  $_" }
        if ($LASTEXITCODE -ne 0) { throw "ilspycmd exited with code $LASTEXITCODE" }
    } else {
        Log "INSTALL_REQUIRED:ilspycmd (dotnet tool install -g ilspycmd)"
        Write-Output "INSTALL_REQUIRED:ilspycmd"
        throw "ilspycmd not available"
    }
}

# ── Step 8: Decompile each extracted assembly ────────────────────────────────

Invoke-PipelineStep -Name 'Decompile extracted assemblies' -Action {
    $extractedDir = Join-Path $OutputDir 'extracted'
    $extractedDlls = Get-ChildItem -Path $extractedDir -Filter '*.dll' -ErrorAction SilentlyContinue

    if (-not $extractedDlls -or $extractedDlls.Count -eq 0) {
        Log "No extracted assemblies found, skipping"
        return
    }

    if (-not (Test-ToolAvailable 'ilspycmd')) {
        Log "INSTALL_REQUIRED:ilspycmd"
        return
    }

    foreach ($dll in $extractedDlls) {
        $name = [System.IO.Path]::GetFileNameWithoutExtension($dll.Name)
        $outDir = Join-Path $extractedDir $name 'src'
        if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir -Force | Out-Null }

        Log "Decompiling extracted: $($dll.Name)"
        & ilspycmd $dll.FullName -p -o $outDir 2>&1 | ForEach-Object { Log "  $_" }
        if ($LASTEXITCODE -ne 0) {
            Log "WARNING: ilspycmd failed on $($dll.Name) (exit $LASTEXITCODE)"
        }
    }
}

# ── Step 9: Build indexes ───────────────────────────────────────────────────

Invoke-PipelineStep -Name 'Build indexes' -Action {
    $indexScript = Join-Path $PSScriptRoot 'build-indexes.ps1'
    $srcDir = Join-Path $OutputDir 'src'
    $metadataDir = Join-Path $OutputDir 'metadata'

    if (Test-Path $indexScript) {
        & $indexScript -SourceDir $srcDir -OutputDir $metadataDir 2>&1 | ForEach-Object { Log "  $_" }
    } else {
        Log "WARNING: build-indexes.ps1 not found at $indexScript"
    }
}

} finally {

# ── Write pipeline summary ──────────────────────────────────────────────────

$pipelineJson = [ordered]@{
    target     = $Target
    recipe     = 'dotnet-confuserex'
    started    = [DateTime]::Now.ToString('o')
    steps      = $script:StepResults
    total_steps = $script:StepNumber
    successful = ($script:StepResults | Where-Object { $_.status -eq 'success' }).Count
    failed     = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
}

$pipelineJsonPath = Join-Path $OutputDir 'pipeline.json'
$pipelineJson | ConvertTo-Json -Depth 5 | Out-File -FilePath $pipelineJsonPath -Encoding utf8
Log "Pipeline JSON written to $pipelineJsonPath"

$failedCount = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
Log "Pipeline complete. $($script:StepNumber) steps, $failedCount failed."

if ($failedCount -gt 0) {
    Write-Output "PIPELINE_STATUS:partial"
    exit 2
} else {
    Write-Output "PIPELINE_STATUS:success"
    exit 0
}

} # end finally
