# Asset Validator

AssetValidator is a cross-engine asset validation tool designed for game development pipelines.

Assets are exported from Unreal Engine to JSON and validated using a shared rule engine.
Validation can be run via CLI (CI-friendly) or UI (human inspection).

## Overview

```
Unreal Editor → JSON → AssetValidator (Core) → Results  
                     → CLI (automation / CI)  
                     → UI (visual inspection)
```

The validation engine is engine-agnostic and operates purely on serialized asset data.

## Projects

- AssetValidator.Core  
  Domain model and validation engine.

- AssetValidator.Cli  
  Command-line interface for automation and CI usage.

- AssetValidator.Ui  
  Desktop UI for visual inspection of validation results.

- AssetValidatorEditor (UE)  
  Unreal Engine editor module that exports asset metadata to JSON.

## CLI Usage

Validate assets:

    AssetValidator.Cli --input examples/assets.valid.json

Output validation results as JSON:

    AssetValidator.Cli --input examples/assets.invalid.json --print-out-as-json

### Exit Codes

| Code | Meaning |
| ---- | ------- |
| 0 | Validation passed |
| 1 | Validation errors found |
| 2 | Tool failure (IO / parsing error) |

## Examples

See the `examples/` directory for sample asset JSON files.

- `assets_valid.json` – all assets pass validation
- `assets_invalid.json` – validation completes with errors
- `assets_malformed.json` – invalid input file (validation does not run)

## Unreal Engine Integration

The Unreal Editor module exports asset metadata to JSON for external validation.
Validation is intentionally performed outside the engine to keep the rules
engine-agnostic and CI-friendly.

## Design Goals

- Engine-agnostic validation
- Deterministic and testable rules
- CI-friendly CLI interface
- Clear separation between data export and validation
