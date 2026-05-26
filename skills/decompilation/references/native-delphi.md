# Native Delphi Decompilation Pipeline

> Loaded by SKILL.md when `recon.json.kind == "native"` and `recon.json.compiler` starts with `Embarcadero Delphi`.

---

## Pipeline Overview

```
IDR (RTTI, VCL names, .idc export)
  → Ghidra + dhrake (.idc import, VMT structs)
    → Resource Hacker (.dfm forms, version info)
      → Export per-function pseudo-C
```

---

## Tool Table

| Tool | Role | CLI? | Notes |
|---|---|---|---|
| **IDR (Interactive Delphi Reconstructor)** | First pass. Restores RTTI, VCL classes, method names. Exports `.idc` and `.map` files | Partial | Best for Delphi 2..XE4, partial support for XE5+. Windows-only |
| **Ghidra** | Main decompiler. Free, cross-platform, headless mode | Yes (`analyzeHeadless`) | Primary decompilation engine after IDR annotation |
| **dhrake** (Ghidra plugin) | Imports IDR's `.idc`, names symbols, creates VMT structs in Ghidra | Via Ghidra script | Must be run as Ghidra post-analysis script |
| **IDA Pro + FLIRT Delphi signatures** | Commercial; best results on modern Delphi (XE5+) | Yes | Same concept as dhrake but inside IDA. Use if available |
| **Binary Ninja + Delphi plugins** | Alternative commercial platform | Yes | Python scripting API for automation |
| **Ultimate Delphi Decompiler** | Commercial; sometimes produces higher-level Object Pascal output | No | Expensive, niche use |
| **Resource Hacker** | Extracts `.dfm` forms, string tables, icons, version info | Yes | Critical for understanding UI structure of Delphi apps |

---

## Step-by-Step Commands

### 1. IDR — Extract RTTI and Symbols

```bash
# Run IDR in auto mode (Windows-only, or via Wine)
idr64.exe -auto target.exe -save-idc output/native/symbols.idc -save-map output/native/symbols.map
```

IDR output:
- `.idc` — IDA/Ghidra-compatible script with symbol names, VMT structures, RTTI info
- `.map` — address-to-name mapping

For interactive use, open IDR GUI, load the binary, let it analyze, then File → Save IDC.

### 2. Ghidra Headless + dhrake

```bash
# Create project and import binary
analyzeHeadless ./output/native/ghidra_project DelphiProj \
    -import target.exe \
    -postScript dhrake_apply.py output/native/symbols.idc

# Export decompiled functions (one .c file per function)
analyzeHeadless ./output/native/ghidra_project DelphiProj \
    -process target.exe \
    -postScript ExportDecompilation.java output/native/functions/
```

The `dhrake_apply.py` script:
- Imports IDR symbol names into Ghidra's symbol table
- Creates VMT (Virtual Method Table) struct definitions
- Applies Delphi calling conventions (register-based: EAX, EDX, ECX)
- Names TForm and TFrame classes from RTTI

### 3. Resource Hacker — Extract Forms and Resources

```bash
# Extract all resources to a folder
ResourceHacker.exe -open target.exe -save output/resources/ -action extract

# Extract specific DFM forms
ResourceHacker.exe -open target.exe -save output/resources/forms.rc -action extract -mask RCDATA,
```

DFM forms contain:
- UI layout (component hierarchy, positions, sizes)
- Property values (captions, colors, fonts)
- Event handler names (maps to methods IDR found)

---

## Output Structure

```
output/
├── native/
│   ├── ghidra_project/          # Ghidra project files
│   ├── functions/               # One .c file per decompiled function
│   └── symbols.idc              # IDR-exported symbol script
├── resources/
│   ├── forms/                   # Extracted .dfm files
│   ├── strings/                 # String tables
│   └── version_info.txt         # Version resource
└── strings/
    └── all_strings.txt          # Raw strings dump
```

---

## Tips

- **Delphi calling convention**: register-based (first 3 params in EAX, EDX, ECX). Ghidra may misidentify as `__stdcall` — dhrake corrects this.
- **String encoding**: Delphi uses `AnsiString` (pre-2009) or `UnicodeString` (2009+). The `strings` tool should use `-e l` for UTF-16LE on modern Delphi.
- **VCL event handlers**: methods named `Button1Click`, `FormCreate` etc. map to UI events. Cross-reference with DFM forms.
- **Large binaries**: Delphi statically links the VCL runtime (~10-15MB). Most of the binary is library code. Focus on user-defined units.
