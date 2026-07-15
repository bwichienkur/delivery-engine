# 11 & 22. Services, Business Components & Design Patterns — Index

| Document | Contents |
|----------|----------|
| [Activities.md](./Activities.md) | Full inventory of WF4 code activities (Section 6/11) |
| [BusinessComponents.md](./BusinessComponents.md) | `DataTransformationBC`, `ConditionBC`, `CapDistributionComponent`, `LeadScoringBusinessComponent`, `ProductProcessingTransactionManager` |
| [DeliveryEngineBC.md](./DeliveryEngineBC.md) | Admin/Lead-Viewer facade over DAOs |
| [Integrations.md](./Integrations.md) | SMTP / HTTP / FTP-SFTP / external DLL integrations (Section 16) |
| [BackgroundProcessing.md](./BackgroundProcessing.md) | Poll loop, RDQ, retries, blackout (Section 15) |
| [DesignPatterns.md](./DesignPatterns.md) | Every design pattern used (Section 22) |
| [DependencyInjection.md](./DependencyInjection.md) | DI/lifetime/factories/hosted services (Section 12) |

## Cross-cutting service facts

- **No DI container.** Services are `new`'d or reached via static `*DataService` holders. See [DependencyInjection.md](./DependencyInjection.md).
- **DAOs** are the single persistence mechanism; see [../Database/](../Database/).
- **Retry/caching/telemetry:** minimal — retries are per-channel (email SMTP retries, POST RDQ retries); **no caching layer**; logging via EntLib + `EDDYLogger`.
