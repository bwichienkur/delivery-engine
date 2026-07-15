# Project: EDDY.IS.DeliveryEngine.Workflow.Activities

**Path:** `/workspace/EDDY.IS.DeliveryEngine.Workflow.Activities/`
**Type:** Class library (.NET 4.5, WF4) · **GUID:** `{088A0D16-F4FD-4AA4-808B-9B0357DAF489}`

## Purpose

The **business-logic + integration layer**, implemented as ~48 WF4 `CodeActivity` classes plus supporting business components and integration helpers. Workflows (`.xaml`/`.xamlx`) compose these activities.

## Responsibilities

- Lead retrieval, delivery-definition selection, condition matching.
- Per-endpoint data transformation.
- Delivery via HTTP POST/GET, email, batch email, FTP/SFTP.
- RDQ lifecycle, blackout handling, retries.
- Status finalization and extensive logging.

## Dependencies

- Project refs: `EDDY.IS.DeliveryEngine.DataAccess`, `EDDY.IS.DeliveryEngine.Entity`.
- DLLs: `System.Activities`, **`WinSCPnet`** (SFTP), `EDDY.Nexus.Common.*`.

## Folder map & patterns

| Folder | Purpose | Pattern |
|--------|---------|---------|
| root | Delivery/retrieval/status/logging activities (48 `CodeActivity`) | Template Method (WF) |
| `DataTransformation/` | `DataTransformationBC` + `IDataTransformationTask` implementations | Strategy + Factory |
| `ConditionValidator/` | `ConditionBC`, `RemoteData`, `GetZips` (test stub) | Rules engine / Plug-in |
| `Cap/` | `CapDistributionComponent` | Facade over DAO |
| `LeadScoring/` | `LeadScoringBusinessComponent` (stub → 100) | Facade (stub) |
| `ProductProcessing/` | `ProductProcessingTransactionManager` (locked) | Facade + lock |
| `Helper/` | `Email`, `FTPSender`, `HTTPPost`, `DeliveryEngineBC`, `ExceptionLoggingActivity` | Adapter / Facade |
| `Interface/` | Contracts for the above | Interface |

## Important classes

- **`DataTransformationBC`** (`DataTransformation/DataTransformationBC.cs`, ~805 lines) — transformation pipeline orchestrator (Strategy + factory switch).
- **`ConditionBC`** (`ConditionValidator/ConditionBC.cs`, ~1080 lines) — XML condition evaluator (god class).
- **`HttpPost`** (`Helper/HTTPPost.cs`, ~495 lines) — HTTP GET/POST/SOAP/JSON via `HttpWebRequest`; **sets global trust-all TLS callback** (`:19-28,80-81`) — security risk.
- **`Email`** (`Helper/Email.cs`) — `System.Net.Mail` SMTP wrapper (retries live in callers).
- **`FTPSender`** (`Helper/FTPSender.cs`) — SFTP via WinSCP, FTP via `FtpWebRequest`.
- **`DeliveryEngineBC`** (`Helper/DeliveryEngineBC.cs`, ~315 lines) — pass-through facade of ~50 DAO calls for admin/Lead-Viewer; **not used by workflows**.
- Delivery activities: `DeliverInstantPost`, `DeliverInstantEmail`, `DeliverBatchEmail`, `DeliverBatchFTP`, `CreatePostData`, `CreateEmailBody`, `CreateBatchLeadFile`, `InterpretPostResponse`.

Full activity inventory: [../Services/Activities.md](../Services/Activities.md).

## Configuration

Reads `appSettings` from the host at runtime: `DeliverySMTPServer`, `DeliveryEmailFrom(GS)`, `EmailRetryMax`, `IsDebugMode`, `MachineKey`, `DE_LogVerbosityLevel`, `BatchFilePath`, `ReferenceLibraryPath`.

## External services

SMTP, HTTP endpoints, FTP/SFTP, SQL Server (via DAOs), external condition DLL (`EDDY.Nexus.BusinessComponent.dll`), Windows Event Log, EDDY logging framework.

## Hardcoded values (see Security)

- Trust-all TLS (`HTTPPost.cs:19-28`), fake Referer `www.google.com` + IE8 UA (`:93-94`).
- XSLT temp dir `C:\DeliveryTransformationTemp\DT_Temp` (`ApplyXsltForXml.cs:50`).
- Test zips (`GetZips.cs`), GS product IDs 16/17 for email From.

## Stubs / incomplete

`GetCap` (→ false), `GetRDQsForProcessing` (empty; DAO commented), `CapDistributionComponent.GetCap` (empty), `LeadScoringBusinessComponent` (→100), `RemoteData.GetWebserviceList` (TODO), `FieldValueMapping` webservice branch (empty), `GetRealtimeLeadData` (swallows exceptions).

## Potential improvements

- Extract `HttpPost`/`Email`/`FTPSender` behind a single `IDeliveryTransport` interface with **injected** retry/timeout policies (Polly-style).
- Remove global TLS trust; validate certs, allow per-endpoint pinning if needed.
- Decompose `ConditionBC` operator methods (huge duplication) into an operator Strategy set.
- Delete/implement stubs; make capping/scoring explicit.
- Introduce cancellation/timeouts on all network calls.
