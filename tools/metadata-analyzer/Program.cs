using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

var target = args[0];
using var stream = File.OpenRead(target);
using var peReader = new PEReader(stream);

if (!peReader.HasMetadata) { Console.WriteLine("NO_METADATA"); return; }

var mdReader = peReader.GetMetadataReader();
int unicodeCount = 0, totalTypes = 0, moduleMethodCount = 0;
bool hasCctor = false;
var sampleNames = new List<string>();

foreach (var th in mdReader.TypeDefinitions) {
    var td = mdReader.GetTypeDefinition(th);
    var name = mdReader.GetString(td.Name);
    var ns = mdReader.GetString(td.Namespace);
    totalTypes++;
    bool isUnicode = name.Any(c => c > 127 || (c < 32 && c != 0));
    bool isNonWord = name.All(c => !char.IsLetterOrDigit(c) && c != '_' && c != '<' && c != '>');
    if (isUnicode || isNonWord) {
        unicodeCount++;
        if (sampleNames.Count < 10) sampleNames.Add($"{ns}.{name}");
    }
    if (name == "<Module>") {
        foreach (var mh in td.GetMethods()) {
            moduleMethodCount++;
            var md = mdReader.GetMethodDefinition(mh);
            if (mdReader.GetString(md.Name) == ".cctor") hasCctor = true;
        }
    }
}

Console.WriteLine($"TOTAL_TYPES:{totalTypes}");
Console.WriteLine($"UNICODE_TYPES:{unicodeCount}");
Console.WriteLine($"RATIO:{(totalTypes > 0 ? Math.Round(unicodeCount * 100.0 / totalTypes, 1) : 0)}");
Console.WriteLine($"MODULE_METHODS:{moduleMethodCount}");
Console.WriteLine($"MODULE_CCTOR:{hasCctor}");
foreach (var s in sampleNames) Console.WriteLine($"SAMPLE:{s}");
