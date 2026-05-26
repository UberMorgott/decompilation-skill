# Output Schema — Folder Structure and JSON Contracts

> Defines the output contract that every pipeline must produce.
> Other agents consume this structure — treat it as a stable API.

---

## Full Output Folder Structure

```
<target-name>/
├── original/                  # Untouched copy of input binary
│   └── <target>.dll
│
├── recon.json                 # Phase 0 output (binary identification)
├── pipeline.log               # Human-readable log: what ran, exit codes, timing
├── pipeline.json              # Machine-readable equivalent of pipeline.log
│
├── intermediate/              # All in-between binaries (numbered by phase)
│   ├── 01_unpacked.dll
│   ├── 02_strings_decrypted.dll
│   ├── 03_cflow_clean.dll
│   ├── 04_proxies_removed.dll
│   └── 05_renamed.dll
│
├── extracted/                 # Embedded sub-assemblies (one folder each, recursive)
│   ├── Area.Admin/
│   │   ├── original/
│   │   ├── recon.json
│   │   ├── intermediate/
│   │   ├── src/
│   │   └── ...
│   └── OtherLib/
│       └── ...
│
├── src/                       # ilspycmd --project output (file-per-type)
│   ├── <Assembly>.csproj
│   └── <Namespace>/<Class>.cs
│
├── strings/
│   ├── decrypted.tsv          # token<TAB>decoded_string — full decryption table
│   ├── all_strings.txt        # Raw `strings` utility dump of original binary
│   ├── url_candidates.txt     # Lines matching http://, https://, or /api/
│   ├── format_strings.txt     # Lines containing {0}, {N}, %s, etc.
│   └── error_messages.txt     # Heuristic filter for error/exception messages
│
├── metadata/
│   ├── index.json             # Type index: type → file, methods, base, interfaces
│   ├── api_surface.json       # HTTP endpoints with routes, params, returns
│   ├── api_surface.md         # Human-readable summary of api_surface.json
│   ├── callgraph.json         # Adjacency list: caller → [callees]
│   ├── callgraph.dot          # Same in Graphviz format
│   ├── attributes.json        # All custom attributes (DI, serialization clues)
│   └── reflection_uses.json   # Sites using Type.GetType / Activator / GetMethod
│
├── native/                    # Only for native binaries (kind == "native")
│   ├── ghidra_project/        # Ghidra project files
│   ├── functions/             # One .c file per function (pseudo-C from Ghidra)
│   └── symbols.idc            # IDR/IDA-compatible symbol script
│
└── README.md                  # Pipeline summary: what ran, what worked, what failed
```

---

## recon.json Schema

Produced by Phase 0 (recon). Every pipeline run starts here.

```json
{
  "path": "target.dll",
  "size_bytes": 7654321,
  "sha256": "a1b2c3d4e5f6...",
  "kind": "managed|native|unity_mono|unity_il2cpp|mixed|unknown",
  "runtime": ".NET 6.0|Embarcadero Delphi 11|MSVC 14.3|go|rust|...",
  "compiler": "Microsoft .NET|Embarcadero Delphi|GCC 12|...",
  "obfuscator": "ConfuserEx 2|Eazfuscator|SmartAssembly|none|...",
  "obfuscator_features": ["constants", "anti_tamper", "ctrl_flow", "ref_proxy", "resources", "anti_debug"],
  "packed": true,
  "embedded_assemblies_suspected": true,
  "embedded_signals": ["AssemblyResolve hook in module ctor", "Costura.Fody resources", "ILRepack signature"],
  "next_phase": "dotnet_deobfuscate|native_decompile|il2cpp_recover|direct_decompile"
}
```

Fields:
- `kind` — primary binary classification, drives top-level routing
- `obfuscator` — specific obfuscator name, drives deobfuscation pipeline choice
- `obfuscator_features` — which protection layers are active (determines pipeline steps)
- `next_phase` — recommended next action for the router
- `embedded_signals` — evidence for embedded assembly detection

---

## index.json Schema

Maps every type to its source file, methods, base class, and interfaces.
Built by `scripts/build-indexes.ps1` using regex-based parsing of decompiled `.cs` files.

```json
{
  "types": [
    {
      "name": "WebMapCore.Area.Admin.Services.OrgsImportService",
      "file": "src/Area.Admin/Services/OrgsImportService.cs",
      "kind": "class",
      "base": "object",
      "interfaces": ["IOrgsImportService"],
      "methods": [
        {
          "name": "ImportAsync",
          "signature": "Task<ImportResult> ImportAsync(Stream)",
          "line": 42
        }
      ],
      "fields": [
        {
          "name": "_logger",
          "type": "ILogger<OrgsImportService>"
        }
      ]
    }
  ]
}
```

Purpose: agents can search by type name, find the file, jump to the right line.
Prevents reading every file to locate a class.

---

## api_surface.json Schema

Pre-extracted map of HTTP endpoints. Agents do not need to parse C# attributes.

```json
{
  "controllers": [
    {
      "type": "WebMapCore.Area.Admin.Controllers.AuthController",
      "file": "src/Area.Admin/Controllers/AuthController.cs",
      "route_prefix": "api/admin/auth",
      "endpoints": [
        {
          "method": "GenerateToken",
          "http_methods": ["POST"],
          "route": "token",
          "parameters": [
            {
              "name": "request",
              "type": "TokenRequest",
              "from": "Body"
            }
          ],
          "returns": "ActionResult<TokenResponse>",
          "attributes": ["Authorize(Roles=\"Admin\")"]
        }
      ]
    }
  ]
}
```

Extracted from: `[HttpGet]`, `[HttpPost]`, `[Route]`, `[FromBody]`, `[FromQuery]`,
`[Authorize]`, minimal API `Map*` calls, and controller route prefixes.

---

## Why Each Piece Matters

| Output | Why |
|---|---|
| `src/` (project mode) | File-per-type is the single biggest readability win. Agents grep and navigate paths far better than single 50k-line dumps |
| `strings/decrypted.tsv` | Agent can grep by integer token OR by content. Token is the bridge between obfuscated code (`iv.a(0x67B8714F)`) and the human string |
| `api_surface.json` | Pre-extracted endpoint map. Agent doesn't have to parse C# attributes — the skill does it once |
| `callgraph.json` | Prevents the agent from reading half the codebase to answer "what calls X". Adjacency list is enough |
| `index.json` | Type-to-file mapping. Agent can locate any class without scanning all files |
| `README.md` | Prevents the agent from "fixing" code that looks broken but is a correctly-decompiled compiler artifact (async state machines, closures, switch tables) |
| `original/` | Lets the agent or a re-run redo any phase without re-acquiring the file |
| `intermediate/` | When debugging the pipeline, see what each stage produced. Diagnose which step corrupted the output |
| `attributes.json` | DI registrations and serialization attributes reveal architectural intent |
| `reflection_uses.json` | Dynamic dispatch sites that static analysis cannot follow — flags for manual review |

---

## pipeline.log Format

Human-readable, one block per phase:

```
=== Phase 0: Recon ===
[2026-05-26 14:30:01] START recon.ps1 target.dll
[2026-05-26 14:30:03] EXIT 0 (2.1s)
[2026-05-26 14:30:03] OUTPUT recon.json written

=== Phase 1: Unpack ===
[2026-05-26 14:30:03] START ConfuserEx-Unpacker-2 target.dll → intermediate/01_unpacked.dll
[2026-05-26 14:30:05] EXIT 0 (1.8s)

=== Phase 2: String Decryption ===
[2026-05-26 14:30:05] START ConfuserEx-Static-String-Decryptor intermediate/01_unpacked.dll
[2026-05-26 14:30:06] EXIT 1 (0.9s) — FAILED: "Unsupported encryption variant"
[2026-05-26 14:30:06] FALLBACK ConfuserEx2_String_Decryptor
[2026-05-26 14:30:06] START ConfuserEx2_String_Decryptor intermediate/01_unpacked.dll
[2026-05-26 14:30:12] EXIT 0 (5.8s) — 847 strings decrypted
```

Each entry: timestamp, action (START/EXIT/FALLBACK/SKIP/ERROR), tool name, exit code, duration.
Failed steps include stderr capture. Never silently omit failures.

`pipeline.json` is the machine-readable equivalent with the same data in JSON format.
