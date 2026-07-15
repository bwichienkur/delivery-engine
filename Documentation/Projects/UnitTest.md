# Project: EDDY.IS.DeliveryEngine.UnitTest

**Path:** `/workspace/EDDY.IS.DeliveryEngine.UnitTest/`
**Type:** MSTest test library (.NET 4.5.2) · **GUID:** `{1820E723-E967-4987-83A0-F7A209085E99}`

## Purpose

Automated tests for a small subset of `Workflow.Activities`. Mixes true unit tests (hand-rolled mocks) with **integration tests that hit a real database**.

## Framework & approach

- **MSTest v1** (`Microsoft.VisualStudio.QualityTools.UnitTestFramework`): `[TestClass]`, `[TestMethod]`, `[TestInitialize]`.
- Activities executed in-process via **`WorkflowInvoker.Invoke`** (no WCF/IIS).
- **Mocks are hand-rolled** (no Moq/NSubstitute): `MockRepositories/DeliveryEngineDAOMock.cs` (subclasses `DeliveryEngineDAO`, most methods `throw NotImplementedException`, a few overridden; injected via `DeliveryEngineDataService.DeliveryEngineDAO = new DeliveryEngineDAOMock()`), `MockRepositories/LeadDAOMock.cs` (unused by current tests).

## Fixtures (`Activities/`)

| Fixture | Activity | Quality |
|---------|----------|---------|
| `DeliverInstantPostFixture` | `DeliverInstantPost` | **Good** — 3 tests asserting URL variable substitution |
| `DeliverInstantEmailFixture` | `DeliverInstantEmail` | **Good** — asserts retry logging via bad SMTP |
| `GetDeliveryDefinitionFixture` | `GetDeliveryDefinition` | **Integration** — 2 DB-dependent tests (real `LeadDataService`) |
| `CreateEmailBodyFixture` | `CreateEmailBody` | **Smoke only** — hits real DB endpoint, no assertions |
| `CreateBatchLeadFileFixture` | `CreateBatchLeadFile` | **Broken** — misnamed; asserts commented out; real DB |
| `DeliveryBatchEmailFixture` | `DeliverBatchEmail` | **Empty** — Act/Assert commented out |

`BaseFixture.cs` provides query-string helpers but is **not inherited** by any fixture.

## Configuration

`App.config`: DB `cycle-sql1` (Nexus/EddyTracking/EddyLogging); `DeliverySMTPServer=asd.qwe.tyu` (intentional bad host for retry tests); `DE_LogVerbosityLevel=6`; EntLib config. **No WCF client endpoints** (in-process only).

## Coverage assessment

- **~7 test methods** total; several without assertions or DB-coupled.
- **Untested:** the vast majority of activities, all workflows (`.xaml`/`.xamlx`), the Windows Service loop, batch/preview/retry paths, capping/scoring, most logging/status activities.
- Overall coverage: **low–medium**, concentrated on instant-delivery edge cases.

## Potential improvements

- Separate **unit** vs **integration** tests; make unit tests DB-free.
- Adopt a mocking library and DI so DAOs can be injected rather than swapping statics.
- Re-enable/repair commented assertions; delete empty fixtures or implement them.
- Add tests for `TransformData`, `InterpretPostResponse`, `ConditionBC` operators, blackout logic, and status aggregation.
