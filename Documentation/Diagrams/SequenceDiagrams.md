# 26. Sequence Diagrams (Major Workflows)

Additional sequences (realtime, batch, email) are inline in [../BusinessProcesses.md](../BusinessProcesses.md). This file adds the retry and preview sequences and the sub-workflow composition.

## Realtime endpoint dispatch (sub-workflow composition)

```mermaid
sequenceDiagram
    participant IDW as InstantDeliveryWF
    participant REW as RealtimeEndpointWF
    participant IPW as InstantPostWF
    participant IEW as InstantEmailWF
    IDW->>REW: run(lead, rdqItem, endpoint)
    REW->>REW: CheckEndpointBlackout
    alt blacked out
        REW->>REW: UpdateRDQForBlackout (516)
    else
        alt DeliveryType == InstantPost
            REW->>IPW: run
            IPW-->>REW: post status (600/515/610)
        else DeliveryType == InstantEmail
            REW->>IEW: run
            IEW-->>REW: email status (600/515)
        end
    end
    REW->>REW: if last & success -> UpdateLeadStatusIfLastRealtimeDelivery
    REW-->>IDW: endpoint status + school/reviewed status
```

## Retry (RDQ repost)

```mermaid
sequenceDiagram
    participant WS as WindowsService
    participant RS as RetryLeadDeliveryWorkflowService
    participant REW as RealtimeEndpointWF
    participant DAO as DeliveryEngineDAO
    WS->>DAO: GetRDQsForProcessingReposts
    DAO-->>WS: RDQ items
    WS->>RS: RetryProcessLeads(rdq, lead, endpointId)
    RS->>DAO: GetDeliveryEndpoint
    RS->>REW: run (single endpoint)
    REW-->>RS: status
    RS->>DAO: InsertTransactionDetail(520)
```

## Preview (dry run)

```mermaid
sequenceDiagram
    participant UI as Admin/Caller
    participant PS as LeadPreviewDeliveryWorkflowService
    participant PW as RealtimePreviewWF
    participant TX as TransformData
    UI->>PS: ProcessPreview(LeadData)
    PS->>PW: run
    PW->>PW: GetDeliveryDefinition
    alt no definition
        PW->>PW: PreviewError -> Result
    else
        loop each realtime endpoint
            PW->>TX: TransformData + CreatePostData/CreateEmailBody
            PW->>PW: Log*Preview* -> Result (NO send)
        end
    end
    PS-->>UI: SendReply(Result dictionary)
```

## Lead processing service (one-way, full)

```mermaid
sequenceDiagram
    participant WS as WindowsService
    participant LPS as LeadProcessingDeliveryWorkflowService
    participant CFG as SeLeadProcessingConfigurationValues
    participant IDW as InstantDeliveryWF
    WS->>LPS: ProcessLeads(DeliveryLeadData)
    LPS->>LPS: InsertTransactionDetail(180)
    LPS->>CFG: get flags (ProcessCap=false, ScoreLead=false hardcoded; DeliverLead from config)
    opt ScoreLead
        LPS->>LPS: ScoreLead -> InsertTransactionDetail(190)
    end
    opt ProcessCap
        LPS->>LPS: ProcessCap -> InsertTransactionDetail(200)
    end
    opt DeliverLead
        LPS->>IDW: run -> FinalizeLeadDelivery -> InsertTransactionDetail(360)
    end
```
