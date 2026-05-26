# Tool Catalog

> Flat catalog of all tools used by the decompilation skill.
> Organized by category. Each entry: name, URL, install command (where applicable), priority.
>
> Priority: **Required** = pipeline fails without it | **Recommended** = significantly improves results | **Optional** = nice to have

---

## Recon / PE Analysis

| Tool | URL | Install | Priority |
|---|---|---|---|
| **Detect It Easy (DiE)** — CLI: `diec` | https://github.com/horsicq/Detect-It-Easy | Download release, add to PATH | **Required** |
| **strings** | Built-in (Linux) or Sysinternals (Windows) | `sudo apt install binutils` or Sysinternals download | **Required** |
| **PEStudio** | https://www.winitor.com | Download portable | Optional |
| **pe-bear** | https://github.com/hasherezade/pe-bear | Download release | Optional |
| **CFF Explorer** | https://ntcore.com/?page_id=388 | Download installer | Optional |
| **ExeInfoPE** | http://exeinfo.atwebpages.com | Download portable | Optional |
| **file / binwalk** | Built-in (Linux) | `sudo apt install file binwalk` | Optional |
| **Resource Hacker** | http://www.angusj.com/resourcehacker/ | Download portable | Recommended |

---

## .NET Decompilers

| Tool | URL | Install | Priority |
|---|---|---|---|
| **ilspycmd** | https://github.com/icsharpcode/ILSpy | `dotnet tool install -g ilspycmd` | **Required** |
| **ICSharpCode.Decompiler** (NuGet) | https://www.nuget.org/packages/ICSharpCode.Decompiler | `dotnet add package ICSharpCode.Decompiler` | Optional |
| **dnSpyEx** (GUI + debugger) | https://github.com/dnSpyEx/dnSpy | Download release | Recommended |
| **dotPeek** (JetBrains) | https://www.jetbrains.com/decompiler/ | JetBrains installer | Optional |
| **JustDecompile** (Telerik) | https://www.telerik.com/products/decompiler.aspx | Download installer | Optional |
| **Project Rover** | https://github.com/lextudio/ProjectRover | Download release | Optional |

---

## .NET Deobfuscators (General)

| Tool | URL | Install | Priority |
|---|---|---|---|
| **de4dot** (original) | https://github.com/de4dot/de4dot | Download release | Recommended |
| **de4dot-cex** (ConfuserEx fork by ViRb3) | https://github.com/ViRb3/de4dot-cex | Download release | Recommended |
| **dnlib** | https://github.com/0xd4d/dnlib | `dotnet add package dnlib` | Recommended |
| **AsmResolver** | https://github.com/Washi1337/AsmResolver | `dotnet add package AsmResolver.DotNet` | Recommended |
| **Harmony / HarmonyX** | https://github.com/pardeike/Harmony / https://github.com/BepInEx/HarmonyX | NuGet: `Lib.Harmony` | Optional |

---

## ConfuserEx-Specific

| Tool | URL | Install | Priority |
|---|---|---|---|
| **NoFuserEx** | https://github.com/undebel/NoFuserEx | Download release | Recommended | (original XenocodeRCE repo deleted) |
| **ConfuserEx-Unpacker-2** | https://github.com/cawk/ConfuserEx-Unpacker-2 | Build from source | Recommended |
| **ConfuserEx-Dynamic-Unpacker** | https://github.com/ryancblack/ConfuserEx-Unpacker | Build from source | Optional |
| **ConfuserEx2_String_Decryptor** (Dump-GUY) | https://github.com/Dump-GUY/ConfuserEx2_String_Decryptor | Build from source | Recommended |
| **ConfuserEx2_Python_String_Decrypt** (iterasec) | https://github.com/iterasec/ConfuserEx2_Python_String_Decrypt | `pip install` + deps | Optional |
| **ConfuserEx-Static-String-Decryptor** | Listed in NotPrab catalog | Build from source | Optional |
| **ConfuserEx-Resources-Decryptor** | Listed in NotPrab catalog | Build from source | Optional |
| **ConfuserExTools** (cawk) | GitHub (cawk) | Build from source | Optional |
| **AntiTamper.Killer** | GitHub | Build from source | Optional |
| **ProxyCall-Remover** (Kaidoz) | https://github.com/Kaidoz/ProxyCall-Remover | Build from source | Recommended |
| **OldRod** (KoiVM devirtualizer) | https://github.com/Washi1337/OldRod | Build from source | Optional |

---

## Other .NET Obfuscator-Specific

| Tool | URL | Install | Priority |
|---|---|---|---|
| **NetReactorSlayer** | https://github.com/SychicBoy/NETReactorSlayer | Download release | Optional |
| **NETReactor.Unpacker** | GitHub | Build from source | Optional |
| **EazFixer / Eazfuscator-deobfuscator** | See NotPrab catalog | Build from source | Optional |
| **SmartAssembly-Decompressor / SA-Killer** | See NotPrab catalog | Build from source | Optional |
| **Babel-Deobfuscator / DeBabelVM / beeless** | See NotPrab catalog | Build from source | Optional |

Curated catalog index: https://github.com/NotPrab/.NET-Deobfuscator

---

## Embedded Assembly Extraction

| Tool | URL | Install | Priority |
|---|---|---|---|
| **CosturaUnpacker** | Community implementations on GitHub | Build from source | Optional |
| **ConfuserEx-Resources-Decryptor** | Listed in NotPrab catalog | Build from source | Optional |
| **netz decompressor** | GitHub | Build from source | Optional |
| **Custom AsmResolver/dnlib script** | Template in `dotnet-obfuscators.md` | Write inline | Recommended |

---

## Native — Delphi

| Tool | URL | Install | Priority |
|---|---|---|---|
| **IDR (Interactive Delphi Reconstructor)** | https://github.com/crypto2011/IDR | Download release | Recommended |
| **Ghidra** | https://github.com/NationalSecurityAgency/ghidra | Download release, requires JDK | **Required** (for native) |
| **dhrake** (Ghidra Delphi scripts) | https://github.com/huettenhain/dhrake | Clone into Ghidra scripts dir | Recommended |
| **IDA Pro + FLIRT Delphi signatures** | Commercial (https://hex-rays.com) | Commercial license | Optional |
| **Resource Hacker** | http://www.angusj.com/resourcehacker/ | Download portable | Recommended |
| **Ultimate Delphi Decompiler** | Commercial | Commercial license | Optional |

---

## Native — General

| Tool | URL | Install | Priority |
|---|---|---|---|
| **Ghidra** | https://github.com/NationalSecurityAgency/ghidra | Download release, requires JDK | **Required** (for native) |
| **IDA Pro + Hex-Rays** | Commercial (https://hex-rays.com) | Commercial license | Optional |
| **Binary Ninja** | Commercial (https://binary.ninja) | Commercial license | Optional |
| **radare2 / Cutter** | https://rada.re / https://cutter.re | `brew install radare2` or download | Optional |
| **angr** | https://angr.io | `pip install angr` | Optional |
| **retdec** (Avast) | https://retdec.com | Download release | Optional |
| **GoReSym** (Go binaries) | https://github.com/mandiant/GoReSym | Download release | Recommended |
| **IDAGolangHelper** | https://github.com/sibears/IDAGolangHelper | IDA plugin | Optional |

---

## Unity / IL2CPP

| Tool | URL | Install | Priority |
|---|---|---|---|
| **Il2CppDumper** | https://github.com/Perfare/Il2CppDumper | Download release | Recommended |
| **Il2CppInspector** | https://github.com/djkaty/Il2CppInspector | Download release | Optional |
| **Cpp2IL** | https://github.com/SamboyCoding/Cpp2IL | Download release | Recommended |
| **BepInEx** | https://github.com/BepInEx/BepInEx | Download release | Optional |
| **MelonLoader** | https://github.com/LavaGang/MelonLoader | Download installer | Optional |
| **UnityExplorer** | https://github.com/sinai-dev/UnityExplorer | Download release (BepInEx/MelonLoader plugin) | Optional |
| **AssetRipper** | https://github.com/AssetRipper/AssetRipper | Download release | Optional |

---

## Dynamic / Network

| Tool | URL | Install | Priority |
|---|---|---|---|
| **mitmproxy** | https://mitmproxy.org | `pip install mitmproxy` or download | Optional |
| **Fiddler Classic / Everywhere** | https://www.telerik.com/fiddler | Download installer | Optional |
| **Proxyman** | https://proxyman.io | Download (macOS primary) | Optional |
| **Wireshark** | https://www.wireshark.org | Download installer | Optional |
| **Frida** | https://frida.re | `pip install frida-tools` | Optional |
| **x64dbg** | https://x64dbg.com | Download release | Optional |
| **API Monitor** | http://www.rohitab.com/apimonitor | Download installer | Optional |
| **Process Monitor** (Sysinternals) | Microsoft Sysinternals | Download from Microsoft | Optional |

---

## Java / Android (Extended)

| Tool | URL | Install | Priority |
|---|---|---|---|
| **CFR** | https://www.benf.org/other/cfr/ | Download JAR | Optional |
| **Procyon** | https://github.com/mstrobel/procyon | Download JAR | Optional |
| **jadx** | https://github.com/skylot/jadx | Download release | Optional |
| **apktool** | https://apktool.org | Download JAR | Optional |

---

## Python (Extended)

| Tool | URL | Install | Priority |
|---|---|---|---|
| **pyinstxtractor** | https://github.com/extremecoders-re/pyinstxtractor | `python pyinstxtractor.py` | Optional |
| **uncompyle6** | https://github.com/rocky/python-uncompyle6 | `pip install uncompyle6` | Optional |
| **pycdc** (Python 3.10+) | https://github.com/zrax/pycdc | Build from source | Optional |

---

## Minimum Required Set

For the skill to function at a basic level, these must be installed:

1. `diec` (Detect It Easy CLI) — recon phase
2. `ilspycmd` — .NET decompilation
3. `strings` — raw string extraction

Everything else enhances capability but the skill can produce useful output with just these three.
