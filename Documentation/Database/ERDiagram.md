# Inferred ER Diagram

> **INFERRED (Medium–High confidence).** No schema artifacts exist in the repo; this diagram is reconstructed from stored-procedure names, entity `*TableField` enums, `IDataReader` column usage, and entity mappings. Column lists are representative, not exhaustive. Column data types, PKs/FKs, indexes, and constraints must be confirmed against the live `Nexus`/`EddyTracking` databases.

```mermaid
erDiagram
    LEAD ||--o{ ADDITIONAL_LEAD_DATA : has
    LEAD ||--o{ REALTIME_DELIVERY_QUEUE : queued_for
    LEAD ||--o{ REALTIME_DELIVERY_LOG : attempts
    LEAD ||--o{ BATCH_DELIVERY_LOG : batched_in
    LEAD ||--o{ LEAD_VIEWER_HISTORY : edited_in
    LEAD }o--|| DELIVERY_DEFINITION : delivered_by

    DELIVERY_DEFINITION ||--o{ DELIVERY_ENDPOINT : contains
    DELIVERY_DEFINITION {
        int DeliveryDefinitionId PK
        int CRId
        int Priority
        xml ConditionXML
        bit IsEnabled
        string CreatedBy
        datetime CreatedDate
    }

    DELIVERY_ENDPOINT ||--o{ REALTIME_DELIVERY_QUEUE : targets
    DELIVERY_ENDPOINT ||--o{ DELIVERY_BLACKOUT : windows
    DELIVERY_ENDPOINT ||--o{ DELIVERY_ENDPOINT_HTTP_HEADER : headers
    DELIVERY_ENDPOINT }o--o| DT_TASK : transformed_by
    DELIVERY_ENDPOINT {
        int DeliveryEndpointId PK
        int DeliveryDefinitionId FK
        int DataTransformationId FK
        xml EndpointDetailXML
        bit IsRealtime
        bit IsPrimary
        int MaxRetryAttempts
    }

    LEAD {
        int LeadId PK
        int DeliveryDefinitionId FK
        int ApplicationId
        int CRId
        string Email
        bit IsBeta
        int DeliveryStatus
    }

    REALTIME_DELIVERY_QUEUE {
        int RDQId PK
        int LeadId FK
        int DeliveryEndpointId FK
        int Status
        int CurrentDeliveryAttempts
        datetime NextDeliveryAttemptDatetime
        uniqueidentifier DeliveryEngineMachineKey
    }

    REALTIME_DELIVERY_LOG {
        int LogId PK
        int LeadId FK
        int DeliveryEndpointId FK
        int Status
        datetime AttemptDatetime
        string ResponseMessage
    }

    DT_TASK ||--o{ DT_CONDITION_XML : gated_by
    DT_TASK ||--o{ DT_TASK_XML : payload
    DT_TASK {
        int TaskId PK
        int DeliveryDefId FK
        int SequenceNo
        int ConditionXmlId FK
        int TaskXmlId FK
        bit IsEnabled
    }

    BATCH_DELIVERY ||--o{ BATCH_DELIVERY_LOG : logs
    BATCH_DELIVERY {
        int BatchDeliveryId PK
        int DeliveryEndpointId FK
        int ProductId
        datetime CreatedDate
    }

    CAP_DISTRIBUTION {
        int CapId PK
        int ClientId
        int SchoolId
        int CapLevel
        int CapValue
        int CapCount
        datetime StartDate
        datetime EndDate
    }

    TRANSACTION_SUMMARY ||--o{ TRANSACTION_DETAIL : steps
    TRANSACTION_SUMMARY {
        int TransactionSummaryId PK
        int LeadId
        string Status
    }
    TRANSACTION_DETAIL {
        int TransactionDetailId PK
        int TransactionSummaryId FK
        int ActivityStepId FK
        string Message
    }
    ACTIVITY_STEP ||--o{ TRANSACTION_DETAIL : classifies
```

## Notes on inferred relationships

- **`LEAD.DeliveryDefinitionId`** is set by `EDDY_DE_Lead_UpdateDeliveryDefinition` (`UpdateLeadDeliveryDefinition`) — the chosen definition per lead.
- **`REALTIME_DELIVERY_QUEUE.DeliveryEngineMachineKey`** partitions work across delivery-engine machines (`GetNewLeadsForProcessing` takes a machine key).
- **`DT_TASK`** links to condition/task XML rows; the actual transformation logic is XML content, not columns.
- **`TRANSACTION_SUMMARY`/`TRANSACTION_DETAIL`/`ACTIVITY_STEP`** live in the **`EddyTracking`** database, separate from `Nexus`.
- **Beta shadow tables** (parallel `*Beta*` SP variants) suggest a duplicated lead/lead-data path used for A/B or migration; not modeled above (confidence Medium).

## Recommended next step (to make this authoritative)

Extract the real schema from the databases, e.g.:

```sql
-- run against Nexus and EddyTracking
SELECT s.name AS [schema], t.name AS [table] FROM sys.tables t JOIN sys.schemas s ON s.schema_id=t.schema_id ORDER BY 1,2;
SELECT name FROM sys.procedures ORDER BY name;      -- compare to StoredProcedures.md
SELECT name, definition FROM sys.sql_modules;       -- SP/view/function bodies (the real business logic)
-- plus sys.foreign_keys, sys.indexes, sys.triggers
```

Commit the results (or an SSDT `.sqlproj`) so the schema is versioned alongside the code.
