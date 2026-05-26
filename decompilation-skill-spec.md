# Universal Decompilation & Deobfuscation Skill — Reference Spec

> This document is a **reference for an agent building a Claude skill**. It is not the final `SKILL.md`. It contains the full toolset, pipelines, decision routing, output spec, and edge cases needed to construct a robust universal decompilation skill covering .NET (managed + obfuscated), native (Delphi, C/C++), Unity/IL2CPP, and prep of decompiled output for downstream AI-agent consumption.
>
> The concrete case that motivated this spec: read decrypted strings from a `ConfuserEx`-protected .NET 6 assembly (`WebMapCore.dll`) and locate endpoints inside a possibly embedded/ILMerged sub-assembly (`Area.Admin`). The skill must generalize beyond this case.

---

## 1. Skill purpose & scope

**The skill should let a downstream agent take a binary (.NET, native, or Unity) and produce a structured, deobfuscated, decompiled folder that another agent can read efficiently to:**

- Understand application behavior.
- Locate and read string constants (including those encrypted by obfuscators).
- Map out HTTP API surface (endpoints, headers, request/response shapes).
- Modify or mod the application (when applicable).
- Trace what an API method actually does at runtime.

**Out of scope (explicitly):**

- Circumventing DRM / commercial license enforcement.
- Anything legally restricted in the user's jurisdiction. The skill should refuse to help bypass licensing or DRM on commercial software the user does not own.

---

## 2. Phase 0 — Recon (always first)

Before choosing any tool, identify the binary's compiler, runtime, and protections. The whole pipeline branches on this.

### Tools

| Tool                          | Purpose                                                                         | CLI? | Notes                                                                                          |
|-------------------------------|---------------------------------------------------------------------------------|------|------------------------------------------------------------------------------------------------|
| **Detect It Easy (DiE)**      | Identifies compiler, packer, obfuscator, signature                              | ✅ `diec` | `diec.exe target.dll -j` → JSON. First tool to run on anything.                                |
| **PEStudio**                  | PE headers, imports, resources, indicators                                      | ⚠️ GUI-first | Useful to see managed `.resources` names (ConfuserEx string blobs live there).                 |
| **pe-bear**                   | PE structure viewer                                                             | ⚠️ GUI | Lightweight, cross-platform.                                                                   |
| **ExeInfoPE**                 | Older but precise on ConfuserEx variants                                        | ⚠️ GUI | Says e.g. "Confuser v1.6.0 (Public)" or "ConfuserEx2 (mkaring fork)".                          |
| **CFF Explorer**              | .NET metadata, resources, module initializer                                    | ⚠️ GUI | Critical for spotting embedded-assembly loaders (AssemblyResolve hook).                        |
| **`file` / `binwalk`**        | Sanity check on Unix                                                            | ✅   | Always run `file` first as a fallback.                                                         |
| **Resource Hacker**           | Native-resource extraction (icons, VCL forms, version info)                     | ✅   | Necessary for Delphi binaries.                                                                 |
| **`strings` (or `rg --null-data`)** | Raw string dump                                                           | ✅   | Cheap and often gives 80% of needed insight before any deobfuscation.                          |

### Recon output (must be produced by the skill)

A `recon.json` written into the working directory with the schema:

```json
{
  "path": "WebMapCore.dll",
  "size_bytes": 7654321,
  "sha256": "…",
  "kind": "managed" | "native" | "unity_mono" | "unity_il2cpp" | "mixed" | "unknown",
  "runtime": ".NET 6.0" | ".NET Framework 4.x" | ".NET 8" | "native_win64" | "native_linux64" | "delphi" | "go" | "rust" | ...,
  "compiler": "Microsoft .NET" | "Embarcadero Delphi 11" | "MSVC 14.3" | "GCC 12" | ...,
  "obfuscator": "ConfuserEx 2" | "ConfuserEx 1.x" | "Eazfuscator" | "SmartAssembly" | "Babel" | "none" | ...,
  "obfuscator_features": ["constants", "anti_tamper", "ctrl_flow", "anti_debug", "resources", "ref_proxy"],
  "packed": true | false,
  "embedded_assemblies_suspected": true | false,
  "embedded_signals": ["AssemblyResolve hook in module ctor", "Costura.Fody resources", "ILRepack signature"],
  "next_phase": "dotnet_deobfuscate" | "native_decompile" | "il2cpp_recover" | "direct_decompile"
}
```

The router (Phase 7) reads this and dispatches.

---

## 3. Phase 1 — .NET decompilation & deobfuscation

### 3.1 Decompiler choice (managed code is readable)

| Tool                            | Strength                                                  | When to use                                                                |
|---------------------------------|-----------------------------------------------------------|----------------------------------------------------------------------------|
| **ilspycmd**                    | Modern C# (12), `--project` output, cross-platform, scriptable | Default. Always use `-p` (project mode) for AI-readable output.            |
| **ICSharpCode.Decompiler (NuGet)** | Programmatic access to ILSpy engine                       | When the skill needs custom formatting (e.g., inject extra comments).      |
| **dnSpyEx (GUI)**               | Edits and debugs assemblies                                | Manual triage, breakpoint debugging, resource extraction.                  |
| **dotPeek (JetBrains)**         | Alternative engine, has CLI `dotPeek.exe /export`         | Cross-check ILSpy when output looks off (rare).                            |
| **JustDecompile (Telerik)**     | Free, open engine                                          | Legacy fallback. Project semi-abandoned.                                   |
| **Project Rover**               | Cross-platform GUI on ILSpy engine                         | Interactive use on macOS/Linux.                                            |

**Default for the skill:** `ilspycmd <dll> -p -o <out>` and optionally `--genpdb` to get debug symbols.

### 3.2 Obfuscator-specific tooling

The skill must route based on `obfuscator` field from `recon.json`.

#### ConfuserEx / ConfuserEx2 (most common open-source)

Canonical pipeline (order matters):

```
unpack (if packed)
  → anti-tamper removal
    → string decryption
      → control flow deobfuscation
        → proxy-call removal
          → symbol rename
            → decompile
```

| Tool                                      | Phase           | Notes                                                                                 |
|-------------------------------------------|-----------------|---------------------------------------------------------------------------------------|
| **ConfuserEx-Unpacker-2**                 | unpack          | Emulation-based, handles most advanced configs.                                       |
| **ConfuserEx-Dynamic-Unpacker**           | unpack          | Invoke-based, works on modified ConfuserEx.                                           |
| **ConfuserEx-Unpacker-Mod-By-Bed**        | unpack          | Wider compatibility for non-vanilla variants.                                         |
| **ConfuserExTools (cawk)**                | anti-tamper + ref proxy | Handles basic AntiTamper, ReferenceProxy, Constants.                                  |
| **AntiTamper.Killer**                     | anti-tamper     | Targeted, when unpackers don't handle it cleanly.                                     |
| **ConfuserEx2_String_Decryptor (Dump-GUY)** | strings       | **Best for advanced/modified ConfuserEx.** AsmResolver + Harmony2 + Reflection. Patches IL to inline decrypted strings. |
| **ConfuserEx2_Python_String_Decrypt (iterasec)** | strings  | Python automation around the same idea — easier to integrate in scripts.              |
| **ConfuserEx-Static-String-Decryptor**    | strings         | Pure static. Only works on vanilla ConfuserEx. Try first as a fast path.              |
| **ConfuserEx-Resources-Decryptor**        | resources       | When `Resources protection` is enabled — decrypts embedded .resources blobs.          |
| **de4dot-cex (ViRb3 fork)**               | ctrl flow + rename | `de4dot-x64.exe target.dll -p crx` — the right fork for ConfuserEx 2. Original de4dot does **not** handle ConfuserEx properly. |
| **ProxyCall-Remover (Kaidoz)**            | proxy           | Inlines `<Module>.smethod_X<T>(token)` wrappers.                                      |
| **NoFuserEx**                             | full chain      | All-in-one for vanilla ConfuserEx. Try as a quick first attempt.                      |

**Fallback when no public tool handles the customization:**

The skill should know how to write a ~30-line C# scratch program using **AsmResolver** or **dnlib** that:

1. Loads target assembly with `Assembly.LoadFrom`.
2. Locates the string-decryption method via heuristic (single `int` parameter, returns `string`, called from many sites, often in `<Module>` or a single utility class).
3. Walks every `call`/`callvirt` to that method, captures the preceding `ldc.i4` argument.
4. Invokes the method reflectively with each argument, builds a `Dictionary<int, string>`.
5. Rewrites IL: replaces `ldc.i4 N; call decryptor` with `ldstr "value"`.
6. Saves the modified assembly.

This is the universal hammer for ConfuserEx-style protections regardless of the specific XOR/RC4/control-flow flavor — execution beats parsing.

#### Other .NET obfuscators

| Obfuscator        | Recommended tool                                  | Notes                                                                          |
|-------------------|----------------------------------------------------|--------------------------------------------------------------------------------|
| **Eazfuscator.NET** | EazFixer, EazFix, Eazfuscator-deobfuscator       | String decryption + virtualization removal. Commercial obfuscator, varies a lot. |
| **SmartAssembly** | SmartAssembly-Decompressor, SA-Killer              | Resource decompression, member rename.                                         |
| **Babel.NET**     | Babel-Deobfuscator, DeBabelVM, beeless             | Has a VM in newer versions — DeBabelVM addresses that.                         |
| **.NET Reactor**  | NetReactorSlayer, NETReactor.Unpacker              | NetReactorSlayer is the modern actively-maintained one.                        |
| **Agile.NET**     | de4dot (built-in support)                          | One of the few de4dot still handles well.                                      |
| **KoiVM** (ConfuserEx custom VM) | OldRod                              | Devirtualizer. Without it, KoiVM-protected methods are unreadable.             |
| **Themida/VMProtect** wrapping a .NET binary | Run dynamic, dump from memory          | Static analysis effectively impossible. Use dnSpyEx + breakpoint after unpack stub. |
| **No obfuscator** | go straight to decompile                           | Most internal/enterprise tools.                                                |

A curated catalog: [`NotPrab/.NET-Deobfuscator`](https://github.com/NotPrab/.NET-Deobfuscator) — the skill should reference this index for protections we don't enumerate above.

### 3.3 Universal symbol-rename pass

After deobfuscation, names like `_3F35B72E` remain. Run `de4dot --rename` (or `de4dot-cex --rename`) to humanize them. This is a one-line step that hugely improves agent readability.

### 3.4 Decompile to project layout

```
ilspycmd <cleaned.dll> -p -o ./out/<assembly-name> --genpdb
```

`--project` (`-p`) writes a real `.csproj` with files split by namespace/type — far easier for an agent than a 50k-line monofile.

---

## 4. Phase 2 — Embedded / merged assembly extraction

If `recon.json` says `embedded_assemblies_suspected: true`, run extraction **before** going deep on the host assembly.

### Signals to detect

- **Costura.Fody**: resources named like `costura.<name>.dll.compressed`, GZip-encoded. Module ctor adds `AppDomain.CurrentDomain.AssemblyResolve += …`.
- **ILRepack / ILMerge**: no resources, but multiple top-level namespaces fused into one module. Detect by checking for incongruous root namespaces (`Area.Admin.*` in `WebMapCore`).
- **Custom loader**: encrypted resource + decryption stub in module ctor. ConfuserEx Resources protection is one variant.

### Tools

| Tool                                       | Purpose                                                                          |
|--------------------------------------------|----------------------------------------------------------------------------------|
| **dnSpyEx (GUI)**                          | Resources tree → Save…  — manual but fast for one-off.                           |
| **CosturaUnpacker**                        | Auto-extracts gzipped Costura DLLs.                                              |
| **`netz` decompressor**                    | Older NETZ-packed assemblies.                                                    |
| **ConfuserEx-Resources-Decryptor**         | Decrypts ConfuserEx-protected resources.                                         |
| **AsmResolver / dnlib script (~10 lines)** | Iterate `module.Resources`, gunzip, check for `MZ` header, save. Universal fallback. |

### Script template the skill should include

```csharp
using AsmResolver.DotNet;
using System.IO.Compression;

var module = ModuleDefinition.FromFile(args[0]);
foreach (var res in module.Resources) {
    if (res is not EmbeddedResource emb) continue;
    var bytes = emb.GetData();
    // try gzip
    try {
        using var ms = new MemoryStream(bytes);
        using var gz = new GZipStream(ms, CompressionMode.Decompress);
        using var outMs = new MemoryStream();
        gz.CopyTo(outMs);
        bytes = outMs.ToArray();
    } catch { /* not gzipped */ }
    if (bytes.Length >= 2 && bytes[0] == 'M' && bytes[1] == 'Z')
        File.WriteAllBytes($"extracted/{res.Name}.dll", bytes);
}
```

After extraction each child assembly goes through the same Phase 1 pipeline.

---

## 5. Phase 3 — Native binaries

Used when `recon.json:kind = native`.

### 5.1 Delphi (Object Pascal, Embarcadero)

| Tool                                         | Role                                                                          |
|----------------------------------------------|-------------------------------------------------------------------------------|
| **IDR (Interactive Delphi Reconstructor)**   | First pass. Restores RTTI, VCL classes, method names. Exports `.idc` script. Best for Delphi 2..XE4, partial for XE5+. |
| **Ghidra**                                   | Main decompiler. Free, cross-platform.                                        |
| **dhrake** (Ghidra plugin)                   | Imports IDR's `.idc`, names symbols, creates VMT structs.                     |
| **IDA Pro + FLIRT Delphi signatures**        | Commercial; best result on modern Delphi. Same idea as dhrake but inside IDA. |
| **Binary Ninja + Delphi plugins**            | Alternative commercial platform.                                              |
| **Ultimate Delphi Decompiler**               | Commercial; sometimes produces higher-level output.                           |
| **Resource Hacker**                          | Extracts `.dfm` forms, strings, icons, version info.                          |

Pipeline:

```
IDR (load .exe, save .idc, save .map)
  → Ghidra (analyze + run dhrake to consume .idc)
    → manual rename of TForm classes
      → export pseudo-C decompilation per function
```

### 5.2 C / C++ / Rust / Go (general native)

| Tool                            | Role                                                                              |
|---------------------------------|-----------------------------------------------------------------------------------|
| **Ghidra**                      | Default free decompiler. Has headless mode (`analyzeHeadless`) — scriptable.      |
| **IDA Pro + Hex-Rays**          | Best output quality, commercial.                                                  |
| **Binary Ninja**                | Modern commercial alternative, good scripting (Python API).                       |
| **radare2 / Cutter**            | OSS alternative, scriptable.                                                      |
| **angr**                        | Symbolic execution, useful for complex deobfuscation of native code.              |
| **retdec** (Avast)              | OSS decompiler. Decent on small binaries, struggles with large/optimized.         |
| **Go-specific:** `IDAGolangHelper`, `GoReSym` | Recover Go symbols and types.                                          |
| **Rust-specific:** check `.debug` section if not stripped; Ghidra Rust plugin     | Rust binaries are usually statically linked and large.  |

Headless Ghidra example for the skill:

```bash
analyzeHeadless ./project ProjName -import target.exe -postScript ExportDecompilation.java
```

### 5.3 Other runtimes (briefly)

- **Java / Kotlin (JVM):** `cfr`, `procyon`, `fernflower`. For Android: `jadx` (does Dalvik → Java directly), `apktool` for resources.
- **Python (PyInstaller / py2exe):** `pyinstxtractor`, then `uncompyle6` / `decompyle3` for `.pyc` (depending on Python version). For 3.10+ use `pycdc`.
- **Electron / Node:** unpack `app.asar` with `asar extract`, inspect JS directly. Use `node_modules` graph to triage.

---

## 6. Phase 4 — Unity (mono + IL2CPP)

A massive subdomain that the skill must handle as a branch.

### 6.1 Detect Unity build type

Check the game folder:

- `*_Data/Managed/*.dll` present → **Mono build** → straight to Phase 1 (often without obfuscation).
- `GameAssembly.dll` (or `libil2cpp.so`) + `*_Data/il2cpp_data/Metadata/global-metadata.dat` → **IL2CPP build**.

### 6.2 IL2CPP pipeline

| Tool                  | Purpose                                                                                       |
|-----------------------|-----------------------------------------------------------------------------------------------|
| **Il2CppDumper**      | Reads `global-metadata.dat`, emits dummy DLLs with all types & method signatures (no bodies). |
| **Il2CppInspector**   | More analysis output: C# stubs, C++ headers, Python scripts to import symbols into IDA/Ghidra. |
| **Cpp2IL**            | Attempts actual IL recovery from compiled C++ (partial). Output is loadable in dnSpy/ILSpy. Use `--just-give-me-dlls-asap-dammit` flag for fast triage. |
| **Ghidra + Il2CppDumper script** | Apply dumped struct/method info to native binary for proper decompilation.            |

Recommended order:

```
Il2CppDumper (always) → dummy DLLs for type info
  + Cpp2IL (best-effort) → IL for as many methods as possible
  + Ghidra with dumped symbols (for methods Cpp2IL couldn't recover)
```

### 6.3 Modding-specific (only if the user wants to mod)

- **BepInEx 5.x / 6.x** — runtime loader. 5 for Mono, 6 for IL2CPP.
- **MelonLoader** — alternative loader, broad game support.
- **HarmonyX / Harmony 2** — runtime method patching from C#.
- **UnityExplorer** — in-game object/component inspector. Critical for understanding live behavior.
- **AssetRipper** — extract assets (textures, prefabs, scripts) back to a Unity-editable project.
- **dnSpyEx with Unity profiles** — debug Unity managed code at runtime.

---

## 7. Phase 5 — Dynamic approaches (when static stalls)

For *any* runtime, when static deobfuscation isn't progressing, dynamic instrumentation is usually faster than continuing to fight the obfuscator.

| Tool                                | Targets                                                                  |
|-------------------------------------|--------------------------------------------------------------------------|
| **Frida** (+ `frida-clr`, `frida-mono`) | Hook methods in running process. Logs arguments/returns.            |
| **Harmony / HarmonyX**              | In-process .NET method patching, more reliable than Frida for managed code. |
| **dnSpyEx debugger**                | Set breakpoints in decompiled .NET code, inspect values live.           |
| **x64dbg**                          | Native debugging on Windows.                                            |
| **gdb / lldb**                      | Native debugging on Linux/macOS.                                        |
| **mitmproxy / Fiddler / Proxyman**  | Capture HTTPS traffic. mitmproxy is best for scripting/CI.              |
| **HTTPDecrypt / EchoMirage**        | Inject into process to log calls *before* TLS — defeats cert pinning.   |
| **Wireshark + sslkeylogfile**       | When the process honors `SSLKEYLOGFILE` env (most Chromium-based, .NET on Linux). |
| **API Monitor**                     | Windows API call tracing.                                               |
| **Process Monitor**                 | File / registry / process syscall tracing.                              |

**Critical pattern for ConfuserEx strings via dynamic invoke** (the universal shortcut):

```python
# pseudocode the skill should be able to emit
1. Load assembly into AppDomain via reflection.
2. Locate decryption method (heuristic: int->string, many xrefs).
3. Collect every int argument from xref sites (static scan with dnlib).
4. Invoke decryptor for each → table.
5. Rewrite IL (replace ldc.i4 + call with ldstr).
6. Save patched DLL.
```

---

## 8. Phase 6 — Output structure for AI-agent consumption

This is the **deliverable layout** the skill must produce. Treat the structure below as the contract.

```
<target-name>/
├── original/
│   └── <target>.dll                  # untouched copy of input
├── recon.json                        # Phase 0 output (see §2)
├── pipeline.log                      # human-readable record of what ran, exit codes, durations
├── pipeline.json                     # machine-readable equivalent
│
├── intermediate/                     # all in-between binaries
│   ├── 01_unpacked.dll
│   ├── 02_strings_decrypted.dll
│   ├── 03_cflow_clean.dll
│   ├── 04_proxies_removed.dll
│   └── 05_renamed.dll
│
├── extracted/                        # embedded sub-assemblies (one folder each)
│   ├── Area.Admin/
│   │   └── (same structure recursively)
│   └── …
│
├── src/                              # ilspycmd --project output
│   ├── <Assembly>.csproj
│   └── …/<Namespace>/<Class>.cs
│
├── strings/
│   ├── decrypted.tsv                 # token<TAB>decoded_string — full table
│   ├── all_strings.txt               # `strings` utility dump of original
│   ├── url_candidates.txt            # lines matching /^https?:|^\//
│   ├── format_strings.txt            # lines containing {0}, {N}, %s, etc.
│   └── error_messages.txt            # heuristic filter for messages
│
├── metadata/
│   ├── index.json                    # { type → file, methods, base, interfaces }
│   ├── api_surface.json              # all [HttpGet], [Route], [FromBody], minimal API maps
│   ├── api_surface.md                # human-readable summary of the above
│   ├── callgraph.json                # adjacency list { caller → [callees] }
│   ├── callgraph.dot                 # same, Graphviz
│   ├── attributes.json               # all custom attributes (DI, serialization clues)
│   └── reflection_uses.json          # sites that use Type.GetType / Activator / GetMethod
│
├── native/                           # only when kind == native
│   ├── ghidra_project/
│   ├── functions/                    # one .c per function (pseudo-C from Ghidra)
│   └── symbols.idc                   # IDR/IDA-compatible
│
└── README.md                         # what was done, what was skipped, what failed
```

### Why each piece matters (for the skill builder agent)

- **`src/` from `--project`**: file-per-type is the single biggest readability win. Agents grep + navigate paths far better than single dumps.
- **`strings/decrypted.tsv`**: agent can grep by integer token *or* by content. Token is the bridge between obfuscated code (`iv.a(0x67B8714F)`) and the human string.
- **`api_surface.json`**: pre-extracted map of endpoints. The agent doesn't have to parse C# attributes; the skill does it once.
- **`callgraph.json`**: prevents the agent from reading half the codebase to answer "what calls X". Adjacency list is enough.
- **`README.md` with pipeline state**: prevents the agent from "fixing" code that looks broken but is actually a correctly-decompiled compiler artifact (async state machines, closures, switch tables).
- **`original/` preserved**: lets the agent (or a re-run) redo any phase without re-acquiring the file.
- **`intermediate/` preserved**: when an agent debugs the pipeline, it needs to see what each stage produced.

### `api_surface.json` schema (example)

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
            { "name": "request", "type": "TokenRequest", "from": "Body" }
          ],
          "returns": "ActionResult<TokenResponse>",
          "attributes": ["Authorize(Roles=\"Admin\")"]
        }
      ]
    }
  ]
}
```

### `index.json` schema (example)

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
        { "name": "ImportAsync", "signature": "Task<ImportResult> ImportAsync(Stream)", "line": 42 }
      ],
      "fields": [
        { "name": "_logger", "type": "ILogger<OrgsImportService>" }
      ]
    }
  ]
}
```

Implementation tip: use **Roslyn** to build both files. Open the generated `.csproj` as a workspace, walk the syntax trees, project to JSON.

---

## 9. Phase 7 — Routing logic

The skill's `SKILL.md` body should embody this decision tree (pseudocode):

```python
recon = run_recon(target)
write_json("recon.json", recon)

if recon.kind == "managed":
    if recon.obfuscator != "none":
        target = run_obfuscator_pipeline(target, recon.obfuscator)  # §3.2
    if recon.embedded_assemblies_suspected:
        children = extract_embedded(target)                          # §4
        for c in children:
            recurse(c)
    decompile_to_project(target)                                     # §3.4

elif recon.kind == "unity_il2cpp":
    dummy_dlls = il2cpp_dump(target)                                 # §6.2
    decompile_to_project(dummy_dlls)
    cpp2il_attempt(target)
    ghidra_with_il2cpp_symbols(target)

elif recon.kind == "unity_mono":
    # same as managed
    ...

elif recon.kind == "native":
    if recon.compiler.startswith("Embarcadero Delphi"):
        idr_then_ghidra_dhrake(target)                               # §5.1
    else:
        ghidra_headless(target)                                      # §5.2

build_indexes(out_dir)                                               # §8
write_readme(out_dir)
```

---

## 10. Flat tool catalog (for the skill's reference/)

Listed for the skill builder to easily generate an install manifest.

### .NET decompilers
- **ilspycmd** — `dotnet tool install -g ilspycmd`
- **ICSharpCode.Decompiler** — NuGet
- **dnSpyEx** — https://github.com/dnSpyEx/dnSpy
- **dotPeek** — https://www.jetbrains.com/decompiler/
- **JustDecompile** — https://www.telerik.com/products/decompiler.aspx
- **Project Rover** — https://github.com/lextudio/ProjectRover

### .NET deobfuscators (general)
- **de4dot** — https://github.com/de4dot/de4dot
- **de4dot-cex (ConfuserEx fork)** — https://github.com/ViRb3/de4dot-cex
- **dnlib** — https://github.com/0xd4d/dnlib
- **AsmResolver** — https://github.com/Washi1337/AsmResolver
- **Harmony / HarmonyX** — https://github.com/pardeike/Harmony, https://github.com/BepInEx/HarmonyX

### ConfuserEx-specific
- **ConfuserEx2_String_Decryptor** — https://github.com/Dump-GUY/ConfuserEx2_String_Decryptor
- **ConfuserEx2_Python_String_Decrypt** — https://github.com/iterasec/ConfuserEx2_Python_String_Decrypt
- **ConfuserEx-Unpacker-2** — https://github.com/cawk/ConfuserEx-Unpacker-2
- **ConfuserEx-Dynamic-Unpacker** — https://github.com/ryancblack/ConfuserEx-Unpacker
- **ConfuserEx-Static-String-Decryptor** — listed in NotPrab catalog
- **ConfuserEx-Resources-Decryptor** — listed in NotPrab catalog
- **ProxyCall-Remover** — https://github.com/Kaidoz/ProxyCall-Remover
- **NoFuserEx** — https://github.com/XenocodeRCE/NoFuserEx
- **OldRod** (KoiVM devirtualizer) — https://github.com/Washi1337/OldRod
- **Catalog index** — https://github.com/NotPrab/.NET-Deobfuscator

### Other .NET obfuscator-specific
- **NetReactorSlayer** — https://github.com/SychicBoy/NETReactorSlayer
- **EazFixer / Eazfuscator-deobfuscator** — see NotPrab catalog
- **Babel-Deobfuscator / DeBabelVM** — see NotPrab catalog
- **SmartAssembly-Decompressor** — see NotPrab catalog

### Embedded-assembly extraction
- **CosturaUnpacker** — community implementations on GitHub
- **dnSpyEx GUI** — resources tree
- **Custom AsmResolver/dnlib script** — template in §4

### Native (Delphi)
- **IDR** — https://github.com/crypto2011/IDR
- **dhrake** (Ghidra Delphi scripts) — https://github.com/huettenhain/dhrake
- **Ghidra** — https://github.com/NationalSecurityAgency/ghidra
- **IDA Pro / Hex-Rays** — commercial
- **Resource Hacker** — http://www.angusj.com/resourcehacker/
- **Ultimate Delphi Decompiler** — commercial

### Native (general)
- **Ghidra** — same as above
- **Binary Ninja** — commercial
- **radare2 / Cutter** — https://rada.re, https://cutter.re
- **angr** — https://angr.io
- **retdec** — https://retdec.com
- **GoReSym** (Go) — https://github.com/mandiant/GoReSym
- **IDAGolangHelper** — https://github.com/sibears/IDAGolangHelper

### Unity / IL2CPP
- **Il2CppDumper** — https://github.com/Perfare/Il2CppDumper
- **Il2CppInspector** — https://github.com/djkaty/Il2CppInspector
- **Cpp2IL** — https://github.com/SamboyCoding/Cpp2IL
- **BepInEx** — https://github.com/BepInEx/BepInEx
- **MelonLoader** — https://github.com/LavaGang/MelonLoader
- **UnityExplorer** — https://github.com/sinai-dev/UnityExplorer
- **AssetRipper** — https://github.com/AssetRipper/AssetRipper

### Dynamic / network
- **Frida** — https://frida.re
- **mitmproxy** — https://mitmproxy.org
- **Fiddler Classic / Fiddler Everywhere** — https://www.telerik.com/fiddler
- **Proxyman** — https://proxyman.io
- **Wireshark** — https://www.wireshark.org
- **API Monitor** — http://www.rohitab.com/apimonitor
- **Process Monitor (Sysinternals)** — Microsoft Sysinternals

### Recon / PE analysis
- **Detect It Easy (DiE)** — https://github.com/horsicq/Detect-It-Easy
- **PEStudio** — https://www.winitor.com
- **pe-bear** — https://github.com/hasherezade/pe-bear
- **CFF Explorer** — https://ntcore.com/?page_id=388
- **ExeInfoPE** — http://exeinfo.atwebpages.com

### Java / Android (out-of-scope for spec but include as branch)
- **CFR** — https://www.benf.org/other/cfr/
- **Procyon** — https://github.com/mstrobel/procyon
- **jadx** — https://github.com/skylot/jadx
- **apktool** — https://apktool.org

---

## 11. Concrete pipeline recipes

The skill should contain these as named runnable scripts under `scripts/`.

### 11.1 `recipe_dotnet_confuserex.sh`

```bash
#!/usr/bin/env bash
set -euo pipefail
TARGET="$1"
OUT="$2"
mkdir -p "$OUT"/{original,intermediate,src,extracted,strings,metadata}
cp "$TARGET" "$OUT/original/"

# 0. Recon (assumed already produced recon.json)

# 1. Unpack if packed
ConfuserEx-Unpacker-2 "$TARGET" "$OUT/intermediate/01_unpacked.dll" || cp "$TARGET" "$OUT/intermediate/01_unpacked.dll"

# 2. Strings (try static first, then dynamic)
ConfuserEx-Static-String-Decryptor "$OUT/intermediate/01_unpacked.dll" "$OUT/intermediate/02_strings_decrypted.dll" \
  || ConfuserEx2_String_Decryptor "$OUT/intermediate/01_unpacked.dll" "$OUT/intermediate/02_strings_decrypted.dll"

# Extract the decoded table from the string-decryptor's verbose log to strings/decrypted.tsv

# 3. Control flow
de4dot-x64 "$OUT/intermediate/02_strings_decrypted.dll" -p crx -o "$OUT/intermediate/03_cflow_clean.dll"

# 4. Proxy calls
ProxyCall-Remover "$OUT/intermediate/03_cflow_clean.dll" "$OUT/intermediate/04_proxies_removed.dll"

# 5. Rename
de4dot-x64 "$OUT/intermediate/04_proxies_removed.dll" --rename -o "$OUT/intermediate/05_renamed.dll"

# 6. Embedded assembly extraction
dotnet run --project tools/EmbeddedExtractor -- "$OUT/intermediate/05_renamed.dll" "$OUT/extracted/"

# 7. Decompile main
ilspycmd "$OUT/intermediate/05_renamed.dll" -p -o "$OUT/src" --genpdb

# 8. Decompile each extracted assembly
for dll in "$OUT/extracted"/*.dll; do
    name=$(basename "$dll" .dll)
    mkdir -p "$OUT/extracted/$name/src"
    ilspycmd "$dll" -p -o "$OUT/extracted/$name/src"
done

# 9. Build indexes (Roslyn-based program)
dotnet run --project tools/IndexBuilder -- "$OUT/src" "$OUT/metadata"
```

### 11.2 `recipe_delphi.sh`

```bash
#!/usr/bin/env bash
set -euo pipefail
TARGET="$1"; OUT="$2"
mkdir -p "$OUT/native/ghidra_project" "$OUT/native/functions" "$OUT/strings"
strings -n 6 "$TARGET" > "$OUT/strings/all_strings.txt"
# 1. IDR (Windows-only; run under wine or on a Windows host)
idr64.exe -auto "$TARGET" -save-idc "$OUT/native/symbols.idc"
# 2. Ghidra headless + dhrake
analyzeHeadless "$OUT/native/ghidra_project" Proj -import "$TARGET" \
    -postScript dhrake_apply.py "$OUT/native/symbols.idc"
# 3. Export all decompiled functions to one file per function
analyzeHeadless "$OUT/native/ghidra_project" Proj -process "$(basename "$TARGET")" \
    -postScript ExportDecompilation.java "$OUT/native/functions/"
```

### 11.3 `recipe_il2cpp.sh`

```bash
#!/usr/bin/env bash
set -euo pipefail
GAME_DIR="$1"; OUT="$2"
mkdir -p "$OUT/dummy_dlls" "$OUT/cpp2il_out" "$OUT/src"

# Locate inputs
ASM=$(find "$GAME_DIR" -name "GameAssembly.dll" -o -name "libil2cpp.so" | head -1)
META=$(find "$GAME_DIR" -name "global-metadata.dat" | head -1)

# 1. Il2CppDumper → dummy DLLs
Il2CppDumper "$ASM" "$META" "$OUT/dummy_dlls"

# 2. Cpp2IL → attempted IL
Cpp2IL --game-path "$GAME_DIR" --output-root "$OUT/cpp2il_out" --just-give-me-dlls-asap-dammit

# 3. Decompile both
for dll in "$OUT/dummy_dlls"/*.dll "$OUT/cpp2il_out"/*.dll; do
    name=$(basename "$dll" .dll)
    ilspycmd "$dll" -p -o "$OUT/src/$name" || true
done
```

---

## 12. Notes for the skill builder agent

### Triggering description (frontmatter)

The skill's `description` field should be pushy and broad. Suggested:

> Use this skill whenever the user wants to decompile, reverse-engineer, inspect, mod, deobfuscate, or read the source/strings/API surface of a compiled binary — `.dll`, `.exe`, `.so`, Unity game folder, Delphi program, .NET assembly, IL2CPP build, or any non-source artifact. Trigger on phrases like "decompile", "read this binary", "what does this dll do", "find endpoints in", "mod this game", "extract strings from", "deobfuscate", "ConfuserEx", "IL2CPP", or whenever the user uploads a binary file without source code. Also trigger when a previous skill or tool has produced a binary the user now wants to read. Prefer this skill over generic file-reading when the target is compiled code.

### SKILL.md structure (recommended)

Keep the `SKILL.md` body under 500 lines. Push detail into reference files:

```
decompilation/
├── SKILL.md                       # routing + high-level workflow
├── scripts/
│   ├── recon.py                   # Phase 0
│   ├── recipe_dotnet_confuserex.sh
│   ├── recipe_dotnet_generic.sh
│   ├── recipe_delphi.sh
│   ├── recipe_native.sh
│   ├── recipe_il2cpp.sh
│   ├── recipe_unity_mono.sh
│   ├── extract_embedded.cs        # AsmResolver-based
│   ├── string_decrypt_dynamic.cs  # universal Reflection-based fallback
│   └── build_indexes.cs           # Roslyn-based
├── references/
│   ├── dotnet_obfuscators.md      # ConfuserEx, Eazfuscator, …
│   ├── native_runtimes.md         # Delphi, Go, Rust, C/C++
│   ├── unity_il2cpp.md            # Phase 4 in depth
│   ├── dynamic_analysis.md        # Phase 5
│   ├── output_schema.md           # §8 schemas
│   └── tool_catalog.md            # §10
└── assets/
    └── (templates: README.md, csproj templates, etc.)
```

### Things the skill MUST do (non-negotiable)

1. **Run recon first.** Never assume the obfuscator/runtime from filename or vibes.
2. **Preserve the original.** Always copy to `original/` before any modification.
3. **Save every intermediate.** A broken pipeline must be diagnosable.
4. **Produce `--project` output**, not single-file dumps.
5. **Build `metadata/index.json` and `api_surface.json`.** These are what makes the output AI-readable.
6. **Write a README.md** that records what ran, what worked, what didn't, what was skipped.
7. **Never delete `extracted/`**, even if extraction looks wrong — the agent may need to retry.

### Things the skill SHOULD do (best effort)

1. Try the fast static path first; fall back to dynamic invoke if it fails.
2. Detect when a sub-assembly is itself obfuscated and recurse the pipeline into it.
3. When `--genpdb` succeeds, register the PDB path so a debugger can use it.
4. Log timing per phase — helps the user understand what's slow and worth caching.
5. Provide a `--minimal` mode: only `recon → strings → decompile`, skip cleanup phases. For "I just want to read constants" cases.

### Things the skill MUST NOT do

1. **MUST NOT** auto-run dynamic-invoke string decryptors against arbitrary user input on the host. ConfuserEx2_String_Decryptor literally executes code from the target binary in-process. The skill must warn the user and recommend a VM/sandbox before running it, especially if the binary is malware-suspected.
2. **MUST NOT** help bypass licensing or DRM on software the user does not own. If the recon indicates a commercial protection scheme (Themida, VMProtect, IonCube) and the user's intent is licensing bypass, decline.
3. **MUST NOT** silently fail mid-pipeline. Failed steps must be logged with exit code and stderr captured to `pipeline.log`.

### Suggested test cases for skill evaluation

- A clean .NET 6 DLL (no obfuscation) — should produce `src/` quickly, no string-decryption phase invoked.
- A ConfuserEx2-vanilla DLL — full pipeline should succeed with public tools.
- A ConfuserEx2 advanced + custom DLL (the original WebMapCore case) — should fall back to dynamic invoke.
- A Costura-fody packed DLL — should detect and extract sub-assemblies.
- A small Delphi 7 EXE — should produce Ghidra pseudo-C with named VMTs via IDR+dhrake.
- A Unity IL2CPP game folder — should produce dummy DLLs + Cpp2IL attempt + decompiled stubs.
- A purely native Win64 EXE (Go binary) — should detect Go, run GoReSym, produce Ghidra output.
- An intentionally malformed DLL — should fail gracefully with a clear error in `pipeline.log`.

---

## 13. Open issues / future extensions

- **Ahead-of-time .NET (NativeAOT, ReadyToRun):** ILSpy partially handles ReadyToRun but NativeAOT is closer to native — needs Ghidra-style pipeline. Spec a separate recipe.
- **Symbol recovery via LLM:** post-process renamed-but-still-cryptic names (`Class3`, `m_field2`) using an LLM call that proposes semantic names from context. Possible enhancement; should be opt-in.
- **WASM:** `wasm-decompile`, `wasm-objdump`, `wabt` toolchain. Add if requested.
- **iOS Mach-O / Swift:** `class-dump`, Hopper, IDA + Swift demangler. Out of scope for v1.

---

## 14. The original concrete case (for regression test)

To make sure the universal skill still handles the seed case:

- **Input:** `WebMapCore.dll`, 7.3 MB, .NET 6.0, obfuscator ConfuserEx (advanced).
- **Specific blocker:** `iv.a(int)` — string decryption from embedded resource, XOR-chain with control-flow flattening.
- **Goal:** read decrypted strings (header names for Schema Upload API); locate endpoints `GenerateToken` and `OrgsImport`, possibly inside an embedded `Area.Admin` sub-assembly.

**Expected pipeline trace:**

1. `recon.json` → `obfuscator: "ConfuserEx 2"`, `obfuscator_features: ["constants","ctrl_flow","ref_proxy","resources"]`, `embedded_assemblies_suspected: true`.
2. `recipe_dotnet_confuserex.sh` runs.
3. Static string decryptor fails (advanced/modified) → dynamic invoke decryptor succeeds → `strings/decrypted.tsv` populated.
4. Embedded extractor finds `Area.Admin.dll` in resources → extracts to `extracted/Area.Admin/`.
5. Recursion: `Area.Admin.dll` is itself ConfuserEx-protected → same pipeline runs on it.
6. `ilspycmd -p` emits `src/` for both.
7. `api_surface.json` lists `Area.Admin.Controllers.AuthController.GenerateToken` and `Area.Admin.Controllers.OrgsController.OrgsImport`.
8. Grep on `strings/decrypted.tsv` for `Schema` or `Upload` returns the header names the user originally asked for.

If a future iteration of the skill fails on this case, it has regressed.
