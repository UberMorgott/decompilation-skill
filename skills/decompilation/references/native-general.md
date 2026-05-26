# General Native Binary Decompilation

> Loaded by SKILL.md when `recon.json.kind == "native"` and compiler is NOT Delphi.
> Covers C, C++, Go, Rust, and other native runtimes.

---

## Default Pipeline: Ghidra Headless

The default approach for any native binary without specialized tooling.

### analyzeHeadless Command

```bash
# Step 1: Import and analyze
analyzeHeadless ./output/native/ghidra_project ProjName \
    -import target.exe \
    -postScript ExportDecompilation.java output/native/functions/

# For existing project (re-process):
analyzeHeadless ./output/native/ghidra_project ProjName \
    -process target.exe \
    -postScript ExportDecompilation.java output/native/functions/
```

### Key Flags

| Flag | Purpose |
|---|---|
| `-import <file>` | Import binary into new project |
| `-process <file>` | Re-analyze already imported binary |
| `-postScript <script> [args]` | Run script after analysis |
| `-scriptPath <dir>` | Additional script search directories |
| `-analysisTimeoutPerFile <seconds>` | Timeout for large binaries (default: 300) |
| `-deleteProject` | Remove project after export (saves disk) |

### Output

Produces `native/functions/` with one `.c` file per function containing Ghidra pseudo-C.

---

## Go Binaries

Go binaries are statically linked and contain embedded type/symbol information.

### Detection

- `recon.json.runtime == "go"` or DiE detects Go compiler
- Large binary size (10-50MB+ for simple programs) due to static linking
- Presence of `runtime.` and `main.` symbol prefixes

### Pipeline: GoReSym → Ghidra

```bash
# Step 1: Extract Go symbols and types
GoReSym -t -d -p target.exe > output/native/go_symbols.json

# Step 2: Import into Ghidra with symbols
analyzeHeadless ./output/native/ghidra_project GoProj \
    -import target.exe \
    -postScript ImportGoSymbols.py output/native/go_symbols.json \
    -postScript ExportDecompilation.java output/native/functions/
```

GoReSym recovers:
- Function names and addresses
- Type definitions (structs, interfaces)
- Package structure
- Source file paths (if not stripped)

---

## Rust Binaries

### Detection

- DiE identifies Rust/LLVM compiler
- Presence of `.debug` section (if not stripped)
- Mangled symbols with `_ZN` prefix or `h` hash suffix

### Pipeline

```bash
# Check if debug info is present
readelf -S target.exe | grep .debug  # Linux
# or check via Ghidra's binary info

# If debug info present: Ghidra will auto-apply it
analyzeHeadless ./output/native/ghidra_project RustProj \
    -import target.exe \
    -postScript ExportDecompilation.java output/native/functions/
```

Tips:
- Rust binaries are usually statically linked and very large
- The standard library makes up most of the binary — focus on non-`std::` functions
- Ghidra Rust plugin (if available) improves demangling and struct recovery
- `rustfilt` can demangle Rust symbols: `cat symbols.txt | rustfilt`

---

## Other Runtimes (Brief)

### Java / Kotlin (JVM)

```bash
# CFR decompiler (best for modern Java)
java -jar cfr.jar target.jar --outputdir output/src/

# jadx (Android DEX → Java, also handles JAR)
jadx -d output/src/ target.apk

# apktool (Android resources + smali)
apktool d target.apk -o output/apktool/
```

### Python (PyInstaller / py2exe)

```bash
# Step 1: Extract from PyInstaller bundle
python pyinstxtractor.py target.exe
# Produces: target.exe_extracted/ with .pyc files

# Step 2: Decompile .pyc to .py
# Python < 3.9:
uncompyle6 -o output/src/ target.exe_extracted/*.pyc

# Python 3.10+:
pycdc target.exe_extracted/main.pyc > output/src/main.py
```

### Electron / Node.js

```bash
# Extract asar archive
npx asar extract resources/app.asar output/src/

# The JS source is now directly readable in output/src/
# Check package.json for entry point
# Inspect node_modules/ for dependency graph
```

---

## Output Structure

```
output/
├── native/
│   ├── ghidra_project/       # Ghidra project files
│   ├── functions/            # One .c file per function (pseudo-C)
│   ├── go_symbols.json       # GoReSym output (Go only)
│   └── symbols.idc           # Symbol export (if applicable)
└── strings/
    └── all_strings.txt       # Raw strings dump
```
