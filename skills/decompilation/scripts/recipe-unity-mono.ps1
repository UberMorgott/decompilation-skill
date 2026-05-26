<#
.SYNOPSIS
    Unity Mono build decompilation pipeline.
.DESCRIPTION
    Finds managed DLLs in *_Data/Managed/, runs recon on each, and delegates
    to recipe-dotnet-generic.ps1 or recipe-dotnet-confuserex.ps1 based on detection.
.PARAMETER Target
    Path to the Unity game folder.
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
$script:LogTag = 'unity-mono'

. (Join-Path $PSScriptRoot '_common.ps1')

if (-not (Test-Path $Target)) {
    Log "ERROR: Target not found: $Target"
    exit 1
}

# ── Setup ────────────────────────────────────────────────────────────────────

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

Log "Unity Mono pipeline started for: $Target"

# ── Find managed DLLs ───────────────────────────────────────────────────────

$managedDlls = @()

if (Test-Path $Target -PathType Container) {
    # Look for *_Data/Managed/*.dll pattern
    $managedDirs = Get-ChildItem -Path $Target -Directory -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -match '_Data$' } |
        ForEach-Object { Join-Path $_.FullName 'Managed' } |
        Where-Object { Test-Path $_ }

    if (-not $managedDirs) {
        # Broader search
        $managedDirs = Get-ChildItem -Path $Target -Recurse -Directory -Filter 'Managed' -ErrorAction SilentlyContinue |
            Select-Object -ExpandProperty FullName
    }

    foreach ($dir in $managedDirs) {
        $dlls = Get-ChildItem -Path $dir -Filter '*.dll' -ErrorAction SilentlyContinue
        $managedDlls += $dlls
    }
} else {
    Log "Target is a file, not a folder. Treating as single assembly."
    $managedDlls += Get-Item $Target
}

if ($managedDlls.Count -eq 0) {
    Log "ERROR: No managed DLLs found in $Target"
    Write-Output "UNITY_MONO_ERROR:no_managed_dlls"
    exit 1
}

Log "Found $($managedDlls.Count) managed DLLs"

# ── Filter: skip system/Unity engine assemblies, focus on game code ──────────

$gameDlls = $managedDlls | Where-Object {
    $name = $_.BaseName
    -not ($name -match '^(mscorlib|System\.|Microsoft\.|UnityEngine\.|Unity\.|Mono\.|netstandard|Newtonsoft|Steamworks)') -or
    $name -match 'Assembly-CSharp'
}

$systemDlls = $managedDlls | Where-Object {
    $name = $_.BaseName
    ($name -match '^(mscorlib|System\.|Microsoft\.|UnityEngine\.|Unity\.|Mono\.|netstandard)') -and
    $name -notmatch 'Assembly-CSharp'
}

Log "Game assemblies: $($gameDlls.Count), System/Engine assemblies: $($systemDlls.Count) (skipped)"

# ── Process each game assembly ───────────────────────────────────────────────

$reconScript = Join-Path $PSScriptRoot 'recon.ps1'
$genericRecipe = Join-Path $PSScriptRoot 'recipe-dotnet-generic.ps1'
$confuserExRecipe = Join-Path $PSScriptRoot 'recipe-dotnet-confuserex.ps1'

$assemblyResults = @()

foreach ($dll in $gameDlls) {
    $assemblyName = $dll.BaseName
    $assemblyOutDir = Join-Path $OutputDir $assemblyName

    Log "── Processing: $($dll.Name) ──"

    if (-not (Test-Path $assemblyOutDir)) {
        New-Item -ItemType Directory -Path $assemblyOutDir -Force | Out-Null
    }

    # Run recon on this assembly
    $reconResult = $null
    if (Test-Path $reconScript) {
        Log "Running recon on $($dll.Name)..."
        & $reconScript -Target $dll.FullName -OutputDir $assemblyOutDir 2>&1 | ForEach-Object {
            if ($_ -match '^RECON_OBFUSCATOR:(.+)$') {
                $reconResult = $Matches[1]
            }
            Log "  $_"
        }
    }

    # Route based on recon
    $recipe = 'generic'
    if ($reconResult -and $reconResult -ne 'none') {
        if ($reconResult -match 'ConfuserEx') {
            $recipe = 'confuserex'
        }
    }

    # Also check recon.json if available
    $reconJsonPath = Join-Path $assemblyOutDir 'recon.json'
    if (Test-Path $reconJsonPath) {
        try {
            $reconJson = Get-Content $reconJsonPath -Raw | ConvertFrom-Json
            if ($reconJson.obfuscator -and $reconJson.obfuscator -ne 'none') {
                if ($reconJson.obfuscator -match 'ConfuserEx') {
                    $recipe = 'confuserex'
                }
            }
        } catch {
            Log "WARNING: Could not parse recon.json for $assemblyName"
        }
    }

    Log "Selected recipe for $assemblyName : $recipe"

    # Delegate to appropriate recipe
    $recipeScript = switch ($recipe) {
        'confuserex' { $confuserExRecipe }
        default      { $genericRecipe }
    }

    if (Test-Path $recipeScript) {
        & $recipeScript -Target $dll.FullName -OutputDir $assemblyOutDir 2>&1 | ForEach-Object { Log "  $_" }
        $status = if ($LASTEXITCODE -eq 0) { 'success' } else { 'partial' }
    } else {
        Log "WARNING: Recipe script not found: $recipeScript"
        $status = 'skipped'
    }

    $assemblyResults += [ordered]@{
        assembly = $dll.Name
        recipe   = $recipe
        status   = $status
        output   = $assemblyOutDir
    }
}

# ── Pipeline summary ────────────────────────────────────────────────────────

$pipelineJson = [ordered]@{
    target     = $Target
    recipe     = 'unity-mono'
    assemblies = $assemblyResults
    total      = $gameDlls.Count
    skipped_system = $systemDlls.Count
}

$pipelineJson | ConvertTo-Json -Depth 5 | Out-File -FilePath (Join-Path $OutputDir 'pipeline.json') -Encoding utf8

$failedCount = ($assemblyResults | Where-Object { $_.status -eq 'failed' }).Count
Log "Unity Mono pipeline complete. Processed $($gameDlls.Count) assemblies, $failedCount failed."

Write-Output "PIPELINE_STATUS:$(if ($failedCount -gt 0) { 'partial' } else { 'success' })"
exit $(if ($failedCount -gt 0) { 2 } else { 0 })
