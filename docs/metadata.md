# Asset Metadata Model

Asset Validator operates on a collection of serialized `Asset` records deserialized from JSON.

This document describes the conceptual data model used by the validator.

It intentionally avoids defining a strict or exhaustive JSON schema.

---

## Asset

An `Asset` represents a single logical piece of content exported from a DCC tool or engine editor.

Each asset contains a small set of normalized, engine-agnostic properties, plus an extensible metadata bag for tool-specific data.

---

## Core Properties

Each asset exposes the following normalized fields:

-  **Name**
Human-readable asset identifier.

-  **Path**
Logical asset path as reported by the source tool.

-  **Type**
High-level asset classification (e.g. texture, mesh, audio).

-  **Source**
Originating tool or pipeline source.

-  **SizeInBytes**
Serialized size of the asset, used for size- and budget-related rules.

These fields are intentionally generic and shared across all exporters.

---

## Metadata

In addition to core properties, each asset contains a metadata dictionary:
```
Metadata: Dictionary<string, object>
```
The metadata dictionary is used to store:
- engine-specific attributes
- tool-specific data
- optional or extensible properties

The validator does not impose a fixed schema on metadata.

Rules may:
- read known metadata keys
- ignore keys they do not understand

This allows different exporters to provide additional information without breaking validation.

---

## Design Rationale

This model intentionally separates:
-  **stable, normalized properties** (core fields)
-  **extensible, tool-specific data** (metadata)

As a result:
- exporters remain simple serializers
- validation logic remains centralized
- new tools can be integrated without changing the core model

---

## Compatibility and Evolution

The metadata format acts as a contract between exporters and the validator.

Exporters are expected to:
- provide complete but not necessarily valid data
- avoid embedding validation logic

The validator is responsible for interpreting metadata through rules.

The model is designed to evolve without forcing changes across all tools.