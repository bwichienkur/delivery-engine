# 25. Data Flow Diagrams

## Request lifecycle (realtime lead)

```mermaid
flowchart LR
    NEW[New lead in Nexus] --> POLL[WindowsService poll]
    POLL --> WCF[ProcessLeads WCF call]
    WCF --> IDW[InstantDeliveryWF]
    IDW --> DEF[GetDeliveryDefinition + ConditionBC]
    DEF --> EP[Per endpoint]
    EP --> TX[TransformData -> DataTransformationBC]
    TX --> PAY[CreatePostData / CreateEmailBody]
    PAY --> SEND[DeliverInstantPost / DeliverInstantEmail]
    SEND --> INT[InterpretPostResponse]
    INT --> STAT[Adjust/Finalize status + RDQ update]
    STAT --> DB[(Nexus: status, logs, RDQ)]
```

## Database interactions

```mermaid
flowchart TB
    A[Activities / BCs] --> DS[*DataService static locators]
    DS --> DAO[DAOs]
    DAO -->|EntLib GetStoredProcCommand| N[(Nexus)]
    PPT[ProductProcessingTransactionDAO] --> T[(EddyTracking)]
    ENTLIB[EntLib Logging listeners] --> L[(EddyLogging)]
```

## Service interactions

```mermaid
flowchart LR
    WS[WindowsService] -->|ProcessLeads| LP[LeadProcessing.xamlx]
    WS -->|RetryProcessLeads| RT[Retry.xamlx]
    CLI[CLI -b / harness] -->|processBatch| BA[Batch.xamlx]
    ADMINUI[Admin UI - external] -->|ProcessPreview| PV[Preview.xamlx]
    LP --> WF[WF4 workflows]
    RT --> WF
    BA --> WF
    PV --> WF
```

## External API interactions

```mermaid
flowchart LR
    SEND[Delivery activities] -->|HTTP POST/GET/SOAP/JSON| PART[Partner endpoints]
    SEND -->|SMTP| MAIL[Partner mailboxes]
    SEND -->|SFTP WinSCP / FTP| DROP[Partner file drops]
    COND[ConditionBC/RemoteData] -->|Assembly.LoadFile + IDataClass| PLUG[EDDY.Nexus.BusinessComponent.dll]
```

## Authentication flow

```mermaid
flowchart LR
    WS[WindowsService LocalSystem] -->|Integrated Security| SQL[(SQL Server)]
    WS -->|basicHttpBinding security=None| IIS[WCF services]
    IIS -->|Integrated Security / app-pool identity| SQL
    Note[No app-level authN/authZ; network-trusted boundary]
```
Confidence: **High** — no auth code exists; DB uses Integrated Security; WCF security mode is `None`.

## Background processing

```mermaid
flowchart TD
    START[OnStart] --> LOOP{stop signalled?}
    LOOP -- yes --> END[join worker, stop]
    LOOP -- no --> POLL[poll new/retry/thirdparty]
    POLL --> DISP[Parallel.ForEach + lock -> WCF]
    DISP --> SLEEP[Thread.Sleep 5000]
    SLEEP --> LOOP
```
