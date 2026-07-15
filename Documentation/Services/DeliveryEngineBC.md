# DeliveryEngineBC — Admin / Lead Viewer Facade

**File:** `EDDY.IS.DeliveryEngine.Workflow.Activities/Helper/DeliveryEngineBC.cs` (~315 lines)

## Purpose

A **Facade** over `DeliveryEngineDAO` exposing ~50 admin and Lead-Viewer operations to an external admin UI (the UI itself is **not** in this repo). Every method is a thin pass-through of the form `return (new DeliveryEngineDAO()).Method(...)` (`:17-309`).

## Responsibilities (by region)

- **Delivery Definitions (`:16-151`):** get/insert/update/clone/delete definitions; conditions; endpoints list; blackout list/CRUD; HTTP headers; email templates; frequencies; expression/condition types; duplicate-priority checks; post-instructions.
- **Lead Viewer (`:154-311`):** search leads (`GetLeadViewerSearchResult(Dynamic)`), get/edit lead data, additional data, history (`GetLeadViewerHistory`, `AddToLeadHistory`), mark for repost, scrub (`ScrubLeadFromLeadViewer`), multiple-edit, campaign/track name, status-type lists, exceeds-3K-rows checks.

## Collaborators

- `DeliveryEngineDAO` (creates a **new instance per call** — no caching/reuse).

## Patterns / lifecycle

- **Facade**; transient DAO instantiation.
- **Not used by workflows** — grep confirms references only within this file; it exists purely for the admin/UI surface.

## Issues / improvements

- ~50 near-identical delegating methods → boilerplate; consider generating or collapsing.
- `new DeliveryEngineDAO()` per call defeats any connection reuse benefits (EntLib pools connections at ADO.NET level, so impact is limited, but it's still noise).
- No authorization here — the calling UI is responsible for access control (none is enforced in this repo).
