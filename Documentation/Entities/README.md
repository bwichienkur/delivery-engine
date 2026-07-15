# 10. Entity Documentation

Catalog of the domain model in `EDDY.IS.DeliveryEngine.Entity`. For each entity: purpose, key relationships/navigation, business meaning, validation, lifecycle. Most entities are `[DataContract]` (WCF) and derive from `CommonEntity` (audit base). See also the class diagram in [../Diagrams/ClassDiagrams.md](../Diagrams/ClassDiagrams.md).

## Audit base & interfaces

### `CommonEntity` — `Common/CommonEntity.cs:10-41`
- **Purpose:** abstract base carrying audit + soft-state fields for most entities.
- **Fields:** `IsEnabled`, `IsDeleted`, `CreatedBy`, `CreatedDate`, `UpdatedBy`, `UpdatedDate`, `RowGuid`, `CurrentUser`, `CurrentCSR`.
- **Business meaning:** `IsEnabled` is the active/deactivated flag used pervasively; `IsDeleted` is **declared but never read/written** by DAOs (legacy). `CurrentCSR`/`CurrentUser` carry the acting operator for audit.
- **Lifecycle:** set on create/update via SP parameters (`@CreatedBy`/`@UpdatedBy`).

### `RequestHeader` — `Common/RequestHeader.cs:7-14`
Adds `ApplicationId`, `SiteId` on top of `CommonEntity`; base for request DTOs like `CapDistributionEntity`.

### `BaseXmlEntity` — `Common/BaseXmlEntity.cs:11-135`
XML serialize/deserialize helpers (`Serialize`, `Deserialize`, `ToXML`, `ToObject`). **No subclasses** in this project (utility base).

## Core delivery entities

### Lead family — `LeadEntity.cs`
| Entity | Lines | Purpose | Key relationships |
|--------|-------|---------|-------------------|
| `LeadEntity` | 19-415 | Core lead: identity, PII, program/CSR/PSI, status, `AdditionalFields` | 1→N `AdditionalLeadData`; N→1 `DeliveryDefinition` (chosen) |
| `LeadProcessingEntity` | 418-537 | Adds cap/tracking dims (`TrackingId`, `ChannelId`, `VendorId`, `UserExperienceList`, `UserDataList`) | — |
| `DeliveryLeadData` | 539-652 | Delivery-time lead + `TransformedNameValuePairs`, `IsRepost`, `IsBeta` | flows through workflows |
| `AdditionalLeadData` | 654-680 | Custom form fields (`ControlId`, `Label`, `Value`) | N→1 `LeadEntity` |

- **Business meaning:** a lead is a prospective-student inquiry. `DeliveryLeadData` is the enriched form the delivery workflows operate on; `IsRepost` bypasses regex conditions; `IsBeta` routes to `*Beta*` SPs/queues.
- **Validation:** condition XML matching; repost/beta flags gate behavior.
- **Lifecycle:** inserted (`EDDY_DE_InsertLead`) → matched → delivered per endpoint → status finalized → optionally reposted/scrubbed via Lead Viewer.

### `DeliveryDefinition` — `DeliveryDefinition.cs:47-78`
- **Purpose:** priority-ordered routing rule for a Client-School Relationship (`CRId`/CSR).
- **Navigation:** `EndPoints` (1→N `DeliveryEndpoint`); `ConditionXML` (match rules, evaluated by `ConditionBC`).
- **Business meaning:** multiple definitions per CSR create a waterfall/priority routing; the highest-priority matching definition wins.
- **Lifecycle:** created/cloned/updated/deleted via `EDDY_DE_*DeliveryDefinition*` SPs; deactivated via `IsEnabled`.

### `DeliveryEndpoint` hierarchy — `DeliveryDefinition.cs:123-424`
- **Abstract `DeliveryEndpoint`** — type, transformation id, retry policy, blackout, test mode, rejection reasons, `IsRealtime`, `IsPrimary`.
- **`InstantPostEndpoint`** — live/test POST/GET URLs, content type, success/fail/duplicate response strings.
- **`InstantEmailEndpoint`** — live/test recipients, template, delimiters.
- **`BatchEmailEndpoint`** — schedule + `BatchFileDefinition`.
- **`BatchFtpEndpoint`** — FTP/SFTP host, credentials, remote folder, schedule.
- **Business meaning:** a destination + how to deliver to it. `DeliveryEndPointItem` (`DeliveryEndPoint.cs:10-50`) is a lightweight admin **list** row (distinct from the rich config object).

### `RealtimeDeliveryQueueItem` — `RealtimeDeliveryQueueItem.cs:6-36`
- **Purpose:** RDQ work item binding `LeadId`+`EndpointId` with status, `CurrentDeliveryAttempts`, `LastDeliveryAttemptDatetime`, `NextDeliveryAttemptDatetime`, and `DeliveryEngineMachineKey` (machine affinity).
- **Lifecycle:** `CreateRDQItem` → retried/blacked-out/failed updates → `RemoveRDQItem` on success or max retries.

### Status lookups
- `RealtimeDeliveryStatus` (`RealtimeDeliveryStatus.cs:10-66`) — status lookup + enum of well-known status IDs (100–999).
- `RealTimeDeliveryStatusType` — status-type grouping for Lead Viewer filters.

## Transformation & condition entities

- `DataTransformationEntity` / `DataTransformationEntityNew` (`DataTransformation/`) — an ordered transformation task tied to a delivery def / form validation / condition; carries `ConditionXml`, `TaskXml`, `XSL`, `SequenceNo`.
- `ConditionXmlEntity`, `TaskXmlEntity`, `DeliveryDefsEntity`, `FormValidationEntity`, `TaskSelectEntity`, `ConditionSelectEntity`, `AppendNameValueTaskItem`, `XsltXmlEntity`, `KeyValueObject`, `ConsumerIdMapping`.
- `Condition` + `FieldToCompare` + `FtcParameter` (`ConditionValidator/`) — the XML rule model: a `Condition` is an array of `FieldToCompare` (field, operator, data source, params). `MatchingCondition`/`Result` are evaluation outputs.

## Cap entities
- `CapDistributionEntity` (`Cap/CapDistributionEntity.cs:7-49`) — create/edit cap request (school/client/level, date range, value, auto-recurring).
- `GetCapEntity` (`Cap/GetCapEntity.cs:10-79`) — cap read model with `IDataReader` mapping ctor.
- `CapLevelEntity` — cap-level assignment to entity metadata.

## Blackout entities — `DeliveryBlackoutPeriod.cs`, `DeliveryBlackoutItem.cs`
- `DeliveryBlackoutItem` — DB-backed blackout period.
- `DeliveryBlackoutPeriod` hierarchy + `BlackoutFactory`/`BlackoutHelper`/`BlackoutToken` — in-memory evaluation of one-time/daily/weekly/monthly/yearly windows.

## Lead Viewer / admin DTOs
`LeadViewerSearchParam`, `LeadViewerResultEntity`, `LeadViewerHistoryItem`, `LVRealtimeLogEntity`, `LVBatchLogEntity`, `BatchLeadViewerEntity`, `DeliveryPI` (post instructions), `DeliveryPostHTTPHeader`, `DeliverySelectItem`, `ConditionDataTypeItem`, `DeliveryRejectionReasons`, `CheckActiveEndPoint`, `EmailTemplate`, `LeadRejectionReasonItem`, `DeliveryDefinitionItem`, `DeliveryDefSearchParam`.

## Product-processing (EddyTracking) DTOs — namespace `ProductProcessing`
- `TransactionSummary` (`TransactionSummary.cs:7-84`), `TransactionDetail` (`TransactionDetail.cs:7-63`), `ActivityStep` (`ActivityStep.cs:5-23`) — transaction tracing header/steps/step-lookup.

## Misc
`UserData`, `UserExperience`/`UserExperienceList`, `BatchFileDefinition`.
