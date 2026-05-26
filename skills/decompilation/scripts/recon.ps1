<#
.SYNOPSIS
    Phase 0 reconnaissance — identify binary type, obfuscator, and route next phase.
.DESCRIPTION
    Runs DiE (Detect It Easy), strings extraction, SHA256, and heuristic checks.
    Produces recon.json with full schema for downstream pipeline routing.
.PARAMETER Target
    Path to a file or folder to analyze.
.PARAMETER OutputDir
    Directory where recon.json and strings output will be written.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$Target,

    [Parameter(Mandatory)]
    [string]$OutputDir
)

$ErrorActionPreference = 'Continue'
$script:ExitCode = 0
$script:PipelineLog = Join-Path $OutputDir 'pipeline.log'
$script:LogTag = 'recon'

. (Join-Path $PSScriptRoot '_common.ps1')

# ── Setup ────────────────────────────────────────────────────────────────────

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

if (-not (Test-Path $Target)) {
    Log "ERROR: Target not found: $Target"
    Write-Error "Target not found: $Target"
    exit 1
}

$isFolder = (Get-Item $Target).PSIsContainer
$targetFile = if ($isFolder) {
    # For folders, pick the first DLL/EXE or GameAssembly.dll
    $ga = Get-ChildItem -Path $Target -Recurse -Filter 'GameAssembly.dll' -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($ga) { $ga.FullName }
    else {
        $first = Get-ChildItem -Path $Target -Recurse -Include '*.dll','*.exe' -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($first) { $first.FullName } else { $null }
    }
} else {
    $Target
}

if (-not $targetFile) {
    Log "ERROR: No analyzable binary found in $Target"
    exit 1
}

Log "Target file: $targetFile"

# ── SHA256 ───────────────────────────────────────────────────────────────────

$sha256 = (Get-FileHash -Path $targetFile -Algorithm SHA256).Hash
Log "SHA256: $sha256"

# ── Read first bytes for magic detection ─────────────────────────────────────

$bytes = [System.IO.File]::ReadAllBytes($targetFile)
$fileSize = $bytes.Length

# PE check (MZ header)
$isPE = ($fileSize -ge 2) -and ($bytes[0] -eq 0x4D) -and ($bytes[1] -eq 0x5A)

# .NET detection via PE CLR Data Directory (index 14) — the standard way
function Test-DotNetAssembly {
    param([byte[]]$Bytes, [int]$FileSize)
    try {
        if ($FileSize -lt 0x80) { return $false }

        # Check MZ
        if ($Bytes[0] -ne 0x4D -or $Bytes[1] -ne 0x5A) { return $false }

        # e_lfanew (PE header offset)
        $peOffset = [BitConverter]::ToInt32($Bytes, 0x3C)
        if ($peOffset -le 0 -or ($peOffset + 24) -ge $FileSize) { return $false }

        # PE signature "PE\0\0"
        $peSig = [BitConverter]::ToUInt32($Bytes, $peOffset)
        if ($peSig -ne 0x00004550) { return $false }

        # COFF header: skip to SizeOfOptionalHeader at peOffset+20
        $optHeaderSize = [BitConverter]::ToUInt16($Bytes, $peOffset + 20)
        if ($optHeaderSize -eq 0) { return $false }

        $optHeaderStart = $peOffset + 24
        if ($optHeaderStart + 2 -ge $FileSize) { return $false }

        $magic = [BitConverter]::ToUInt16($Bytes, $optHeaderStart)

        # PE32 (0x10B): data dirs at offset 96; PE32+ (0x20B): at offset 112
        if ($magic -eq 0x10B) {
            $dataDirOffset = $optHeaderStart + 96
        } elseif ($magic -eq 0x20B) {
            $dataDirOffset = $optHeaderStart + 112
        } else {
            return $false
        }

        # CLR Runtime Header is data directory index 14, each entry = 8 bytes
        $clrDirOffset = $dataDirOffset + (14 * 8)
        if ($clrDirOffset + 8 -gt $optHeaderStart + $optHeaderSize) { return $false }
        if ($clrDirOffset + 8 -gt $FileSize) { return $false }

        $clrRva  = [BitConverter]::ToUInt32($Bytes, $clrDirOffset)
        $clrSize = [BitConverter]::ToUInt32($Bytes, $clrDirOffset + 4)

        return ($clrRva -ne 0 -and $clrSize -ne 0)
    }
    catch { return $false }
}

$hasDotNetMetadata = $false
if ($isPE) {
    # Primary: check PE CLR Data Directory entry 14
    $hasDotNetMetadata = Test-DotNetAssembly -Bytes $bytes -FileSize $fileSize

    # Secondary fallback: BSJB signature scan (covers edge cases)
    if (-not $hasDotNetMetadata -and $fileSize -gt 512) {
        $searchLimit = [Math]::Min($fileSize - 4, 10 * 1024 * 1024)  # up to 10 MB
        for ($i = 0; $i -lt $searchLimit; $i++) {
            if ($bytes[$i] -eq 0x42 -and $bytes[$i+1] -eq 0x53 -and $bytes[$i+2] -eq 0x4A -and $bytes[$i+3] -eq 0x42) {
                $hasDotNetMetadata = $true
                break
            }
        }
    }
}

# ── DiE detection ────────────────────────────────────────────────────────────

$dieResult = $null
$dieAvailable = Test-ToolAvailable 'diec'

if (-not $dieAvailable) {
    Log "INSTALL_REQUIRED:diec — Detect It Easy CLI not found in PATH, using fallback heuristics"
    Write-Output "INSTALL_REQUIRED:diec"
    $script:ExitCode = 2
} else {
    try {
        Log "Running diec on $targetFile"
        $dieRaw = & diec $targetFile -j 2>&1
        $dieJson = $dieRaw | Out-String
        $dieResult = $dieJson | ConvertFrom-Json -ErrorAction SilentlyContinue
        Log "DiE detection complete"
    } catch {
        Log "WARNING: DiE execution failed: $_"
        $script:ExitCode = 2
    }
}

# ── Strings extraction ──────────────────────────────────────────────────────

$stringsDir = Join-Path $OutputDir 'strings'
if (-not (Test-Path $stringsDir)) {
    New-Item -ItemType Directory -Path $stringsDir -Force | Out-Null
}

$allStringsFile = Join-Path $stringsDir 'all_strings.txt'
Log "Extracting strings..."

try {
    if (Test-ToolAvailable 'strings') {
        & strings -accepteula -n 6 $targetFile | Out-File -FilePath $allStringsFile -Encoding utf8
    } else {
        # PowerShell fallback: extract ASCII and Unicode printable strings (min length 6)
        $content = [System.IO.File]::ReadAllBytes($targetFile)

        # ASCII strings
        $asciiStrings = [System.Text.Encoding]::ASCII.GetString($content) -split '[^\x20-\x7E]+' |
            Where-Object { $_.Length -ge 6 }

        # Unicode strings
        $unicodeStrings = [System.Text.Encoding]::Unicode.GetString($content) -split '[^\x20-\x7E]+' |
            Where-Object { $_.Length -ge 6 }

        ($asciiStrings + $unicodeStrings) | Sort-Object -Unique | Out-File -FilePath $allStringsFile -Encoding utf8
    }
    Log "Strings extracted to $allStringsFile"

    # Filter URL candidates
    $urlFile = Join-Path $stringsDir 'url_candidates.txt'
    Get-Content $allStringsFile -ErrorAction SilentlyContinue |
        Where-Object { $_ -match '^https?:|^/' } |
        Out-File -FilePath $urlFile -Encoding utf8

    # Filter error messages
    $errFile = Join-Path $stringsDir 'error_messages.txt'
    Get-Content $allStringsFile -ErrorAction SilentlyContinue |
        Where-Object { $_ -match '(?i)(error|exception|fail|invalid|cannot|unable|denied|timeout|unauthorized)' } |
        Out-File -FilePath $errFile -Encoding utf8

    # Filter format strings ({0}, {1}, %s, %d, etc.)
    $formatResults = @(Get-Content $allStringsFile -ErrorAction SilentlyContinue |
        Select-String -Pattern '\{[0-9]+\}|%[sdfu]' |
        ForEach-Object { $_.Line })
    $formatResults | Out-File -FilePath (Join-Path $stringsDir 'format_strings.txt') -Encoding utf8

} catch {
    Log "WARNING: Strings extraction failed: $_"
}

# ── Parse DiE output for compiler/obfuscator ────────────────────────────────

$runtime   = 'unknown'
$compiler  = 'unknown'
$obfuscator = 'none'
$obfuscatorFeatures = @()
$packed    = $false

if ($dieResult) {
    $dieText = ($dieResult | ConvertTo-Json -Depth 10).ToLower()

    # Runtime / compiler detection from DiE
    if ($dieText -match 'confuserex') {
        $obfuscator = 'ConfuserEx 2'
    }
    if ($dieText -match 'eazfuscator') { $obfuscator = 'Eazfuscator' }
    if ($dieText -match 'smartassembly') { $obfuscator = 'SmartAssembly' }
    if ($dieText -match '\.net reactor') { $obfuscator = '.NET Reactor' }
    if ($dieText -match 'babel') { $obfuscator = 'Babel.NET' }
    if ($dieText -match 'koivm') { $obfuscator = 'KoiVM' }
    if ($dieText -match 'themida') { $obfuscator = 'Themida'; $packed = $true }
    if ($dieText -match 'vmprotect') { $obfuscator = 'VMProtect'; $packed = $true }

    if ($dieText -match '\.net') {
        $compiler = 'Microsoft .NET'
        if ($dieText -match '\.net\s*([\d.]+)') { $runtime = ".NET $($Matches[1])" }
        elseif ($dieText -match 'framework') { $runtime = '.NET Framework' }
        else { $runtime = '.NET' }
    }
    if ($dieText -match 'delphi') {
        $compiler = 'Embarcadero Delphi'
        $runtime = 'Delphi'
    }
    if ($dieText -match 'msvc|visual c') {
        $compiler = 'MSVC'
        $runtime = 'native_win64'
    }
    if ($dieText -match 'gcc') { $compiler = 'GCC'; $runtime = 'native' }
    if ($dieText -match 'golang|go\b') { $compiler = 'Go'; $runtime = 'Go' }
    if ($dieText -match 'rust') { $compiler = 'Rust'; $runtime = 'Rust' }
    if ($dieText -match 'upx|packed') { $packed = $true }
}

# ── ConfuserEx heuristic detection ──────────────────────────────────────────

function Test-ConfuserExHeuristic {
    param(
        [string]$Target,
        [string]$AllStringsPath
    )

    $markers = 0
    $details = @()

    # 1. Check strings for explicit ConfuserEx signature (sometimes left in resources)
    if (Test-Path $AllStringsPath) {
        $content = Get-Content $AllStringsPath -ErrorAction SilentlyContinue
        if ($content | Select-String -Pattern 'ConfuserEx|Confuser\.Core|ConfuserAttribute' -Quiet) {
            $markers += 3  # definitive
            $details += "ConfuserEx signature found in strings"
        }
        # Check for encrypted/random resource names (ConfuserEx resource protection)
        $randomResources = $content | Where-Object { $_ -match '^[a-f0-9]{32}$|^[A-Za-z0-9+/]{20,}=*$' }
        if (($randomResources | Measure-Object).Count -gt 2) {
            $markers += 1
            $details += "Random/hash-like resource names found"
        }
    }

    # 2. PUA byte scan — fast, reliable, no SDK needed
    # ConfuserEx renames types/methods to Unicode Private Use Area (U+E000-U+F8FF).
    # In UTF-8: 0xEE 0x80-0xBE 0xXX. Count occurrences and compute density per MB.
    # Tested thresholds (real binaries):
    #   ConfuserEx:  824-872 PUA/MB
    #   Clean .NET:  39 PUA/MB
    #   Threshold:   200 PUA/MB → obfuscated
    try {
        $bytes = [System.IO.File]::ReadAllBytes($Target)
        $puaCount = 0
        for ($i = 0; $i -lt $bytes.Length - 2; $i++) {
            if ($bytes[$i] -eq 0xEE -and $bytes[$i+1] -ge 0x80 -and $bytes[$i+1] -le 0xBE) {
                $puaCount++
            }
        }
        $sizeMB = $bytes.Length / 1MB
        $density = if ($sizeMB -gt 0) { [math]::Round($puaCount / $sizeMB, 0) } else { 0 }

        if ($density -gt 500) {
            $markers += 3
            $details += "Very high PUA density: $puaCount occurrences, $density/MB (strong ConfuserEx indicator)"
        } elseif ($density -gt 200) {
            $markers += 2
            $details += "High PUA density: $puaCount occurrences, $density/MB (likely ConfuserEx)"
        } elseif ($density -gt 100) {
            $markers += 1
            $details += "Moderate PUA density: $puaCount occurrences, $density/MB"
        }
    }
    catch {
        $details += "PUA scan failed: $_"
    }

    return @{
        IsConfuserEx = ($markers -ge 2)
        Confidence   = if ($markers -ge 3) { "high" } elseif ($markers -ge 2) { "medium" } else { "low" }
        Markers      = $markers
        Details      = $details
    }
}

# ── Determine kind ──────────────────────────────────────────────────────────

$kind = 'unknown'

# Check for Unity folder structure
$isUnityIL2CPP = $false
$isUnityMono = $false

if ($isFolder) {
    $gameAssembly = Get-ChildItem -Path $Target -Recurse -Filter 'GameAssembly.dll' -ErrorAction SilentlyContinue | Select-Object -First 1
    $globalMeta = Get-ChildItem -Path $Target -Recurse -Filter 'global-metadata.dat' -ErrorAction SilentlyContinue | Select-Object -First 1
    $managedDlls = Get-ChildItem -Path $Target -Recurse -Filter '*.dll' -ErrorAction SilentlyContinue |
        Where-Object { $_.DirectoryName -match 'Managed' }

    if ($gameAssembly -and $globalMeta) {
        $isUnityIL2CPP = $true
        $kind = 'unity_il2cpp'
    } elseif ($managedDlls) {
        $isUnityMono = $true
        $kind = 'unity_mono'
    }
}

if ($kind -eq 'unknown') {
    if ($hasDotNetMetadata) {
        $kind = 'managed'
        if ($compiler -eq 'unknown') { $compiler = 'Microsoft .NET' }
        if ($runtime -eq 'unknown') { $runtime = '.NET' }
    } elseif ($isPE) {
        $kind = 'native'
        if ($runtime -eq 'unknown') { $runtime = 'native_win64' }

        # Heuristic Delphi detection (when DiE is not available or missed it)
        if ($compiler -eq 'unknown' -or $compiler -eq 'MSVC') {
            $delphiMarkers = 0
            $strContent = if (Test-Path $allStringsFile) { Get-Content $allStringsFile -Raw -ErrorAction SilentlyContinue } else { '' }
            if ($strContent) {
                if ($strContent -match '(?i)Borland|Embarcadero') { $delphiMarkers++ }
                if ($strContent -match 'TObject|TForm|TPersistent') { $delphiMarkers++ }
                if ($strContent -match 'System\.SysUtils|Vcl\.Forms') { $delphiMarkers++ }
                if ($strContent -match '(?i)borlndmm\.dll|rtl\d*\.bpl|vcl\d*\.bpl') { $delphiMarkers++ }
            }
            if ($delphiMarkers -ge 2) {
                $compiler = 'Embarcadero Delphi (heuristic)'
                $runtime = 'delphi'
            }
        }
    }
}

# ── Detect obfuscator features from strings ─────────────────────────────────

if ($obfuscator -match 'ConfuserEx') {
    $allStr = if (Test-Path $allStringsFile) { Get-Content $allStringsFile -Raw -ErrorAction SilentlyContinue } else { '' }
    if ($allStr -match 'ConfuserEx') { }  # confirmed
    # Common features detected heuristically
    $obfuscatorFeatures = @('constants', 'ctrl_flow', 'ref_proxy')
    if ($allStr -match 'anti.?tamper') { $obfuscatorFeatures += 'anti_tamper' }
    if ($allStr -match 'resources') { $obfuscatorFeatures += 'resources' }
}

# ── Detect embedded assembly signals (managed binaries only) ───────────────

$embeddedSuspected = $false
$embeddedSignals = @()

if ($kind -eq 'managed' -or $kind -eq 'unity_mono') {
    if (Test-Path $allStringsFile) {
        $strContent = Get-Content $allStringsFile -Raw -ErrorAction SilentlyContinue
        if ($strContent) {
            if ($strContent -match 'costura\.' -or $strContent -match 'Costura\.Fody') {
                $embeddedSuspected = $true
                $embeddedSignals += 'Costura.Fody resources'
            }
            if ($strContent -match 'AssemblyResolve') {
                $embeddedSuspected = $true
                $embeddedSignals += 'AssemblyResolve hook'
            }
            if ($strContent -match 'ILRepack|ILMerge') {
                $embeddedSuspected = $true
                $embeddedSignals += 'ILRepack signature'
            }
        }
    }

    # Also check raw bytes for Costura resource pattern
    if ($hasDotNetMetadata -and $fileSize -gt 1024) {
        $rawText = [System.Text.Encoding]::UTF8.GetString($bytes)
        if ($rawText -match 'costura\..*\.dll\.compressed') {
            $embeddedSuspected = $true
            if ('Costura.Fody resources' -notin $embeddedSignals) {
                $embeddedSignals += 'Costura.Fody resources'
            }
        }
    }

    # Check via reflection metadata: types in namespaces that don't match assembly name (ILMerge indicator)
    try {
        Add-Type -AssemblyName "System.Reflection.Metadata" -ErrorAction Stop
        Add-Type -AssemblyName "System.Reflection.PortableExecutable" -ErrorAction Stop

        $stream2 = [System.IO.File]::OpenRead($targetFile)
        $peReader2 = [System.Reflection.PortableExecutable.PEReader]::new($stream2)
        $mdReader2 = $peReader2.GetMetadataReader()

        $asmName = [System.IO.Path]::GetFileNameWithoutExtension($targetFile)
        $foreignNamespaces = @{}
        foreach ($typeHandle in $mdReader2.TypeDefinitions) {
            $typeDef = $mdReader2.GetTypeDefinition($typeHandle)
            $ns = $mdReader2.GetString($typeDef.Namespace)
            if ($ns -and $ns -ne '' -and -not $ns.StartsWith($asmName) -and -not $ns.StartsWith('System') -and
                -not $ns.StartsWith('Microsoft') -and -not $ns.StartsWith('<') -and -not $ns.StartsWith('Internal')) {
                $foreignNamespaces[$ns] = $true
            }
        }
        if ($foreignNamespaces.Count -gt 3) {
            $embeddedSuspected = $true
            $embeddedSignals += "Foreign namespaces detected ($($foreignNamespaces.Count)): possible ILMerge/embedded assemblies"
        }

        $peReader2.Dispose()
        $stream2.Dispose()
    }
    catch {
        # Reflection not available — skip this check
    }
}

# ── Determine next phase ────────────────────────────────────────────────────

$nextPhase = switch ($kind) {
    'managed'       { if ($obfuscator -ne 'none') { 'dotnet_deobfuscate' } else { 'direct_decompile' } }
    'unity_il2cpp'  { 'il2cpp_recover' }
    'unity_mono'    { 'direct_decompile' }
    'native'        { 'native_decompile' }
    default         { 'unknown' }
}

# ── ConfuserEx heuristic fallback (when DiE missed it) ─────────────────────

if ($kind -eq 'managed' -and $obfuscator -eq 'none') {
    $cfxResult = Test-ConfuserExHeuristic -Target $targetFile -AllStringsPath $allStringsFile
    if ($cfxResult.IsConfuserEx) {
        $obfuscator = "ConfuserEx (heuristic, $($cfxResult.Confidence) confidence)"
        $obfuscatorFeatures = @("unknown_features")
        $nextPhase = "dotnet_deobfuscate"
        Log "ConfuserEx detected by heuristic: $($cfxResult.Details -join '; ')"
    }
}

# ── Build recon.json ─────────────────────────────────────────────────────────

$recon = [ordered]@{
    path                         = $targetFile
    size_bytes                   = $fileSize
    sha256                       = $sha256
    kind                         = $kind
    runtime                      = $runtime
    compiler                     = $compiler
    obfuscator                   = $obfuscator
    obfuscator_features          = $obfuscatorFeatures
    packed                       = $packed
    embedded_assemblies_suspected = $embeddedSuspected
    embedded_signals             = $embeddedSignals
    next_phase                   = $nextPhase
}

$reconPath = Join-Path $OutputDir 'recon.json'
$recon | ConvertTo-Json -Depth 5 | Out-File -FilePath $reconPath -Encoding utf8
Log "Recon written to $reconPath"

# ── Machine-readable output lines ────────────────────────────────────────────

Write-Output "RECON_KIND:$kind"
Write-Output "RECON_OBFUSCATOR:$obfuscator"
Write-Output "RECON_NEXT:$nextPhase"

if (-not $dieAvailable) {
    Write-Output "RECON_FALLBACK:heuristics_only"
}

Log "Recon complete. Kind=$kind, Obfuscator=$obfuscator, Next=$nextPhase"

exit $script:ExitCode
