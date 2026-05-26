<#
.SYNOPSIS
    Delphi binary decompilation pipeline.
.DESCRIPTION
    Uses IDR for RTTI/VCL recovery, Ghidra headless with dhrake for decompilation,
    and Resource Hacker for .dfm form extraction. Exports per-function pseudo-C.
.PARAMETER Target
    Path to the Delphi executable.
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
$script:StepResults = @()
$script:StepNumber = 0
$script:LogTag = 'delphi'

. (Join-Path $PSScriptRoot '_common.ps1')

$ghidraScripts = Join-Path $PSScriptRoot 'ghidra'

try {

# ── Create folder structure ──────────────────────────────────────────────────

$dirs = @('original', 'strings', 'native/ghidra_project', 'native/functions', 'native/forms', 'metadata')
foreach ($d in $dirs) {
    $p = Join-Path $OutputDir $d
    if (-not (Test-Path $p)) { New-Item -ItemType Directory -Path $p -Force | Out-Null }
}

Log "Delphi pipeline started for: $Target"

# ── Copy original ────────────────────────────────────────────────────────────

Copy-Item -Path $Target -Destination (Join-Path $OutputDir 'original' (Split-Path $Target -Leaf)) -Force
Log "Original preserved"

# ── Check tool availability ──────────────────────────────────────────────────

$missingTools = @()
if (-not (Test-ToolAvailable 'idr64')) { $missingTools += 'idr64'; Log "INSTALL_REQUIRED:idr64 (Interactive Delphi Reconstructor)" }
if (-not (Test-ToolAvailable 'analyzeHeadless')) { $missingTools += 'analyzeHeadless'; Log "INSTALL_REQUIRED:analyzeHeadless (Ghidra)" }
if (-not (Test-ToolAvailable 'ResourceHacker')) { $missingTools += 'ResourceHacker'; Log "INSTALL_OPTIONAL:ResourceHacker" }

foreach ($t in $missingTools) { Write-Output "INSTALL_REQUIRED:$t" }

# ── Step 1: Strings extraction ───────────────────────────────────────────────

Invoke-PipelineStep -Name 'Strings extraction' -Action {
    $allStringsFile = Join-Path $OutputDir 'strings' 'all_strings.txt'

    if (Test-ToolAvailable 'strings') {
        & strings -accepteula -n 6 $Target | Out-File -FilePath $allStringsFile -Encoding utf8
    } else {
        $content = [System.IO.File]::ReadAllBytes($Target)
        $ascii = [System.Text.Encoding]::ASCII.GetString($content) -split '[^\x20-\x7E]+' |
            Where-Object { $_.Length -ge 6 }
        $unicode = [System.Text.Encoding]::Unicode.GetString($content) -split '[^\x20-\x7E]+' |
            Where-Object { $_.Length -ge 6 }
        ($ascii + $unicode) | Sort-Object -Unique | Out-File -FilePath $allStringsFile -Encoding utf8
    }

    # URL candidates
    Get-Content $allStringsFile -ErrorAction SilentlyContinue |
        Where-Object { $_ -match '^https?:|^/' } |
        Out-File -FilePath (Join-Path $OutputDir 'strings' 'url_candidates.txt') -Encoding utf8

    # Error messages
    Get-Content $allStringsFile -ErrorAction SilentlyContinue |
        Where-Object { $_ -match '(?i)(error|exception|fail|invalid|cannot|unable|denied|timeout)' } |
        Out-File -FilePath (Join-Path $OutputDir 'strings' 'error_messages.txt') -Encoding utf8

    # Format strings ({0}, {1}, %s, %d, etc.)
    $formatResults = @(Get-Content $allStringsFile -ErrorAction SilentlyContinue |
        Select-String -Pattern '\{[0-9]+\}|%[sdfu]' |
        ForEach-Object { $_.Line })
    $formatResults | Out-File -FilePath (Join-Path $OutputDir 'strings' 'format_strings.txt') -Encoding utf8

    Log "Strings extracted"
}

# ── Step 2: IDR auto-mode → .idc export ─────────────────────────────────────

$idcFile = Join-Path $OutputDir 'native' 'symbols.idc'

Invoke-PipelineStep -Name 'IDR analysis (.idc export)' -Action {
    if (-not (Test-ToolAvailable 'idr64')) {
        throw "IDR (idr64) not available"
    }

    Log "Running IDR in auto mode..."
    & idr64 -auto $Target -save-idc $idcFile 2>&1 | ForEach-Object { Log "  $_" }

    if ($LASTEXITCODE -ne 0) { throw "IDR exited with code $LASTEXITCODE" }
    if (-not (Test-Path $idcFile)) { throw "IDR did not produce .idc file" }
    Log "IDR .idc exported to $idcFile"
}

# ── Step 3: Ghidra headless + dhrake ────────────────────────────────────────

Invoke-PipelineStep -Name 'Ghidra headless + dhrake' -Action {
    if (-not (Test-ToolAvailable 'analyzeHeadless')) {
        throw "Ghidra analyzeHeadless not available"
    }

    $ghidraProjectDir = Join-Path $OutputDir 'native' 'ghidra_project'
    $projectName = 'DelphiProject'
    $targetName = Split-Path $Target -Leaf

    # Import binary into Ghidra
    Log "Importing into Ghidra..."
    & analyzeHeadless $ghidraProjectDir $projectName `
        -import $Target `
        -scriptPath $ghidraScripts `
        -postScript dhrake_apply.py $idcFile `
        2>&1 | ForEach-Object { Log "  $_" }

    if ($LASTEXITCODE -ne 0) {
        Log "WARNING: Ghidra import/analysis returned code $LASTEXITCODE (may still have partial results)"
    }

    # Export decompilation per function
    Log "Exporting per-function decompilation..."
    $functionsDir = Join-Path $OutputDir 'native' 'functions'
    & analyzeHeadless $ghidraProjectDir $projectName `
        -process $targetName `
        -scriptPath $ghidraScripts `
        -postScript ExportDecompilation.java $functionsDir `
        2>&1 | ForEach-Object { Log "  $_" }

    if ($LASTEXITCODE -ne 0) {
        Log "WARNING: Ghidra export returned code $LASTEXITCODE"
    }

    $exportedCount = (Get-ChildItem -Path $functionsDir -Filter '*.c' -ErrorAction SilentlyContinue).Count
    Log "Exported $exportedCount function files"
}

# ── Step 4: Resource Hacker (.dfm forms) ────────────────────────────────────

Invoke-PipelineStep -Name 'Resource Hacker (forms/resources)' -Action {
    if (-not (Test-ToolAvailable 'ResourceHacker')) {
        throw "ResourceHacker not available"
    }

    $formsDir = Join-Path $OutputDir 'native' 'forms'

    # Extract all resources
    & ResourceHacker -open $Target -save (Join-Path $formsDir 'resources.rc') -action extract 2>&1 | ForEach-Object { Log "  $_" }

    # Specifically extract DFM forms
    & ResourceHacker -open $Target -save (Join-Path $formsDir 'forms.rc') -action extract -mask RCDATA 2>&1 | ForEach-Object { Log "  $_" }

    # Extract version info
    & ResourceHacker -open $Target -save (Join-Path $formsDir 'version.rc') -action extract -mask VERSIONINFO 2>&1 | ForEach-Object { Log "  $_" }

    $extractedCount = (Get-ChildItem -Path $formsDir -ErrorAction SilentlyContinue).Count
    Log "Extracted $extractedCount resource files"
}

} finally {

# ── Pipeline summary ────────────────────────────────────────────────────────

$pipelineJson = [ordered]@{
    target      = $Target
    recipe      = 'delphi'
    steps       = $script:StepResults
    total_steps = $script:StepNumber
    successful  = ($script:StepResults | Where-Object { $_.status -eq 'success' }).Count
    failed      = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
}

$pipelineJson | ConvertTo-Json -Depth 5 | Out-File -FilePath (Join-Path $OutputDir 'pipeline.json') -Encoding utf8

$failedCount = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
Log "Delphi pipeline complete. $($script:StepNumber) steps, $failedCount failed."

} # end finally

if ($failedCount -gt 0) {
    Write-Output "PIPELINE_STATUS:partial"
    exit 2
} else {
    Write-Output "PIPELINE_STATUS:success"
    exit 0
}
