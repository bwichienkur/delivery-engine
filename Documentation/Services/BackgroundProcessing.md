# 15. Background Processing

## Hosted service

The only background processor is the **Windows Service** worker thread. There is **no** Hangfire, Quartz, Azure Functions, or Windows Task Scheduler integration in code (grep-confirmed).

- **Host:** `WorkflowLauncherService : ServiceBase` (`WindowsService/WorkflowLauncherService.cs`).
- **Worker:** a single `Thread` running `ProcessDeliveryWorkflow` (`:128-252`), started in `OnStart` (`:79-80`).
- **Cadence:** `Thread.Sleep(5000)` (~5s) with `_stop.WaitOne(10)` for shutdown (`:159-164`). Stale comment mentions 90s.
- **Shutdown:** `OnStop` signals `_stop`, `_worker.Join()` (`:105-122`).

## What each cycle does

1. Read feature flags (`Process_RealTime_Lead_Delivery`, `Process_Retry_Lead_Delivery`, `Process_ThirdParty_Delivery`, `Number_Parallel_Processes`, `IsBeta`) (`:134-151`).
2. Poll DB (`:171-189`):
   - `GetNewLeadsForProcessing(batch, machine, true, isBeta)` → `EDDY_DE_GetNewLeadsForProcessing_Slate`
   - `GetRDQsForProcessingReposts(...)` → `EDDY_DE_GetRepostsForDelivery`
   - `GetThirdPartyLeadsForProcessing(...)`
3. Dispatch (`:213-241`):
   - New/third-party → `Parallel.ForEach` → `LeadProcessingDeliveryWorkflowServiceClient.ProcessLeads` (each call inside `lock(Locker)`).
   - Retries → `RetryLeadDeliveryWorkflowServiceClient.RetryProcessLeads`.
4. On `SqlException`: log, sleep 5s, continue (no reconnect; the Test harness recreates the DAO instead).

## The Realtime Delivery Queue (RDQ)

- A **DB table** acting as a durable work/retry queue (`RealtimeDeliveryQueue`).
- Machine affinity via `DeliveryEngineMachineKey` (from `MachineKey`/`MachineName`) supports multiple delivery-engine hosts.
- Activities: `CreateRDQItem`, `UpdateRDQFailedStatus` (515), `UpdateRDQForBlackout` (516), `RemoveRDQItem`, `GetRDQsForProcessingReposts`, `ResetRealtimeDeliveryQueueForMachine`.

## Retry & blackout logic

- **POST retries:** `InstantPostWF` retries up to `MaxRetryAttempts` (endpoint or `DefaultPOSTRetryAttempts`), scheduling `NextDeliveryAttemptDatetime` (status 515); at max → 610.
- **Email retries:** SMTP-level (`EmailRetryMax`), no RDQ max-retry branch.
- **Blackout:** `CheckEndpointBlackout` → `UpdateRDQForBlackout` (516) defers next attempt.
- **Cron/schedules:** none in code; batch runs are triggered externally.

## Concurrency caveat

`Parallel.ForEach` is used, but each WCF call is wrapped in `lock(Locker)` (`:302-306,343-347`), so throughput is effectively **serial**. This is a prime modernization target (see [../Refactoring/](../Refactoring/)).
