# Project: EDDY.IS.DeliveryEngine.Workflow.Service

**Path:** `/workspace/EDDY.IS.DeliveryEngine.Workflow.Service/`
**Type:** ASP.NET / WCF Workflow Service host (.NET 4.5, IIS) · **GUID:** `{0C7C072C-BF35-4D58-9A01-8FC06AE1CB5F}`

## Purpose

**IIS-hosted WCF Workflow Services** — the runtime executor. Each `.xamlx` file is a workflow service activated by IIS; it receives a WCF request, runs a workflow (usually delegating to a `.xaml` from the `Workflow` project), and (for request-reply) replies with a status.

## Responsibilities

- Expose delivery operations over WCF (`basicHttpBinding`).
- Host `.xamlx` services and a small set of **service-only** activities.

## Dependencies

- Project refs: `EDDY.IS.DeliveryEngine.Workflow`, `Workflow.Activities`, `DataAccess`, `Entity`.
- DLLs: EntLib 5.0.414 (incl. `ExceptionHandling.WCF`), `EDDY.Nexus.Common.*`, `System.ServiceModel.Activities`.

## Services (.xamlx) → operations

| File | Operation | Pattern | Runs |
|------|-----------|---------|------|
| `LeadProcessingDeliveryWorkflowService.xamlx` | `ProcessLeads(DeliveryLeadData)` | one-way | scoring/cap flags → `InstantDeliveryWF` → `FinalizeLeadDelivery` |
| `RealtimeLeadProcessingDeliveryWorkflowService.xamlx` | `ProcessLeads(LeadId) → Status` | request-reply (correlated) | `GetRealtimeLeadData` → `InstantDeliveryWF` → `RemoveRePost` → status |
| `BatchLeadDeliveryWorkflowService.xamlx` | `processBatch(deliveryEndpointId, productId)` | one-way | `BatchDeliveryWF` |
| `LeadPreviewDeliveryWorkflowService.xamlx` | `ProcessPreview(LeadData) → Dictionary` | request-reply | `RealtimePreviewWF` |
| `RetryLeadDeliveryWorkflowService.xamlx` | `RetryProcessLeads(RDQ, LeadData, endpointId)` | one-way | `GetDeliveryEndpoint` → `RealtimeEndpointWF` |

Full contract detail: [../APIs/WorkflowServices.md](../APIs/WorkflowServices.md).

## Activities folder — compiled vs dead

**Only 6 activity files are compiled** (`.csproj:277-282`):
`ScoreLead`, `ProcessCap`, `InsertTransactionDetail`, `SeLeadProcessingConfigurationValues`, `WriteToEventLog`, `ExceptionLoggingActivity` (a **forked, enhanced** copy that also runs the EntLib `WORKFLOW_SERVICE_POLICY`).

**~51 other `.cs` files under `Activities/` are NOT in the `.csproj`** and use old `EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities` namespaces — **dead/legacy forks** that do not build. Confidence: **High**.

> ⚠️ `SeLeadProcessingConfigurationValues.cs:23-24` **hardcodes** `ProcessCap=false`, `ScoreLead=false`, ignoring `Web.config:106-107`. Only `DeliverLead` is read from config.

## Configuration

- `Web.config`: EntLib logging/exception/data config; connection strings (`Nexus`/`EddyTracking`/`EddyLogging`, Integrated Security); delivery `appSettings`; minimal `system.serviceModel` (metadata + debug behaviors); **no explicit services/bindings/persistence/tracking**.
- Web.*.config XDT transforms per environment (see [../Deployment/](../Deployment/)).
- `Master.config`: token template → `Web.config`.

## External services

SQL Server; indirectly SMTP/HTTP/FTP via activities.

## NuGet / DLL packages

No NuGet. EntLib + `EDDY.Nexus.Common.*` DLLs; framework WCF/WF assemblies.

## Potential improvements

- **Delete the 51 uncompiled activity files** to remove confusion.
- Fix `SeLeadProcessingConfigurationValues` to honor config (or document the intentional override loudly).
- Set `serviceMetadata httpGetEnabled="false"` in production; add transport security/auth.
- Add WF persistence/tracking **or** document explicitly that instances are in-memory and durability relies on the DB.
- Reconcile duplicate `ExceptionLoggingActivity` between this project and `Workflow.Activities`.
