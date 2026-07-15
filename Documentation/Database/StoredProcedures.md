# Stored Procedure Catalog

All database access is via stored procedures called through the Enterprise Library Data block. This catalog lists every SP name found in source, the calling DAO method, and the source line. Dynamic (runtime-chosen) SPs are noted. Source: static analysis of `EDDY.IS.DeliveryEngine.DataAccess/*` (confidence: **High** for names/lines).

## LeadDAO (`LeadDAO.cs`) — DB: Nexus
| SP | Method | Line |
|----|--------|------|
| `dbo.Eddy_GetLeadById` | `GetDeliveryLeadDataByLeadId` | 26 |
| `dbo.EDDY_GetRealtimeLeadById` | `GetRealtimeDeliveryLeadDataByLeadId` | 103 |

## CapDistributionDAO (`CapDistributionDAO.cs`) — DB: Nexus
| SP | Method | Line |
|----|--------|------|
| `Eddy_CreateCap` | `CreateCAP` | 27 |
| `dbo.Eddy_GetCapDistribution` | `GetCap` | 55 |
| `Eddy_EditCap` | `EditCap` | 86 |
| `EDDY_UpdateCapValue` | `UpdateCapValueByCSRId` | 116 |
| `Eddy_CreateCap` | `CreateCapLevel` | 137 |
| `CAP_ExecuteCappingProcess` | `ProcessCap` | 177 |
| `Eddy_CAP_TerminateOrfantDuration` | `TerminateOrfantCapDuration` (not on interface) | 199 |

## ConditionDAO (`ConditionDAO.cs`) — DB: Nexus
| SP | Method | Line |
|----|--------|------|
| *(dynamic `storedProcedureName`)* | `GetComparisonTable` | 34 |
| `EDDY_DT_Task_Select` | `GetXmlTable` | 82 |

## ProductProcessingTransactionDAO (`ProductProcessingTransactionDAO.cs`) — DB: EddyTracking
| SP | Method | Line |
|----|--------|------|
| `dbo.EDDYT_PPT_TransactionDetail_Insert` | `InsertTransactionDetail` | 42 |
| `dbo.EDDYT_PPT_TransactionSummary_Save` | `SaveTransactionSummary` | 80 |
| `EDDYT_PPT_TransactionSummary_Get` | `GetTransactionSummary` | 121 |
| `EDDYT_PPT_TransactionDetail_Get` | `GetTransactionDetail` | 169 |

## DataTransformationDAO (`DataTransformationDAO.cs`) — DB: Nexus
| SP | Method | Line |
|----|--------|------|
| `EDDY_DT_Task_Insert` | `CreateData` | 34 |
| `EDDY_DT_Log_Insert` | `CreateLog` | 72 |
| *(dynamic `procedureName`)* | `GetStoredProcResult` | 102 |
| `EDDY_DT_Task_Delete` | `DeleteData` | 130 |
| `EDDY_DT_Task_Select` | `GetAllData` | 164 |
| `EDDY_DT_Task_Select` | `GetData` | 200 |
| `EDDY_DT_GetDeliveryDefs` | `GetDeliveryDefs` | 236 |
| `EDDY_DT_GetFieldNames` | `GetFieldNames` | 274 |
| `EDDY_DT_GetFormValidations` | `GetFormValidations` | 310 |
| `EDDY_DT_Task_SelectByConditionXMLId` | `GetTasksByConditionId` | 349 |
| `EDDY_DT_Task_SelectByDeliveryDefId` | `GetTasksByDeliveryDefId` / `LoadDataTransformationTable` | 389, 534 |
| `EDDY_DT_Task_SelectByFormValidationId` | `GetTasksByFormValidationId` / `LoadDataTransformationTable` | 429, 554 |
| `EDDY_DT_GetValueCodeFromProgramId` | `GetValueCodeFromProgramId` | 468 |
| `EDDY_DT_GetXSLById` | `GetXSLById` | 499 |
| `EDDY_DT_ConditionXML_Update` | `UpdateConditionXml` | 590 |
| `EDDY_DT_Task_Update` | `UpdateData` | 626 |
| `EDDY_DT_Task_Reorder` | `UpdateSequenceNo` | 664 |
| `EDDY_DT_TaskXML_Update` | `UpdateTaskXml` | 695 |
| `dbo.[EDDY_GetCorrespondingNexusIds]` | `GetELRNexusMappingData` / `GetConsumerIdMapping` | 724, 779 |
| `EDDY_DT_Task_Select_New` | `GetDataTransformationTask` | 823 |
| `EDDY_DT_GetDataTransformationTaskConditions` | `GetDataTransformationCondition` | 855 |
| `EDDY_DT_AddUpdateXsltXml` | `AddUpdateXsltXmlTasks` | 890 |
| `EDDY_DT_Task_GetXsltXmlTask` | `GetXsltXmlTask` | 932 |
| `EDDY_DT_Task_GetAppendNameValuePairs` | `GetDataTransformationAppendNameValueTask` | 966 |
| `EDDY_DT_RemoveDataTransformationCondition` | `RemoveDataTransformationCondition` | 1001 |
| `EDDY_DT_AddUpdateDataTransformationTaskConditions` | `AddUpdateDataTransformationCondition` | 1032 |
| `EDDY_DT_AddUpdateTasks` | `AddUpdateDataTransformationTasks` | 1075 |
| `EDDY_DT_Task_GetAppnedNameValueTasks` | `GetAppendNameValueTasks` | 1118 |
| `[EDDY_DT_Task_GetFieldFormatTasks]` | `GetFieldFormatTasks` | 1157 |
| `[EDDY_DT_GetDataTranformationFormats]` | `GetDataTranformationFormats` | 1196 |
| `EDDY_DT_TaskCheckDuplicateSequenceNo` | `DTCheckDuplicateSequence` | 1229 |
| `[EDDY_DT_Task_GetXsltXmlTasks]` | `GetXsltXmlTasks` | 1268 |
| `[EDDY_DT_Task_GetEmailSubjectAndEmailToTasks]` | `GetEmailSubjectAndEmailToTasks` | 1306 |

## DeliveryEngineDAO (`DeliveryEngineDAO.cs`) — DB: Nexus (largest surface, ~90 SPs)
| SP | Method | Line |
|----|--------|------|
| `dbo.EDDY_DE_CheckActiveEndPoints` | `CheckActiveEndPoint` | 77 |
| `EDDY_DE_GetCRDeliveryDefinitions` | `GetCRDeliveryDefinitions` (3 overloads) | 118, 166, 216 |
| `EDDY_DE_UpdateDeliveryDefinition` | `UpdateDeliveryDefinition` | 268 |
| `EDDY_DE_UpdateDeliveryConditions` | `UpdateDeliveryCondition` | 306 |
| `EDDY_DE_CloneDeliveryDefinition` | `CloneDeliveryDefinition` | 339 |
| `EDDY_DE_DeleteDeliveryDefinition` | `DeleteNonActiveDeliveryDefinition` | 370 |
| `EDDY_DE_GetDeliveryConditionWithType` | `GetDeliveryConditionWithType` | 402 |
| `EDDY_DE_GetDistinctCSRIds` | `GetDistinctCSRIds` | 434 |
| `EDDY_DE_TempDeliveryEngineLog_Insert` | `TempLog` | 468 |
| `EDDY_DE_GetDistinctPSIIds` | `GetDistinctPSIIds` | 505 |
| `EDDY_DE_GetProgramIDFromCSRId` | `GetProgramIDFromCSRId` | 539 |
| `EDDY_DE_InsertLead` | `InsertLead` | 578 |
| `EDDY_DE_GetDeliveryDefinition` | `GetDeliveryDefinition` | 636 |
| `EDDY_DE_GetDeliveryEndpoint` | `GetEndpointById` | 793 |
| `EDDY_DE_GetDTStoredProcedureNames` | `GetDTStoredProcedureNames` | 822 |
| `EDDY_DE_GetLeadsForBatch` | `GetLeadsForBatch` | 854 |
| `EDDY_DE_Batch_Insert` | `CreateNewBatchDelivery` | 880 |
| `EDDY_DE_Batch_Update` | `LogBatchDeliveryComplete` | 909 |
| `dbo.EDDY_DE_ResetRealtimeQueueForMachine` | `ResetRealtimeDeliveryQueueForMachine` | 941 |
| `EDDY_DE_PreviewDeliveryLog_Insert` | `LogPreviewDeliveryComplete` | 971 |
| `EDDY_DE_RealtimeDeliveryLog_Insert` | `LogRealtimeDeliveryComplete` | 1017 |
| `EDDY_DE_RealtimeDeliveryQueue_Insert` | `CreateRDQItem` | 1076 |
| `EDDY_DE_RealtimeDeliveryQueue_Update` | `UpdateRealtimeDeliveryQueue` | 1108 |
| `EDDY_DE_GetRepostsForDelivery` | `GetRDQsForProcessingReposts` | 1144 |
| `EDDY_DE_GetNewLeadsForProcessing_Slate` | `GetNewLeadsForProcessing` | 1199 |
| `EDDY_DE_GetLeadForPreview` | `GetLeadForPreview` | 1243 |
| `EDDY_DE_RealtimeDeliveryQueue_Delete` | `DeleteRealtimeDeliveryQueueRecord` | 1277 |
| `EDDY_DE_Lead_UpdateDeliveryDefinition` | `UpdateLeadDeliveryDefinition` | 1304 |
| `EDDY_DE_UpdateLeadDelivery` | `CreateLeadDeliveryRecords` | 1333 |
| `EDDY_DE_LogBatchLead` | `LogBatchLead` | 1360 |
| `EDDY_DE_FinalizeLeadDelivery` | `FinalizeLeadDelivery` | 1386 |
| `EDDY_DE_UpdateLeadStatusIfLastRealtimeDelivery` | `UpdateLeadStatusIfLastRealtimeDelivery` | 1444 |
| `dbo.EDDY_DE_GetDeliveryDataTypeTypes` | `GetDeliveryDataTypes` | 1474 |
| `EDDY_DE_AddUpdateDeliveryPostInstructions` | `AddUpdateDeliveryPostInstructions` (×2) | 1511, 1554 |
| `EDDY_DE_AddDefaultDTFields_Gradschools` / `_International` | `AddDefaultDataTransformationFields` | 1597, 1601 |
| `dbo.EDDY_DE_GetDeliveryPostInstructions` | `GetDeliveryPostInstructions` | 1624 |
| `dbo.EDDY_DE_GetDeliveryConditionTypes` | `GetDeliveryConditionTypes` | 1669 |
| `dbo.EDDY_DE_GetEndPointList` | `GetDeliveryEndPointList` | 1702 |
| `dbo.EDDY_DE_GetDeliveryBlackOutList` | `GetDeliveryBlackOutList` | 1735 |
| `dbo.EDDY_DE_GetDeliveryFrequencies` | `GetDeliveryFrequencies` | 1767 |
| `dbo.EDDY_DE_GetDeliveryBlackOut` | `GetDeliveryBlackOut` | 1797 |
| `dbo.EDDY_DE_GetCsrOffset` | `GetCrOffset` | 1848 |
| `dbo.EDDY_DE_HasActiveEndpoints` | `HasActiveEndpoints` | 1881 |
| `dbo.EDDY_DE_IsDeactivatingDefinition` | `IsDeactivatingDefinition` | 1917 |
| `dbo.EDDY_DE_AddUpdateDeliveryBlackOut` | `AddUpdateDeliveryBlackOut` | 1953 |
| `dbo.EDDY_DE_GetBloackOutDateTimeOffSets` | `GetBloackOutDateTimeOffSets` | 1995 |
| `dbo.EDDY_DE_GetDeliveryName` | `GetDeliveryName` | 2026 |
| `dbo.EDDY_DT_GetEmailTemplates` | `GetEmailTemplateNames` | 2057 |
| `dbo.EDDY_DE_GetDeliveryExpressionTypes` | `GetDeliveryExpressionTypes` | 2088 |
| `dbo.EDDY_DE_GetDeliveryConditions` | `GetDeliveryConditions` | 2119 |
| `dbo.EDDY_DE_CheckDuplicatePriority` | `CheckDuplicatePriority` | 2155 |
| `dbo.EDDY_DE_InsertDeliveryDefinition` | `InsertDeliveryDefinition` | 2190 |
| `dbo.EDDY_DE_DeliveryHasConditions` | `DeliveryHasConditions` | 2230 |
| `dbo.EDDY_DE_UpdateBatchStatusIfPrimaryDelivery` | `UpdateBatchStatusIfPrimaryDelivery` | 2265 |
| `dbo.EDDY_DE_FinalizeLeadDeliveryOnError` | `FinalizeLeadDeliveryOnError` | 2297 |
| `dbo.EDDY_DE_GetCountAgainstCapStatus` | `GetCountAgainstCapStatus` | 2326 |
| `dbo.EDDY_DE_RemoveRePostData` | `RemoveRePostData` | 2346 |
| `dbo.EDDY_DE_LoadGSData` | `LoadGSDataForLead` | 2363 |
| `EDDY_DE_GetThirdPartyLeadsForProcessing` | `GetThirdPartyLeadsForProcessing` | 2398 |
| `dbo.EDDY_DE_GetLeadRejectionReason` | `GetLeadRejectionReason` | 2438 |
| `dbo.EDDY_DE_GetNewReviewedStatusID` | `GetNewReviewedStatusID` | 2488 |
| `EDDY_DE_LogNewReviewedStatusID` | `LogNewReviewedStatusID` | 2526 |
| `EDDY_DE_AddToLeadHistory` | `LogForLeadViewerHistory` / `AddToLeadHistory` | 2558, 3709 |
| `dbo.EDDY_DE_GetEndpointBlackouts` | `GetEndpointBlackouts` | 2736 |
| `EDDY_DE_GetDefinitionEndpoints` | `GetDefinitionEndpoints` | 2803 |
| `EDDY_DE_GetEmailTemplate` | `GetEmailTemplate` | 2837 |
| `dbo.EDDY_DE_GetBETALeadViewerSearchResultsDynamic` / `...GetLeadViewerSearchResultsDynamic` | `GetLeadViewerSearchResult` | 3148, 3149 |
| `dbo.EDDY_DE_GetLeadDeliveryStatus` | `GetRealtimeDeliveryStatusList` | 3254 |
| `EDDY_DE_GetLeadDataByLeadId` | `GetLeadDataByLeadId` | 3290 |
| `dbo.EDDY_DE_UpdateLeadData` / `...UpdateBetaLeadData` | `UpdateLeadData` | 3332, 3335 |
| `dbo.EDDY_DE_GetAdditionalLeadDataByLeadId` / beta | `GetAdditionalLeadDataByLeadId` | 3395, 3397 |
| `EDDY_DE_GetLeadViewerHistory` | `GetleadViewerHistory` | 3432 |
| `EDDY_DE_GetCSRDeliveryDefinitions` | `CheckCRhasDeliveryDefinitions` | 3463 |
| `dbo.EDDY_DE_UpdateSelectedLeadsforRepost` / beta | `UpdateSelectedLeadsforRepost` | 3492, 3495 |
| `dbo.EDDY_DE_GetBetaLeadViewerSearchResults` / `...ForBatch` | `GetLeadViewerSearchResultForBatch` | 3525, 3526 |
| `dbo.EDDY_DE_Check(Beta)LeadViewerSearchResultsExceeds3KRows` | `CheckLeadViewerSearchResultsExceeds3KRows` | 3601 |
| `EDDY_DE_FailLeadFromLeadViewer` | `ScrubLeadFromLeadViewer` | 3650 |
| `EDDY_DE_MultipleLeadEdit` | `MultipleLeadEdit` | 3676 |
| `EDDY_DE_GetTrackCampaignName` | `GetTrackCampaignName` | 3736 |
| `dbo.EDDY_DE_GetDeliveryStatusType` | `GetDeliveryStatusTypeList` | 3759 |
| `EDDY_DE_GetDefaultPaidTemplateProductId` | `GetDefaultPaidTemplateProductId` | 3786 |
| `dbo.EDDY_DE_GetDeliveryEndpointHTTPHeaders` | `GetHTTPHeaders` | 3814 |

## EntLib logging SPs (EddyLogging DB) — from config
`WriteLog`, `EDDY_WriteException`, `AddCategory` (`Web.config:10-11`).

## Dynamic/runtime SPs
- Any SP name returned by `EDDY_DE_GetDTStoredProcedureNames`.
- Condition comparison SPs invoked by `ConditionDAO.GetComparisonTable` (from Condition XML `<FieldToCompare>` `StoredProcedure` source).
- Transformation SPs invoked by `DataTransformationDAO.GetStoredProcResult`.
- `SetEmailTo` uses `Eddy_DE_LookUpDeliveryEmail` (`Workflow.Activities/DataTransformation/SetEmailTo.cs:72`).
