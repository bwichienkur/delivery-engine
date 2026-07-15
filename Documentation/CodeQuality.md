# 21. Code Quality

## God classes / large files
| Class/file | Size | Concern |
|-----------|------|---------|
| `DeliveryEngineDAO.cs` | ~3800 lines, ~90 methods | Massive DAO spanning many aggregates |
| `ConditionBC.cs` | ~1080 lines | Duplicated per-operator/per-type comparison logic |
| `DataTransformationBC.cs` | ~805 lines | Orchestrator + dead code |
| `DataTransformationDAO.cs` | ~1300 lines | Many DT SP methods |
| `HTTPPost.cs` | ~495 lines | Duplicated GET/POST/SOAP overloads |
| `LeadEntity.cs` | ~680 lines | 4 entities in one file |
| `DeliveryEngineBC.cs` | ~315 lines | ~50 pass-through delegates |

## Long methods
`InterpretPostResponse.Execute` (~160 lines), `CreateBatchLeadFile.CreateDelimitedFile` (~110), `ConditionBC` operator methods, `DataTransformationBC.CreateOperationDictionary`.

## Duplicated logic
- `FieldToCompare` exists in `Entity/ConditionValidator` and (uncompiled) `DataAccess/Common`.
- `ExceptionLoggingActivity` duplicated (Activities vs Workflow.Service, forked behavior).
- ~51 forked activity copies in `Workflow.Service/Activities` (not compiled).
- HTTP overloads in `HTTPPost` largely copy-paste.
- `WindowsService` vs `Test` harness `WorkflowLauncherService` near-duplicates.

## Dead / unused code
- Uncompiled `Workflow.Service/Activities/*` (~51 files).
- `DataAccess/Common/FieldToCompare.cs` (not in csproj).
- `LeadPreview` WCF client/reference (never called); `_previewList`, `LeadPreviewWorkflowHost` unused.
- Self-hosting `WorkflowServiceHost` path (`Start()` commented).
- `BaseFixture` (not inherited); `LeadDAOMock` (unused).
- `EddyTransactionDb`/`EddyLoggingDb` declared but never initialized.

## Stubs / underengineering
`GetCap` (→false), `GetRDQsForProcessing` (empty), `CapDistributionComponent.GetCap` (empty), `LeadScoringBusinessComponent` (→100), `RemoteData.GetWebserviceList` (TODO), `FieldValueMapping` webservice branch (empty).

## Overengineering
- Full WF4 + WCF Workflow Services + multiple `.xamlx` for what is largely a linear ETL-style pipeline.
- Many environment build configurations (11) with overlapping transforms.

## SOLID / DRY violations
- **SRP:** god classes (`DeliveryEngineDAO`, `ConditionBC`).
- **DIP:** concrete `new` + static locators instead of injected abstractions.
- **OCP:** adding a condition operator or task type means editing big `switch`/`if` blocks.
- **ISP:** `IDeliveryEngineDAO` is enormous; consumers depend on far more than they use.
- **DRY:** duplication listed above.

## Naming / config issues
- Misspellings: `TerminateOrfantCapDuration`, `GetBloackOutDateTimeOffSets`, `LeadPreviwDeliveryWorkflowService`, `AddDefaultDTFields_Gradschools`.
- Namespace drift: `EmailTemplate` (`EDDY.Nexus.DeliveryEngine.Entity`), `ProductProcessing` types, `EDDY.Nexus.WindowsService.DeliveryEngine` installer namespace.
- Installer service name ("Delivery Service") ≠ `ServiceName` ("EDDYWorkflowLauncherService").
- Stale comments (90s poll vs 5s actual).

## Circular dependencies
None at the **project** level (the reference graph is a DAG). Within `DeliveryEngineDAO`, methods are highly interdependent but not circular across assemblies.

## Positives
- Clear layer boundaries; consistent DAO/interface pairing.
- Data-driven design enables config-based onboarding.
- Some meaningful unit tests exist for the trickiest activities.
