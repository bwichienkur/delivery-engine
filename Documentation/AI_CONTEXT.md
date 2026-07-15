# AI_CONTEXT — Delivery Engine

Dense orientation for an AI agent. Read this first; it links to deeper docs. All claims are grounded in source; **the database schema is inferred** (no `.sql` in repo).

## What this is
A back-office **lead distribution engine** for EducationDynamics ("EDDY"/"Nexus"/"Cheetah"). It routes prospective-student **leads** to partner schools/vendors via **HTTP POST, email, or batch file (email/FTP/SFTP)** based on data-driven **Delivery Definitions** (priority-ordered routing rules with XML match conditions + per-endpoint transformation pipelines).

## Platform
.NET Framework 4.5/4.5.2 · Windows Workflow Foundation 4 (WF4) · WCF Workflow Services (`.xamlx`) · Enterprise Library 5.0.414 (Data/Logging/ExceptionHandling) · SQL Server (stored procedures, no ORM) · WinSCP (SFTP). **No DI container, no NuGet, no EF, no REST API.** Legacy, effectively end-of-life stack.

## Runtime shape (2 processes)
1. **`EDDY.IS.DeliveryEngine.WindowsService`** ("Delivery Service", LocalSystem, Automatic): polls `Nexus` DB ~every 5s (`Thread.Sleep(5000)`), dispatches leads via WCF (`Parallel.ForEach` + `lock` → effectively serial).
2. **`EDDY.IS.DeliveryEngine.Workflow.Service`** (IIS, WCF `.xamlx`): executes WF4 workflows that compose code activities that call DAOs that run stored procedures.

The `WindowsService ↔ Workflow.Service` boundary is **WCF-only** (no project ref) — the cleanest seam for re-platforming.

## Projects (7 in `EDDY.Services.DE.sln`; +1 not in sln)
`Entity` (DTOs, `[DataContract]`) → `DataAccess` (DAOs + EntLib + SPs) → `Workflow.Activities` (WF4 activities + helpers) → `Workflow` (`.xaml`) → `Workflow.Service` (`.xamlx`, IIS) ; `WindowsService` (poller, WCF client) ; `UnitTest` (MSTest). `Test` (WinForms harness) is **not** in the solution and has broken build paths. See [Projects/](./Projects/).

## Core workflows
- **Realtime:** `LeadProcessingDeliveryWorkflowService` → `InstantDeliveryWF` → `RealtimeEndpointWF` → `InstantPostWF`/`InstantEmailWF`.
- **Retry:** `RetryLeadDeliveryWorkflowService` → `RealtimeEndpointWF` (RDQ reposts).
- **Batch:** `BatchLeadDeliveryWorkflowService` → `BatchDeliveryWF`.
- **Preview:** `LeadPreviewDeliveryWorkflowService` → `RealtimePreviewWF` (dry run, no send).
See [BusinessProcesses.md](./BusinessProcesses.md) and [APIs/WorkflowServices.md](./APIs/WorkflowServices.md).

## Key business rules
- **Definition selection:** highest-priority active definition whose **Condition XML** matches the lead; none → status **444** (`GetDeliveryDefinition.cs`).
- **Conditions:** `ConditionBC` evaluates `FieldToCompare` (operators Equals/In/GT/LT/Regex); AND semantics; empty condition ⇒ true; **repost bypasses regex**; sources Local/StoredProcedure/Class(reflection)/Webservice(TODO).
- **Transformation:** ordered `IDataTransformationTask` pipeline per endpoint (`DataTransformationBC`); ≥1 `AppendNameValuePair` mandatory; `ApplyXsltForXml` replaces payload; task/condition XML stored in DB.
- **Delivery:** POST retries up to max (515→retry, 610→give up, 600→success); email SMTP retries; blackout defers (516).
- **Capping & scoring are effectively DISABLED** in code: `SeLeadProcessingConfigurationValues.cs:23-24` hardcodes `ProcessCap=false`/`ScoreLead=false`; `GetCap`→false; `LeadScoringBusinessComponent`→100.
- **Status codes:** 110/200/444/450/460/475/515/516/555/600/610 (see [BusinessProcesses.md](./BusinessProcesses.md#status-code-reference)).

## Data
- Databases: **`Nexus`** (main), **`EddyTracking`** (transaction tracing, `EDDYT_PPT_*`), **`EddyLogging`** (EntLib logs). Windows Integrated Security.
- Access: **stored procedures only** via EntLib Data (`BaseDataSource`); ~110+ SPs cataloged in [Database/StoredProcedures.md](./Database/StoredProcedures.md). Some SP names are dynamic (conditions/transformations).
- Inferred tables/ER: [Database/](./Database/). Soft-delete = `IsEnabled`; `IsDeleted` unused; audit via `CreatedBy/UpdatedBy/RowGuid` on `CommonEntity`.

## Naming conventions
- SPs: `EDDY_DE_*` (delivery engine), `EDDY_DT_*` (transformation), `EDDYT_PPT_*` (tracking), `Eddy_*`/`CAP_*` (cap).
- Entities: derive from `CommonEntity` (audit base); `[DataContract]` for WCF; XML-serializable for condition/task config.
- Activities: WF4 `CodeActivity` overriding `Execute`; naming by verb (`Get*`, `Deliver*`, `Create*`, `Log*`, `Update*`, `Finalize*`).
- Watch for misspellings (`Orfant`, `Bloack`, `Previw`, `Gradschools`) and namespace drift.

## Coding standards (observed, not enforced)
- Static `*DataService` locators return DAO singletons; helpers/DAOs `new`'d directly.
- Synchronous I/O throughout; free-text logging (`WriteToDELog` + EntLib); Windows Event Log source `"EDDY"`.
- Config via `ConfigurationManager.AppSettings` (no Options classes); env via `Master.config` tokens + Web.config XDT transforms.

## External systems
SQL Server; partner HTTP endpoints; SMTP; FTP/SFTP (WinSCP); external condition plugin DLL (`EDDY.Nexus.BusinessComponent.dll` via reflection); Windows Event Log. See [Services/Integrations.md](./Services/Integrations.md).

## Top risks (do not regress)
1. **Global TLS trust-all** in `HTTPPost.cs` (remove — do not "keep for compatibility").
2. **Committed secrets/PII/hostnames**; `bin/obj` in source control.
3. **WCF has no auth**; metadata published.
4. **Business logic hidden in SPs** not in this repo.
5. **Silently disabled capping/scoring** — confirm intent before "fixing".
See [Security/](./Security/) and [Refactoring/](./Refactoring/).

## If you modify this system
- The DB is the real domain layer — changing behavior often means changing SPs (not in repo). Extract/version the schema first ([Refactoring/](./Refactoring/) R4).
- Only the **compiled** activity set matters; ignore the ~51 dead files under `Workflow.Service/Activities/`.
- Tests are minimal and DB-coupled; add characterization tests before refactors.
- Don't add `bin/obj` changes to commits; they're already (wrongly) tracked.
- Preserve the WCF seam if re-platforming incrementally.

## Where to look
| Need | File |
|------|------|
| Big picture | [ExecutiveSummary.md](./ExecutiveSummary.md), [Architecture.md](./Architecture.md) |
| Processes & diagrams | [BusinessProcesses.md](./BusinessProcesses.md), [Diagrams/](./Diagrams/) |
| Per project | [Projects/](./Projects/) |
| Data & SPs | [Database/](./Database/) |
| Entities | [Entities/](./Entities/) |
| Services/patterns/DI | [Services/](./Services/) |
| Security/perf | [Security/](./Security/), [Performance/](./Performance/) |
| Deploy | [Deployment/](./Deployment/) |
| Backlog | [Refactoring/](./Refactoring/) |
