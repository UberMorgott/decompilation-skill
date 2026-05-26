<#
.SYNOPSIS
    Generate metadata indexes from decompiled C# source files.
.DESCRIPTION
    Parses decompiled .cs files using regex to build:
    - index.json (types, methods, fields, files)
    - api_surface.json (HTTP endpoints with routes, params, returns)
    - callgraph.json (caller -> callees adjacency list)
    - api_surface.md (human-readable summary)
.PARAMETER SourceDir
    Path to the decompiled src/ folder containing .cs files.
.PARAMETER OutputDir
    Path to the metadata/ output directory.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$SourceDir,

    [Parameter(Mandatory)]
    [string]$OutputDir
)

$ErrorActionPreference = 'Continue'
$script:PipelineLog = Join-Path (Split-Path $OutputDir -Parent) 'pipeline.log'
if (-not (Test-Path (Split-Path $script:PipelineLog -Parent))) {
    $script:PipelineLog = Join-Path $OutputDir 'build-indexes.log'
}
$script:LogTag = 'build-indexes'

. (Join-Path $PSScriptRoot '_common.ps1')

# ── Setup ────────────────────────────────────────────────────────────────────

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

if (-not (Test-Path $SourceDir)) {
    Log "ERROR: Source directory not found: $SourceDir"
    Write-Output "INDEX_ERROR:source_not_found"
    exit 1
}

$csFiles = Get-ChildItem -Path $SourceDir -Filter '*.cs' -Recurse -ErrorAction SilentlyContinue
if (-not $csFiles -or $csFiles.Count -eq 0) {
    Log "WARNING: No .cs files found in $SourceDir"
    Write-Output "INDEX_WARNING:no_cs_files"
    exit 0
}

Log "Building indexes from $($csFiles.Count) C# files in $SourceDir"

# ── Parse types, methods, fields ─────────────────────────────────────────────

$types = @()
$controllers = @()
$callGraph = @{}

$namespaceRegex = '^\s*namespace\s+([\w.]+)'
$classRegex = '^\s*(?:public|internal|private|protected)?\s*(?:static|abstract|sealed|partial)?\s*(?:class|struct|interface|enum|record)\s+(\w+)(?:\s*<[^>]+>)?(?:\s*:\s*(.+))?'
$methodRegex = '^\s*(?:public|internal|private|protected)?\s*(?:static|virtual|override|abstract|async|new)?\s*(?:static|virtual|override|abstract|async|new)?\s*([\w<>\[\],\s?]+?)\s+(\w+)\s*(?:<[^>]+>)?\s*\(([^)]*)\)'
$fieldRegex = '^\s*(?:public|internal|private|protected)?\s*(?:static|readonly|const|volatile)?\s*(?:static|readonly|const|volatile)?\s*([\w<>\[\],\s?]+?)\s+(\w+)\s*[;=]'
$attributeRegex = '^\s*\[(\w+)(?:\(([^)]*)\))?\]'

# HTTP attribute patterns
$httpMethodAttrs = @('HttpGet', 'HttpPost', 'HttpPut', 'HttpDelete', 'HttpPatch', 'HttpHead', 'HttpOptions')
$routeRegex = '\[Route\("([^"]+)"\)\]'

foreach ($file in $csFiles) {
    $relativePath = $file.FullName.Substring($SourceDir.Length).TrimStart('\', '/')
    $lines = Get-Content $file.FullName -ErrorAction SilentlyContinue
    if (-not $lines) { continue }

    $currentNamespace = ''
    $currentType = $null
    $currentMethods = @()
    $currentFields = @()
    $currentInterfaces = @()
    $currentBase = 'object'
    $pendingAttributes = @()
    $isController = $false
    $routePrefix = ''
    $currentEndpoints = @()
    $currentMethodName = ''

    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        $lineNum = $i + 1

        # Namespace
        if ($line -match $namespaceRegex) {
            $currentNamespace = $Matches[1]
            continue
        }

        # Attributes (accumulate for next declaration)
        if ($line -match $attributeRegex) {
            $attrName = $Matches[1]
            $attrArgs = if ($Matches[2]) { $Matches[2] } else { '' }
            $pendingAttributes += [ordered]@{ name = $attrName; args = $attrArgs }

            # Route prefix at class level
            if ($attrName -eq 'Route') {
                $routePrefix = $attrArgs -replace '"','' -replace '\[controller\]','*'
            }
            if ($attrName -eq 'ApiController') { $isController = $true }
            continue
        }

        # Class/struct/interface/enum
        if ($line -match $classRegex) {
            # Save previous type if exists
            if ($currentType) {
                $fullName = if ($currentNamespace) { "$currentNamespace.$currentType" } else { $currentType }
                $typeEntry = [ordered]@{
                    name       = $fullName
                    file       = $relativePath
                    kind       = 'class'
                    base       = $currentBase
                    interfaces = $currentInterfaces
                    methods    = $currentMethods
                    fields     = $currentFields
                }
                $types += $typeEntry

                if ($isController -and $currentEndpoints.Count -gt 0) {
                    $controllers += [ordered]@{
                        type         = $fullName
                        file         = $relativePath
                        route_prefix = $routePrefix
                        endpoints    = $currentEndpoints
                    }
                }
            }

            $currentType = $Matches[1]
            $inheritance = if ($Matches[2]) { [string]$Matches[2] -replace '^\s+|\s+$' } else { '' }

            # Parse base class and interfaces
            if ($inheritance) {
                $parts = $inheritance -split ',' | ForEach-Object { $_.Trim() }
                $currentBase = $parts[0]
                $currentInterfaces = @($parts | Select-Object -Skip 1)
            } else {
                $currentBase = 'object'
                $currentInterfaces = @()
            }

            $currentMethods = @()
            $currentFields = @()
            $currentEndpoints = @()

            # Check if this looks like an API controller
            if ($currentType -match 'Controller$' -or $isController -or ($pendingAttributes | Where-Object { $_.name -eq 'ApiController' })) {
                $isController = $true
            }

            $pendingAttributes = @()
            continue
        }

        # Methods
        if ($line -match $methodRegex) {
            $returnType = "$($Matches[1])".Trim()
            $methodName = $Matches[2]
            $params = "$($Matches[3])".Trim()
            $currentMethodName = $methodName

            # Skip property getters/setters and constructors that look like methods
            if ($methodName -eq 'get' -or $methodName -eq 'set' -or $methodName -eq 'add' -or $methodName -eq 'remove') {
                continue
            }

            $signature = "$returnType $methodName($params)"
            $currentMethods += [ordered]@{
                name      = $methodName
                signature = $signature
                line      = $lineNum
            }

            # Check for HTTP endpoint attributes
            if ($isController) {
                $httpMethods = @()
                $endpointRoute = ''
                $endpointParams = @()
                $endpointAttrs = @()

                foreach ($attr in $pendingAttributes) {
                    if ($attr.name -in $httpMethodAttrs) {
                        $httpMethods += $attr.name -replace '^Http','' | ForEach-Object { $_.ToUpper() }
                        if ($attr.args) { $endpointRoute = $attr.args -replace '"','' }
                    }
                    if ($attr.name -match 'Authorize|AllowAnonymous') {
                        $endpointAttrs += "$($attr.name)$(if ($attr.args) { "($($attr.args))" })"
                    }
                }

                if ($httpMethods.Count -gt 0) {
                    # Parse parameters
                    if ($params) {
                        $paramList = $params -split ',' | ForEach-Object {
                            $p = $_.Trim()
                            $fromBinding = 'Query'
                            if ($p -match '\[From(\w+)\]') {
                                $fromBinding = $Matches[1]
                                $p = $p -replace '\[From\w+\]\s*',''
                            }
                            $pParts = $p -split '\s+' | Where-Object { $_ }
                            if ($pParts.Count -ge 2) {
                                [ordered]@{
                                    name = $pParts[-1]
                                    type = ($pParts[0..($pParts.Count-2)] -join ' ')
                                    from = $fromBinding
                                }
                            }
                        } | Where-Object { $_ }
                    } else {
                        $paramList = @()
                    }

                    $currentEndpoints += [ordered]@{
                        method       = $methodName
                        http_methods = $httpMethods
                        route        = $endpointRoute
                        parameters   = @($paramList)
                        returns      = $returnType
                        attributes   = $endpointAttrs
                    }
                }
            }

            $pendingAttributes = @()
            continue
        }

        # Fields
        if ($line -match $fieldRegex -and $currentType) {
            $fieldType = "$($Matches[1])".Trim()
            $fieldName = $Matches[2]

            # Skip common noise
            if ($fieldName -notmatch '^(get_|set_|add_|remove_)') {
                $currentFields += [ordered]@{
                    name = $fieldName
                    type = $fieldType
                }
            }
            $pendingAttributes = @()
            continue
        }

        # Call graph: detect method calls within method bodies
        if ($currentMethodName -and $currentType -and $line -match '(\w+)\s*(?:<[^>]+>)?\s*\(') {
            $callee = $Matches[1]
            # Skip C# keywords and very short names
            $csharpKeywords = @('if','else','for','foreach','while','do','switch','case','return','throw','try','catch','finally','lock','using','new','typeof','sizeof','nameof','default','checked','unchecked','await','yield')
            if ($callee -notin $csharpKeywords -and $callee.Length -gt 1) {
                $callerKey = if ($currentNamespace) { "$currentNamespace.$currentType.$currentMethodName" } else { "$currentType.$currentMethodName" }
                if (-not $callGraph.ContainsKey($callerKey)) {
                    $callGraph[$callerKey] = @()
                }
                if ($callee -notin $callGraph[$callerKey]) {
                    $callGraph[$callerKey] += $callee
                }
            }
        }

        # Reset pending attributes on non-attribute, non-blank lines
        if ($line.Trim() -and $line -notmatch '^\s*\[' -and $line -notmatch '^\s*//' -and $line -notmatch '^\s*$') {
            # Only reset if the line is not part of a continued attribute
            if ($line -notmatch '^\s*\]') {
                $pendingAttributes = @()
            }
        }
    }

    # Save last type in file
    if ($currentType) {
        $fullName = if ($currentNamespace) { "$currentNamespace.$currentType" } else { $currentType }
        $typeEntry = [ordered]@{
            name       = $fullName
            file       = $relativePath
            kind       = 'class'
            base       = $currentBase
            interfaces = $currentInterfaces
            methods    = $currentMethods
            fields     = $currentFields
        }
        $types += $typeEntry

        if ($isController -and $currentEndpoints.Count -gt 0) {
            $controllers += [ordered]@{
                type         = $fullName
                file         = $relativePath
                route_prefix = $routePrefix
                endpoints    = $currentEndpoints
            }
        }
    }
}

Log "Parsed $($types.Count) types from $($csFiles.Count) files"

# ── Write index.json ─────────────────────────────────────────────────────────

$indexJson = [ordered]@{ types = $types }
$indexPath = Join-Path $OutputDir 'index.json'
$indexJson | ConvertTo-Json -Depth 10 | Out-File -FilePath $indexPath -Encoding utf8
Log "index.json written ($($types.Count) types)"

# ── Write api_surface.json ───────────────────────────────────────────────────

$apiSurface = [ordered]@{ controllers = $controllers }
$apiSurfacePath = Join-Path $OutputDir 'api_surface.json'
$apiSurface | ConvertTo-Json -Depth 10 | Out-File -FilePath $apiSurfacePath -Encoding utf8
Log "api_surface.json written ($($controllers.Count) controllers)"

# ── Write callgraph.json ────────────────────────────────────────────────────

$callGraphPath = Join-Path $OutputDir 'callgraph.json'
$callGraph | ConvertTo-Json -Depth 5 | Out-File -FilePath $callGraphPath -Encoding utf8
Log "callgraph.json written ($($callGraph.Count) callers)"

# ── Write callgraph.dot ────────────────────────────────────────────────────

$dotPath = Join-Path $OutputDir 'callgraph.dot'
$dot = [System.Text.StringBuilder]::new()
[void]$dot.AppendLine("digraph callgraph {")
foreach ($caller in $callGraph.Keys) {
    foreach ($callee in $callGraph[$caller]) {
        $escapedCaller = $caller -replace '"','\"'
        $escapedCallee = $callee -replace '"','\"'
        [void]$dot.AppendLine("  `"$escapedCaller`" -> `"$escapedCallee`";")
    }
}
[void]$dot.AppendLine("}")
$dot.ToString() | Out-File -FilePath $dotPath -Encoding utf8
Log "callgraph.dot written"

# ── Write attributes.json ──────────────────────────────────────────────────

$commonKeywords = @('public','private','protected','internal','static','virtual','override','abstract','sealed','partial','readonly','const','volatile','async','new','extern')
$attrResults = @()

foreach ($file in $csFiles) {
    $relativePath = $file.FullName.Substring($SourceDir.Length).TrimStart('\', '/')
    $lines = Get-Content $file.FullName -ErrorAction SilentlyContinue
    if (-not $lines) { continue }

    $currentTypeName = $null
    $typeAttrs = @()

    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]

        # Detect type declarations
        if ($line -match '^\s*(?:public|internal|private|protected)?\s*(?:static|abstract|sealed|partial)?\s*(?:class|struct|interface|enum|record)\s+(\w+)') {
            # Save previous type if it had attributes
            if ($currentTypeName -and $typeAttrs.Count -gt 0) {
                $attrResults += [ordered]@{ name = $currentTypeName; file = $relativePath; attributes = $typeAttrs }
            }
            $currentTypeName = $Matches[1]
            $typeAttrs = @()
        }

        # Collect attributes: [Something(args)]
        if ($line -match '^\s*\[(\w+)(?:\(([^)]*)\))?\]') {
            $attrName = $Matches[1]
            $attrArgs = if ($Matches[2]) { $Matches[2] } else { '' }
            if ($attrName -notin $commonKeywords) {
                $attrEntry = if ($attrArgs) { "$attrName($attrArgs)" } else { $attrName }
                if ($attrEntry -notin $typeAttrs) {
                    $typeAttrs += $attrEntry
                }
            }
        }
    }

    # Save last type
    if ($currentTypeName -and $typeAttrs.Count -gt 0) {
        $attrResults += [ordered]@{ name = $currentTypeName; file = $relativePath; attributes = $typeAttrs }
    }
}

$attributesJson = [ordered]@{ types = $attrResults }
$attributesPath = Join-Path $OutputDir 'attributes.json'
$attributesJson | ConvertTo-Json -Depth 10 | Out-File -FilePath $attributesPath -Encoding utf8
Log "attributes.json written ($($attrResults.Count) types with attributes)"

# ── Write reflection_uses.json ─────────────────────────────────────────────

$reflectionPatterns = @('Type\.GetType', 'Activator\.CreateInstance', 'GetMethod', 'Assembly\.Load', 'MethodInfo\.Invoke')
$reflectionSites = @()

foreach ($file in $csFiles) {
    $relativePath = $file.FullName.Substring($SourceDir.Length).TrimStart('\', '/')
    $lines = Get-Content $file.FullName -ErrorAction SilentlyContinue
    if (-not $lines) { continue }

    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        foreach ($pat in $reflectionPatterns) {
            if ($line -match $pat) {
                $reflectionSites += [ordered]@{
                    file    = $relativePath
                    line    = ($i + 1)
                    pattern = ($pat -replace '\\','')
                    context = $line.Trim()
                }
            }
        }
    }
}

$reflectionJson = [ordered]@{ sites = $reflectionSites }
$reflectionPath = Join-Path $OutputDir 'reflection_uses.json'
$reflectionJson | ConvertTo-Json -Depth 10 | Out-File -FilePath $reflectionPath -Encoding utf8
Log "reflection_uses.json written ($($reflectionSites.Count) reflection sites)"

# ── Write api_surface.md ────────────────────────────────────────────────────

$mdPath = Join-Path $OutputDir 'api_surface.md'
$md = [System.Text.StringBuilder]::new()
[void]$md.AppendLine("# API Surface")
[void]$md.AppendLine("")

if ($controllers.Count -eq 0) {
    [void]$md.AppendLine("No API controllers detected.")
} else {
    foreach ($ctrl in $controllers) {
        [void]$md.AppendLine("## $($ctrl.type)")
        [void]$md.AppendLine("")
        if ($ctrl.route_prefix) {
            [void]$md.AppendLine("**Base route:** ``$($ctrl.route_prefix)``")
            [void]$md.AppendLine("")
        }
        [void]$md.AppendLine("**Source:** ``$($ctrl.file)``")
        [void]$md.AppendLine("")

        if ($ctrl.endpoints -and $ctrl.endpoints.Count -gt 0) {
            [void]$md.AppendLine("| Method | HTTP | Route | Parameters | Returns |")
            [void]$md.AppendLine("|--------|------|-------|------------|---------|")

            foreach ($ep in $ctrl.endpoints) {
                $httpStr = ($ep.http_methods -join ', ')
                $paramStr = if ($ep.parameters -and $ep.parameters.Count -gt 0) {
                    ($ep.parameters | ForEach-Object { "$($_.name): $($_.type) [$($_.from)]" }) -join '; '
                } else { '-' }
                $route = if ($ep.route) { $ep.route } else { '-' }
                [void]$md.AppendLine("| $($ep.method) | $httpStr | ``$route`` | $paramStr | ``$($ep.returns)`` |")
            }
            [void]$md.AppendLine("")
        }
    }
}

$md.ToString() | Out-File -FilePath $mdPath -Encoding utf8
Log "api_surface.md written"

# ── Summary ──────────────────────────────────────────────────────────────────

$totalEndpoints = ($controllers | ForEach-Object { $_.endpoints.Count } | Measure-Object -Sum).Sum
Log "Index building complete: $($types.Count) types, $($controllers.Count) controllers, $totalEndpoints endpoints, $($callGraph.Count) call sites"

Write-Output "INDEX_TYPES:$($types.Count)"
Write-Output "INDEX_CONTROLLERS:$($controllers.Count)"
Write-Output "INDEX_ENDPOINTS:$totalEndpoints"
Write-Output "INDEX_CALLERS:$($callGraph.Count)"

exit 0
