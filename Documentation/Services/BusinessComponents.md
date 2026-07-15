# Business Components (Section 6/11)

## DataTransformationBC — `DataTransformation/DataTransformationBC.cs` (~805 lines)

- **Purpose:** orchestrate the per-endpoint transformation pipeline (see [../BusinessProcesses.md](../BusinessProcesses.md) Process 4).
- **Collaborators:** `ConditionBC` (task gating), `IDataTransformationTask` implementations (Strategy), `DataTransformationDAO`/`ConditionDAO` (task/SP data), `DataTransformationDAO.CreateLog`.
- **Key methods:** `Transform` (`:335-443`), `CreateOperationDictionary` (factory switch, `:496-612`), `MergeRawDataWithLeadData` (`:645-697`).
- **Patterns:** Strategy (tasks), Factory (task creation by XML element name), Pipeline (ordered execution).
- **Thread safety:** relies on per-call instances; XSLT temp files not locked.
- **Issues:** god class; commented dead code; mandatory `AppendNameValuePair` throws `ArgumentException` if none pass (`:408-411`) — surprising coupling.

## ConditionBC — `ConditionValidator/ConditionBC.cs` (~1080 lines)

- **Purpose:** evaluate XML `Condition`/`FieldToCompare` rules against a lead's name/value data (see Process 5).
- **Collaborators:** `ConditionDAO.GetComparisonTable` (SP source), `RemoteData` (class/webservice source), `DeliveryEngineDAO.TempLog` (debug).
- **Operators:** `Equals`, `In`, `GreaterThan(OrEqualTo)`, `LessThan(OrEqualTo)` (numeric only), `RegularExpression`, `Other` (no-op).
- **Rules:** AND across all fields; empty condition ⇒ true; `csrid`→`CRId`; repost bypasses regex; debug logging per field.
- **Patterns:** (implicit) Strategy-by-`switch`; would benefit from an operator Strategy set.
- **Issues:** god class with massive duplicated per-operator/per-type comparison methods; shared `_log` `StringBuilder` (thread-safety); `Webservice` source is a TODO stub.

## CapDistributionComponent — `Cap/CapDistributionComponent.cs` (`:16-57`)

- **Purpose:** thin wrapper over `CapDistributionDAO` (create/edit cap, update value by CSR, `ProcessCap`).
- **Issue:** `GetCap` calls the DAO but returns an empty list (`:30-35`); runtime capping is effectively disabled (see `SeLeadProcessingConfigurationValues`).

## LeadScoringBusinessComponent — `LeadScoring/LeadScoringBusinessComponent.cs` (`:10-13`)

- **Purpose (intended):** score a lead. **Reality:** returns constant `100`. Placeholder.

## ProductProcessingTransactionManager — `ProductProcessing/ProductProcessingTransactionManager.cs` (`:15-63`)

- **Purpose:** persist transaction detail/summary to the `EddyTracking` DB for tracing (`InsertTransactionDetail`, `SaveTransactionSummary`).
- **Thread safety:** wraps DAO writes in `lock(_Locker)` (correct serialization).
- **Consumers:** `WriteToDELog`/`WriteToRealtimeDELog` (when `ActivityStepId > 0`), `InsertTransactionDetail` activity.
- **Issue:** `GetActivitySteps` returns empty list (stub).
