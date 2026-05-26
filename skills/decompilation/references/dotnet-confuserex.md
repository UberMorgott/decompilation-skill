# ConfuserEx / ConfuserEx2 Deobfuscation Pipeline

> Loaded by SKILL.md when `recon.json.obfuscator` matches `ConfuserEx*`.

---

## Canonical Order

Order matters. Each phase depends on the previous one being clean.

```
unpack → anti-tamper removal → string decryption → control flow → proxy removal → rename → decompile
```

Skipping or reordering phases leads to broken IL, failed decompilation, or silent data loss.

---

## Tool Table

| Tool | Phase | Notes |
|---|---|---|
| **ConfuserEx-Unpacker-2** | unpack | Emulation-based, handles most advanced configs |
| **ConfuserEx-Dynamic-Unpacker** | unpack | Invoke-based, works on modified ConfuserEx |
| **ConfuserEx-Unpacker-Mod-By-Bed** | unpack | Wider compatibility for non-vanilla variants |
| **ConfuserExTools (cawk)** | anti-tamper + ref proxy | Handles basic AntiTamper, ReferenceProxy, Constants |
| **AntiTamper.Killer** | anti-tamper | Targeted, when unpackers don't handle it cleanly |
| **ConfuserEx2_String_Decryptor (Dump-GUY)** | strings | Best for advanced/modified ConfuserEx. AsmResolver + Harmony2 + Reflection. Patches IL to inline decrypted strings |
| **ConfuserEx2_Python_String_Decrypt (iterasec)** | strings | Python automation, easier to integrate in scripts |
| **ConfuserEx-Static-String-Decryptor** | strings | Pure static. Only works on vanilla ConfuserEx. Try first as fast path |
| **ConfuserEx-Resources-Decryptor** | resources | Decrypts ConfuserEx-protected `.resources` blobs |
| **de4dot-cex (ViRb3 fork)** | ctrl flow + rename | `de4dot-x64.exe target.dll -p crx` — the right fork for ConfuserEx 2. Original de4dot does NOT handle ConfuserEx properly |
| **ProxyCall-Remover (Kaidoz)** | proxy | Inlines `<Module>.smethod_X<T>(token)` wrappers |
| **NoFuserEx** | full chain | All-in-one for vanilla ConfuserEx. Try as quick first attempt |

---

## Decision Tree

```
1. Try NoFuserEx (all-in-one, fast path)
   ├── SUCCESS → skip to step 6 (rename)
   └── FAIL ↓

2. Staged pipeline:
   a. ConfuserEx-Unpacker-2 → intermediate/01_unpacked.dll
   b. ConfuserEx-Static-String-Decryptor → intermediate/02_strings_decrypted.dll
      ├── SUCCESS → continue to step c
      └── FAIL → try ConfuserEx2_String_Decryptor (Dump-GUY)
          ├── SUCCESS → continue to step c
          └── FAIL → Dynamic invoke fallback (see below)
   c. de4dot-cex -p crx → intermediate/03_cflow_clean.dll
   d. ProxyCall-Remover → intermediate/04_proxies_removed.dll

3. If ALL static tools fail → dynamic analysis (see dynamic-analysis.md)
```

---

## Dynamic Invoke Fallback — C# Template

When no public static tool handles the customization, write a ~30-line C# program using
AsmResolver + Reflection that executes the target's own decryption method.

```csharp
// dynamic-string-decrypt.cs — Universal ConfuserEx string decryptor via Reflection
// WARNING: This EXECUTES code from the target binary. Run in a VM/sandbox.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

var targetPath = args[0];
var outputPath = args.Length > 1 ? args[1] : Path.ChangeExtension(targetPath, ".decrypted.dll");

// 1. Load via Reflection (executes module ctor — sandbox!)
var asm = Assembly.LoadFrom(Path.GetFullPath(targetPath));

// 2. Load via AsmResolver for IL rewriting
var module = ModuleDefinition.FromFile(targetPath);

// 3. Locate decryption method: single int param, returns string, many call sites
var candidates = module.GetAllTypes()
    .SelectMany(t => t.Methods)
    .Where(m => m.Signature?.ReturnType.FullName == "System.String"
             && m.Signature.GetParameterCount() == 1
             && m.CilMethodBody != null)
    .OrderByDescending(m => CountCallSites(module, m))
    .ToList();

var decryptorDef = candidates.FirstOrDefault()
    ?? throw new Exception("Cannot locate decryption method heuristically");

Console.WriteLine($"[*] Decryptor: {decryptorDef.FullName}");

// 4. Resolve via Reflection
var decryptorInfo = asm.GetType(decryptorDef.DeclaringType!.FullName)!
    .GetMethod(decryptorDef.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)!;

// 5. Walk call sites, invoke, collect table
var table = new Dictionary<int, string>();
foreach (var type in module.GetAllTypes())
foreach (var method in type.Methods.Where(m => m.CilMethodBody != null))
{
    var instrs = method.CilMethodBody!.Instructions;
    for (int i = 1; i < instrs.Count; i++)
    {
        if (instrs[i].OpCode == CilOpCodes.Call
            && instrs[i].Operand is IMethodDescriptor md
            && md.FullName == decryptorDef.FullName
            && instrs[i - 1].IsLdcI4())
        {
            int token = instrs[i - 1].GetLdcI4Constant();
            if (!table.ContainsKey(token))
            {
                try { table[token] = (string)decryptorInfo.Invoke(null, new object[] { token })!; }
                catch { /* skip failed tokens */ }
            }
            // 6. Rewrite IL: replace ldc.i4 + call with ldstr
            instrs[i - 1].OpCode = CilOpCodes.Nop;
            instrs[i].OpCode = CilOpCodes.Ldstr;
            instrs[i].Operand = table[token];
        }
    }
}

// 7. Save
module.Write(outputPath);
Console.WriteLine($"[+] Decrypted {table.Count} strings → {outputPath}");

// Dump TSV for strings/decrypted.tsv
File.WriteAllLines(
    Path.ChangeExtension(outputPath, ".tsv"),
    table.Select(kv => $"{kv.Key}\t{kv.Value}"));

static int CountCallSites(ModuleDefinition mod, MethodDefinition target)
{
    return mod.GetAllTypes()
        .SelectMany(t => t.Methods)
        .Where(m => m.CilMethodBody != null)
        .SelectMany(m => m.CilMethodBody!.Instructions)
        .Count(i => i.OpCode == CilOpCodes.Call
                  && i.Operand is IMethodDescriptor md
                  && md.FullName == target.FullName);
}
```

**Security gate:** This script literally executes target code via `Assembly.LoadFrom`.
MUST warn the user. MUST recommend VM/sandbox. MUST NOT auto-run without confirmation.

---

## Universal Symbol Rename Pass

After deobfuscation, names like `_3F35B72E` or `\u0001\u0002` remain unreadable.
Run a rename pass to humanize them:

```bash
de4dot-x64.exe intermediate/04_proxies_removed.dll --rename -o intermediate/05_renamed.dll
```

This is a single-line step that hugely improves agent readability of the decompiled output.

---

## Final Decompile

```bash
ilspycmd intermediate/05_renamed.dll -p -o ./src --genpdb
```

- `-p` (project mode): writes a real `.csproj` with files split by namespace/type
- `--genpdb`: generates PDB for debugger attachment
- Output goes to `src/` in the output directory

---

## Full Pipeline Script (PowerShell)

See `scripts/recipe-dotnet-confuserex.ps1` for the automated version that:
1. Creates output directory structure
2. Copies original to `original/`
3. Runs each phase with fallback logic
4. Logs every step to `pipeline.log`
5. Extracts embedded assemblies and recurses
6. Decompiles to project layout

---

## Common Failure Modes

| Symptom | Cause | Fix |
|---|---|---|
| Unpacker produces 0-byte DLL | Anti-tamper not removed first | Run AntiTamper.Killer before unpacker |
| String decryptor crashes with `BadImageFormatException` | x86/x64 mismatch | Use matching bitness (de4dot-x64 vs de4dot-x86) |
| de4dot-cex reports "Unknown cflow deobfuscator" | Not using the ViRb3 fork | Install de4dot-cex, not vanilla de4dot |
| Decompiled code still has `smethod_X<T>` wrappers | Proxy removal was skipped | Run ProxyCall-Remover before decompile |
| ilspycmd output has `/* Error */` blocks | Corrupted IL from aggressive patching | Try decompiling an earlier intermediate |
