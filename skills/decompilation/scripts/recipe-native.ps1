<#
.SYNOPSIS
    General native binary decompilation pipeline.
.DESCRIPTION
    Uses Ghidra headless for analysis and per-function decompilation export.
    Detects Go binaries and runs GoReSym for symbol recovery.
.PARAMETER Target
    Path to the native binary (EXE, DLL, SO).
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
$script:LogTag = 'native'

. (Join-Path $PSScriptRoot '_common.ps1')

$ghidraScripts = Join-Path $PSScriptRoot 'ghidra'

# ── Create folder structure ──────────────────────────────────────────────────

$dirs = @('original', 'strings', 'native/ghidra_project', 'native/functions', 'metadata')
foreach ($d in $dirs) {
    $p = Join-Path $OutputDir $d
    if (-not (Test-Path $p)) { New-Item -ItemType Directory -Path $p -Force | Out-Null }
}

Log "Native pipeline started for: $Target"

# ── Copy original ────────────────────────────────────────────────────────────

Copy-Item -Path $Target -Destination (Join-Path $OutputDir 'original' (Split-Path $Target -Leaf)) -Force
Log "Original preserved"

# ── Detect Go binary ────────────────────────────────────────────────────────

$isGoBinary = $false
try {
    $bytes = [System.IO.File]::ReadAllBytes($Target)
    $rawText = [System.Text.Encoding]::ASCII.GetString($bytes)
    if ($rawText -match 'Go build ID:|runtime\.gopanic|go\.buildid') {
        $isGoBinary = $true
        Log "Detected Go binary"
    }
} catch {
    Log "WARNING: Could not read binary for Go detection: $_"
}

try {

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

# ── Step 2: GoReSym (if Go binary) ──────────────────────────────────────────

if ($isGoBinary) {
    Invoke-PipelineStep -Name 'GoReSym symbol recovery' -Action {
        if (-not (Test-ToolAvailable 'GoReSym')) {
            Log "INSTALL_REQUIRED:GoReSym (https://github.com/mandiant/GoReSym)"
            Write-Output "INSTALL_REQUIRED:GoReSym"
            throw "GoReSym not available"
        }

        $goreSymOutput = Join-Path $OutputDir 'native' 'goresym.json'
        & GoReSym -t -d -p $Target | Out-File -FilePath $goreSymOutput -Encoding utf8

        if ($LASTEXITCODE -ne 0) { throw "GoReSym exited with code $LASTEXITCODE" }
        Log "GoReSym symbols exported to $goreSymOutput"
    }
}

# ── Step 3: Ghidra headless import + analysis ───────────────────────────────

Invoke-PipelineStep -Name 'Ghidra headless analysis' -Action {
    if (-not (Test-ToolAvailable 'analyzeHeadless')) {
        Log "INSTALL_REQUIRED:analyzeHeadless (Ghidra — https://github.com/NationalSecurityAgency/ghidra)"
        Write-Output "INSTALL_REQUIRED:analyzeHeadless"
        throw "Ghidra analyzeHeadless not available"
    }

    $ghidraProjectDir = Join-Path $OutputDir 'native' 'ghidra_project'
    $projectName = 'NativeProject'

    Log "Importing and analyzing in Ghidra..."
    & analyzeHeadless $ghidraProjectDir $projectName `
        -import $Target `
        -scriptPath $ghidraScripts `
        2>&1 | ForEach-Object { Log "  $_" }

    if ($LASTEXITCODE -ne 0) {
        Log "WARNING: Ghidra analysis returned code $LASTEXITCODE (may have partial results)"
    }
}

# ── Step 4: Export per-function decompilation ────────────────────────────────

Invoke-PipelineStep -Name 'Export per-function decompilation' -Action {
    if (-not (Test-ToolAvailable 'analyzeHeadless')) {
        throw "Ghidra analyzeHeadless not available"
    }

    $ghidraProjectDir = Join-Path $OutputDir 'native' 'ghidra_project'
    $projectName = 'NativeProject'
    $targetName = Split-Path $Target -Leaf
    $functionsDir = Join-Path $OutputDir 'native' 'functions'

    Log "Exporting function decompilation..."
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

} finally {

# ── Pipeline summary ────────────────────────────────────────────────────────

$pipelineJson = [ordered]@{
    target       = $Target
    recipe       = 'native'
    is_go_binary = $isGoBinary
    steps        = $script:StepResults
    total_steps  = $script:StepNumber
    successful   = ($script:StepResults | Where-Object { $_.status -eq 'success' }).Count
    failed       = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
}

$pipelineJson | ConvertTo-Json -Depth 5 | Out-File -FilePath (Join-Path $OutputDir 'pipeline.json') -Encoding utf8

$failedCount = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
Log "Native pipeline complete. $($script:StepNumber) steps, $failedCount failed."

} # end finally

if ($failedCount -gt 0) {
    Write-Output "PIPELINE_STATUS:partial"
    exit 2
} else {
    Write-Output "PIPELINE_STATUS:success"
    exit 0
}
