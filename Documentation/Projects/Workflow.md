# Project: EDDY.IS.DeliveryEngine.Workflow

**Path:** `/workspace/EDDY.IS.DeliveryEngine.Workflow/`
**Type:** Class library of WF4 XAML workflows (.NET 4.5) · **GUID:** `{83B111BB-15FA-4A7B-8B36-FED3D351315B}`

## Purpose

Holds the **workflow definitions** (`.xaml`) that orchestrate the activities from `Workflow.Activities` into end-to-end delivery processes. Compiled into an assembly and consumed by the WCF workflow services and (historically) self-hosts.

## Responsibilities

- Declaratively sequence activities with WF control flow (`If`, `ForEach`, `Switch`, `TryCatch`).
- Compose sub-workflows (e.g., `RealtimeEndpointWF` invokes `InstantPostWF`/`InstantEmailWF`).

## Dependencies

- Project refs: `EDDY.IS.DeliveryEngine.Workflow.Activities`, `EDDY.IS.DeliveryEngine.Entity`, `EDDY.IS.DeliveryEngine.DataAccess`.
- Framework: `System.Activities`, `System.ServiceModel.Activities`.

## Workflows (files)

| File | Role | Invokes sub-workflow |
|------|------|----------------------|
| `InstantDeliveryWF.xaml` | Lead-level orchestrator: match definition, loop realtime endpoints | `RealtimeEndpointWF` |
| `RealtimeEndpointWF.xaml` | Per-endpoint router: blackout check, dispatch by type, aggregate status | `InstantPostWF`, `InstantEmailWF` |
| `InstantPostWF.xaml` | Leaf: transform → POST → interpret → RDQ update/status | — |
| `InstantEmailWF.xaml` | Leaf: transform → email → status | — |
| `BatchDeliveryWF.xaml` | Batch: gather leads → transform → build file → email/FTP → status | — |
| `RealtimePreviewWF.xaml` | Dry-run preview into a `Result` dictionary | — |

Detailed orchestration (arguments, step-by-step, flow control) is in [../BusinessProcesses.md](../BusinessProcesses.md) and [../APIs/WorkflowServices.md](../APIs/WorkflowServices.md).

## Configuration

- **`Master.config`** — tokenized template (`output="Web.config"`) carrying EntLib config + delivery `appSettings` + `@@NEXUSDBSERVER` tokens; used by deploy tooling to generate the host config. Not runtime config for this library itself.

## External services

None directly; activities it composes reach SQL/HTTP/SMTP/FTP.

## NuGet / DLL packages

No NuGet. Framework + 3 project refs.

## Potential improvements

- WF4 XAML is hard to diff/review and unsupported on modern .NET. When modernizing, **re-express workflows as explicit C# orchestrator classes** (or a lightweight state machine) so logic is testable and code-reviewable.
- Reconcile `InstantEmailWF` vs `InstantPostWF`: email path lacks max-retry RDQ handling that POST has — confirm intended.
- Fix mislabeled display names (e.g., batch "not bach" guard).
