# Delivery Engine — Engineering Documentation

> **Solution:** `EDDY.Services.DE.sln`
> **Product family:** EducationDynamics ("EDDY" / "Nexus" / "Cheetah") lead management platform
> **Component:** Delivery Engine (lead routing & distribution)
> **Platform:** .NET Framework 4.5 / 4.5.2, Windows Workflow Foundation 4 (WF4), WCF Workflow Services, Enterprise Library 5.0.414, SQL Server
> **Source control (historical):** TFS (`http://eddytfs2015:8080/tfs/eddyprojectcollection` — see `EDDY.Services.DE.sln` lines 520–546)

This documentation reverse-engineers the Delivery Engine so a new engineering team (or an AI agent) can maintain, extend, and eventually modernize it **without** the original developers.

Every non-trivial claim references source code (path + line). Where a fact is **inferred** (e.g., database schema, which has no `.sql` files in the repo), the confidence level is stated explicitly.

---

## How to read this documentation

Start here → then follow the numbered flow:

| # | Document | What it answers |
|---|----------|-----------------|
| 1 | [ExecutiveSummary.md](./ExecutiveSummary.md) | What is this, who uses it, why it exists |
| 2 | [Architecture.md](./Architecture.md) | Architecture style, layers, violations, DI, config, logging, exceptions |
| 3 | [BusinessProcesses.md](./BusinessProcesses.md) | The real workflows (realtime, batch, retry, preview, cap, transformation) |
| 4 | [Projects/](./Projects/) | One document per project (purpose, deps, classes, packages, improvements) |
| 5 | [Entities/](./Entities/) | Domain model / entity catalog |
| 6 | [Database/](./Database/) | DbContext (none — EntLib), tables, stored procs, ER diagram, audit/soft-delete |
| 7 | [APIs/](./APIs/) | WCF workflow service "endpoints" (operations) |
| 8 | [Services/](./Services/) | Business components, DAOs, integration helpers |
| 9 | [Security/](./Security/) | Security review + recommendations |
| 10 | [Performance/](./Performance/) | Performance review |
| 11 | [Deployment/](./Deployment/) | Build, environments, hosting (IIS + Windows Service) |
| 12 | [Diagrams/](./Diagrams/) | Mermaid: data flow, sequence, class, flowcharts, dependency graphs |
| 13 | [Refactoring/](./Refactoring/) | Ranked, actionable modernization backlog |
| 14 | [AI_CONTEXT.md](./AI_CONTEXT.md) | Dense summary for another AI agent |

---

## 30-second orientation

The Delivery Engine takes **leads** (prospective-student inquiries captured by web forms) and **delivers** them to partner schools/vendors via **HTTP POST**, **email**, or **batch file (email/FTP/SFTP)**, according to per-relationship **Delivery Definitions** (priority-ordered routing rules with XML match conditions and per-endpoint **data transformation** pipelines).

- A **Windows Service** (`EDDY.IS.DeliveryEngine.WindowsService`) polls the `Nexus` SQL database (~every 5 seconds) for new/retry leads and calls…
- …**WCF Workflow Services** (`EDDY.IS.DeliveryEngine.Workflow.Service`, IIS-hosted `.xamlx`) which run…
- …**WF4 workflows** (`EDDY.IS.DeliveryEngine.Workflow`, `.xaml`) composed of **code activities** (`EDDY.IS.DeliveryEngine.Workflow.Activities`) that call…
- …**DAOs** (`EDDY.IS.DeliveryEngine.DataAccess`) which execute **~110+ stored procedures** against SQL Server using the Enterprise Library Data Access block.

See [Architecture.md](./Architecture.md) for the full picture.

---

## Important caveats about this repository

- **Compiled artifacts are committed.** `bin/`, `Bin/`, and `obj/` folders (DLLs, PDBs, generated `*.g.cs`) are in source control. All analysis here is based on **source only** (`.cs`, `.xaml`, `.xamlx`, `.csproj`, `.config`).
- **No database project / migrations.** The schema is **inferred** from stored-procedure names, `IDataReader` column usage, and entity mappings. See [Database/](./Database/).
- **Two near-duplicate host projects.** `EDDY.IS.DeliveryEngine.Test` (WinForms harness) is a fork of the Windows Service and is **not** part of `EDDY.Services.DE.sln`.
- **Dead/forked code exists.** `EDDY.IS.DeliveryEngine.Workflow.Service/Activities/` contains ~51 stale activity files that are **not compiled** (not in the `.csproj`).
- **Secrets & internal hostnames are committed** in config files (SMTP servers, internal DB/service host names, email addresses, a HubSpot test API key). See [Security/](./Security/).
