# Validation Rules

This document describes how validation rules are structured and executed.

---

## Rule Model

A rule is a small, isolated unit of logic that inspects asset metadata and emits zero or more validation results.

Rules:
- operate on serialized metadata
- are deterministic
- have no side effects

Rules do not:
- load assets
- access the file system
- depend on engine APIs

---

## Severity

Each validation result has an associated severity.

Severity influences:
- how results are displayed in the UI
- the CLI exit code

Severities include:
- Error
- Warning
- Info

---

## Categories

Rules are grouped into categories for organizational purposes, such as:
- Naming
- Size
- Format
- Performance
- Quality
- Structure
- Metadata

Categories exist to aid filtering and inspection and do not affect execution.

---

## Execution Model

During validation:
1. Metadata is deserialized from JSON
2. All applicable rules are executed
3. Results are collected and reported

Rule execution order does not affect correctness.

---

## Testing Expectations

Each rule is expected to have unit test coverage.

Tests should assert:
- valid input produces no results
- invalid input produces expected results
- edge cases are handled correctly
