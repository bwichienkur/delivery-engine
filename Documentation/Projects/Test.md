# Project: EDDY.IS.DeliveryEngine.Test (WinForms harness)

**Path:** `/workspace/EDDY.IS.DeliveryEngine.Test/`
**Type:** WinForms app (`WinExe`, x86, .NET 4.5) · **NOT in `EDDY.Services.DE.sln`**

## Purpose

A **manual developer harness** to trigger delivery workflows from a small UI without installing the Windows Service. It is a near-fork of `EDDY.IS.DeliveryEngine.WindowsService`.

## Behavior

- `Program.cs:14-18` — standard WinForms `Application.Run(new Form1())`.
- `Form1.cs`:
  - **"Start Lead Processing"** (`:29-33`) → `new WorkflowLauncherService().Start()` — runs the same ~5s poll loop as the service (non-`ServiceBase` variant).
  - **"Start Batch Processing"** (`:68-78`) → hardcoded `deliveryEndPointId = 36677`, `productId = 1` → `BatchLeadDeliveryWorkflowServiceClient.processBatch(...)`.
- `WorkflowLauncherService` (Test copy): uses **concrete** `new LeadDAO()`/`new DeliveryEngineDAO()` (vs static `*DataService` in the real service) and **recreates the DAO on `SqlException`** (`:182`) — a Test-only recovery path.
- Self-host workflow classes exist but are **not wired to the UI**.

## Service references & config

- WCF refs: LeadProcessing, Retry, Batch (no Preview). `App.config` client endpoints target `http://localhost/...` plus an unused `IDataTransformation` at `http://localhost/EDDY.DataTransformation.Service/DataTransformation.svc`.
- **DB target is `PRODSUPPISDB.eddycorp.local`** (`App.config:88`) — points at a prod-support DB; risky for casual manual testing.

## Build status

- **Not in the solution** — must be opened/built separately.
- Broken `HintPath`s reference machine-specific folders (`DE_SFTP\...`, `Users\dkarna\Desktop\New folder\...`) — **unlikely to build on a clean checkout**. Confidence: **High**.

## Recommendation

Treat as a legacy local tool. If a manual trigger is still needed, replace with a small maintained CLI that uses project references (not arbitrary DLL paths) and points at a **non-production** database by default.
