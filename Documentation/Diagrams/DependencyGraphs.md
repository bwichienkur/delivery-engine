# 29. Dependency Graphs

## Project reference graph (build-time)

```mermaid
graph TD
    Entity
    DataAccess --> Entity
    Activities[Workflow.Activities] --> DataAccess
    Activities --> Entity
    Workflow --> Activities
    Workflow --> DataAccess
    Workflow --> Entity
    Service[Workflow.Service] --> Workflow
    Service --> Activities
    Service --> DataAccess
    Service --> Entity
    WinSvc[WindowsService] --> Activities
    WinSvc --> DataAccess
    WinSvc --> Entity
    UnitTest --> Activities
    UnitTest --> DataAccess
    UnitTest --> Entity
```

## Runtime dependency graph (process/service)

```mermaid
graph LR
    WinSvc[WindowsService process] -->|WCF basicHttp| Service[IIS WCF services]
    WinSvc -->|ADO.NET/EntLib| Nexus[(Nexus)]
    Service -->|ADO.NET/EntLib| Nexus
    Service -->|ADO.NET/EntLib| Track[(EddyTracking)]
    Service -->|EntLib logging| Log[(EddyLogging)]
    Service -->|SMTP| Smtp[(SMTP)]
    Service -->|HTTP| Partners[(Partner endpoints)]
    Service -->|SFTP/FTP| Files[(Partner file drops)]
    Service -->|Assembly.LoadFile| Plugin[EDDY.Nexus.BusinessComponent.dll]
```

## External library dependencies

```mermaid
graph TD
    subgraph Solution
        DataAccess
        Activities
        Service
        WinSvc
    end
    subgraph InternalDLLs[EDDY.Nexus.Common.* - source not in repo]
        Util[Utilities]
        Logg[Logging]
        Exc[ExceptionHandler]
    end
    subgraph EntLib[Enterprise Library 5.0.414]
        ELData[Data]
        ELLog[Logging + Logging.Database]
        ELExc[ExceptionHandling + .Logging + .WCF]
        ELCommon[Common]
    end
    DataAccess --> Util
    DataAccess --> ELData
    DataAccess --> ELExc
    Service --> ELLog
    Service --> ELExc
    Activities --> WinSCP[WinSCPnet.dll]
    Activities --> Util
    WinSvc --> Logg
```

## Notable coupling observations

- `Entity` is a **stable hub** (everything depends on it; it depends on nothing internal).
- `DeliveryEngineDAO` is a **hotspot** (~3800 lines) that many activities/BCs reach through `DeliveryEngineDataService`.
- `WindowsService`↔`Workflow.Service` coupling is **contract-only** (WCF), not project references — the cleanest seam in the system and the natural place to re-platform.
