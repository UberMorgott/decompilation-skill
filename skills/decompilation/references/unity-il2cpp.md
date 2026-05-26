# Unity IL2CPP Decompilation Pipeline

> Loaded by SKILL.md when `recon.json.kind == "unity_il2cpp"`.
> For Unity Mono builds, use the standard .NET pipeline instead.

---

## Auto-Detection: Mono vs IL2CPP

Check the game folder structure to determine the build type:

| Signal | Build Type | Action |
|---|---|---|
| `*_Data/Managed/*.dll` present | **Mono** | Treat as .NET — go to standard managed pipeline |
| `GameAssembly.dll` (or `libil2cpp.so`) + `*_Data/il2cpp_data/Metadata/global-metadata.dat` | **IL2CPP** | Use this pipeline |

Detection logic:
```
if (exists GameAssembly.dll AND exists global-metadata.dat):
    kind = "unity_il2cpp"
elif (exists *_Data/Managed/Assembly-CSharp.dll):
    kind = "unity_mono"  → route to .NET pipeline
```

---

## IL2CPP Pipeline

```
Il2CppDumper → dummy DLLs (types + method signatures, no bodies)
  + Cpp2IL → partial IL recovery (best-effort)
    → ilspycmd -p on both sets of DLLs
      → Ghidra + dumped symbols (for methods Cpp2IL couldn't recover)
```

---

## Tool Table

| Tool | Purpose | CLI? | Notes |
|---|---|---|---|
| **Il2CppDumper** | Reads `global-metadata.dat`, emits dummy DLLs with all types and method signatures (no bodies) | Yes | Always run first. Produces type info even when Cpp2IL fails |
| **Il2CppInspector** | More analysis: C# stubs, C++ headers, Python scripts for IDA/Ghidra symbol import | Yes | Alternative/complement to Il2CppDumper. More output formats |
| **Cpp2IL** | Attempts actual IL recovery from compiled C++. Output loadable in ILSpy/dnSpy | Yes | Use `--just-give-me-dlls-asap-dammit` flag for fast triage. Partial success is normal |
| **Ghidra + Il2CppDumper script** | Apply dumped struct/method info to native binary for proper decompilation | Via script | For methods Cpp2IL couldn't recover — read native C++ with symbol names |
| **ilspycmd** | Decompile dummy DLLs and Cpp2IL output to C# project | Yes | Standard decompiler, works on dummy DLLs for type browsing |

---

## Step-by-Step Commands

### 1. Il2CppDumper — Extract Type Information

```bash
Il2CppDumper GameAssembly.dll global-metadata.dat output/dummy_dlls/
```

Output:
- `DummyDll/` — DLLs with types, fields, method signatures (no implementation)
- `dump.cs` — flat C# dump of all types
- `script.json` — Ghidra/IDA import script data
- `il2cpp.h` — C++ header with struct definitions

### 2. Cpp2IL — Attempt IL Recovery

```bash
Cpp2IL \
    --game-path /path/to/game/ \
    --output-root output/cpp2il_out/ \
    --just-give-me-dlls-asap-dammit
```

Flags:
- `--game-path` — root game directory (auto-detects GameAssembly.dll and metadata)
- `--just-give-me-dlls-asap-dammit` — fast mode, skip detailed analysis
- `--output-root` — where to write recovered DLLs

Cpp2IL recovery is partial. Expect 40-70% of methods to have actual IL bodies.
The rest will be empty stubs (better than nothing — signatures are preserved).

### 3. Decompile Both Sets

```bash
# Decompile Il2CppDumper dummy DLLs (type info only)
for dll in output/dummy_dlls/DummyDll/*.dll; do
    name=$(basename "$dll" .dll)
    ilspycmd "$dll" -p -o "output/src/types/$name/" || true
done

# Decompile Cpp2IL output (partial implementations)
for dll in output/cpp2il_out/*.dll; do
    name=$(basename "$dll" .dll)
    ilspycmd "$dll" -p -o "output/src/recovered/$name/" || true
done
```

### 4. Ghidra with Symbols (Unrecovered Methods)

```bash
# Import Il2CppDumper's script.json as Ghidra symbols
analyzeHeadless ./output/native/ghidra_project IL2CPPProj \
    -import GameAssembly.dll \
    -postScript Il2CppDumperGhidra.py output/dummy_dlls/script.json \
    -postScript ExportDecompilation.java output/native/functions/
```

This gives pseudo-C decompilation with proper method and type names for methods
that Cpp2IL could not recover.

---

## Modding Tools (Optional)

Only relevant when the user wants to modify or extend the game.

| Tool | Purpose | Notes |
|---|---|---|
| **BepInEx 5.x** | Runtime mod loader for Mono builds | Plugin system, config, logging |
| **BepInEx 6.x** | Runtime mod loader for IL2CPP builds | IL2CPP support via Il2CppInterop |
| **MelonLoader** | Alternative mod loader, broad game support | Works with both Mono and IL2CPP |
| **HarmonyX** | Runtime method patching from C# | Prefix/Postfix/Transpiler hooks |
| **UnityExplorer** | In-game object/component inspector | Critical for understanding live behavior. Browse GameObjects, inspect components, invoke methods |
| **AssetRipper** | Extract assets back to a Unity-editable project | Textures, prefabs, scripts, scenes, audio |
| **dnSpyEx with Unity profiles** | Debug Unity managed code at runtime | Mono builds only |

### Modding Workflow

```
1. Identify target method via decompiled source
2. Install BepInEx/MelonLoader as loader
3. Write Harmony patch (Prefix/Postfix) targeting the method
4. Use UnityExplorer to verify behavior at runtime
5. AssetRipper for asset extraction if needed
```

---

## Common Issues

| Problem | Cause | Solution |
|---|---|---|
| Il2CppDumper crashes | Metadata version mismatch | Update Il2CppDumper to latest version |
| Cpp2IL produces empty DLLs | Unsupported Unity version or heavy stripping | Use Il2CppDumper dummy DLLs + Ghidra instead |
| Missing `global-metadata.dat` | Game uses encrypted metadata | Check for decryption in `UserAssembly.dll` init |
| Types show as `Il2CppClass_XYZ` | Metadata partially stripped | Use Il2CppInspector for better recovery |
