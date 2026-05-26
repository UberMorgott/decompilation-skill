<#
.SYNOPSIS
    Shared helper functions for decompilation pipeline scripts.
.DESCRIPTION
    Provides Log, Test-ToolAvailable, and Invoke-PipelineStep used across all pipeline scripts.
    Dot-source this file at the top of each script after setting $script:PipelineLog.
#>

function Log {
    param([string]$Message)
    $tag = if ($script:LogTag) { $script:LogTag } else { 'pipeline' }
    $line = "$([DateTime]::Now.ToString('HH:mm:ss')) [$tag] $Message"
    Write-Host $line
    if ($script:PipelineLog) {
        $retries = 3
        while ($retries -gt 0) {
            try {
                Add-Content -Path $script:PipelineLog -Value $line -ErrorAction Stop
                break
            } catch {
                $retries--
                Start-Sleep -Milliseconds 50
            }
        }
    }
}

function Test-ToolAvailable {
    param([string]$Name)
    $null -ne (Get-Command $Name -ErrorAction SilentlyContinue)
}

function Invoke-PipelineStep {
    param(
        [string]$Name,
        [scriptblock]$Action
    )
    $script:StepNumber++
    $stepLabel = "Step $($script:StepNumber): $Name"
    Log "── $stepLabel ──"
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    $result = [ordered]@{ step = $script:StepNumber; name = $Name; status = 'unknown'; exit_code = -1; duration_ms = 0; error = $null }

    try {
        & $Action
        $result.status = 'success'
        $result.exit_code = 0
        Log "$stepLabel completed successfully"
    } catch {
        $result.status = 'failed'
        $result.exit_code = 1
        $result.error = $_.Exception.Message
        Log "$stepLabel FAILED: $($_.Exception.Message)"
    }

    $sw.Stop()
    $result.duration_ms = $sw.ElapsedMilliseconds
    Log "$stepLabel duration: $($sw.ElapsedMilliseconds)ms"
    $script:StepResults += [PSCustomObject]$result
}

function Ensure-Tool {
    <#
    .SYNOPSIS
        Locate a tool binary in PATH or the centralized tools directory.
    .DESCRIPTION
        Checks system PATH and $env:USERPROFILE\.claude\tools\decompilation\ (recursively).
        Returns the full path to the executable if found, or $null with a suggestion.
    .PARAMETER Name
        The executable name (e.g. 'diec', 'ilspycmd'). Extension optional on Windows.
    #>
    param(
        [Parameter(Mandatory)]
        [string]$Name
    )

    $toolsRoot = Join-Path $env:USERPROFILE '.claude\tools\decompilation'

    # Normalize: add .exe if no extension on Windows
    $binName = if ($Name -notmatch '\.\w+$') { "$Name.exe" } else { $Name }

    # 1. Check system PATH
    $cmd = Get-Command $binName -ErrorAction SilentlyContinue
    if (-not $cmd) { $cmd = Get-Command $Name -ErrorAction SilentlyContinue }
    if ($cmd) { return $cmd.Source }

    # 2. Check centralized tools directory
    if (Test-Path $toolsRoot) {
        $found = Get-ChildItem -Path $toolsRoot -Filter $binName -Recurse -ErrorAction SilentlyContinue |
            Select-Object -First 1
        if (-not $found) {
            $found = Get-ChildItem -Path $toolsRoot -Filter $Name -Recurse -ErrorAction SilentlyContinue |
                Select-Object -First 1
        }
        if ($found) {
            # Add its directory to PATH for this session
            $dir = Split-Path $found.FullName -Parent
            if ($dir -notin ($env:PATH -split ';')) {
                $env:PATH = "$dir;$env:PATH"
            }
            return $found.FullName
        }
    }

    # 3. Not found — suggest install
    $installScript = Join-Path $PSScriptRoot 'install-tools.ps1'
    Log "WARN: Tool '$Name' not found. Run:  $installScript -Tool $Name"
    return $null
}

function Extract-Strings {
    <#
    .SYNOPSIS
        Extract strings from a binary and produce filtered subsets.
    .DESCRIPTION
        Runs Sysinternals strings (or a PowerShell fallback) on the target binary,
        then filters the result into url_candidates.txt, error_messages.txt, and
        format_strings.txt inside the given StringsDir.
    .PARAMETER Target
        Path to the binary file to extract strings from.
    .PARAMETER StringsDir
        Directory where all_strings.txt and filtered files will be written.
    #>
    param(
        [Parameter(Mandatory)][string]$Target,
        [Parameter(Mandatory)][string]$StringsDir
    )

    if (-not (Test-Path $StringsDir)) {
        New-Item -ItemType Directory -Path $StringsDir -Force | Out-Null
    }

    $allStringsFile = Join-Path $StringsDir 'all_strings.txt'

    Log "Extracting strings..."
    try {
        if (Test-ToolAvailable 'strings') {
            & strings -accepteula -n 6 $Target | Out-File -FilePath $allStringsFile -Encoding utf8
        } else {
            # PowerShell fallback: extract ASCII and Unicode printable strings (min length 6)
            $content = [System.IO.File]::ReadAllBytes($Target)

            $asciiStrings = [System.Text.Encoding]::ASCII.GetString($content) -split '[^\x20-\x7E]+' |
                Where-Object { $_.Length -ge 6 }

            $unicodeStrings = [System.Text.Encoding]::Unicode.GetString($content) -split '[^\x20-\x7E]+' |
                Where-Object { $_.Length -ge 6 }

            ($asciiStrings + $unicodeStrings) | Sort-Object -Unique | Out-File -FilePath $allStringsFile -Encoding utf8
        }
        Log "Strings extracted to $allStringsFile"

        # Filter URL candidates
        Get-Content $allStringsFile -ErrorAction SilentlyContinue |
            Where-Object { $_ -match '^https?:|^/' } |
            Out-File -FilePath (Join-Path $StringsDir 'url_candidates.txt') -Encoding utf8

        # Filter error messages
        Get-Content $allStringsFile -ErrorAction SilentlyContinue |
            Where-Object { $_ -match '(?i)(error|exception|fail|invalid|cannot|unable|denied|timeout|unauthorized)' } |
            Out-File -FilePath (Join-Path $StringsDir 'error_messages.txt') -Encoding utf8

        # Filter format strings ({0}, {1}, %s, %d, etc.)
        $formatResults = @(Get-Content $allStringsFile -ErrorAction SilentlyContinue |
            Select-String -Pattern '\{[0-9]+\}|%[sdfu]' |
            ForEach-Object { $_.Line })
        $formatResults | Out-File -FilePath (Join-Path $StringsDir 'format_strings.txt') -Encoding utf8

    } catch {
        Log "WARNING: Strings extraction failed: $_"
    }
}
