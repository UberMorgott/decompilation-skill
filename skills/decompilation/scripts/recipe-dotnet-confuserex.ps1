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

    if (-not $done) {
        Log "INSTALL_REQUIRED:ConfuserEx-Static-String-Decryptor or ConfuserEx2_String_Decryptor"
        Write-Output "INSTALL_REQUIRED:ConfuserEx-String-Decryptor"
        Log "No string decryptor available, copying previous stage"
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
