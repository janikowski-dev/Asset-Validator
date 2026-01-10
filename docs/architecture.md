# Architecture

This document describes internal structure and responsibility boundaries
within the Asset Validator repository.

It intentionally avoids restating high-level goals covered in the README.

---

## Core Layer (`AssetValidator.Core`)

The core library contains all validation behavior.

Responsibilities:
- validation rules
- domain models
- result aggregation

Non-responsibilities:
- UI concerns
- CLI formatting
- engine or editor APIs
- file system access

The core is designed to be deterministic and fully unit-testable.

---

## CLI Layer (`AssetValidator.Cli`)

The CLI is a thin integration layer around the core.

Responsibilities:
- loading metadata from JSON
- invoking validation
- formatting results for machines and humans
- returning CI-friendly exit codes

The CLI contains no validation logic.

---

## UI Layer (`AssetValidator.Ui`)

The UI is intended for local inspection of validation results.

Responsibilities:
- visual presentation of validation output
- grouping and filtering by severity

The UI does not influence validation outcomes.

---

## Tests (`AssetValidator.Core.Tests`)

Tests focus exclusively on the core layer.

Covered areas:
- individual rule behavior
- edge cases in metadata
- validation result aggregation

The CLI and UI are intentionally not unit-tested here.