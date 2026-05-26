---
name: decompile
description: Decompile and analyze a binary file
argument-hint: <path-to-binary-or-folder>
allowed-tools: [Bash, PowerShell, Read, Write, Edit, Glob, Grep, Agent]
---

# /decompile Command

Accepts a path to a binary file (.dll, .exe) or game folder and produces a structured decompilation output.

## Workflow

1. **Validate input** — confirm the path exists and is a file or folder. Resolve to absolute path.

2. **Create output directory** — `<target-name>/` next to the target (or user-specified location). Copy original into `<target-name>/original/`.

3. **Run Phase 0 Recon:**
   ```powershell
   & "<skill-root>/scripts/recon.ps1" -Target "<target-path>" -OutputDir "<output-dir>"
   ```
   Read the resulting `recon.json`. If exit code 1, report missing tools and stop. If exit code 2, proceed with fallback data.

4. **Route by `recon.kind`** — follow the routing tree defined in SKILL.md:
   - `managed` / `unity_mono` — deobfuscate if needed, extract embedded, decompile with ilspycmd
   - `unity_il2cpp` — Il2CppDumper + Cpp2IL + Ghidra
   - `native` — Delphi (IDR + Ghidra) or general (Ghidra headless)
   - For each branch, Read the corresponding `references/*.md` file for detailed instructions.

5. **Build indexes:**
   ```powershell
   & "<skill-root>/scripts/build-indexes.ps1" -SourceDir "<output-dir>/src" -OutputDir "<output-dir>/metadata"
   ```

6. **Write README.md** from template in `assets/README.template.md` with pipeline results.

7. **Report to user** — summarize what was found: binary type, obfuscator (if any), key outputs, any failed steps from `pipeline.log`.

## Flags

- `--minimal` — skip deep analysis, run only: recon + strings extraction + direct decompile.

## Error Handling

- Every step logs to `pipeline.log` with exit code and stderr.
- If a tool is missing, report `INSTALL_REQUIRED` message and stop gracefully.
- Never silently skip failed steps.
