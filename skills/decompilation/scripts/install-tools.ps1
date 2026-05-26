<#
.SYNOPSIS
    Auto-install/update decompilation tools.
.DESCRIPTION
    Downloads and installs required, recommended, and optional tools for the
    decompilation pipeline. Supports GitHub releases, dotnet tools, pip packages,
    and direct URL downloads.
.PARAMETER Tool
    Install or update a specific tool by name.
.PARAMETER All
    Install all required and recommended tools.
.PARAMETER Check
    Report installation status of all tools without installing anything.
.PARAMETER Force
    Force reinstall even if tool is already present.
#>
[CmdletBinding(DefaultParameterSetName = 'Check')]
param(
    [Parameter(ParameterSetName = 'Single', Mandatory)]
    [string]$Tool,

    [Parameter(ParameterSetName = 'All')]
    [switch]$All,

    [Parameter(ParameterSetName = 'Check')]
    [switch]$Check,

    [switch]$Force
)

$ErrorActionPreference = 'Stop'

# ── Paths ────────────────────────────────────────────────────────────────────

$ToolsRoot = Join-Path $env:USERPROFILE '.claude\tools\decompilation'
if (-not (Test-Path $ToolsRoot)) {
    New-Item -ItemType Directory -Path $ToolsRoot -Force | Out-Null
}

$TempDir = Join-Path $env:TEMP 'claude-tool-install'
if (-not (Test-Path $TempDir)) {
    New-Item -ItemType Directory -Path $TempDir -Force | Out-Null
}

# ── Tool Registry ────────────────────────────────────────────────────────────

$ToolRegistry = [ordered]@{

    # === Required ===
    diec = @{
        Category    = 'Required'
        Repo        = 'horsicq/DIE-engine'
        Binary      = 'diec.exe'
        Method      = 'github-release'
        AssetFilter = '*win64*portable*.zip'
        Description = 'Detect It Easy CLI'
    }
    ilspycmd = @{
        Category    = 'Required'
        Binary      = 'ilspycmd.exe'
        Method      = 'dotnet-tool'
        DotnetId    = 'ilspycmd'
        Description = 'ILSpy command-line decompiler'
    }
    strings = @{
        Category    = 'Required'
        Binary      = 'strings.exe'
        Method      = 'direct-url'
        Url         = 'https://download.sysinternals.com/files/Strings.zip'
        Description = 'Sysinternals Strings'
    }

    # === Recommended ===
    'de4dot-cex' = @{
        Category    = 'Recommended'
        Repo        = 'ViRb3/de4dot-cex'
        Binary      = 'de4dot.exe'
        Method      = 'github-release'
        AssetFilter = '*.zip'
        Description = 'de4dot fork for ConfuserEx'
    }
    ghidra = @{
        Category    = 'Recommended'
        Repo        = 'NationalSecurityAgency/ghidra'
        Binary      = 'ghidraRun.bat'
        Method      = 'github-release'
        AssetFilter = '*ghidra*.zip'
        Description = 'Ghidra reverse engineering suite'
    }
    Il2CppDumper = @{
        Category    = 'Recommended'
        Repo        = 'Perfare/Il2CppDumper'
        Binary      = 'Il2CppDumper.exe'
        Method      = 'github-release'
        AssetFilter = '*win*'
        Description = 'Unity IL2CPP metadata dumper'
    }
    Cpp2IL = @{
        Category    = 'Recommended'
        Repo        = 'SamboyCoding/Cpp2IL'
        Binary      = 'Cpp2IL.exe'
        Method      = 'github-release'
        AssetFilter = '*Windows*'
        Description = 'IL2CPP reverse engineering tool'
    }

    # === ConfuserEx Tools ===
    NoFuserEx = @{
        Category    = 'ConfuserEx'
        Repo        = 'XenocodeRCE/NoFuserEx'
        Binary      = 'NoFuserEx.exe'
        Method      = 'github-release'
        AssetFilter = '*.zip'
        Description = 'ConfuserEx control-flow cleaner'
    }
    'ConfuserEx-Unpacker-2' = @{
        Category    = 'ConfuserEx'
        Repo        = 'cawk/ConfuserEx-Unpacker-2'
        Binary      = 'ConfuserEx-Unpacker-2.exe'
        Method      = 'manual'
        Description = 'ConfuserEx unpacker (build from source)'
    }
    'ProxyCall-Remover' = @{
        Category    = 'ConfuserEx'
        Repo        = 'Kaidoz/ProxyCall-Remover'
        Binary      = 'ProxyCall-Remover.exe'
        Method      = 'github-release'
        AssetFilter = '*.zip'
        Description = 'ConfuserEx proxy call remover'
    }

    # === Optional ===
    GoReSym = @{
        Category    = 'Optional'
        Repo        = 'mandiant/GoReSym'
        Binary      = 'GoReSym.exe'
        Method      = 'github-release'
        AssetFilter = '*windows*'
        Description = 'Go binary symbol recovery'
    }
    NetReactorSlayer = @{
        Category    = 'Optional'
        Repo        = 'SychicBoy/NETReactorSlayer'
        Binary      = 'NETReactorSlayer.CLI.exe'
        Method      = 'github-release'
        AssetFilter = '*.zip'
        Description = '.NET Reactor deobfuscator'
    }
    OldRod = @{
        Category    = 'Optional'
        Repo        = 'Washi1337/OldRod'
        Binary      = 'OldRod.exe'
        Method      = 'github-release'
        AssetFilter = '*.zip'
        Description = 'KoiVM devirtualizer'
    }
    AssetRipper = @{
        Category    = 'Optional'
        Repo        = 'AssetRipper/AssetRipper'
        Binary      = 'AssetRipper.exe'
        Method      = 'github-release'
        AssetFilter = '*win*x64*'
        Description = 'Unity asset extraction tool'
    }
    frida = @{
        Category    = 'Optional'
        Binary      = 'frida.exe'
        Method      = 'pip'
        PipPackage  = 'frida-tools'
        Description = 'Dynamic instrumentation toolkit'
    }
    mitmproxy = @{
        Category    = 'Optional'
        Binary      = 'mitmproxy.exe'
        Method      = 'pip'
        PipPackage  = 'mitmproxy'
        Description = 'HTTP/HTTPS proxy for interception'
    }
}

# ── Helper Functions ─────────────────────────────────────────────────────────

function Get-ToolPath {
    param([string]$Name, [hashtable]$Info)

    # Check tools root (and subdirectories)
    $localPath = Get-ChildItem -Path $ToolsRoot -Filter $Info.Binary -Recurse -ErrorAction SilentlyContinue |
        Select-Object -First 1
    if ($localPath) { return $localPath.FullName }

    # Check system PATH
    $cmd = Get-Command $Info.Binary -ErrorAction SilentlyContinue
    if ($cmd) { return $cmd.Source }

    return $null
}

function Get-InstalledVersion {
    param([string]$Path, [string]$Name)

    $versionFile = Join-Path (Split-Path $Path -Parent) '.version'
    if (Test-Path $versionFile) {
        return (Get-Content $versionFile -Raw).Trim()
    }
    return 'unknown'
}

function Get-LatestGitHubRelease {
    param([string]$Repo)

    $apiUrl = "https://api.github.com/repos/$Repo/releases/latest"
    try {
        $headers = @{ 'User-Agent' = 'claude-decompiler-installer' }
        if ($env:GITHUB_TOKEN) {
            $headers['Authorization'] = "Bearer $env:GITHUB_TOKEN"
        }
        $release = Invoke-RestMethod -Uri $apiUrl -Headers $headers -TimeoutSec 15
        return $release
    } catch {
        Write-Warning "Failed to fetch release info for $Repo : $_"
        return $null
    }
}

function Select-Asset {
    param($Release, [string]$Filter)

    foreach ($asset in $Release.assets) {
        if ($asset.name -like $Filter) {
            return $asset
        }
    }
    # Fallback: try broader match
    foreach ($asset in $Release.assets) {
        if ($asset.name -like '*.zip' -or $asset.name -like '*.exe') {
            return $asset
        }
    }
    return $null
}

function Install-FromGitHubRelease {
    param([string]$Name, [hashtable]$Info)

    $release = Get-LatestGitHubRelease -Repo $Info.Repo
    if (-not $release) {
        Write-Host "  ERROR: Could not fetch release for $($Info.Repo)" -ForegroundColor Red
        return $false
    }

    $asset = Select-Asset -Release $release -Filter $Info.AssetFilter
    if (-not $asset) {
        Write-Host "  ERROR: No matching asset found (filter: $($Info.AssetFilter))" -ForegroundColor Red
        Write-Host "  Available assets:" -ForegroundColor Yellow
        foreach ($a in $release.assets) { Write-Host "    - $($a.name)" }
        return $false
    }

    $version = $release.tag_name
    $destDir = Join-Path $ToolsRoot $Name
    $downloadPath = Join-Path $TempDir $asset.name

    Write-Host "  Downloading $($asset.name) ($version)..." -ForegroundColor Cyan

    $headers = @{ 'User-Agent' = 'claude-decompiler-installer' }
    if ($env:GITHUB_TOKEN) {
        $headers['Authorization'] = "Bearer $env:GITHUB_TOKEN"
    }
    Invoke-WebRequest -Uri $asset.browser_download_url -OutFile $downloadPath -Headers $headers

    # Create destination
    if (Test-Path $destDir) { Remove-Item -Path $destDir -Recurse -Force }
    New-Item -ItemType Directory -Path $destDir -Force | Out-Null

    # Extract or copy
    if ($downloadPath -match '\.zip$') {
        Write-Host "  Extracting to $destDir..." -ForegroundColor Cyan
        Expand-Archive -Path $downloadPath -DestinationPath $destDir -Force

        # If archive contains a single subfolder, flatten it
        $children = Get-ChildItem -Path $destDir
        if ($children.Count -eq 1 -and $children[0].PSIsContainer) {
            $subDir = $children[0].FullName
            Get-ChildItem -Path $subDir | Move-Item -Destination $destDir -Force
            Remove-Item -Path $subDir -Force
        }
    } else {
        Copy-Item -Path $downloadPath -Destination $destDir -Force
    }

    # Write version marker
    $version | Set-Content -Path (Join-Path $destDir '.version') -NoNewline

    # Cleanup temp
    Remove-Item -Path $downloadPath -Force -ErrorAction SilentlyContinue

    Write-Host "  Installed $Name $version to $destDir" -ForegroundColor Green
    return $true
}

function Install-FromDirectUrl {
    param([string]$Name, [hashtable]$Info)

    $destDir = Join-Path $ToolsRoot $Name
    $fileName = [System.IO.Path]::GetFileName($Info.Url)
    $downloadPath = Join-Path $TempDir $fileName

    Write-Host "  Downloading from $($Info.Url)..." -ForegroundColor Cyan
    Invoke-WebRequest -Uri $Info.Url -OutFile $downloadPath

    if (Test-Path $destDir) { Remove-Item -Path $destDir -Recurse -Force }
    New-Item -ItemType Directory -Path $destDir -Force | Out-Null

    if ($downloadPath -match '\.zip$') {
        Expand-Archive -Path $downloadPath -DestinationPath $destDir -Force
    } else {
        Copy-Item -Path $downloadPath -Destination $destDir -Force
    }

    # Write version marker with date
    "downloaded-$(Get-Date -Format 'yyyy-MM-dd')" | Set-Content -Path (Join-Path $destDir '.version') -NoNewline

    Remove-Item -Path $downloadPath -Force -ErrorAction SilentlyContinue

    Write-Host "  Installed $Name to $destDir" -ForegroundColor Green
    return $true
}

function Install-DotnetTool {
    param([string]$Name, [hashtable]$Info)

    $dotnetId = if ($Info.DotnetId) { $Info.DotnetId } else { $Name }

    Write-Host "  Installing dotnet tool: $dotnetId..." -ForegroundColor Cyan
    try {
        & dotnet tool install -g $dotnetId 2>&1 | ForEach-Object { Write-Host "    $_" }
        if ($LASTEXITCODE -ne 0) {
            # Try update if already installed
            & dotnet tool update -g $dotnetId 2>&1 | ForEach-Object { Write-Host "    $_" }
        }
        Write-Host "  Installed $dotnetId via dotnet tool" -ForegroundColor Green
        return $true
    } catch {
        Write-Host "  ERROR: dotnet tool install failed: $_" -ForegroundColor Red
        return $false
    }
}

function Install-PipPackage {
    param([string]$Name, [hashtable]$Info)

    $pkg = if ($Info.PipPackage) { $Info.PipPackage } else { $Name }

    Write-Host "  Installing pip package: $pkg..." -ForegroundColor Cyan
    try {
        & pip install --upgrade $pkg 2>&1 | ForEach-Object { Write-Host "    $_" }
        Write-Host "  Installed $pkg via pip" -ForegroundColor Green
        return $true
    } catch {
        Write-Host "  ERROR: pip install failed: $_" -ForegroundColor Red
        return $false
    }
}

function Install-Tool {
    param([string]$Name)

    if (-not $ToolRegistry.Contains($Name)) {
        Write-Host "ERROR: Unknown tool '$Name'. Use -Check to see available tools." -ForegroundColor Red
        return $false
    }

    $info = $ToolRegistry[$Name]
    Write-Host "`nInstalling: $Name ($($info.Description))" -ForegroundColor White

    # Check if already installed (unless -Force)
    if (-not $Force) {
        $existing = Get-ToolPath -Name $Name -Info $info
        if ($existing) {
            Write-Host "  Already installed at: $existing" -ForegroundColor Yellow
            Write-Host "  Use -Force to reinstall" -ForegroundColor Yellow
            return $true
        }
    }

    switch ($info.Method) {
        'github-release' { return Install-FromGitHubRelease -Name $Name -Info $info }
        'direct-url'     { return Install-FromDirectUrl -Name $Name -Info $info }
        'dotnet-tool'    { return Install-DotnetTool -Name $Name -Info $info }
        'pip'            { return Install-PipPackage -Name $Name -Info $info }
        'manual' {
            Write-Host "  MANUAL: $Name must be built from source." -ForegroundColor Yellow
            Write-Host "  Repo: https://github.com/$($info.Repo)" -ForegroundColor Yellow
            Write-Host "  Clone, build, and place binary in: $(Join-Path $ToolsRoot $Name)" -ForegroundColor Yellow
            return $false
        }
        default {
            Write-Host "  ERROR: Unknown install method '$($info.Method)'" -ForegroundColor Red
            return $false
        }
    }
}

function Show-Status {
    Write-Host "`n═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host "  Decompilation Tools Status Report" -ForegroundColor Cyan
    Write-Host "  Tools directory: $ToolsRoot" -ForegroundColor DarkGray
    Write-Host "═══════════════════════════════════════════════════════════`n" -ForegroundColor Cyan

    $currentCategory = ''

    foreach ($name in $ToolRegistry.Keys) {
        $info = $ToolRegistry[$name]

        if ($info.Category -ne $currentCategory) {
            $currentCategory = $info.Category
            Write-Host "── $currentCategory ──" -ForegroundColor White
        }

        $path = Get-ToolPath -Name $name -Info $info
        if ($path) {
            $version = Get-InstalledVersion -Path $path -Name $name

            # For GitHub tools, check if update is available
            if ($info.Method -eq 'github-release' -and $info.Repo) {
                $release = Get-LatestGitHubRelease -Repo $info.Repo
                $latest = if ($release) { $release.tag_name } else { 'unknown' }

                if ($latest -ne 'unknown' -and $version -ne 'unknown' -and $version -ne $latest) {
                    Write-Host "  OUTDATED  $name  installed=$version  latest=$latest" -ForegroundColor Yellow
                    Write-Output "TOOL_OUTDATED:${name}:${version}:${latest}"
                } else {
                    Write-Host "  OK        $name  $version" -ForegroundColor Green
                    Write-Output "TOOL_OK:${name}:${version}"
                }
            } else {
                Write-Host "  OK        $name  $version" -ForegroundColor Green
                Write-Output "TOOL_OK:${name}:${version}"
            }
        } else {
            if ($info.Method -eq 'manual') {
                Write-Host "  MANUAL    $name  (build from source)" -ForegroundColor DarkYellow
                Write-Output "TOOL_MISSING:${name}"
            } else {
                Write-Host "  MISSING   $name" -ForegroundColor Red
                Write-Output "TOOL_MISSING:${name}"
            }
        }
    }

    Write-Host ""
}

# ── Main ─────────────────────────────────────────────────────────────────────

# Ensure tools root is in PATH for this session
$toolDirs = Get-ChildItem -Path $ToolsRoot -Directory -ErrorAction SilentlyContinue |
    ForEach-Object { $_.FullName }
$toolDirs += $ToolsRoot
foreach ($d in $toolDirs) {
    if ($env:PATH -notlike "*$d*") {
        $env:PATH = "$d;$env:PATH"
    }
}

switch ($PSCmdlet.ParameterSetName) {
    'Check' {
        Show-Status
    }
    'Single' {
        $result = Install-Tool -Name $Tool
        if ($result) { exit 0 } else { exit 1 }
    }
    'All' {
        $failed = @()
        $installed = @()

        $targets = $ToolRegistry.Keys | Where-Object {
            $ToolRegistry[$_].Category -in @('Required', 'Recommended', 'ConfuserEx')
        }

        foreach ($name in $targets) {
            $result = Install-Tool -Name $name
            if ($result) { $installed += $name } else { $failed += $name }
        }

        Write-Host "`n═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
        Write-Host "  Summary" -ForegroundColor Cyan
        Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
        Write-Host "  Installed: $($installed.Count)" -ForegroundColor Green
        if ($failed.Count -gt 0) {
            Write-Host "  Failed:    $($failed -join ', ')" -ForegroundColor Red
        }
        Write-Host ""

        if ($failed.Count -gt 0) { exit 1 } else { exit 0 }
    }
}
