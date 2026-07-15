# 23. Testing

## What exists
- **One test project:** `EDDY.IS.DeliveryEngine.UnitTest` (MSTest v1, .NET 4.5.2). Detail in [Projects/UnitTest.md](./Projects/UnitTest.md).
- **~7 test methods** across 6 fixtures, executed via `WorkflowInvoker.Invoke` (in-process WF4).
- **Hand-rolled mocks** (`DeliveryEngineDAOMock`, `LeadDAOMock`) injected by swapping static `*DataService` fields. No Moq/NSubstitute.

## Coverage snapshot
| Area | Status |
|------|--------|
| `DeliverInstantPost` (URL variable substitution) | ✅ unit tests with assertions |
| `DeliverInstantEmail` (retry logging) | ✅ unit tests |
| `GetDeliveryDefinition` | ⚠️ integration (hits real DB) |
| `CreateEmailBody` | ⚠️ smoke only (no assertions) |
| `CreateBatchLeadFile` | ❌ broken (assertions commented, real DB) |
| `DeliverBatchEmail` | ❌ empty (Act/Assert commented) |
| Everything else (workflows, WindowsService loop, transformation, condition operators, batch/preview/retry, capping/scoring, status aggregation, DAOs) | ❌ untested |

## Mocking strategy
- Subclass the concrete DAO, override the few methods a test needs, `throw NotImplementedException` for the rest, assign to the static locator. Brittle and stateful across tests.

## Integration-test hazards
- Tests reference **specific IDs** (endpoint 37628/37925/36677, lead 15278052/15267993, batch 101723) that must exist in the target DB.
- `UnitTest/App.config` points at `cycle-sql1`; a HubSpot API key is embedded in `DeliverInstantPostFixture` test XML (rotate — see [Security/](./Security/)).

## Recommendations
1. Split **unit** (no DB) from **integration** (tagged, opt-in) tests.
2. Adopt DI + a mocking library so DAOs/transports are injectable.
3. Re-enable or delete commented assertions; remove empty fixtures.
4. Add coverage for the true business logic: `ConditionBC` operators, `DataTransformationBC` pipeline, `InterpretPostResponse`, blackout evaluation, status aggregation, RDQ retry transitions.
5. Add **characterization tests** around the stored procedures (they hold most logic) before any migration — e.g., golden-master tests capturing SP outputs for representative inputs.
6. Introduce CI to run tests on every push (none exists today).
