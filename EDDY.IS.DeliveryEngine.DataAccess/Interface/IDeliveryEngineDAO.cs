using System;
using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.Nexus.DeliveryEngine.Entity;

namespace EDDY.IS.DeliveryEngine.DataAccess.Interface
{
    public interface IDeliveryEngineDAO //: IBaseDataSource
    {
        # region Public Methods

        DeliveryDefinition GetDeliveryDefinition(int deliveryDefinitionId);

        List<DeliveryDefinition> GetCRDeliveryDefinitions(DeliveryDefSearchParam deliveryparam);

        List<DeliveryDefinition> GetCRDeliveryDefinitions(int CsrId);

        List<DeliveryEndpoint> GetDefinitionEndpoints(DeliveryDefinition deliveryDefinition);

        List<DeliveryEndpoint> GetDefinitionEndpoints(int deliveryDefinitionId);

        DeliveryEndpoint GetEndpointById(int deliveryEndpointId);

        List<DeliveryLeadData> GetLeadsForBatch(int batchDeilveryId);

        int CreateNewBatchDelivery(int deliveryEndpointId, int userId, int productId, string machineName);

        bool LogBatchDeliveryComplete(int batchDeilveryId, DeliveryEndpoint deliveryEndpoint, string batchFileName, int leadCount, int userId);

        bool LogRealtimeDeliveryComplete(int leadid, int deliveryEndpointId, string leadData, string endpointDetailXML, bool deliverySuccess, DateTime deliveryDateTime, int attemptNumber, string serverResponse, string postResponse, Guid machineKey, int userId, bool IsBeta, string Detination, int rejectionReasonId, int reviewedStatusId, int schoolValidationResponseStatusID);

        RealtimeDeliveryQueueItem CreateRDQItem(int leadId, int deliveryEndpointId, Guid machinekey, int userId);

        bool UpdateRealtimeDeliveryQueue(int realtimeDeliveryQueueId, int deliveryStatusId, Guid machineKey, int currentDeliveryAttempts, DateTime lastDeliveryAttemptDatetime, DateTime nextDeliveryAttemptDatetime, int userId);

        List<RealtimeDeliveryQueueItem> GetRDQsForProcessingReposts(int returnRecordsCount, string machineKey, bool updateStatus);

        List<DeliveryLeadData> GetNewLeadsForProcessing(int returnRecordsCount, string machineName, bool updateStatus, bool IsBeta);

        bool DeleteRealtimeDeliveryQueueRecord(int realtimeDeliveryQueueId);

        bool UpdateLeadDeliveryDefinition(int leadId, int deliveryDefinitionId, int userId);

        bool ResetRealtimeDeliveryQueueForMachine(string machineKey);

        int CreateLeadDeliveryRecords(int leadId, int deliveryDefinitionId, int userId,bool IsBeta);

        bool FinalizeLeadDelivery(int leadId, int leadRealtimeDeliveryStatus, int userId);

        bool LogForLeadViewerHistory(int leadId, string originalStatusIDxml, string modifiedStatusIDxml);

        bool UpdateLeadStatusIfLastRealtimeDelivery(int leadId, int status, int userId, bool isTest, bool isPrimary,bool IsBeta);

        List<DeliverySelectItem> GetDeliveryDataTypes();

        List<DeliverySelectItem> GetDeliveryConditionTypes(int DelDefID);

        List<DeliverySelectItem> GetDeliveryExpressionTypes();

        DeliveryDefinition GetDeliveryConditions(int CsrID, int DelDefID);
        bool UpdateDeliveryDefinition(DeliveryDefinition deliveryDefinition);

        List<DeliveryEndPointItem> GetDeliveryEndPointList(int DelDefID, int DeliveryTypeID);

        bool CheckDuplicatePriority(int CsrID, int Priority, int DelDefID);

        List<ConditionDataTypeItem> GetDeliveryConditionWithType();

        bool UpdateDeliveryCondition(int CsrID, int DelDefID, string ConditionXML);

        DeliveryPI GetDeliveryPostInstructions(int EndPointID);

        int AddUpdateDeliveryPostInstructions(DeliveryPI delPI);

        int InsertDeliveryDefinition(DeliveryDefinitionItem deldef);

        string GetDeliveryName(int deldefId);

        List<DeliveryBlackoutItem> GetDeliveryBlackOutList(int EndPointID);

        List<DeliverySelectItem> GetDeliveryFrequencies();

        DeliveryBlackoutItem GetDeliveryBlackOut(int BlackoutID);

        List<DeliverySelectItem> GetBloackOutDateTimeOffSets();

        bool AddUpdateDeliveryBlackOut(DeliveryBlackoutItem blackout);

        int GetCrOffset(int CsrID);

        CheckActiveEndPoint CheckActiveEndPoint(int CSRID, int DelDefID, int EndPointID);

        int HasActiveEndpoints(int DelDefID);

        int IsDeactivatingDefinition(int DelDefID, int @EndPointID);

        string DeliveryHasConditions(int DelDefID);

        List<LeadViewerResultEntity> GetLeadViewerSearchResult(LeadViewerSearchParam lvsearch);

        List<EmailTemplate> GetEmailTemplateNames();

        bool LogPreviewDeliveryComplete(int leadid, int deliveryEndpointId, string leadData, string endpointDetailXML, bool deliverySuccess, DateTime deliveryDateTime, int attemptNumber, string serverResponse, string postResponse, Guid machineKey, int userId);

        List<DeliveryLeadData> GetLeadForPreview(int LeadID);

        bool CloneDeliveryDefinition(int DelDefID);

        List<LeadViewerHistoryItem> GetleadViewerHistory(int LeadID);
        
        string UpdateSelectedLeadsforRepost(string LeadsIDs, int UserID, string isBetaLead);

        bool UpdateBatchStatusIfPrimaryDelivery(int leadId, int deliveryEndpointId, bool isSuccess);

        bool FinalizeLeadDeliveryOnError(int leadId, int userId);

        string ScrubLeadFromLeadViewer(string LeadIDList, int userId);

        bool AddToLeadHistory(int leadId, string OriginalLeadXmlString, string ModifiedLeadXmlString, int UserID);

       
        # endregion

        List<string> GetDTStoredProcedureNames();
        void LogBatchLead(int leadId, int batchid, int statusid);
        List<BatchLeadViewerEntity> GetLeadViewerSearchResultForBatch(LeadViewerSearchParam lvsearch);
        int CheckLeadViewerSearchResultsExceeds3KRows(LeadViewerSearchParam lvsearch);
        bool GetCountAgainstCapStatus(int realtimeDeliveryStatusId);
        int RemoveRePostData(int leadId);
        Dictionary<string, string> LoadGSDataForLead(int leadId);
        List<DeliveryLeadData> GetThirdPartyLeadsForProcessing(int numParallelProcesses, string machineName, bool b);
        bool DeleteNonActiveDeliveryDefinition(int DelDefID);
        List<DeliveryDefinition> GetCRDeliveryDefinitionsForLead(int crId, int leadId);
        void AddDefaultDataTransformationFields(int crId, int endPointId, int userId, int emailTemplateId);
        bool TempLog(int leadId, int deliveryEngineEventId, int deliveryEndpointId, string message, int userId, bool IsBeta);
        #region Code-PostRoman

        List<DeliveryPostHTTPHeader> GetHTTPHeaders(int DeliveryEndPointID);

        #endregion
    }
}
