# 9. Database Documentation

> **CRITICAL CAVEAT — everything here is INFERRED.** There is **no** `.sql` file, database project, EF model, or migration in this repository. The application uses the **Enterprise Library Data Access block** to call **stored procedures**; the DB schema, indexes, FKs, views, functions, and triggers live only in the SQL Server instances (`Nexus`, `EddyTracking`, `EddyLogging`), which are not in this repo. Tables and relationships below are inferred from **stored-procedure names**, **`IDataReader` column usage**, **entity `*TableField` enums**, and **entity property mappings**. Confidence is stated per item.

## Databases (from connection strings)

`EDDY.IS.DeliveryEngine.Workflow.Service/Web.config:82-86` (and host `app.config`):

| Connection name | Catalog | Auth | Used by |
|-----------------|---------|------|---------|
| `Nexus` (default) | `Nexus` | Windows Integrated Security | all DAOs except PPT |
| `EddyTracking` | `EddyTracking` | Integrated Security | `ProductProcessingTransactionDAO` (transaction tracing) |
| `EddyLogging` | `EddyLogging` | Integrated Security | Enterprise Library logging listeners (`WriteLog`, `EDDY_WriteException`, `AddCategory`) |

There is **no `DbContext`** (this is not EF). The nearest equivalent is `BaseDataSource` (`DataAccess/Common/BaseDataSource.cs:8-17`) exposing EntLib `Database` handles.

## Access mechanism

- **Enterprise Library Data 5.0.414** via `DatabaseUtilities.CreateDatabase()` (`EDDY.Nexus.Common.Utilities`).
- **Stored procedures only** — `db.GetStoredProcCommand(...)` + `ExecuteReader`/`ExecuteNonQuery`/`ExecuteScalar`/`ExecuteDataSet`. No inline SQL in DAOs.
- Some SP names are **dynamic** (chosen at runtime): condition comparison SPs (`ConditionDAO.GetComparisonTable`), transformation SPs (`DataTransformationDAO.GetStoredProcResult`, and SP names returned by `EDDY_DE_GetDTStoredProcedureNames`).

## Stored procedures

The full catalog (~110+ procedures) with the calling DAO/method and line numbers is in **[StoredProcedures.md](./StoredProcedures.md)**.

Naming conventions:
- `EDDY_DE_*` — Delivery Engine (definitions, endpoints, RDQ, batch, lead viewer).
- `EDDY_DT_*` — Data Transformation (tasks, conditions, XSLT).
- `EDDYT_PPT_*` — Product Processing Tracking (EddyTracking DB).
- `Eddy_*` / `CAP_*` — Cap distribution.
- `WriteLog` / `EDDY_WriteException` / `AddCategory` — EntLib logging (EddyLogging DB).

## Inferred tables

| Inferred table | Confidence | Evidence |
|----------------|-----------|----------|
| `Lead` | High | `EDDY_DE_InsertLead`, `Eddy_GetLeadById`, 50+ lead columns |
| `RealtimeDeliveryQueue` | High | `EDDY_DE_RealtimeDeliveryQueue_*`; RDQ columns (`DeliveryEngineDAO.cs:1159-1166`) |
| `RealtimeDeliveryLog` | High | `EDDY_DE_RealtimeDeliveryLog_Insert`; `LVRealtimeLogEntity` |
| `PreviewDeliveryLog` | Medium | `EDDY_DE_PreviewDeliveryLog_Insert` |
| `BatchDelivery` / `BatchDeliveryLog` | High | `EDDY_DE_Batch_*`, `EDDY_DE_LogBatchLead` |
| `DeliveryDefinition` | High | `EDDY_DE_*DeliveryDefinition*` (`CRId`, `Priority`, `ConditionXML`) |
| `DeliveryEndpoint` | High | `EDDY_DE_GetDeliveryEndpoint` (`EndpointDetailXML`, `DataTransformationId`) |
| `DeliveryEndpointHTTPHeader` | High | `EDDY_DE_GetDeliveryEndpointHTTPHeaders` |
| `DeliveryBlackout` | High | `EDDY_DE_*BlackOut*`; `DeliveryBlackoutItem` |
| `LeadViewerHistory` | High | `EDDY_DE_GetLeadViewerHistory`, `EDDY_DE_AddToLeadHistory` |
| `DT_Task` | High | `TaskTableField` enum + `EDDY_DT_Task_*` |
| `DT_ConditionXML` | High | `ConditionXMLTableField` enum |
| `DT_TaskXML` | High | `TaskXMLTableField` enum |
| `DT_Log` | Medium | `LogTableField` enum, `EDDY_DT_Log_Insert` |
| `Cap` / `CapDistribution` | Medium | `Eddy_GetCapDistribution`, `Eddy_CreateCap`, `CAP_ExecuteCappingProcess` |
| `RealtimeDeliveryStatus` (lookup) | High | `EDDY_DE_GetLeadDeliveryStatus` + enum |
| `TransactionSummary` / `TransactionDetail` (EddyTracking) | High | `EDDYT_PPT_*`; `TransactionSummaryTableField` |
| `ActivityStep` (lookup, EddyTracking) | Medium | `TransactionDetail.ActivityStepName` |
| Beta shadow lead tables | Medium | `*Beta*` SP variants imply parallel beta storage |

## Inferred relationships (Medium–High)

```
ClientSchoolRelationship (CRId/CSRId)
  └── DeliveryDefinition (1:N, priority-ordered)
        ├── ConditionXML (match rules; stored as XML column)
        └── DeliveryEndpoint (1:N)
              ├── DT_Task (via DataTransformationId)
              ├── DeliveryBlackout (1:N)
              ├── DeliveryEndpointHTTPHeader (1:N)
              └── RealtimeDeliveryQueue (1:N per Lead)

Lead (1)
  ├── RealtimeDeliveryQueue (N endpoints)
  ├── RealtimeDeliveryLog (N attempts)
  ├── BatchDeliveryLog (batch path)
  ├── LeadViewerHistory (edits)
  └── AdditionalLeadData (1:N custom fields)

CapDistribution — scoped by SchoolId/ClientId/CapLevel — counts against Lead delivery
```

FKs, indexes, views, functions, and triggers **cannot be determined from this repo** and must be extracted from the live databases (see [ERDiagram.md](./ERDiagram.md) for the notation and the recommended extraction step).

## Migration history

**None in repo.** No migrations framework. Schema changes were historically applied directly to SQL Server (and, given the TFS metadata, likely tracked in a separate DB project not included here). Confidence: **High** that no migration history is recoverable from this repository.

## Soft-delete strategy

- **`IsEnabled`** is the effective active/inactive flag (ubiquitous on `CommonEntity` and endpoint/blackout readers).
- **`IsDeleted`** exists on `CommonEntity` (`Common/CommonEntity.cs:18`) but is **never read/written** by DAOs — likely legacy or DB-only.
- **Hard deletes** occur for transient rows: `EDDY_DT_Task_Delete`, `EDDY_DE_RealtimeDeliveryQueue_Delete`. Definition deletes are guarded (`DeleteNonActiveDeliveryDefinition`).

## Audit fields

`CreatedBy/CreatedDate/UpdatedBy/UpdatedDate/RowGuid/IsEnabled` propagate from `CommonEntity` to write SPs (`@CreatedBy`/`@UpdatedBy`). Lead edits write before/after XML to `LeadViewerHistory` via `EDDY_DE_AddToLeadHistory`.
