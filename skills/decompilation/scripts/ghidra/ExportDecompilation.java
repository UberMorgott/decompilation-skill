// ExportDecompilation.java — Ghidra headless script
// Exports decompiled pseudo-C for each function to a separate .c file
// Usage: analyzeHeadless ... -postScript ExportDecompilation.java <outputDir>
import ghidra.app.script.GhidraScript;
import ghidra.app.decompiler.*;
import ghidra.program.model.listing.*;
import java.io.*;

public class ExportDecompilation extends GhidraScript {
    @Override
    public void run() throws Exception {
        String[] args = getScriptArgs();
        if (args.length < 1) {
            println("Usage: ExportDecompilation.java <outputDir>");
            return;
        }
        String outputDir = args[0];
        File outDir = new File(outputDir);
        outDir.mkdirs();
        if (!outDir.isDirectory()) {
            println("ERROR: Cannot create output directory: " + outputDir);
            return;
        }

        DecompInterface decomp = new DecompInterface();
        decomp.openProgram(currentProgram);

        try {
            FunctionIterator funcs = currentProgram.getFunctionManager().getFunctions(true);
            int count = 0;
            int errors = 0;

            while (funcs.hasNext() && !monitor.isCancelled()) {
                Function func = funcs.next();
                String name = func.getName();
                String addr = func.getEntryPoint().toString();

                // Sanitize filename
                String safeName = name.replaceAll("[^a-zA-Z0-9_.]", "_");
                String fileName = safeName + "_" + addr + ".c";

                try {
                    DecompileResults results = decomp.decompileFunction(func, 30, monitor);
                    if (results.getDecompiledFunction() != null) {
                        String code = results.getDecompiledFunction().getC();
                        File outFile = new File(outputDir, fileName);
                        try (PrintWriter pw = new PrintWriter(outFile)) {
                            pw.println("// Function: " + name);
                            pw.println("// Address: " + addr);
                            pw.println();
                            pw.println(code);
                        }
                        count++;
                    } else {
                        errors++;
                    }
                } catch (Exception e) {
                    errors++;
                }
            }

            println("Exported " + count + " functions (" + errors + " errors) to " + outputDir);
        } finally {
            decomp.dispose();
        }
    }
}
