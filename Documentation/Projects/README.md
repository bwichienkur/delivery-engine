# 4 & 5. Project & Folder Documentation — Index

One document per project (purpose, responsibilities, dependencies, important classes, configuration, external services, NuGet/DLL packages, potential improvements). Folder-level notes are embedded within each project doc.

| Project | Doc |
|---------|-----|
| Entity | [Entity.md](./Entity.md) |
| DataAccess | [DataAccess.md](./DataAccess.md) |
| Workflow.Activities | [Workflow.Activities.md](./Workflow.Activities.md) |
| Workflow | [Workflow.md](./Workflow.md) |
| Workflow.Service | [Workflow.Service.md](./Workflow.Service.md) |
| WindowsService | [WindowsService.md](./WindowsService.md) |
| UnitTest | [UnitTest.md](./UnitTest.md) |
| Test (WinForms harness, not in sln) | [Test.md](./Test.md) |

## Common facts across all projects

- **Target framework:** `.NET Framework 4.5` (UnitTest is `4.5.2`).
- **Dependency management:** direct DLL/GAC references; **no NuGet `packages.config`** in any project.
- **Shared internal libraries (DLL-only, source not in repo):** `EDDY.Nexus.Common.Utilities`, `EDDY.Nexus.Common.Logging`, `EDDY.Nexus.Common.ExceptionHandler`.
- **Enterprise Library 5.0.414** blocks: `Common`, `Data`, `Logging`, `Logging.Database`, `ExceptionHandling`, `ExceptionHandling.Logging`, `ExceptionHandling.WCF`.
- **Build configurations:** `Debug`, `Release`, plus environment configs `Demo`, `DEV01`, `DevStage`, `DEVSTAGE02`, `MikeTestEnv`, `PRODUCTIONSUPORT`, `QA`, `QAStage`, `QASTAGE02`, `UAT` (`EDDY.Services.DE.sln:21-58`).
- **Committed build artifacts:** `bin/`, `Bin/`, `obj/` are in source control (should be `.gitignore`d).
