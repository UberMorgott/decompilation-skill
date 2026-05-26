# Universal Decompilation Skill — Design Spec

> Approved design for a Claude Code skill that takes any compiled binary and produces a structured, AI-readable project folder.

## 1. Overview

Personal Claude Code skill (`~/.claude/skills/decompilation/`) for reverse-engineering Windows binaries: .NET (obfuscated and clean), native (Delphi, C/C++, Go, Rust), Unity (Mono + IL2CPP). GitHub repo as source of truth.

**Core principle:** Recon first, route by binary type, preserve every intermediate, produce structured output that another agent can grep/navigate efficiently.

## 2. Architecture — Hybrid (Compact SKILL.md + Heavy References)

```
~/.claude/skills/decompilation/
├── SKILL.md                          # ~280 lines: trigger, recon, routing, rules
├── commands/
│   └── decompile.md                  # /decompile slash command
├── references/
│   ├── dotnet-confuserex.md          # ConfuserEx pipeline + dynamic invoke fallback
│   ├── dotnet-obfuscators.md         # Other .NET obfuscators routing table
│   ├── native-delphi.md              # IDR → Ghidra → dhrake
│   ├── native-general.md             # Ghidra headless for C/C++/Go/Rust
│   ├── unity-il2cpp.md               # Il2CppDumper → Cpp2IL pipeline
│   ├── dynamic-analysis.md           # Frida, mitmproxy, x64dbg, dnSpyEx
│   ├── output-schema.md              # Output folder contract + JSON schemas
│   └── tool-catalog.md               # All tools: install commands, URLs, flags
├── scripts/
│   ├── recon.ps1                     # Phase 0: DiE + strings + heuristics → recon.json
│   ├── recipe-dotnet-confuserex.ps1  # Full ConfuserEx deobfuscation chain
│   ├── recipe-dotnet-generic.ps1     # Clean .NET → ilspycmd --project
│   ├── recipe-delphi.ps1             # IDR → Ghidra → export
│   ├── recipe-native.ps1             # Ghidra headless general
│   ├── recipe-il2cpp.ps1             # Unity IL2CPP pipeline
│   ├── recipe-unity-mono.ps1         # Unity Mono (delegates to .NET)
│   ├── extract-embedded.ps1          # Costura/ILMerge/ConfuserEx resource extraction
│   └── build-indexes.ps1             # Generate index.json + api_surface.json
└── assets/
    └── README.template.md            # Template for output README
```

**Why this structure:**
- SKILL.md stays under 300 lines — LLM reads only routing logic
- References loaded on-demand per branch (ConfuserEx → read dotnet-confuserex.md)
- Scripts are .ps1 only (Windows-focused, personal skill)
- Best ideas merged from: SimoneAvogadro (machine-readable output), incogbyte (adaptive bypass), gl0bal01 (orchestrator pattern)

## 3. SKILL.md Frontmatter

```yaml
---
name: decompilation
description: Use when user wants to decompile, reverse-engineer, deobfuscate, inspect, or read source/strings/API surface of compiled binaries — .dll, .exe, Unity game folder, Delphi program, .NET assembly, IL2CPP build. Trigger on "decompile", "what does this dll do", "find endpoints", "extract strings", "deobfuscate", "ConfuserEx", "IL2CPP", or when user provides binary without source code.
---
```

## 4. Phase 0 — Recon (Always First)

Inline in SKILL.md. Runs `scripts/recon.ps1 <target>`.

**Input:** path to binary or game folder
**Output:** `recon.json` with schema:

```json
{
  "path": "target.dll",
  "size_bytes": 7654321,
  "sha256": "...",
  "kind": "managed|native|unity_mono|unity_il2cpp|mixed|unknown",
  "runtime": ".NET 6.0|Embarcadero Delphi 11|MSVC 14.3|...",
  "compiler": "Microsoft .NET|...",
  "obfuscator": "ConfuserEx 2|Eazfuscator|none|...",
  "obfuscator_features": ["constants","anti_tamper","ctrl_flow","ref_proxy","resources"],
  "packed": true|false,
  "embedded_assemblies_suspected": true|false,
  "embedded_signals": ["AssemblyResolve hook","Costura.Fody resources","ILRepack signature"],
  "next_phase": "dotnet_deobfuscate|native_decompile|il2cpp_recover|direct_decompile"
}
```

**Script exit codes:**
- 0 + recon.json → success
- 1 + stderr → tool missing (`INSTALL_REQUIRED:diec`)
- 2 + partial recon.json → fallback heuristics used

**Fallback when DiE unavailable:** check PE magic bytes, .NET metadata token `0x424A5342`, resource names heuristics.

## 5. Routing Logic

```python
recon = run_recon(target)

if recon.kind == "managed":
    if recon.obfuscator != "none":
        # Read references/dotnet-confuserex.md (or obfuscator-specific)
        target = run_obfuscator_pipeline(target, recon.obfuscator)
    if recon.embedded_assemblies_suspected:
        children = extract_embedded(target)  # scripts/extract-embedded.ps1
        for child in children:
            recurse(child)  # same pipeline
    decompile_to_project(target)  # ilspycmd -p -o ./src

elif recon.kind == "unity_il2cpp":
    # Read references/unity-il2cpp.md
    il2cpp_dump(target) → dummy DLLs
    cpp2il_attempt(target)
    ghidra_with_symbols(target)

elif recon.kind == "unity_mono":
    # same as managed

elif recon.kind == "native":
    if recon.compiler starts with "Embarcadero Delphi":
        # Read references/native-delphi.md
        idr_then_ghidra(target)
    else:
        # Read references/native-general.md
        ghidra_headless(target)

# Always:
build_indexes(out_dir)  # scripts/build-indexes.ps1
write_readme(out_dir)
```

## 6. .NET Deobfuscation — ConfuserEx Pipeline

In `references/dotnet-confuserex.md`. Canonical order (order matters):

```
unpack → anti-tamper removal → string decryption → control flow → proxy removal → rename → decompile
```

**Tool chain:**
1. NoFuserEx (try all-in-one first, fast path)
2. If fails → staged: ConfuserEx-Unpacker-2 → ConfuserEx2_String_Decryptor → de4dot-cex -p crx → ProxyCall-Remover → de4dot-cex --rename
3. If string decryptor fails → dynamic invoke fallback (AsmResolver + Reflection, ~30 lines C#)
4. If all static fails → dynamic analysis (references/dynamic-analysis.md)

**Security gate:** Dynamic invoke executes target code. MUST warn user. Recommend VM/sandbox.

**Other obfuscators** in `references/dotnet-obfuscators.md`:
| Obfuscator | Primary | Fallback |
|---|---|---|
| Eazfuscator | EazFixer | de4dot |
| SmartAssembly | SA-Killer | de4dot |
| .NET Reactor | NetReactorSlayer | — |
| Babel.NET | DeBabelVM | beeless |
| KoiVM | OldRod | — |
| Themida/VMProtect | Dynamic dump | dnSpyEx breakpoint |

## 7. Native Pipelines

### Delphi (`references/native-delphi.md`)
```
IDR (RTTI, VCL names, .idc export)
  → Ghidra + dhrake (.idc import, VMT structs)
    → Resource Hacker (.dfm forms, version info)
      → Export per-function pseudo-C
```

### General native (`references/native-general.md`)
- Default: `analyzeHeadless` + ExportDecompilation script
- Go: GoReSym → Ghidra
- Rust: check .debug section, Ghidra Rust plugin
- Output: `native/functions/` one .c per function

## 8. Unity IL2CPP Pipeline

In `references/unity-il2cpp.md`.

**Auto-detect:**
- `*_Data/Managed/*.dll` → Mono → treat as .NET
- `GameAssembly.dll` + `global-metadata.dat` → IL2CPP

**Pipeline:**
```
Il2CppDumper → dummy DLLs (types + signatures)
  + Cpp2IL --just-give-me-dlls-asap-dammit → partial IL
    → ilspycmd -p on both
      → Ghidra + dumped symbols (unrecovered methods)
```

Modding (optional): BepInEx, MelonLoader, HarmonyX, UnityExplorer, AssetRipper.

## 9. Dynamic Analysis Fallback

In `references/dynamic-analysis.md`. Triggered when static pipeline stalls.

| Level | Tools | When |
|---|---|---|
| Traffic capture | mitmproxy, Fiddler | Need endpoints/headers |
| .NET runtime | dnSpyEx, Harmony | Need decrypted values at runtime |
| Native/deep | Frida, x64dbg, API Monitor | Anti-debug, packed native |

## 10. Output Contract

In `references/output-schema.md`. Full structure:

```
<target-name>/
├── original/              # untouched copy
├── recon.json             # Phase 0 output
├── pipeline.log           # what ran, exit codes, timing
├── pipeline.json          # machine-readable equivalent
├── intermediate/          # 01_unpacked → 02_strings → 03_cflow → ...
├── extracted/             # embedded sub-assemblies (recursive)
├── src/                   # ilspycmd --project output (file-per-type)
├── strings/
│   ├── decrypted.tsv      # token<TAB>decoded_string
│   ├── all_strings.txt    # raw strings dump
│   ├── url_candidates.txt # http/https matches
│   └── error_messages.txt # heuristic filter
├── metadata/
│   ├── index.json         # type → file, methods, interfaces
│   ├── api_surface.json   # HTTP endpoints with routes, params, returns
│   ├── api_surface.md     # human-readable summary
│   ├── callgraph.json     # caller → [callees]
│   └── attributes.json    # DI, serialization clues
├── native/                # only for native binaries
│   ├── ghidra_project/
│   └── functions/         # one .c per function
└── README.md              # pipeline summary
```

## 11. /decompile Command

```yaml
---
name: decompile
description: Decompile and analyze a binary file
argument-hint: <path-to-binary-or-folder>
allowed-tools: [Bash, PowerShell, Read, Write, Edit, Glob, Grep, Agent]
---
```

## 12. Tool Dependencies

**Required:** diec (DiE CLI), ilspycmd, strings
**Recommended:** de4dot-cex, Ghidra + analyzeHeadless, Il2CppDumper, Cpp2IL
**Optional:** IDR, mitmproxy, frida, dnSpyEx

`recon.ps1` checks on startup, outputs `INSTALL_REQUIRED:`/`INSTALL_OPTIONAL:` with install commands.

## 13. MUST / MUST NOT Rules

**MUST:**
1. Run recon first — never assume type from filename
2. Preserve original/ — always copy before modification
3. Save every intermediate — diagnosable pipeline
4. Use --project mode — file-per-type output
5. Build metadata/index.json + api_surface.json
6. Write README.md with pipeline state
7. Log failed steps with exit code + stderr to pipeline.log

**MUST NOT:**
1. Auto-run dynamic invoke on host without user warning (executes target code)
2. Help bypass DRM/licensing on software user doesn't own
3. Silently fail mid-pipeline — every failure must be logged
4. Delete extracted/ even if extraction looks wrong

**SHOULD:**
1. Try fast static path first, fall back to dynamic
2. Detect obfuscated sub-assemblies and recurse
3. Log timing per phase
4. Provide --minimal mode (recon → strings → decompile only)

## 14. Test Cases

1. Clean .NET 6 DLL — no deobfuscation, fast path
2. ConfuserEx2-vanilla DLL — full pipeline with public tools
3. ConfuserEx2 advanced (WebMapCore case) — fallback to dynamic invoke
4. Costura-Fody packed DLL — detect + extract sub-assemblies
5. Delphi 7 EXE — IDR + Ghidra + dhrake
6. Unity IL2CPP game folder — dummy DLLs + Cpp2IL
7. Native Win64 Go binary — GoReSym + Ghidra
8. Malformed DLL — graceful failure in pipeline.log

## 15. Priority Case

AutoGRAPH ecosystem:
- AG Desktop: .NET 6 + ConfuserEx (AutoGRAPHShell.dll 3MB, UI 7.7MB)
- AG Server: Native Delphi RAD Studio 27.0 (22.8MB)
- WebMapCore: .NET 6 + ConfuserEx (7.3MB, Area.Admin ILMerged)
- Goal: decrypt ConfuserEx strings → find 7 header names for Schema Upload API
