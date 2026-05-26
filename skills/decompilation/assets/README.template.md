# Decompilation Report: {{TARGET_NAME}}

## Binary Info
- **Path:** {{ORIGINAL_PATH}}
- **Size:** {{SIZE_BYTES}} bytes
- **SHA256:** {{SHA256}}
- **Kind:** {{KIND}}
- **Runtime:** {{RUNTIME}}
- **Obfuscator:** {{OBFUSCATOR}}

## Pipeline Summary
{{PIPELINE_STEPS}}

## What Worked
{{SUCCESS_NOTES}}

## What Failed / Was Skipped
{{FAILURE_NOTES}}

## Output Structure
{{OUTPUT_TREE}}

## Notes for AI Agents
- Decompiled source is in `src/` — one file per type, organized by namespace
- String constants (including decrypted) are in `strings/decrypted.tsv` (token<TAB>value)
- API endpoints are pre-extracted in `metadata/api_surface.json`
- Type index is in `metadata/index.json` — use for navigation
- Intermediate binaries preserved in `intermediate/` for re-processing
- If code looks broken (async state machines, closure classes) — it's correct decompiler output, not a bug
