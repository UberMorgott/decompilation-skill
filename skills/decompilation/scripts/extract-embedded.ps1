<#
.SYNOPSIS
    Extract embedded assemblies from a .NET binary.
.DESCRIPTION
    Detects and extracts assemblies embedded via Costura.Fody, ILMerge/ILRepack,
    or ConfuserEx resource protection. Supports GZip-compressed Costura resources,
    ConfuserEx-Resources-Decryptor, and dotnet-script/AsmResolver fallback.
.PARAMETER Target
    Path to the .NET assembly to extract from.
.PARAMETER OutputDir
    Directory where extracted DLLs will be saved.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$Target,

    [Parameter(Mandatory)]
    [string]$OutputDir
)

$ErrorActionPreference = 'Continue'
$script:PipelineLog = Join-Path (Split-Path $OutputDir -Parent) 'pipeline.log'
if (-not (Test-Path (Split-Path $script:PipelineLog -Parent))) {
    $script:PipelineLog = Join-Path $OutputDir 'extract.log'
}
$script:LogTag = 'extract-embedded'

. (Join-Path $PSScriptRoot '_common.ps1')

# ── Setup ────────────────────────────────────────────────────────────────────

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

Log "Embedded assembly extraction started for: $Target"

$extractedCount = 0
$embeddingType = 'unknown'

# ── Read binary for resource detection ───────────────────────────────────────

$bytes = [System.IO.File]::ReadAllBytes($Target)
$scanLen = [Math]::Min($bytes.Length, 10MB)
$rawText = [System.Text.Encoding]::UTF8.GetString($bytes, 0, $scanLen)

# ── Detect embedding type ────────────────────────────────────────────────────

$isCostura = $rawText -match 'costura\..*\.dll\.compressed|Costura\.Fody'
$isILMerge = $rawText -match 'ILMerge|ILRepack'
$isConfuserExResources = $rawText -match 'ConfuserEx' -and $rawText -match '\.resources'

# ConfuserEx resource protection often strips its own name from the binary.
# Detect via: AssemblyResolve hook + ApplicationPart attributes listing sub-assemblies.
# This is the pattern used by ASP.NET apps with ConfuserEx-encrypted embedded assemblies.
$hasAssemblyResolve = $rawText -match 'AssemblyResolve'
$applicationParts = @()
$ilspy = Ensure-Tool 'ilspycmd'
if ($ilspy -and $hasAssemblyResolve) {
    $decompOutput = & $ilspy $Target 2>&1 | Out-String
    $appPartMatches = [regex]::Matches($decompOutput, '\[assembly:\s*ApplicationPart\("([^"]+)"\)\]')
    $applicationParts = @($appPartMatches | ForEach-Object { $_.Groups[1].Value } | Where-Object {
        # Filter out known framework parts
        $_ -notmatch '^Microsoft\.|^Swashbuckle\.|^System\.'
    })
    if ($applicationParts.Count -gt 0) {
        $isConfuserExResources = $true
        Log "Found $($applicationParts.Count) ApplicationPart references (AssemblyResolve + encrypted resources):"
        $applicationParts | ForEach-Object { Log "  - $_" }
    }
}

if ($isCostura) {
    $embeddingType = 'Costura'
    Log "Detected embedding type: Costura.Fody"
} elseif ($isConfuserExResources) {
    $embeddingType = 'ConfuserEx'
    Log "Detected embedding type: ConfuserEx resource protection"
} elseif ($isILMerge) {
    $embeddingType = 'ILMerge'
    Log "Detected embedding type: ILMerge/ILRepack"
} else {
    Log "No clear embedding signal detected, will attempt generic extraction"
}

Write-Output "EMBED_TYPE:$embeddingType"

# ── Costura extraction ───────────────────────────────────────────────────────

if ($embeddingType -eq 'Costura') {
    Log "Attempting Costura resource extraction..."

    # Try CosturaUnpacker tool first
    if (Test-ToolAvailable 'CosturaUnpacker') {
        Log "Using CosturaUnpacker..."
        & CosturaUnpacker $Target $OutputDir 2>&1 | ForEach-Object { Log "  $_" }
        $extractedCount = (Get-ChildItem -Path $OutputDir -Filter '*.dll' -ErrorAction SilentlyContinue).Count
    }

    # PowerShell fallback: find costura resources and decompress
    if ($extractedCount -eq 0) {
        Log "CosturaUnpacker not available or failed, trying PowerShell extraction..."

        # Search for costura resource patterns in binary
        # Costura resources are named: costura.<assemblyname>.dll.compressed
        # They are GZip compressed and stored as embedded resources

        $resourcePattern = 'costura\.([a-zA-Z0-9._-]+)\.dll\.compressed'
        $matches = [regex]::Matches($rawText, $resourcePattern)

        if ($matches.Count -gt 0) {
            $resourceNames = $matches | ForEach-Object { $_.Value } | Sort-Object -Unique
            Log "Found $($resourceNames.Count) Costura resource references"

            foreach ($resName in $resourceNames) {
                $dllName = $resName -replace '^costura\.','' -replace '\.compressed$',''
                Log "  Resource: $resName -> $dllName"
            }
        }

        # Try to extract using inline .NET code if dotnet-script is available
        if (Test-ToolAvailable 'dotnet-script') {
            Log "Attempting extraction via dotnet-script..."
            $extractorCs = @"
#r "nuget: AsmResolver.DotNet, 6.0.0"
using AsmResolver.DotNet;
using System.IO;
using System.IO.Compression;

var module = ModuleDefinition.FromFile(Args[0]);
var outDir = Args[1];
int count = 0;

foreach (var res in module.Resources) {
    if (res is not ManifestResource mr) continue;
    if (mr.Implementation is not null) continue;
    if (!mr.IsEmbedded) continue;
    var emb = (IResourceProvider)mr;
    // Not all resources are embedded resources with data
    try {
        var seg = mr.EmbeddedDataSegment;
        if (seg == null) continue;
        var data = seg.ToArray();
        byte[] final = data;
        // Try GZip decompress
        try {
            using var ms = new MemoryStream(data);
            using var gz = new GZipStream(ms, CompressionMode.Decompress);
            using var outMs = new MemoryStream();
            gz.CopyTo(outMs);
            final = outMs.ToArray();
        } catch { /* not gzipped, keep original */ }
        // Check for MZ header (PE/DLL)
        if (final.Length >= 2 && final[0] == (byte)'M' && final[1] == (byte)'Z') {
            var name = mr.Name?.ToString() ?? $"resource_{count}";
            name = name.Replace("costura.", "").Replace(".compressed", "");
            if (!name.EndsWith(".dll")) name += ".dll";
            File.WriteAllBytes(Path.Combine(outDir, name), final);
            Console.WriteLine($"EXTRACTED:{name}");
            count++;
        }
    } catch { }
}
Console.WriteLine($"EXTRACT_COUNT:{count}");
"@
            $csFile = Join-Path $env:TEMP 'costura_extract.csx'
            $extractorCs | Out-File -FilePath $csFile -Encoding utf8
            & dotnet-script $csFile -- $Target $OutputDir 2>&1 | ForEach-Object { Log "  $_" }
            Remove-Item $csFile -ErrorAction SilentlyContinue
        } else {
            Log "INSTALL_OPTIONAL:dotnet-script (dotnet tool install -g dotnet-script)"

            # Last resort: try dotnet run with inline project
            if (Test-ToolAvailable 'dotnet') {
                Log "Attempting extraction via dotnet run with temp project..."
                $tempProjectDir = Join-Path $env:TEMP "costura_extract_$(Get-Random)"
                New-Item -ItemType Directory -Path $tempProjectDir -Force | Out-Null

                $csproj = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="AsmResolver.DotNet" Version="6.0.0" />
  </ItemGroup>
</Project>
"@
                $programCs = @"
using AsmResolver.DotNet;
using System.IO;
using System.IO.Compression;

var module = ModuleDefinition.FromFile(args[0]);
var outDir = args[1];
int count = 0;

foreach (var res in module.Resources)
{
    if (res is not ManifestResource mr || !mr.IsEmbedded) continue;
    try
    {
        var seg = mr.EmbeddedDataSegment;
        if (seg == null) continue;
        var data = seg.ToArray();
        byte[] final_data = data;
        try
        {
            using var ms = new MemoryStream(data);
            using var gz = new GZipStream(ms, CompressionMode.Decompress);
            using var outMs = new MemoryStream();
            gz.CopyTo(outMs);
            final_data = outMs.ToArray();
        }
        catch { }
        if (final_data.Length >= 2 && final_data[0] == (byte)'M' && final_data[1] == (byte)'Z')
        {
            var name = mr.Name?.ToString() ?? "resource_" + count;
            name = name.Replace("costura.", "").Replace(".compressed", "");
            if (!name.EndsWith(".dll")) name += ".dll";
            File.WriteAllBytes(Path.Combine(outDir, name), final_data);
            Console.WriteLine("EXTRACTED:" + name);
            count++;
        }
    }
    catch { }
}
Console.WriteLine("EXTRACT_COUNT:" + count);
"@
                $csproj | Out-File -FilePath (Join-Path $tempProjectDir 'extract.csproj') -Encoding utf8
                $programCs | Out-File -FilePath (Join-Path $tempProjectDir 'Program.cs') -Encoding utf8

                & dotnet run --project $tempProjectDir -- $Target $OutputDir 2>&1 | ForEach-Object { Log "  $_" }

                Remove-Item -Path $tempProjectDir -Recurse -Force -ErrorAction SilentlyContinue
            } else {
                Log "WARNING: No .NET extraction method available"
                Log "INSTALL_REQUIRED:dotnet SDK"
            }
        }
    }

    $extractedCount = (Get-ChildItem -Path $OutputDir -Filter '*.dll' -ErrorAction SilentlyContinue).Count
    Log "Costura extraction complete: $extractedCount DLLs"
}

# ── ConfuserEx resource extraction ───────────────────────────────────────────

if ($embeddingType -eq 'ConfuserEx') {
    Log "Attempting ConfuserEx resource extraction..."

    if (Test-ToolAvailable 'ConfuserEx-Resources-Decryptor') {
        & 'ConfuserEx-Resources-Decryptor' $Target $OutputDir 2>&1 | ForEach-Object { Log "  $_" }
        $extractedCount = (Get-ChildItem -Path $OutputDir -Filter '*.dll' -ErrorAction SilentlyContinue).Count
    } else {
        Log "INSTALL_REQUIRED:ConfuserEx-Resources-Decryptor"
        Write-Output "INSTALL_REQUIRED:ConfuserEx-Resources-Decryptor"
        Log "Manual extraction may be needed — ConfuserEx encrypts resources at runtime"
    }

    # Write manifest of expected embedded assemblies (from ApplicationPart attributes)
    if ($applicationParts.Count -gt 0) {
        $manifest = @{
            type              = "confuserex_resources"
            host_assembly     = [System.IO.Path]::GetFileNameWithoutExtension($Target)
            embedded_parts    = $applicationParts
            extracted_count   = $extractedCount
            note              = "ConfuserEx encrypts these assemblies in resources. They are decrypted at runtime via AssemblyResolve hook. Use ConfuserEx-Resources-Decryptor or dynamic invoke to extract."
        }
        $manifestPath = Join-Path $OutputDir 'embedded_manifest.json'
        $manifest | ConvertTo-Json -Depth 3 | Out-File -FilePath $manifestPath -Encoding utf8
        Log "Wrote embedded assembly manifest: $manifestPath ($($applicationParts.Count) parts)"
        Write-Output "EMBED_MANIFEST:$manifestPath"
    }
}

# ── ILMerge detection (namespace analysis) ───────────────────────────────────

if ($embeddingType -eq 'ILMerge') {
    Log "ILMerge/ILRepack detected — assemblies are merged at IL level, not embedded as resources"
    Log "Extraction requires decompilation first, then splitting by namespace"
    Log "Recommendation: decompile with ilspycmd, identify merged namespaces from the output"
    Write-Output "EMBED_NOTE:ILMerge assemblies are merged at IL level, not extractable as separate DLLs"
}

# ── ILMerge detection via namespace analysis (when still unknown) ────────────

if ($embeddingType -eq 'unknown' -and $extractedCount -eq 0) {
    Log "No embedding signal found. Attempting ILMerge detection via namespace analysis..."

    $ilmergeDetected = $false
    $mergedNamespaces = @()
    $hostAssembly = [System.IO.Path]::GetFileNameWithoutExtension($Target)

    if (Test-ToolAvailable 'ilspycmd') {
        try {
            Log "Decompiling to stdout for namespace analysis..."
            $decompOutput = & ilspycmd $Target 2>$null
            $allNamespaces = @($decompOutput | Select-String -Pattern '^\s*namespace\s+([\w.]+)' |
                ForEach-Object { $_.Matches[0].Groups[1].Value } | Sort-Object -Unique)

            # Extract root namespaces (first segment before '.')
            $rootNamespaces = @($allNamespaces | ForEach-Object { ($_ -split '\.')[0] } | Sort-Object -Unique)

            # Filter out namespaces that match the assembly name or are system namespaces
            $foreignRoots = @($rootNamespaces | Where-Object {
                $_ -ne $hostAssembly -and
                $_ -notmatch '^(System|Microsoft|Internal|Interop|FxResources|<)$'
            })

            if ($foreignRoots.Count -gt 3) {
                $ilmergeDetected = $true
                $mergedNamespaces = $foreignRoots
                $embeddingType = 'ILMerge'
                Log "ILMerge suspected: $($foreignRoots.Count) foreign root namespaces detected"
                Log "  Host assembly: $hostAssembly"
                Log "  Foreign namespaces: $($foreignRoots -join ', ')"
            } else {
                Log "Only $($foreignRoots.Count) foreign root namespaces found — not enough to suspect ILMerge"
            }
        } catch {
            Log "WARNING: Namespace analysis failed: $_"
        }
    } else {
        Log "ilspycmd not available, skipping ILMerge namespace analysis"
    }

    if ($ilmergeDetected) {
        Log "ILMerge assemblies are merged at IL level — cannot extract as separate DLLs"
        Log "Writing ilmerge_namespaces.json manifest for post-decompilation separation"

        $manifest = [ordered]@{
            type             = 'ilmerge'
            host_assembly    = $hostAssembly
            merged_namespaces = $mergedNamespaces
            note             = 'ILMerge merges at IL level. Use namespace filtering during decompilation to separate.'
        }
        $manifest | ConvertTo-Json -Depth 5 | Out-File -FilePath (Join-Path $OutputDir 'ilmerge_namespaces.json') -Encoding utf8

        Write-Output "EMBED_TYPE:ILMerge"
        Write-Output "EMBED_NOTE:ILMerge assemblies are merged at IL level, not extractable as separate DLLs"
    }
}

# ── Generic resource scan (no specific type detected) ────────────────────────

if ($embeddingType -eq 'unknown') {
    Log "Attempting generic resource scan for embedded PE files..."

    # Scan raw bytes for embedded MZ headers (crude but effective)
    $mzPositions = @()
    for ($i = 256; $i -lt ($bytes.Length - 256); $i++) {
        if ($bytes[$i] -eq 0x4D -and $bytes[$i+1] -eq 0x5A) {
            # Verify this looks like a real PE (check for "PE\0\0" at e_lfanew offset)
            if (($i + 64) -lt $bytes.Length) {
                $peOffset = [BitConverter]::ToInt32($bytes, $i + 0x3C)
                if ($peOffset -gt 0 -and $peOffset -lt 0x1000 -and ($i + $peOffset + 4) -lt $bytes.Length) {
                    if ($bytes[$i + $peOffset] -eq 0x50 -and $bytes[$i + $peOffset + 1] -eq 0x45 -and
                        $bytes[$i + $peOffset + 2] -eq 0x00 -and $bytes[$i + $peOffset + 3] -eq 0x00) {
                        $mzPositions += $i
                    }
                }
            }
        }
    }

    if ($mzPositions.Count -gt 0) {
        Log "Found $($mzPositions.Count) potential embedded PE files"

        for ($idx = 0; $idx -lt $mzPositions.Count; $idx++) {
            $offset = $mzPositions[$idx]
            # Estimate size: distance to next MZ or end of file
            $endOffset = if ($idx -lt $mzPositions.Count - 1) { $mzPositions[$idx + 1] } else { $bytes.Length }
            $size = $endOffset - $offset

            if ($size -gt 4096) {  # Minimum viable DLL size
                $embeddedBytes = New-Object byte[] $size
                [Array]::Copy($bytes, $offset, $embeddedBytes, 0, $size)

                $outFile = Join-Path $OutputDir "embedded_0x$($offset.ToString('X8')).dll"
                [System.IO.File]::WriteAllBytes($outFile, $embeddedBytes)
                Log "  Extracted embedded PE at offset 0x$($offset.ToString('X8')) ($size bytes)"
                $extractedCount++
            }
        }
    } else {
        Log "No embedded PE files detected"
    }
}

# ── Summary ──────────────────────────────────────────────────────────────────

$extractedDlls = Get-ChildItem -Path $OutputDir -Filter '*.dll' -ErrorAction SilentlyContinue
$extractedCount = if ($extractedDlls) { $extractedDlls.Count } else { 0 }

Log "Extraction complete. $extractedCount assemblies extracted."
Write-Output "EXTRACT_TOTAL:$extractedCount"

if ($extractedCount -gt 0) {
    foreach ($dll in $extractedDlls) {
        Write-Output "EXTRACTED_DLL:$($dll.FullName)"
        Log "  -> $($dll.Name) ($($dll.Length) bytes)"
    }
}

exit 0
