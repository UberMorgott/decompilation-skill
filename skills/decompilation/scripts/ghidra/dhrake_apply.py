# dhrake_apply.py — Import IDR-exported .idc symbols into Ghidra
# Usage: analyzeHeadless ... -postScript dhrake_apply.py <path-to-symbols.idc>
# @category Delphi
import re
from ghidra.program.model.symbol import SourceType

args = getScriptArgs()
if len(args) < 1:
    print("Usage: dhrake_apply.py <path-to-idc-file>")
else:
    idc_path = args[0]
    count = 0
    with open(idc_path, 'r') as f:
        for line in f:
            m = re.match(r'MakeName\s*\(\s*0x([0-9A-Fa-f]+)\s*,\s*"([^"]+)"\s*\)', line)
            if m:
                addr_str, name = m.group(1), m.group(2)
                addr = toAddr(long(addr_str, 16))
                if addr is not None:
                    createLabel(addr, name, True)
                    count += 1
    print("Applied {} IDR symbols from {}".format(count, idc_path))
