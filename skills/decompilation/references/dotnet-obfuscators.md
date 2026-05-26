# Other .NET Obfuscators — Routing Table

> Loaded by SKILL.md when `recon.json.obfuscator` is NOT ConfuserEx but IS a known .NET obfuscator.
> For ConfuserEx, see `dotnet-confuserex.md` instead.

---

## Obfuscator Routing Table

| Obfuscator | Primary Tool | Fallback | Notes |
|---|---|---|---|
| **Eazfuscator.NET** | EazFixer, EazFix | de4dot, Eazfuscator-deobfuscator | String decryption + virtualization removal. Commercial obfuscator, varies significantly between versions |
| **SmartAssembly** | SA-Killer, SmartAssembly-Decompressor | de4dot | Resource decompression, member rename. Older versions well-handled by de4dot |
| **Babel.NET** | DeBabelVM, Babel-Deobfuscator | beeless | Has a VM in newer versions — DeBabelVM addresses that. beeless for older non-VM variants |
| **.NET Reactor** | NetReactorSlayer | NETReactor.Unpacker | NetReactorSlayer is the modern actively-maintained tool |
| **Agile.NET (CliSecure)** | de4dot (built-in) | — | One of the few obfuscators de4dot still handles well natively |
| **KoiVM** (ConfuserEx VM) | OldRod | — | Devirtualizer. Without it, KoiVM-protected methods are unreadable black boxes |
| **Themida / VMProtect** wrapping .NET | Dynamic dump from memory | dnSpyEx breakpoint after unpack stub | Static analysis effectively impossible. Must run the binary and dump after unpacking |
| **No obfuscator** | (skip to decompile) | — | Most internal/enterprise tools. Go straight to ilspycmd |

---

## General Strategy

1. Check `recon.json.obfuscator` field
2. Look up primary tool in table above
3. Run primary tool on the target
4. If primary fails, try fallback
5. If both fail, try generic `de4dot` with auto-detect: `de4dot-x64.exe target.dll`
6. If all fail, proceed to decompile anyway (partial results are better than none)
7. Always run `de4dot --rename` as final cleanup pass

---

## Community Catalog

For protections not listed above, consult the curated index:

**NotPrab/.NET-Deobfuscator**: https://github.com/NotPrab/.NET-Deobfuscator

This repository catalogs deobfuscation tools organized by target obfuscator. Check it when
encountering an unknown or uncommon protection scheme.

---

## Embedded Assembly Extraction

When `recon.json.embedded_assemblies_suspected` is true, extract embedded assemblies
BEFORE deep analysis of the host.

### Detection Signals

| Signal | Indicator | Extraction Method |
|---|---|---|
| **Costura.Fody** | Resources named `costura.<name>.dll.compressed`, GZip-encoded. Module ctor adds `AppDomain.CurrentDomain.AssemblyResolve += ...` | CosturaUnpacker or AsmResolver script |
| **ILRepack / ILMerge** | No resources, but multiple incongruous root namespaces fused into one module (e.g., `Area.Admin.*` inside `WebMapCore`) | Manual separation or ilspycmd will show merged namespaces |
| **Custom loader** | Encrypted resource + decryption stub in module ctor. ConfuserEx Resources protection is one variant | ConfuserEx-Resources-Decryptor or dynamic extraction |
| **NETZ packer** | NETZ signature in resources | `netz` decompressor |

### Extraction Tools

| Tool | Purpose |
|---|---|
| **CosturaUnpacker** | Auto-extracts gzipped Costura DLLs |
| **ConfuserEx-Resources-Decryptor** | Decrypts ConfuserEx-protected resources |
| **dnSpyEx (GUI)** | Resources tree → Save — manual but fast for one-off |
| **netz decompressor** | Older NETZ-packed assemblies |
| **AsmResolver / dnlib script** | Universal fallback (see template below) |

### AsmResolver Extraction Script Template

```csharp
using AsmResolver.DotNet;
using System.IO.Compression;

var module = ModuleDefinition.FromFile(args[0]);
var outDir = args.Length > 1 ? args[1] : "extracted";
Directory.CreateDirectory(outDir);

foreach (var res in module.Resources)
{
    if (res is not EmbeddedResource emb) continue;
    var bytes = emb.GetData();

    // Try gzip decompression (Costura pattern)
    try
    {
        using var ms = new MemoryStream(bytes);
        using var gz = new GZipStream(ms, CompressionMode.Decompress);
        using var outMs = new MemoryStream();
        gz.CopyTo(outMs);
        bytes = outMs.ToArray();
    }
    catch { /* not gzipped, use raw bytes */ }

    // Check for MZ header (PE / DLL)
    if (bytes.Length >= 2 && bytes[0] == 'M' && bytes[1] == 'Z')
    {
        var name = res.Name?.ToString() ?? "unknown";
        // Clean up Costura naming: costura.foo.dll.compressed → foo.dll
        name = name.Replace("costura.", "").Replace(".compressed", "");
        if (!name.EndsWith(".dll")) name += ".dll";
        File.WriteAllBytes(Path.Combine(outDir, name), bytes);
        Console.WriteLine($"[+] Extracted: {name} ({bytes.Length} bytes)");
    }
}
```

### Post-Extraction

Each extracted child assembly goes through the SAME pipeline from Phase 0 (recon).
The child may itself be obfuscated — the pipeline must detect and handle recursively.

Output structure for extracted assemblies:
```
extracted/
├── Area.Admin/
│   ├── original/
│   ├── recon.json
│   ├── intermediate/
│   ├── src/
│   └── ...
└── OtherLib/
    └── ...
```
