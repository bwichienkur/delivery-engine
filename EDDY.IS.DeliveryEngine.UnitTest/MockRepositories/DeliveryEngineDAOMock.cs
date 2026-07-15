using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.Nexus.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Common;
using EDDY.IS.DeliveryEngine.DataAccess;

namespace EDDY.IS.DeliveryEngine.UnitTest.MockRepositories
{
    public class DeliveryEngineDAOMock : DeliveryEngineDAO, IDeliveryEngineDAO
    {
        public bool AddToLeadHistoryResults;
        public bool AddUpdateDeliveryBlackOutResults;
        public int AddUpdateDeliveryPostInstructionsResults;
        public CheckActiveEndPoint CheckActiveEndPointResults;
        public bool CheckDuplicatePriorityResults;
        public int CheckLeadViewerSearchResultsExceeds3KRowsResults;
        public bool CloneDeliveryDefinitionResults;
        public int CreateLeadDeliveryRecordsResults;
        public int CreateNewBatchDeliveryResults;
        public RealtimeDeliveryQueueItem CreateRDQItemResults;
        public bool DeleteNonActiveDeliveryDefinitionResults;
        public bool DeleteRealtimeDeliveryQueueRecordResults;
        public string DeliveryHasConditionsResults;
        public bool FinalizeLeadDeliveryResults;
        public bool FinalizeLeadDeliveryOnErrorResults;
        public List<DeliverySelectItem> GetBloackOutDateTimeOffSetsResults;
        public bool GetCountAgainstCapStatusResults;
        public List<DeliveryDefinition> GetCRDeliveryDefinitionsResults;        
        public List<DeliveryDefinition> GetCRDeliveryDefinitionsForLeadResults;
        public int GetCrOffsetResults;
        public List<DeliveryEndpoint> GetDefinitionEndpointsResults;
        public DeliveryBlackoutItem GetDeliveryBlackOutResults;
        public List<DeliveryBlackoutItem> GetDeliveryBlackOutListResults;
        public DeliveryDefinition GetDeliveryConditionsResults;
        public List<DeliverySelectItem> GetDeliveryConditionTypesResults;
        public List<ConditionDataTypeItem> GetDeliveryConditionWithTypeResults;
        public List<DeliverySelectItem> GetDeliveryDataTypesResults;
        public DeliveryDefinition GetDeliveryDefinitionResults;
        public List<DeliveryEndPointItem> GetDeliveryEndPointListResults;
        public List<DeliverySelectItem> GetDeliveryExpressionTypesResults;
        public List<DeliverySelectItem> GetDeliveryFrequenciesResults;
        public string GetDeliveryNameResults;
        public DeliveryPI GetDeliveryPostInstructionsResults;
        public List<string> GetDTStoredProcedureNamesResults;
        public List<EmailTemplate> GetEmailTemplateNamesResults;
        public DeliveryEndpoint GetEndpointByIdResults;
        public List<DeliveryPostHTTPHeader> GetHTTPHeadersResults;
        public List<DeliveryLeadData> GetLeadForPreviewResults;
        public List<DeliveryLeadData> GetLeadsForBatchResults;
        public List<LeadViewerHistoryItem> GetleadViewerHistoryResults;
        public List<LeadViewerResultEntity> GetLeadViewerSearchResultResults;
        public List<BatchLeadViewerEntity> GetLeadViewerSearchResultForBatchResults;
        public List<DeliveryLeadData> GetNewLeadsForProcessingResults;
        public List<RealtimeDeliveryQueueItem> GetRDQsForProcessingRepostsResults;
        public List<DeliveryLeadData> GetThirdPartyLeadsForProcessingResults;
        public int HasActiveEndpointsResults;
        public int InsertDeliveryDefinitionResults;
        public int IsDeactivatingDefinitionResults;
        public Dictionary<string, string> LoadGSDataForLeadResults;
        public bool LogBatchDeliveryCompleteResults;
        public bool LogForLeadViewerHistoryResults;
        public bool LogPreviewDeliveryCompleteResults;
        public bool LogRealtimeDeliveryCompleteResults;
        public int RemoveRePostDataResults;
        public bool ResetRealtimeDeliveryQueueForMachineResults;
        public string ScrubLeadFromLeadViewerResults;
        public bool UpdateBatchStatusIfPrimaryDeliveryResults;
        public bool UpdateDeliveryConditionResults;
        public bool UpdateDeliveryDefinitionResults;
        public bool UpdateLeadDeliveryDefinitionResults;
        public bool UpdateLeadStatusIfLastRealtimeDeliveryResults;
        public bool UpdateRealtimeDeliveryQueueResults;
        public string UpdateSelectedLeadsforRepostResults;

        public IList<string> TempLogMessages = new List<string>();

        public void AddDefaultDataTransformationFields(int crId, int endPointId, int userId, int emailTemplateId) {}

        public bool AddToLeadHistory(int leadId, string OriginalLeadXmlString, string ModifiedLeadXmlString, int UserID)
        {
            throw new NotImplementedException();
        }

        public bool AddUpdateDeliveryBlackOut(DeliveryBlackoutItem blackout)
        {
            throw new NotImplementedException();
        }

        public int AddUpdateDeliveryPostInstructions(DeliveryPI delPI)
        {
            throw new NotImplementedException();
        }

        public CheckActiveEndPoint CheckActiveEndPoint(int CSRID, int DelDefID, int EndPointID)
        {
            throw new NotImplementedException();
        }

        public bool CheckDuplicatePriority(int CsrID, int Priority, int DelDefID)
        {
            throw new NotImplementedException();
        }

        public int CheckLeadViewerSearchResultsExceeds3KRows(LeadViewerSearchParam lvsearch)
        {
            throw new NotImplementedException();
        }

        public bool CloneDeliveryDefinition(int DelDefID)
        {
            throw new NotImplementedException();
        }

        public int CreateLeadDeliveryRecords(int leadId, int deliveryDefinitionId, int userId, bool IsBeta)
        {
            throw new NotImplementedException();
        }

        public int CreateNewBatchDelivery(int deliveryEndpointId, int userId, int productId, string machineName)
        {
            throw new NotImplementedException();
        }

        public RealtimeDeliveryQueueItem CreateRDQItem(int leadId, int deliveryEndpointId, Guid machinekey, int userId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteNonActiveDeliveryDefinition(int DelDefID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRealtimeDeliveryQueueRecord(int realtimeDeliveryQueueId)
        {
            throw new NotImplementedException();
        }

        public string DeliveryHasConditions(int DelDefID)
        {
            throw new NotImplementedException();
        }

        public bool FinalizeLeadDelivery(int leadId, int leadRealtimeDeliveryStatus, int userId)
        {
            throw new NotImplementedException();
        }

        public bool FinalizeLeadDeliveryOnError(int leadId, int userId)
        {
            throw new NotImplementedException();
        }

        public List<DeliverySelectItem> GetBloackOutDateTimeOffSets()
        {
            throw new NotImplementedException();
        }

        public bool GetCountAgainstCapStatus(int realtimeDeliveryStatusId)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryDefinition> GetCRDeliveryDefinitions(int CsrId)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryDefinition> GetCRDeliveryDefinitions(DeliveryDefSearchParam deliveryparam)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryDefinition> GetCRDeliveryDefinitionsForLead(int crId, int leadId)
        {
            return GetCRDeliveryDefinitionsForLeadResults;
        }

        public int GetCrOffset(int CsrID)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryEndpoint> GetDefinitionEndpoints(int deliveryDefinitionId)
        {
            return GetDefinitionEndpointsResults;
        }

        public List<DeliveryEndpoint> GetDefinitionEndpoints(DeliveryDefinition deliveryDefinition)
        {
            return GetDefinitionEndpointsResults;
        }

        public DeliveryBlackoutItem GetDeliveryBlackOut(int BlackoutID)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryBlackoutItem> GetDeliveryBlackOutList(int EndPointID)
        {
            throw new NotImplementedException();
        }

        public DeliveryDefinition GetDeliveryConditions(int CsrID, int DelDefID)
        {
            throw new NotImplementedException();
        }

        public List<DeliverySelectItem> GetDeliveryConditionTypes(int DelDefID)
        {
            throw new NotImplementedException();
        }

        public List<ConditionDataTypeItem> GetDeliveryConditionWithType()
        {
            throw new NotImplementedException();
        }

        public List<DeliverySelectItem> GetDeliveryDataTypes()
        {
            throw new NotImplementedException();
        }

        public DeliveryDefinition GetDeliveryDefinition(int deliveryDefinitionId)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryEndPointItem> GetDeliveryEndPointList(int DelDefID, int DeliveryTypeID)
        {
            throw new NotImplementedException();
        }

        public List<DeliverySelectItem> GetDeliveryExpressionTypes()
        {
            throw new NotImplementedException();
        }

        public List<DeliverySelectItem> GetDeliveryFrequencies()
        {
            throw new NotImplementedException();
        }

        public string GetDeliveryName(int deldefId)
        {
            throw new NotImplementedException();
        }

        public DeliveryPI GetDeliveryPostInstructions(int EndPointID)
        {
            throw new NotImplementedException();
        }

        public List<string> GetDTStoredProcedureNames()
        {
            throw new NotImplementedException();
        }

        public List<EmailTemplate> GetEmailTemplateNames()
        {
            throw new NotImplementedException();
        }

        public DeliveryEndpoint GetEndpointById(int deliveryEndpointId)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryPostHTTPHeader> GetHTTPHeaders(int DeliveryEndPointID)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryLeadData> GetLeadForPreview(int LeadID)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryLeadData> GetLeadsForBatch(int batchDeilveryId)
        {
            throw new NotImplementedException();
        }

        public List<LeadViewerHistoryItem> GetleadViewerHistory(int LeadID)
        {
            throw new NotImplementedException();
        }

        public List<LeadViewerResultEntity> GetLeadViewerSearchResult(LeadViewerSearchParam lvsearch)
        {
            throw new NotImplementedException();
        }

        public List<BatchLeadViewerEntity> GetLeadViewerSearchResultForBatch(LeadViewerSearchParam lvsearch)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryLeadData> GetNewLeadsForProcessing(int returnRecordsCount, string machineName, bool updateStatus, bool IsBeta)
        {
            throw new NotImplementedException();
        }

        public List<RealtimeDeliveryQueueItem> GetRDQsForProcessingReposts(int returnRecordsCount, string machineKey, bool updateStatus)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryLeadData> GetThirdPartyLeadsForProcessing(int numParallelProcesses, string machineName, bool b)
        {
            throw new NotImplementedException();
        }

        public int HasActiveEndpoints(int DelDefID)
        {
            throw new NotImplementedException();
        }

        public int InsertDeliveryDefinition(DeliveryDefinitionItem deldef)
        {
            throw new NotImplementedException();
        }

        public int IsDeactivatingDefinition(int DelDefID, int EndPointID)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> LoadGSDataForLead(int leadId)
        {
            throw new NotImplementedException();
        }

        public bool LogBatchDeliveryComplete(int batchDeilveryId, DeliveryEndpoint deliveryEndpoint, string batchFileName, int leadCount, int userId)
        {
            throw new NotImplementedException();
        }

        public void LogBatchLead(int leadId, int batchid, int statusid)
        {
            throw new NotImplementedException();
        }

        public bool LogForLeadViewerHistory(int leadId, string originalStatusIDxml, string modifiedStatusIDxml)
        {
            throw new NotImplementedException();
        }

        public bool LogPreviewDeliveryComplete(int leadid, int deliveryEndpointId, string leadData, string endpointDetailXML, bool deliverySuccess, DateTime deliveryDateTime, int attemptNumber, string serverResponse, string postResponse, Guid machineKey, int userId)
        {
            throw new NotImplementedException();
        }

        public bool LogRealtimeDeliveryComplete(int leadid, int deliveryEndpointId, string leadData, string endpointDetailXML, bool deliverySuccess, DateTime deliveryDateTime, int attemptNumber, string serverResponse, string postResponse, Guid machineKey, int userId, bool IsBeta, string Detination, int rejectionReasonId, int reviewedStatusId, int schoolValidationResponseStatusID)
        {
            throw new NotImplementedException();
        }

        public int RemoveRePostData(int leadId)
        {
            throw new NotImplementedException();
        }

        public bool ResetRealtimeDeliveryQueueForMachine(string machineKey)
        {
            throw new NotImplementedException();
        }

        public string ScrubLeadFromLeadViewer(string LeadIDList, int userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBatchStatusIfPrimaryDelivery(int leadId, int deliveryEndpointId, bool isSuccess)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDeliveryCondition(int CsrID, int DelDefID, string ConditionXML)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDeliveryDefinition(DeliveryDefinition deliveryDefinition)
        {
            throw new NotImplementedException();
        }

        public bool UpdateLeadDeliveryDefinition(int leadId, int deliveryDefinitionId, int userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateLeadStatusIfLastRealtimeDelivery(int leadId, int status, int userId, bool isTest, bool isPrimary, bool IsBeta)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRealtimeDeliveryQueue(int realtimeDeliveryQueueId, int deliveryStatusId, Guid machineKey, int currentDeliveryAttempts, DateTime lastDeliveryAttemptDatetime, DateTime nextDeliveryAttemptDatetime, int userId)
        {
            throw new NotImplementedException();
        }

        new public string UpdateSelectedLeadsforRepost(string LeadsIDs, int UserID, string isBetaLead)
        {
            throw new NotImplementedException();
        }

        public bool TempLog(int leadId, int deliveryEngineEventId, int deliveryEndpointId, string message, int userId, bool IsBeta)
        {
            TempLogMessages.Add(message);         

            return true;
        }
    }
}
