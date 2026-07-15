# Project: EDDY.IS.DeliveryEngine.WindowsService

**Path:** `/workspace/EDDY.IS.DeliveryEngine.WindowsService/`
**Type:** Windows Service (`Exe`, .NET 4.5) · **GUID:** `{2930661E-1A2D-4182-B038-CBD5DF428F1E}`

## Purpose

The **driver / poller**. Runs continuously as a Windows Service ("Delivery Service"), polls the `Nexus` DB for new/retry/third-party leads, and invokes the IIS-hosted WCF workflow services to deliver them.

## Responsibilities

- Poll the DB on a ~5s cadence.
- Dispatch leads to `LeadProcessingDeliveryWorkflowService.ProcessLeads` and RDQ items to `RetryLeadDeliveryWorkflowService.RetryProcessLeads` (WCF clients).
- Provide a CLI batch trigger (`-b <endpointId> <productId>`).

## Startup & lifecycle

- `Program.cs:17-27` — `ServiceBase.Run(new WorkflowLauncherService())`; `:29-49` — CLI `-b` batch mode.
- `WorkflowLauncherService.OnStart` (`:70-92`) spawns one worker `Thread` running `ProcessDeliveryWorkflow`; the self-host `Start()` is commented out (`:76`).
- `OnStop` (`:105-122`) signals `_stop`, joins the worker, calls host `Stop()`.
- Loop: `Thread.Sleep(5000)` + `_stop.WaitOne(10)`; `Parallel.ForEach` + `lock(Locker)` (serialized WCF calls). `:128-252`.

Detail: [../Architecture.md](../Architecture.md) §2.3 and [../Services/BackgroundProcessing.md](../Services/BackgroundProcessing.md).

## Important classes

- `WorkflowLauncherService` (`ServiceBase`) — the poll/dispatch loop.
- `ProjectInstaller` — installs as **LocalSystem**, **Automatic** start (`ProjectInstaller.Designer.cs:36,42-43`). Installed service display name **"Delivery Service"** ≠ internal `ServiceName` **"EDDYWorkflowLauncherService"** (`WorkflowLauncherService.Designer.cs:32`).
- `LeadProcessingWorkflowHost` / `RetryLeadProcessingWorkflowHost` / `LeadPreviewWorkflowHost` — self-hosting `WorkflowServiceHost` wrappers, **present but disabled** (only `Stop()` called). `LeadPreviewWorkflowHost` has a contract typo `"LeadPreviwDeliveryWorkflowService"` (`:35`).

## WCF client references (`Service References/`)

| Reference | Contract | Operation | Used? |
|-----------|----------|-----------|-------|
| LeadProcessingDeliveryWorkflowService | `ILeadProcessingDeliveryWorkflowService` | `ProcessLeads` | ✅ main loop |
| RetryLeadDeliveryWorkflowService | `IRetryLeadDeliveryWorkflowService` | `RetryProcessLeads` | ✅ retry loop |
| BatchLeadProcessingWorkflowService | `BatchLeadDeliveryWorkflowService` | `processBatch` | CLI `-b` only |
| LeadPreviewWorkflowService | `ILeadPreviewWorkflowNewService` | `ProcessPreview` | ❌ dead |

Client endpoints target `http://localhost/EDDY.IS.DeliveryEngine.Workflow.Service/*.xamlx`, `basicHttpBinding`, security `None` (`app.config:125-175`).

## Configuration

- `app.config`: connection strings; delivery/poll `appSettings` (`Process_RealTime_Lead_Delivery`, `Process_Retry_Lead_Delivery`, `Number_Parallel_Processes`, `MachineKey`, `DeliverySMTPServer`, `BatchFilePath`, `ServiceXAMLDirectory`, `IsBeta`, `IsDebugMode`, etc.); EntLib config.
- `Master.config`: token template → `EDDY.IS.DeliveryEngine.WindowsService.exe.config`; differs from `app.config` (retries on, parallelism 2, prod SMTP, extra rethrow policy).
- `EDDY.IS.DeliveryEngine.WindowsService_TemporaryKey.pfx` — ClickOnce manifest signing key (thumbprint `99407A621C05C1AD1B0BE77E0907A76D040060BE`), **not** WCF TLS.

## External services

SQL Server (`Nexus`/`EddyTracking`/`EddyLogging`); WCF workflow services (IIS); SMTP for alerts (EntLib).

## Potential improvements

- Align installer display name and `ServiceName`.
- Replace `Thread.Sleep` poll with a proper scheduler/timer + backoff; make cadence configurable (currently hardcoded 5s).
- Reconsider `Parallel.ForEach` + `lock` (it defeats parallelism); use bounded concurrency (e.g., `SemaphoreSlim`) or async.
- Remove dead self-host + preview client code.
- Add DB reconnect/backoff on `SqlException` (the Windows Service only sleeps; the Test harness recreates the DAO).
