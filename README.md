
# Asset Validator

Asset Validator is an engine-agnostic asset validation framework designed for game development pipelines.

It validates serialized asset data (JSON) using a shared rule engine that can be run via CLI (CI-friendly) or desktop UI (human inspection).

## Overview

```
Engine Export → JSON → AssetValidator (Core) → Results
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

- AssetValidator.Core.Tests  
  Unit tests for validation rules and core logic.

## CLI Usage

Validate assets:

    AssetValidator.Cli --input examples/assets.valid.json

Output validation results as JSON:

    AssetValidator.Cli --input examples/assets_valid.json --json-results

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

## Engine integrations

Engine-specific integrations are implemented as standalone repositories.

👉 Unreal Asset Validator  
https://github.com/janikowski-dev/Unreal-Asset-Validator

The plugin is responsible for exporting asset metadata to JSON and invoking the external validator.

## Design Goals

- Engine-agnostic validation
- Deterministic and testable rules
- CI-friendly CLI interface
- Clear separation between data export and validation

## Notice

This repository contains only the validation framework and tooling. Engine integrations are intentionally kept separate.
