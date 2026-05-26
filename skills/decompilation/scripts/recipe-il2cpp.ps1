<#
.SYNOPSIS
    Unity IL2CPP decompilation pipeline.
.DESCRIPTION
    Auto-detects GameAssembly.dll + global-metadata.dat, runs Il2CppDumper for
    dummy DLLs, attempts Cpp2IL for IL recovery, decompiles results with ilspycmd,
    and optionally applies symbols to Ghidra.
.PARAMETER Target
    Path to the Unity game folder (containing GameAssembly.dll).
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
$script:LogTag = 'il2cpp'

. (Join-Path $PSScriptRoot '_common.ps1')

# ── Create folder structure ──────────────────────────────────────────────────

$dirs = @('original', 'dummy_dlls', 'cpp2il_out', 'src', 'strings', 'metadata', 'native/ghidra_project', 'native/functions')
foreach ($d in $dirs) {
    $p = Join-Path $OutputDir $d
    if (-not (Test-Path $p)) { New-Item -ItemType Directory -Path $p -Force | Out-Null }
}

Log "IL2CPP pipeline started for: $Target"

# ── Auto-detect GameAssembly.dll and global-metadata.dat ─────────────────────

$gameAssemblyPath = $null
$globalMetadataPath = $null

if (Test-Path $Target -PathType Container) {
    # Search in folder
    $gaFiles = Get-ChildItem -Path $Target -Recurse -Filter 'GameAssembly.dll' -ErrorAction SilentlyContinue
    $soFiles = Get-ChildItem -Path $Target -Recurse -Filter 'libil2cpp.so' -ErrorAction SilentlyContinue
    $metaFiles = Get-ChildItem -Path $Target -Recurse -Filter 'global-metadata.dat' -ErrorAction SilentlyContinue

    $gameAssemblyPath = if ($gaFiles) { $gaFiles[0].FullName } elseif ($soFiles) { $soFiles[0].FullName } else { $null }
    $globalMetadataPath = if ($metaFiles) { $metaFiles[0].FullName } else { $null }
} else {
    # Single file provided — assume it's GameAssembly.dll, look for metadata nearby
    $gameAssemblyPath = $Target
    $parentDir = Split-Path $Target -Parent
    $metaSearch = Get-ChildItem -Path $parentDir -Recurse -Filter 'global-metadata.dat' -ErrorAction SilentlyContinue
    $globalMetadataPath = if ($metaSearch) { $metaSearch[0].FullName } else { $null }
}

if (-not $gameAssemblyPath) {
    Log "ERROR: GameAssembly.dll / libil2cpp.so not found in $Target"
    Write-Output "IL2CPP_ERROR:no_game_assembly"
    exit 1
}

if (-not $globalMetadataPath) {
    Log "ERROR: global-metadata.dat not found near $Target"
    Write-Output "IL2CPP_ERROR:no_global_metadata"
    exit 1
}

Log "GameAssembly: $gameAssemblyPath"
Log "Metadata: $globalMetadataPath"

# ── Copy originals ──────────────────────────────────────────────────────────

Copy-Item -Path $gameAssemblyPath -Destination (Join-Path $OutputDir 'original' (Split-Path $gameAssemblyPath -Leaf)) -Force
Copy-Item -Path $globalMetadataPath -Destination (Join-Path $OutputDir 'original' (Split-Path $globalMetadataPath -Leaf)) -Force

# ── Step 1: Il2CppDumper → dummy DLLs ───────────────────────────────────────

Invoke-PipelineStep -Name 'Il2CppDumper (dummy DLLs)' -Action {
    if (-not (Test-ToolAvailable 'Il2CppDumper')) {
        Log "INSTALL_REQUIRED:Il2CppDumper (https://github.com/Perfare/Il2CppDumper)"
        Write-Output "INSTALL_REQUIRED:Il2CppDumper"
        throw "Il2CppDumper not available"
    }

    $dummyDir = Join-Path $OutputDir 'dummy_dlls'
    & Il2CppDumper $gameAssemblyPath $globalMetadataPath $dummyDir 2>&1 | ForEach-Object { Log "  $_" }

    if ($LASTEXITCODE -ne 0) { throw "Il2CppDumper exited with code $LASTEXITCODE" }

    $dllCount = (Get-ChildItem -Path $dummyDir -Filter '*.dll' -ErrorAction SilentlyContinue).Count
    Log "Il2CppDumper produced $dllCount dummy DLLs"
}

# ── Step 2: Cpp2IL attempt ───────────────────────────────────────────────────

Invoke-PipelineStep -Name 'Cpp2IL (IL recovery)' -Action {
    if (-not (Test-ToolAvailable 'Cpp2IL')) {
        Log "INSTALL_OPTIONAL:Cpp2IL (https://github.com/SamboyCoding/Cpp2IL)"
        Write-Output "INSTALL_OPTIONAL:Cpp2IL"
        throw "Cpp2IL not available"
    }

    $gameDir = if (Test-Path $Target -PathType Container) { $Target } else { Split-Path $Target -Parent }
    $cpp2ilOut = Join-Path $OutputDir 'cpp2il_out'

    & Cpp2IL --game-path $gameDir --output-root $cpp2ilOut --just-give-me-dlls-asap-dammit 2>&1 | ForEach-Object { Log "  $_" }

    if ($LASTEXITCODE -ne 0) {
        Log "WARNING: Cpp2IL returned code $LASTEXITCODE (partial results may exist)"
    }

    $dllCount = (Get-ChildItem -Path $cpp2ilOut -Filter '*.dll' -Recurse -ErrorAction SilentlyContinue).Count
    Log "Cpp2IL produced $dllCount DLLs"
}

# ── Step 3: Decompile with ilspycmd ──────────────────────────────────────────

Invoke-PipelineStep -Name 'Decompile DLLs (ilspycmd)' -Action {
    if (-not (Test-ToolAvailable 'ilspycmd')) {
        Log "INSTALL_REQUIRED:ilspycmd (dotnet tool install -g ilspycmd)"
        Write-Output "INSTALL_REQUIRED:ilspycmd"
        throw "ilspycmd not available"
    }

    $srcDir = Join-Path $OutputDir 'src'

    # Decompile dummy DLLs
    $dummyDlls = Get-ChildItem -Path (Join-Path $OutputDir 'dummy_dlls') -Filter '*.dll' -ErrorAction SilentlyContinue
    foreach ($dll in $dummyDlls) {
        $name = [System.IO.Path]::GetFileNameWithoutExtension($dll.Name)
        # Skip system/Unity engine DLLs for cleaner output
        if ($name -match '^(mscorlib|System\.|UnityEngine\.|Unity\.)' -and $name -notmatch 'Assembly-CSharp') {
            continue
        }
        $outDir = Join-Path $srcDir "dummy_$name"
        if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir -Force | Out-Null }

        Log "Decompiling dummy: $($dll.Name)"
        & ilspycmd $dll.FullName -p -o $outDir 2>&1 | Out-Null
    }

    # Decompile Cpp2IL output if available
    $cpp2ilDlls = Get-ChildItem -Path (Join-Path $OutputDir 'cpp2il_out') -Filter '*.dll' -Recurse -ErrorAction SilentlyContinue
    foreach ($dll in $cpp2ilDlls) {
        $name = [System.IO.Path]::GetFileNameWithoutExtension($dll.Name)
        if ($name -match '^(mscorlib|System\.|UnityEngine\.|Unity\.)' -and $name -notmatch 'Assembly-CSharp') {
            continue
        }
        $outDir = Join-Path $srcDir "cpp2il_$name"
        if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir -Force | Out-Null }

        Log "Decompiling Cpp2IL: $($dll.Name)"
        & ilspycmd $dll.FullName -p -o $outDir 2>&1 | Out-Null
    }
}

# ── Step 4: Strings extraction ───────────────────────────────────────────────

Invoke-PipelineStep -Name 'Strings extraction' -Action {
    $allStringsFile = Join-Path $OutputDir 'strings' 'all_strings.txt'

    if (Test-ToolAvailable 'strings') {
        & strings -accepteula -n 6 $gameAssemblyPath | Out-File -FilePath $allStringsFile -Encoding utf8
    } else {
        $content = [System.IO.File]::ReadAllBytes($gameAssemblyPath)
        [System.Text.Encoding]::ASCII.GetString($content) -split '[^\x20-\x7E]+' |
            Where-Object { $_.Length -ge 6 } |
            Out-File -FilePath $allStringsFile -Encoding utf8
    }

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

# ── Step 5: Optional Ghidra with IL2CPP symbols ─────────────────────────────

Invoke-PipelineStep -Name 'Ghidra with IL2CPP symbols (optional)' -Action {
    if (-not (Test-ToolAvailable 'analyzeHeadless')) {
        Log "INSTALL_OPTIONAL:analyzeHeadless (Ghidra)"
        Log "Skipping Ghidra analysis (optional step)"
        return
    }

    $ghidraProjectDir = Join-Path $OutputDir 'native' 'ghidra_project'
    $projectName = 'IL2CPP_Project'

    # Check for Il2CppDumper script.json (Ghidra import script)
    $scriptJson = Join-Path $OutputDir 'dummy_dlls' 'script.json'
    $ghidraIl2cppScript = Join-Path $OutputDir 'dummy_dlls' 'ghidra_with_struct.py'

    if ((Test-Path $scriptJson) -and (Test-Path $ghidraIl2cppScript)) {
        Log "Found Il2CppDumper script.json + ghidra_with_struct.py — importing with IL2CPP symbols"
        & analyzeHeadless $ghidraProjectDir $projectName `
            -import $gameAssemblyPath `
            -postScript $ghidraIl2cppScript $scriptJson `
            2>&1 | ForEach-Object { Log "  $_" }
    } else {
        if (Test-Path $scriptJson) {
            Log "Found script.json but ghidra_with_struct.py missing — importing without IL2CPP symbols"
        } else {
            Log "No Il2CppDumper Ghidra script found — importing without IL2CPP symbols"
        }
        & analyzeHeadless $ghidraProjectDir $projectName `
            -import $gameAssemblyPath `
            2>&1 | ForEach-Object { Log "  $_" }
    }

    $functionsDir = Join-Path $OutputDir 'native' 'functions'
    & analyzeHeadless $ghidraProjectDir $projectName `
        -process (Split-Path $gameAssemblyPath -Leaf) `
        -postScript ExportDecompilation.java $functionsDir `
        2>&1 | ForEach-Object { Log "  $_" }

    $exportedCount = (Get-ChildItem -Path $functionsDir -Filter '*.c' -ErrorAction SilentlyContinue).Count
    Log "Ghidra exported $exportedCount function files"
}

# ── Pipeline summary ────────────────────────────────────────────────────────

$pipelineJson = [ordered]@{
    target              = $Target
    recipe              = 'il2cpp'
    game_assembly       = $gameAssemblyPath
    global_metadata     = $globalMetadataPath
    steps               = $script:StepResults
    total_steps         = $script:StepNumber
    successful          = ($script:StepResults | Where-Object { $_.status -eq 'success' }).Count
    failed              = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
}

$pipelineJson | ConvertTo-Json -Depth 5 | Out-File -FilePath (Join-Path $OutputDir 'pipeline.json') -Encoding utf8

$failedCount = ($script:StepResults | Where-Object { $_.status -eq 'failed' }).Count
Log "IL2CPP pipeline complete. $($script:StepNumber) steps, $failedCount failed."

if ($failedCount -gt 0) {
    Write-Output "PIPELINE_STATUS:partial"
    exit 2
} else {
    Write-Output "PIPELINE_STATUS:success"
    exit 0
}
