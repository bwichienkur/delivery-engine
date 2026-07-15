# 22. Design Patterns

Patterns identified in source, with how each is implemented and confidence.

| Pattern | Where / how | Confidence |
|---------|-------------|-----------|
| **Layered architecture** | Entity → DataAccess → Activities → Workflow → Service/Host | High |
| **Data Access Object (DAO)** | `*DAO` + `I*DAO` interfaces in `DataAccess` | High |
| **Service Locator (static)** | `*DataService` static holders returning DAO singletons | High |
| **Facade** | `DeliveryEngineBC` (admin surface), `*DataService` | High |
| **Strategy** | `IDataTransformationTask` implementations selected per task; (implicit) condition operators in `ConditionBC` | High |
| **Factory (switch)** | `DataTransformationBC.CreateOperationDictionary` builds tasks by XML element name; `BlackoutFactory` builds blackout period types (`Entity/DeliveryBlackoutPeriod.cs`) | High |
| **Pipeline / Chain** | Ordered `IDataTransformationTask` execution in `DataTransformationBC.Transform` | High |
| **Template Method** | Every WF4 `CodeActivity.Execute` override | High |
| **Adapter** | `Helper/HTTPPost`, `Email`, `FTPSender` adapt raw .NET APIs to endpoint config | High |
| **DTO + (XML/DataContract) serialization** | `Entity` types; `Tasks`, `Condition`, `TargetedField`, `MapValue` | High |
| **Plugin via reflection** | `RemoteData` loads `EDDY.Nexus.BusinessComponent.dll` + `IDataClass` | High |
| **Orchestration / Workflow** | WF4 `.xaml`/`.xamlx` compose activities | High |
| **Queue-based load leveling (DB)** | RDQ table polled by the service | Medium-High |
| **Correlation** | WCF `RequestReplyCorrelationInitializer` in realtime/preview services | High |
| **Unit of Work / Repository (formal)** | ❌ Not present — DAOs are per-method, no UoW/transaction scope in C# (transactions, if any, live in SPs) | High |
| **Mediator / CQRS / Decorator / Observer / Specification / Builder** | ❌ Not present in any recognizable form | Medium |

## Notes

- **Repository vs DAO:** the code uses classic DAOs (SP-centric), not the domain-driven Repository pattern. There is no `IUnitOfWork`; multi-step consistency is delegated to stored procedures.
- **Strategy in `ConditionBC`** is realized as large `switch`/`if` blocks rather than polymorphic operator classes — a refactor opportunity to make it a true Strategy set.
- **Factory** methods create by string/enum discriminators from XML — flexible but stringly-typed.
