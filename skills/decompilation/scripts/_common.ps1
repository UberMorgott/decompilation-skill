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
