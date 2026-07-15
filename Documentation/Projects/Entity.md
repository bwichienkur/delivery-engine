# Project: EDDY.IS.DeliveryEngine.Entity

**Path:** `/workspace/EDDY.IS.DeliveryEngine.Entity/`
**Type:** Class library (.NET 4.5) · **Project GUID:** `{470AEA68-64FF-4B7F-8EBC-0151B0A75B59}`

## Purpose

The **shared domain / DTO library**. Defines every entity, DTO, enum, and interface exchanged between layers and serialized across WCF. Almost every type is `[DataContract]`/`[Serializable]` for WCF and some are XML-serializable for the condition/transformation XML stored in the DB.

## Responsibilities

- Domain model for leads, delivery definitions, endpoints, realtime queue, caps, transformations, blackouts.
- WCF data contracts (serialization surface).
- XML-serializable structures for condition matching and transformation task configuration.
- Shared enums and audit base classes.

## Dependencies

- Project refs: **none** (foundation layer).
- DLLs: `EDDY.Nexus.Common.Utilities`, `EDDY.Nexus.Common.ExceptionHandler`, `System.Runtime.Serialization`, `System.Xml`.

## Folder map & design intent

| Folder | Purpose | Pattern |
|--------|---------|---------|
| `Common/` | Audit base (`CommonEntity`), `RequestHeader`, `BaseXmlEntity` (XML helpers), `EventType` enum | Base class / Template |
| `Cap/` | Cap distribution DTOs (`CapDistributionEntity`, `GetCapEntity`, `CapLevelEntity`) | DTO |
| `ConditionValidator/` | `Condition`, `FieldToCompare`, `FtcParameter`, `MatchingCondition`, `Result`, enums | XML-serialized rule model |
| `DataTransformation/` | Transformation task DTOs + `ConsumerIdMapping`, `KeyValueObject`, exceptions | DTO |
| `Interface/` | Contracts (`ICommonEntity`, DT interfaces, `IDeliveryDefSearchParam`) | Interface segregation (partial) |
| root | Leads, definitions, endpoints, realtime, blackout, lead-viewer DTOs | DTO |

## Important classes

- **`CommonEntity`** (`Common/CommonEntity.cs:10-41`) — abstract audit base: `IsEnabled`, `IsDeleted`, `CreatedBy/Date`, `UpdatedBy/Date`, `RowGuid`, `CurrentUser`, `CurrentCSR`.
- **`LeadEntity`** and subclasses `LeadProcessingEntity` → `DeliveryLeadData` (`LeadEntity.cs:19-652`) — the lead through its lifecycle; `DeliveryLeadData` carries `TransformedNameValuePairs` and `IsRepost`.
- **`DeliveryDefinition`** (`DeliveryDefinition.cs:47-78`) — priority-ordered routing rule per CSR; holds `ConditionXML` + `EndPoints`.
- **`DeliveryEndpoint`** hierarchy (`DeliveryDefinition.cs:123-424`) — abstract endpoint + `InstantPostEndpoint`, `InstantEmailEndpoint`, `BatchEmailEndpoint`, `BatchFtpEndpoint`.
- **`RealtimeDeliveryQueueItem`** (`RealtimeDeliveryQueueItem.cs:6-36`) — RDQ work item (lead+endpoint+status+attempts+machine key).
- **`DataTransformationEntity`/`New`** (`DataTransformation/`) — a transformation task tied to a delivery def/form validation/condition.
- **Blackout model** (`DeliveryBlackoutPeriod.cs`) — `DeliveryBlackoutPeriod` hierarchy + `BlackoutFactory`/`BlackoutHelper` (in-memory evaluation) and `DeliveryBlackoutItem` (DB-backed).

Full entity catalog: [../Entities/README.md](../Entities/README.md).

## Configuration

None (pure library). No config files.

## External services

None directly (types are consumed by other layers).

## NuGet / DLL packages

No NuGet. DLL refs only (`EDDY.Nexus.Common.*`, framework serialization/XML).

## Potential improvements

- **Split the god file `LeadEntity.cs`** (4 entities, ~680 lines) into one type per file.
- **Consolidate duplicate `FieldToCompare`** (this project vs the uncompiled `DataAccess/Common/FieldToCompare.cs`).
- **Fix namespace inconsistencies**: `EmailTemplate` uses `EDDY.Nexus.DeliveryEngine.Entity`; `ActivityStep`/`TransactionSummary`/`TransactionDetail` use `ProductProcessing` namespace — inconsistent with the assembly root namespace.
- **`ICommonEntity` omits `IsDeleted`** while `CommonEntity` defines it — align the interface, or remove unused `IsDeleted` (never read/written in DAOs).
- Replace hand-written XML/DataContract DTOs with modern records + `System.Text.Json`/DTO mapping when modernizing.
