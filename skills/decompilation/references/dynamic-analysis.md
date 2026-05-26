# Dynamic Analysis Fallback

> Loaded by SKILL.md when static pipeline stalls or when runtime behavior inspection is needed.
> Triggered when static deobfuscation tools fail or when traffic/runtime data is required.

---

## Three Levels of Dynamic Analysis

| Level | Scope | Tools | When to Use |
|---|---|---|---|
| **Traffic capture** | Network layer | mitmproxy, Fiddler, Proxyman, Wireshark | Need endpoints, headers, request/response shapes. Least invasive |
| **.NET runtime hook** | Managed code | dnSpyEx debugger, Harmony/HarmonyX, Frida-CLR | Need decrypted values at runtime, method return values, string decryptor output |
| **Native / deep hook** | Process level | Frida, x64dbg, gdb/lldb, API Monitor, Process Monitor | Anti-debug bypasses, packed native code, kernel-level inspection |

Always start at the lowest level that can answer the question. Escalate only when needed.

---

## Level 1: Traffic Capture

### mitmproxy (Recommended for Scripting)

```bash
# Start proxy
mitmproxy --listen-port 8080

# Or use mitmdump for non-interactive capture
mitmdump -w output/traffic/capture.flow --listen-port 8080

# Convert to readable format
mitmdump -r output/traffic/capture.flow --set flow_detail=3 > output/traffic/requests.txt
```

Configure the target application to use the proxy (system proxy or app-specific settings).

### Fiddler

GUI-first. Use Fiddler Classic (Windows) or Fiddler Everywhere (cross-platform).
Export sessions as HAR for machine processing.

### Wireshark + SSLKEYLOGFILE

```bash
# Set environment variable before launching the target
export SSLKEYLOGFILE=output/traffic/sslkeys.log

# Start target application...

# Wireshark can decrypt TLS using the key log
wireshark -o ssl.keylog_file:output/traffic/sslkeys.log -r capture.pcap
```

Works with Chromium-based apps, .NET on Linux, and apps that honor the env variable.

---

## Level 2: .NET Runtime Hooks

### dnSpyEx Debugger

1. Open the target DLL in dnSpyEx
2. Set breakpoints on suspected decryption methods or API calls
3. Run the application via Debug → Start
4. Inspect local variables, return values, and decrypted strings at breakpoints

Best for: one-off investigation, understanding control flow, verifying decryption output.

### Harmony / HarmonyX (Programmatic)

```csharp
// Patch a method to log its inputs and outputs
[HarmonyPatch(typeof(TargetClass), "DecryptString")]
class DecryptStringPatch
{
    static void Postfix(int __0, ref string __result)
    {
        Console.WriteLine($"DecryptString({__0}) => \"{__result}\"");
    }
}
```

Best for: systematic capture of all decrypted values during normal execution.

### Frida-CLR

```javascript
// Attach to running .NET process
CLR.perform(() => {
    const method = CLR.use("TargetAssembly.Module::DecryptString");
    method.implementation = function(token) {
        const result = this.original(token);
        console.log(`Decrypt(${token}) => ${result}`);
        return result;
    };
});
```

---

## Level 3: Native / Deep Hooks

### Frida (General)

```bash
# Attach to running process
frida -p <PID> -l hook_script.js

# Or spawn with instrumentation
frida -f target.exe -l hook_script.js --no-pause
```

### x64dbg

1. Open target in x64dbg
2. Set breakpoints on suspected crypto functions or API calls
3. Use conditional logging breakpoints for automated capture
4. Script via x64dbg scripting engine for batch operations

### API Monitor

- Trace Windows API calls (file, registry, network, crypto)
- Filter by API category to reduce noise
- Export as XML for machine processing

### Process Monitor (Sysinternals)

- File system, registry, network, process/thread activity
- Filter by process name
- Export as CSV for analysis

---

## ConfuserEx Dynamic String Decryption Pattern

When static string decryptors fail on customized ConfuserEx, use dynamic invocation.

### Pseudocode

```
1. Load target assembly into AppDomain via Assembly.LoadFrom()
   - This executes the module constructor (initializes decryption state)
   - WARNING: executes target code — sandbox required

2. Locate decryption method heuristically:
   - Signature: takes single int parameter, returns string
   - Many cross-references (called from dozens/hundreds of sites)
   - Usually in <Module> class or a single utility class
   - Often has control-flow obfuscation internally

3. Collect every int argument from cross-reference sites:
   - Use dnlib/AsmResolver to statically scan all methods
   - Find pattern: ldc.i4 <N> → call <decryptor>
   - Build list of all unique int tokens

4. Invoke decryptor for each token:
   - Use Reflection: MethodInfo.Invoke(null, new object[] { token })
   - Build Dictionary<int, string> mapping
   - Log each: token → decrypted string

5. Rewrite IL in the assembly:
   - Replace each [ldc.i4 N + call decryptor] pair with [ldstr "value"]
   - This produces a clean assembly with all strings inlined

6. Save patched DLL and dump string table to TSV
```

See `dotnet-confuserex.md` for the full C# implementation template.

---

## Security Warnings

### CRITICAL: Dynamic analysis executes target code

| Risk | Mitigation |
|---|---|
| Target binary may be malware | Run in an isolated VM or sandbox |
| `Assembly.LoadFrom` executes module constructors | May trigger anti-analysis, data exfiltration, or system modification |
| Frida injection modifies process memory | Only use on processes you own/control |
| mitmproxy intercepts TLS | Only use on your own traffic |

### Mandatory Checklist Before Dynamic Analysis

1. **WARN the user** that dynamic analysis will execute target code
2. **Recommend VM/sandbox** — never run untrusted binaries on the host
3. **Get explicit user confirmation** before proceeding
4. **Log all dynamic actions** to `pipeline.log`
5. **Never auto-run** dynamic invoke without user consent

### Legal Considerations

- Dynamic analysis of software you own or have authorization to test: OK
- Bypassing DRM/licensing protections: NOT OK — decline the request
- Malware analysis in a sandbox for security research: OK with appropriate caution
