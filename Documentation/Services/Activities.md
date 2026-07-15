# 6. Activity Inventory (Workflow.Activities)

~48 WF4 `CodeActivity` classes. All follow the **Template Method** pattern (override `Execute(CodeActivityContext)`), are effectively **stateless per execution**, and typically call a `*DataService` DAO. Line references are into each file's `Execute` region.

## Delivery
| Activity | Purpose |
|----------|---------|
| `DeliverInstantPost` | HTTP(S) POST/GET with header + variable-URL substitution, TLS1.2 (`:22-103`) |
| `DeliverInstantEmail` | SMTP send with retry; product-specific From (`:21-118`) |
| `DeliverBatchEmail` | Email batch file attachment; skips empty GS batches (`:20-125`) |
| `DeliverBatchFTP` | Upload batch file via WinSCP SFTP or `FtpWebRequest` (`:16-55`) |
| `CreateBatchDelivery` | Insert batch record → `BatchDeliveryId` (`:17-36`) |
| `CreateBatchLeadFile` | Build delimited/XML file under dated dir tree (`:27-360`) |
| `CreateEmailBody` | Build HTML/text/template body (`:16-93`) |
| `CreatePostData` | Build form/XML/JSON payload (`:20-83`) |
| `CreateRDQItem` | Insert RDQ item (skipped for beta) (`:19-42`) |
| `RemoveRDQItem` | Delete RDQ record on success (`:18-33`) |
| `InterpretPostResponse` | Parse response → success/reject/duplicate/auto-review/school status (`:31-190`) |
| `TransformData` | Run `DataTransformationBC` per endpoint (`:33-196`) |
| `CheckEndpointBlackout` | Evaluate blackout; output next attempt time (`:14-55`) |
| `PreviewError` | Populate preview result with "definition not found" (`:13-19`) |

## Data retrieval
| Activity | Purpose |
|----------|---------|
| `GetLeadData` | Load `DeliveryLeadData` by id (`:18-33`) |
| `GetRealtimeLeadData` | Load realtime lead; **swallows exceptions** (`:18-40`) |
| `GetDeliveryDefinition` | Highest-priority active matching definition (`:26-111`) |
| `GetDeliveryEndpoint` | Load endpoint by id (`:18-28`) |
| `GetLeadsForBatch` | Load leads for a batch (`:19-49`) |
| `GetRDQsForProcessing` | **Stub** → empty list (`:19-30`) |
| `GetCap` | **Stub** → not capped (`:11-21`) |
| `GetLeadDeliveryStatus` | Finalize with computed status; cap→0/1 (`:23-64`) |
| `GetLeadRealtimeStatusFromEndpointStatuses` | Aggregate endpoint statuses (`:14-55`) |

## Status / update
| Activity | Purpose |
|----------|---------|
| `UpdateLeadDelivery` | Create per-endpoint delivery records (`:16-27`) |
| `UpdateLeadDeliveryDefinition` | Persist chosen definition id (`:17-28`) |
| `UpdateRDQFailedStatus` | RDQ 515 + next attempt (`:19-46`) |
| `UpdateRDQForBlackout` | RDQ 516 + deferred time (`:19-48`) |
| `AdjustLeadRealtimeStatus` | Adjust aggregate from primary endpoint (`:15-71`) |
| `UpdateLeadStatusIfLastRealtimeDelivery` | Update on last endpoint (`:23-52`) |
| `UpdateStatusForBatchIfPrimaryDelivery` | Update batch lead if primary (`:21-36`) |
| `FinalizeLeadDelivery` | Write final realtime status (`:19-55`) |
| `FinalizeLeadPreview` | Finalize preview status (`:20-36`) |
| `RemoveRePost` | Clear repost flag/data (`:17-29`) |

## Logging (15)
`WriteToDELog` (`:27-81`), `WriteToRealtimeDELog` (`:28-83`), `LogBatchSent`, `LogBatchLead`, `LogInstantEmailSuccessful` (200), `LogInstantEmailFailed` (555), `LogInstantEmailBlackedOut` (console), `LogSuccessfulPost`, `LogFailedPost`, `LogInstantPostBlackedOut` (console), `LogInstantEmailPreviewSuccessful`, `LogInstantEmailPreviewFailed` (console), `LogSuccessfulPreviewPost`, `LogFailedPostPreview`, `Helper/ExceptionLoggingActivity` (Event Log source `"EDDY"`).

## Service-only activities (Workflow.Service/Activities, compiled)
`ScoreLead`, `ProcessCap`, `InsertTransactionDetail`, `SeLeadProcessingConfigurationValues` (hardcodes cap/score false), `WriteToEventLog`, `ExceptionLoggingActivity` (forked, runs EntLib `WORKFLOW_SERVICE_POLICY`).

## Complexity hotspots
- `CreateBatchLeadFile` (~361 lines; `CreateDelimitedFile` ~110).
- `InterpretPostResponse` (~190 lines of nested regex/status logic in one `Execute`).
- `HttpPost` (~495; duplicated overloads).

## Thread-safety notes
- `HttpPost` mutates global `ServicePointManager.ServerCertificateValidationCallback` per call (`:80-81`) — AppDomain-wide side effect.
- `ConditionBC._log` `StringBuilder` reused across calls — unsafe if a single instance is shared across threads.
- `ProductProcessingTransactionManager` uses `lock(_Locker)` correctly to serialize DB writes.
- `ApplyXsltForXml` uses temp files (random names) with no locking.
