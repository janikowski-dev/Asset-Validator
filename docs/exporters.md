# Metadata Exporters

This document defines expectations placed on external metadata exporters.

---

## Scope

Exporters are responsible only for extracting asset metadata and exporting it to JSON.

They must not:
- implement validation logic
- enforce project-specific constraints
- make assumptions about validation rules

---

## Validator Expectations

The validator assumes that:
- exported metadata is complete but not necessarily valid
- validation decisions are centralized in the validator
- metadata format is stable enough to act as a contract

---

## Supported Exporters

- Unreal Engine Metadata Exporter  
  https://github.com/janikowski-dev/Unreal-Metadata-Exporter

- Blender Metadata Exporter  
  https://github.com/janikowski-dev/Blender-Metadata-Exporter