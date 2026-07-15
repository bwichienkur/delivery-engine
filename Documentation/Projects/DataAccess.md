# Project: EDDY.IS.DeliveryEngine.DataAccess

**Path:** `/workspace/EDDY.IS.DeliveryEngine.DataAccess/`
**Type:** Class library (.NET 4.5) · **GUID:** `{2EBE8685-28D5-4B72-BC23-91D53BB88E82}`

## Purpose

The **persistence layer**. Executes SQL Server **stored procedures** through the **Enterprise Library Data Access block** and maps results to/from `Entity` DTOs. There is **no ORM and no inline SQL** — every query is a stored procedure.

## Responsibilities

- Encapsulate all database access behind DAO classes + interfaces.
- Map `IDataReader` rows to entities and entity properties to SP parameters.
- Provide static `*DataService` facades (a simple service locator) used by activities/hosts.

## Dependencies

- Project ref: `EDDY.IS.DeliveryEngine.Entity`.
- DLLs: `EDDY.Nexus.Common.Utilities` (`DatabaseUtilities.CreateDatabase()`), Enterprise Library `Common`, `Data`, `ExceptionHandling(.Logging/.WCF)`, `Logging(.Database)`; `Microsoft.Practices.ServiceLocation`, `Microsoft.Practices.Unity(.Interception)` (transitive).

## Design & folder map

| Folder | Purpose | Pattern |
|--------|---------|---------|
| root | DAO implementations (`DeliveryEngineDAO`, `LeadDAO`, `CapDistributionDAO`, `ConditionDAO`, `DataTransformationDAO`, `ProductProcessingTransactionDAO`) | Data Access Object |
| `Interface/` | DAO contracts + `Interface/Common/IBaseDataSource` | Interface |
| `Common/` | `BaseDataSource`, `EntityTypeUtility`, `FieldToCompare` (**not compiled**) | Base class / util |
| `DataService/` | Static holders wiring concrete DAOs | Service Locator / Facade |

## Important classes

- **`BaseDataSource`** (`Common/BaseDataSource.cs:8-17`) — base for DAOs; exposes `Db`/`EddyTransactionDb`/`EddyLoggingDb` (`Database` from EntLib). Only `Db` is initialized (`DatabaseUtilities.CreateDatabase()`); the other two are declared but unused.
- **`DeliveryEngineDAO`** (`DeliveryEngineDAO.cs`, ~3800 lines) — the largest class; ~90 methods over `EDDY_DE_*` SPs (definitions, endpoints, RDQ, batch, lead viewer, blackouts, headers, templates).
- **`LeadDAO`** — `Eddy_GetLeadById`, `EDDY_GetRealtimeLeadById`.
- **`DataTransformationDAO`** (~1300 lines) — `EDDY_DT_*` SPs for transformation tasks/conditions/XSLT + dynamic SP execution.
- **`ConditionDAO`** — dynamic comparison SP execution (`GetComparisonTable`) + `EDDY_DT_Task_Select`.
- **`CapDistributionDAO`** — cap CRUD + `CAP_ExecuteCappingProcess`.
- **`ProductProcessingTransactionDAO`** — `EDDYT_PPT_*` SPs against the **`EddyTracking`** DB (`AppSettings["EddyTrackingDb"]`).
- **`EntityTypeUtility`** (`Common/EntityTypeUtility.cs:9-155`) — maps entity data-type strings/enums to `DbType`/`System.Type` for SP params.

Full DAO + stored-procedure catalog: [../Database/StoredProcedures.md](../Database/StoredProcedures.md).

## Configuration

- Reads connection info from the **host** config (`Nexus` default via `dataConfiguration defaultDatabase="Nexus"`; `EddyTracking` via `AppSettings["EddyTrackingDb"]`).
- `SQLCommandTimeOut` app setting influences command timeout (used in hosts).

## External services

- **SQL Server** (`Nexus`, `EddyTracking`). Windows Integrated Security.

## NuGet / DLL packages

No NuGet. Enterprise Library 5.0.414 + `EDDY.Nexus.Common.*` DLLs.

## Potential improvements

- **Break up `DeliveryEngineDAO`** by aggregate (Definitions, Endpoints, RDQ, Batch, LeadViewer, Blackout).
- **Add a schema/migrations project** (SSDT `.sqlproj` or DbUp) so the ~110+ SPs and tables are versioned in this repo.
- **Remove uncompiled `Common/FieldToCompare.cs`** (dead duplicate).
- **Parameterize "beta" vs "prod" SP selection** rather than branching to `*Beta*` SP names inline.
- Replace static `*DataService` locators with constructor injection to enable real mocking (tests currently swap static fields).
- Consider async DB APIs when modernizing (current code is fully synchronous).
