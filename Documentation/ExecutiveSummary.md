# 1. Executive Summary

## What the application does

The **Delivery Engine** is a back-office lead-distribution system. It routes **leads** (prospective-student inquiries, typically captured by marketing web forms) to **partner destinations** (schools, universities, and third-party vendors) using configurable routing rules.

For each lead it:

1. Selects the highest-priority **Delivery Definition** whose XML match condition the lead satisfies (`GetDeliveryDefinition.cs:26-111`).
2. For each **endpoint** in that definition, runs a **data-transformation pipeline** that reshapes the lead's fields into the partner's required format (`TransformData.cs`, `DataTransformation/DataTransformationBC.cs`).
3. Delivers the transformed payload via one of four channels:
   - **Instant HTTP POST/GET** to a partner URL (`DeliverInstantPost.cs`, `Helper/HTTPPost.cs`).
   - **Instant email** (`DeliverInstantEmail.cs`, `Helper/Email.cs`).
   - **Batch email** with a generated file attachment (`DeliverBatchEmail.cs`, `CreateBatchLeadFile.cs`).
   - **Batch FTP/SFTP** file upload (`DeliverBatchFTP.cs`, `Helper/FTPSender.cs`).
4. Interprets the partner's response (accept / reject / duplicate / auto-review) (`InterpretPostResponse.cs:31-190`), records delivery status, retries realtime failures via a **Realtime Delivery Queue (RDQ)**, honors **blackout windows**, and finalizes the lead's aggregate delivery status.

It also supports **preview** (dry-run transformation without sending — `RealtimePreviewWF.xaml`), **lead capping** (volume limits, largely stubbed in current code — see Weaknesses), **lead scoring** (stubbed to a constant), and an admin/"Lead Viewer" surface for searching, editing, reposting, and scrubbing leads (`Helper/DeliveryEngineBC.cs`, `DeliveryEngineDAO.cs`).

## The business problem it solves

Lead-generation businesses monetize inquiries by delivering them to buyers under contract. Each buyer wants:

- **Different data** (field names, formats, encodings, SOAP/JSON/form/XML/file layouts).
- **Different transport** (HTTP endpoint, email, FTP drop).
- **Different rules** (only certain programs/geographies, volume caps, delivery windows/blackouts, retries).
- **Auditable outcomes** (was it accepted? rejected? duplicate? when? how many attempts?).

The Delivery Engine centralizes this into **data-driven configuration** (Delivery Definitions, Endpoints, Condition XML, Transformation Task XML stored in SQL Server) so new partners can be onboarded largely through configuration rather than code.

## Primary users

| User type | How they interact | Evidence |
|-----------|-------------------|----------|
| **Automated system (leads pipeline)** | The Windows Service polls for new leads and drives delivery; no human in the loop for the happy path | `WorkflowLauncherService.cs:128-252` |
| **Operations / delivery admins** | Configure Delivery Definitions, endpoints, blackouts, caps, email templates; search/edit/repost/scrub leads via a "Lead Viewer" UI (the UI itself is **not** in this repo — this repo exposes the DAO/BC surface it calls) | `Helper/DeliveryEngineBC.cs:14-314`, `DeliveryEngineDAO.cs` Lead-Viewer region (~lines 3148-3814) |
| **Developers/support** | Trigger workflows manually via the WinForms harness or CLI batch mode | `EDDY.IS.DeliveryEngine.Test/Form1.cs`, `WindowsService/Program.cs:29-49` |
| **Partner schools/vendors** | Passive recipients of delivered leads (HTTP/email/FTP) | delivery activities |

## Major workflows

1. **Realtime lead delivery** — new lead → match definition → per-endpoint transform + POST/email → interpret response → status. (`InstantDeliveryWF.xaml` → `RealtimeEndpointWF.xaml` → `InstantPostWF.xaml`/`InstantEmailWF.xaml`.)
2. **Retry delivery** — RDQ items that previously failed are re-attempted for a single endpoint. (`RetryLeadDeliveryWorkflowService.xamlx` → `RealtimeEndpointWF.xaml`.)
3. **Batch delivery** — collect all leads for a batch endpoint, transform, build a file, deliver by email or FTP. (`BatchDeliveryWF.xaml`.)
4. **Preview** — dry-run transformation and payload generation with no actual send. (`RealtimePreviewWF.xaml`.)
5. **Admin/Lead Viewer** — search, edit, repost, scrub leads; manage definitions/endpoints/blackouts/caps. (`DeliveryEngineDAO.cs`.)

See [BusinessProcesses.md](./BusinessProcesses.md) for full detail with sequence diagrams.

## Major integrations

| Integration | Direction | Tech | Where |
|-------------|-----------|------|-------|
| **SQL Server** (`Nexus`, `EddyTracking`, `EddyLogging`) | R/W | Enterprise Library Data + stored procedures | `DataAccess/*`, `Web.config:82-86` |
| **Partner HTTP endpoints** | Outbound | `HttpWebRequest` (GET/POST, form/XML/JSON/SOAP) | `Helper/HTTPPost.cs` |
| **SMTP email** | Outbound | `System.Net.Mail` | `Helper/Email.cs`, `DeliverInstantEmail.cs`, `DeliverBatchEmail.cs` |
| **FTP / SFTP** | Outbound | `FtpWebRequest` (FTP) + **WinSCP** (SFTP) | `Helper/FTPSender.cs` |
| **External condition plug-ins** | In-process | Reflection load of `EDDY.Nexus.BusinessComponent.dll` | `ConditionValidator/RemoteData.cs:56-77` |
| **Windows Event Log** | Outbound | `System.Diagnostics.EventLog` (source `"EDDY"`) | `Helper/ExceptionLoggingActivity.cs`, `Activities/WriteToEventLog.cs` |
| **Enterprise Library logging** (DB + flat file + email) | Outbound | EntLib Logging block | `Web.config:8-49` |

## High-level architecture

A **classic mid-2000s layered .NET architecture** with a workflow-orchestration twist:

```
Windows Service (poller)  ──WCF──▶  WCF Workflow Services (IIS, .xamlx)
                                          │ host
                                          ▼
                                   WF4 Workflows (.xaml)
                                          │ compose
                                          ▼
                                 WF4 Code Activities (business logic)
                                          │ call
                                          ▼
                          DAOs (Enterprise Library) ──▶ SQL Server (stored procs)
```

Cross-cutting: Entity/DTO library (shared, `[DataContract]`), Enterprise Library (Data, Logging, Exception Handling), Unity (transitively referenced, **not used** in source). See [Architecture.md](./Architecture.md).

## Technology stack

- **Runtime:** .NET Framework 4.5 (4.5.2 for UnitTest). `obj/*/.NETFramework,Version=v4.5*.AssemblyAttributes.cs`.
- **Orchestration:** Windows Workflow Foundation 4 (`System.Activities`), WCF Workflow Services (`System.ServiceModel.Activities`, `.xamlx`).
- **Data:** Microsoft Enterprise Library 5.0.414 (Data, Logging, Logging.Database, ExceptionHandling[.Logging/.WCF]); ADO.NET; **stored procedures only** (no ORM, no EF, no `DbContext`).
- **Hosting:** Windows Service (`ServiceBase`) + IIS (WCF service activation).
- **Integrations:** `System.Net.Mail`, `System.Net.HttpWebRequest`, `System.Net.FtpWebRequest`, WinSCP (`WinSCPnet.dll`).
- **Testing:** MSTest v1 (`Microsoft.VisualStudio.QualityTools.UnitTestFramework`); hand-rolled mocks.
- **Shared internal libs:** `EDDY.Nexus.Common.Utilities`, `EDDY.Nexus.Common.Logging`, `EDDY.Nexus.Common.ExceptionHandler` (referenced as DLLs; source not in this repo).
- **Dependency management:** **No NuGet `packages.config`** — all references are direct DLL/GAC references.

## Deployment model

- **WCF Workflow Services** deploy to **IIS** as an ASP.NET app (`Default Web Site/Eddy.DeliveryEngine.WorkflowService`, `.csproj:44`).
- **Windows Service** ("Delivery Service") installs via `ProjectInstaller` as **LocalSystem**, **Automatic** start (`ProjectInstaller.Designer.cs:36,42-43`).
- **Environment configuration** via `Master.config` token replacement (`@@NEXUSDBSERVER`, `@@ENVIRONMENT`) and Web.config XDT transforms per environment (Demo, DEV01, DevStage, DEVSTAGE02, MikeTestEnv, PRODUCTIONSUPORT, QA, QAStage, QASTAGE02, UAT, Release). See [Deployment/](./Deployment/).

## Strengths

- **Highly configurable / data-driven routing** — new partners onboarded via DB config (definitions, endpoints, condition XML, transformation task XML) rather than code.
- **Clear separation of orchestration (WF) from steps (activities) from persistence (DAO).**
- **Multiple transports** (HTTP/email/FTP/SFTP, batch and realtime) behind a common endpoint abstraction (`DeliveryEndpoint` hierarchy).
- **Rich delivery-status model** with retries, blackout windows, and auditable per-attempt logs (`RealtimeDeliveryLog`, `LeadViewerHistory`).
- **Some unit tests exist** for the trickiest activities (URL variable substitution, email retry).

## Weaknesses

- **Obsolete platform.** WF4 + WCF Workflow Services + Enterprise Library 5 are effectively end-of-life; not supported on modern .NET.
- **Fat stored procedures, thin C#.** ~110+ SPs hold most business logic; the DB is the real "domain layer" and it isn't in this repo.
- **Stubbed features shipped as no-ops:** capping (`GetCap.cs:11-21` returns not-capped; `SeLeadProcessingConfigurationValues.cs:23-24` hardcodes `ProcessCap=false`), scoring (`LeadScoringBusinessComponent.cs` returns `100`), `GetRDQsForProcessing.cs` returns empty, webservice condition source is a TODO.
- **Security issues:** global "trust all TLS certs" (`HTTPPost.cs:19-28,80-81`), committed secrets/hostnames/PII, `serviceMetadata httpGetEnabled="true"`.
- **God classes & duplication:** `ConditionBC` (~1080 lines), `DataTransformationBC` (~805), `DeliveryEngineDAO` (~3800), `DeliveryEngineBC` (~315 pass-throughs), duplicated HTTP overloads.
- **Dead/forked code & config drift:** ~51 uncompiled activity files; duplicate host project not in solution; `Web.Release.config` has conflicting connection strings; installer service name ≠ `ServiceName`.
- **Weak test coverage:** ~7 test methods, several with assertions commented out or hitting a real DB.

## Technical debt (summary — full ranked backlog in [Refactoring/](./Refactoring/))

| Theme | Examples | Severity |
|-------|----------|----------|
| Platform obsolescence | WF4, WCF WF services, EntLib 5, .NET 4.5 | High |
| Security | Trust-all TLS, committed secrets, metadata exposure | High |
| Dead/stubbed logic | Capping/scoring/RDQ stubs, 51 uncompiled files | Medium-High |
| Schema opacity | No DB project; logic hidden in SPs | High |
| God classes / duplication | `ConditionBC`, `DataTransformationBC`, `DeliveryEngineDAO` | Medium |
| Config drift | Conflicting transforms, service-name mismatch | Medium |
| Test coverage | Minimal, brittle, DB-coupled | Medium |
