<#
.SYNOPSIS
    Simple .NET decompilation pipeline — no obfuscation.
.DESCRIPTION
    Decompiles a clean .NET assembly using ilspycmd in project mode.
    Extracts strings and builds metadata indexes.
.PARAMETER Target
    Path to the .NET DLL or EXE.
.PARAMETER OutputDir
    Root output directory for results.
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
$script:LogTag = 'dotnet-generic'

. (Join-Path $PSScriptRoot '_common.ps1')

if (-not (Test-Path $Target)) {
    Log "ERROR: Target not found: $Target"
    exit 1
}

# ── Create folder structure ──────────────────────────────────────────────────

$dirs = @('original', 'src', 'strings', 'metadata')
foreach ($d in $dirs) {
    $p = Join-Path $OutputDir $d
    if (-not (Test-Path $p)) { New-Item -ItemType Directory -Path $p -Force | Out-Null }
}

Log "Pipeline started for: $Target"

# ── Copy original ────────────────────────────────────────────────────────────

$originalCopy = Join-Path $OutputDir 'original' (Split-Path $Target -Leaf)
Copy-Item -Path $Target -Destination $originalCopy -Force
Log "Original preserved at $originalCopy"

# ── Decompile with ilspycmd ──────────────────────────────────────────────────

$srcDir = Join-Path $OutputDir 'src'
$sw = [System.Diagnostics.Stopwatch]::StartNew()

if (Test-ToolAvailable 'ilspycmd') {
    Log "Running ilspycmd -p -o"
    & ilspycmd $Target -p -o $srcDir 2>&1 | ForEach-Object { Log "  $_" }

    if ($LASTEXITCODE -ne 0) {
        Log "ERROR: ilspycmd failed with exit code $LASTEXITCODE"
    }

    $sw.Stop()
    Log "Decompilation completed in $($sw.ElapsedMilliseconds)ms"
} else {
    Log "INSTALL_REQUIRED:ilspycmd (dotnet tool install -g ilspycmd)"
    Write-Output "INSTALL_REQUIRED:ilspycmd"
}

# ── Extract strings ──────────────────────────────────────────────────────────

$stringsDir = Join-Path $OutputDir 'strings'
$allStringsFile = Join-Path $stringsDir 'all_strings.txt'
Extract-Strings -Target $Target -StringsDir $stringsDir

# ── Build indexes ────────────────────────────────────────────────────────────

$indexScript = Join-Path $PSScriptRoot 'build-indexes.ps1'
$metadataDir = Join-Path $OutputDir 'metadata'

if (Test-Path $indexScript) {
    Log "Building indexes..."
    & $indexScript -SourceDir $srcDir -OutputDir $metadataDir 2>&1 | ForEach-Object { Log "  $_" }
    Log "Indexes built"
} else {
    Log "WARNING: build-indexes.ps1 not found at $indexScript"
}

# ── Pipeline summary ────────────────────────────────────────────────────────

$pipelineJson = [ordered]@{
    target  = $Target
    recipe  = 'dotnet-generic'
    steps   = @(
        [ordered]@{ step = 1; name = 'Decompile'; status = if ((Get-ChildItem -LiteralPath $srcDir -Filter '*.cs' -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1)) { 'success' } else { 'partial' } }
        [ordered]@{ step = 2; name = 'Extract strings'; status = if (Test-Path $allStringsFile) { 'success' } else { 'failed' } }
        [ordered]@{ step = 3; name = 'Build indexes'; status = if (Test-Path (Join-Path $metadataDir 'index.json') -ErrorAction SilentlyContinue) { 'success' } else { 'skipped' } }
    )
}

$pipelineJson | ConvertTo-Json -Depth 5 | Out-File -FilePath (Join-Path $OutputDir 'pipeline.json') -Encoding utf8

Log "Pipeline complete"
Write-Output "PIPELINE_STATUS:success"
exit 0
