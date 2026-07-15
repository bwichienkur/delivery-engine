using System.Collections.Generic;
using EDDY.Nexus.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess;
//using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DataTransformation;
//using EDDY.Nexus.Entity.DeliveryEngine;


namespace EDDY.IS.DeliveryEngine.Workflow.Activities.Helper
{
    public class DeliveryEngineBC
    {
        #region Delivery Definitions
        public List<DeliverySelectItem> GetDeliveryDataTypes()
        {
            return (new DeliveryEngineDAO()).GetDeliveryDataTypes();
        }
        public List<DeliverySelectItem> GetDeliveryConditionTypes(int DelDefID)
        {
            return (new DeliveryEngineDAO()).GetDeliveryConditionTypes(DelDefID);
        }
        public List<DeliverySelectItem> GetDeliveryExpressionTypes()
        {
            return (new DeliveryEngineDAO()).GetDeliveryExpressionTypes();
        }
        public List<DeliveryDefinition> GetCRDeliveryDefinitions(DeliveryDefSearchParam deliveryparam)
        {
            return (new DeliveryEngineDAO()).GetCRDeliveryDefinitions(deliveryparam);
        }
        public List<DeliveryDefinition> GetCRDeliveryDefinitions(int CrId)
        {
            return (new DeliveryEngineDAO()).GetCRDeliveryDefinitions(CrId);
        }
        public DeliveryDefinition GetDeliveryConditions(int CrID, int DelDefID)
        {
            return (new DeliveryEngineDAO()).GetDeliveryConditions(CrID, DelDefID);

        }
        public bool UpdateDeliveryDefinition(DeliveryDefinition deliveryDefinition)
        {
            return (new DeliveryEngineDAO()).UpdateDeliveryDefinition(deliveryDefinition);
        }
        public List<DeliveryEndPointItem> GetDeliveryEndPointList(int DelDefID, int DeliveryTypeID)
        {
            return (new DeliveryEngineDAO()).GetDeliveryEndPointList(DelDefID, DeliveryTypeID);
        }
        public bool CheckDuplicatePriority(int CrID, int Priority, int DelDefID)
        {
            return (new DeliveryEngineDAO()).CheckDuplicatePriority(CrID, Priority, DelDefID);
        }
        public List<ConditionDataTypeItem> GetDeliveryConditionWithType()
        {
            return (new DeliveryEngineDAO()).GetDeliveryConditionWithType();

        }

        public bool UpdateDeliveryCondition(int CrID, int DelDefID, string ConditionXML)
        {
            return (new DeliveryEngineDAO()).UpdateDeliveryCondition(CrID, DelDefID, ConditionXML);

        }

        public DeliveryPI GetDeliveryPostInstructions(int EndPointID)
        {
            return (new DeliveryEngineDAO()).GetDeliveryPostInstructions(EndPointID);
        }
        public int AddUpdateDeliveryPostInstructions(DeliveryPI delPI)
        {
            return (new DeliveryEngineDAO()).AddUpdateDeliveryPostInstructions(delPI);
        }
        public int AddUpdateDeliveryPostInstructions(DeliveryPI delPI, bool createDefaultTransformations)
        {
            return (new DeliveryEngineDAO()).AddUpdateDeliveryPostInstructions(delPI, createDefaultTransformations);
        }
        public void AddDefaultDataTransformationFields(int crId, int endPointId, int userId, int DefaultPaidTemplateProductId)
        {
            (new DeliveryEngineDAO()).AddDefaultDataTransformationFields(crId, endPointId, userId, DefaultPaidTemplateProductId);
        }
        public int InsertDeliveryDefinition(DeliveryDefinitionItem deldef)
        {
            return (new DeliveryEngineDAO()).InsertDeliveryDefinition(deldef);
        }
        public string GetDeliveryName(int deldefId)
        {
            return (new DeliveryEngineDAO()).GetDeliveryName(deldefId);
        }
        public List<DeliveryBlackoutItem> GetDeliveryBlackOutList(int EndPointID)
        {
            return (new DeliveryEngineDAO()).GetDeliveryBlackOutList(EndPointID);
        }
        public List<DeliverySelectItem> GetDeliveryFrequencies()
        {
            return (new DeliveryEngineDAO()).GetDeliveryFrequencies();
        }
        public DeliveryBlackoutItem GetDeliveryBlackOut(int BlackoutID)
        {
            return (new DeliveryEngineDAO()).GetDeliveryBlackOut(BlackoutID);

        }

        public List<DeliverySelectItem> GetBloackOutDateTimeOffSets()
        {
            return (new DeliveryEngineDAO()).GetBloackOutDateTimeOffSets();
        }

        public bool AddUpdateDeliveryBlackOut(DeliveryBlackoutItem blackout)
        {
            return (new DeliveryEngineDAO()).AddUpdateDeliveryBlackOut(blackout);
        }
        public int GetCrOffset(int CrID)
        {
            return (new DeliveryEngineDAO()).GetCrOffset(CrID);

        }
        public CheckActiveEndPoint CheckActiveEndPoint(int CRID, int DelDefID, int EndPointID)
        {
            return (new DeliveryEngineDAO()).CheckActiveEndPoint(CRID, DelDefID, EndPointID);

        }
        public int HasActiveEndpoints(int DelDefID)
        {
            return (new DeliveryEngineDAO()).HasActiveEndpoints(DelDefID);
        }
        public int IsDeactivatingDefinition(int DelDefID,int EndPointID)
        {
            return (new DeliveryEngineDAO()).IsDeactivatingDefinition(DelDefID,EndPointID);
        }
        public string DeliveryHasConditions(int DelDefID)
        {
            return (new DeliveryEngineDAO()).DeliveryHasConditions(DelDefID); 
        }
        
        public List<EmailTemplate> GetEmailTemplateNames()
        {
            return (new DeliveryEngineDAO()).GetEmailTemplateNames();
        }

        public List<LeadRejectionReasonItem> GetleadRejectionReasons()
        {
            return (new DeliveryEngineDAO()).GetleadRejectionReasons();
        }

        public int GetDefaultPaidTemplateProductId(int EmailTemplateID)
        {
            return (new DeliveryEngineDAO()).GetDefaultPaidTemplateProductId(EmailTemplateID);
        }

        #endregion


        #region Lead Viewer

        public List<LeadViewerResultEntity> GetLeadViewerSearchResult(LeadViewerSearchParam lvsearch)
        {
            return (new DeliveryEngineDAO()).GetLeadViewerSearchResult(lvsearch);
        }
              
        public List<RealtimeDeliveryStatus> GetRealtimeDeliveryStatusList(int DeliveryStatusTypeId = 0)
        {
            return (new DeliveryEngineDAO()).GetRealtimeDeliveryStatusList(DeliveryStatusTypeId);
        }
       
        public LeadEntity GetLeadDataByLeadId(int leadId)
        {
            return (new DeliveryEngineDAO()).GetLeadDataByLeadId(leadId);
        }

        public string UpdateLeadData(LeadEntity leadEntity, int userId)
        {            
            Dictionary<string, object> additionalData = new Dictionary<string, object>();
            foreach (var obj in leadEntity.AdditionalLeadDataList)
            {
                if (obj.ControlId != null)
                    additionalData.Add(obj.ControlId, obj.Value);
            }

            if (additionalData.Count > 0)
            {
                leadEntity.AdditionalFields = additionalData;
                leadEntity.AdditionalFieldXml = XmlUtilities.GetXMLFromDictionary(additionalData);
            }

            return (new DeliveryEngineDAO()).UpdateLeadData(leadEntity, userId);
        }

        public int SaveLeadPreviewData(LeadEntity leadEntity)
        {

            Dictionary<string, object> additionalData = new Dictionary<string, object>();

            foreach (var obj in leadEntity.AdditionalLeadDataList)
            {
                additionalData.Add(obj.ControlId, obj.Value);
            }

            if (additionalData.Count > 0)
            {
                leadEntity.AdditionalFields = additionalData;
                leadEntity.AdditionalFieldXml = XmlUtilities.GetXMLFromDictionary(additionalData);
            }

            return (new DeliveryEngineDAO()).SaveLeadPreviewData(leadEntity);
        }
        
        public List<AdditionalLeadData> GetAdditionalLeadDataByLeadId(int leadId)
        {
            return (new DeliveryEngineDAO()).GetAdditionalLeadDataByLeadId(leadId);
        }

        public LeadViewerEntity GetLeadDeliveryPostResponseByLeadId(int leadId, int deliveryId, bool IsBeta, int EndPointId, int DeliveryLogId)
        {
            return (new DeliveryEngineDAO()).GetLeadDeliveryPostResponseByLeadId(leadId, deliveryId,IsBeta, EndPointId, DeliveryLogId);
        }
        
        public bool CloneDeliveryDefinition(int DelDefID)
        {
            return (new DeliveryEngineDAO()).CloneDeliveryDefinition(DelDefID);

        }
        public bool DeleteNonActiveDeliveryDefinition(int DelDefID)
        {
            return (new DeliveryEngineDAO()).DeleteNonActiveDeliveryDefinition(DelDefID);

        }
        public List<LeadViewerHistoryItem> GetleadViewerHistory(int LeadID)
        {

            return (new DeliveryEngineDAO()).GetleadViewerHistory(LeadID);
        }
        
        public bool CheckCRhasDeliveryDefinitions(int CsrID)
        {
            return (new DeliveryEngineDAO()).CheckCRhasDeliveryDefinitions(CsrID);
        }
        
        public string UpdateSelectedLeadsforRepost(string LeadsIDs, int UserID, string isBetaLead)
        {
            return (new DeliveryEngineDAO()).UpdateSelectedLeadsforRepost(LeadsIDs, UserID, isBetaLead);
           
        }
        
        public List<BatchLeadViewerEntity> GetLeadViewerSearchResultForBatch(LeadViewerSearchParam lvsearch)
        {
            return (new DeliveryEngineDAO()).GetLeadViewerSearchResultForBatch(lvsearch);
        }
        
        public int CheckLeadViewerSearchResultsExceeds3KRows(LeadViewerSearchParam lvsearch)
        {
            return (new DeliveryEngineDAO()).CheckLeadViewerSearchResultsExceeds3KRows(lvsearch);
        }

        public string ScrubLeadFromLeadViewer(string LeadIDList, int userId)
        {

            return (new DeliveryEngineDAO()).ScrubLeadFromLeadViewer(LeadIDList, userId);
        }

        public string MultipleLeadEdit(string leadIdList, int newDeliveryStatusId, string newTrackId, string editNotes, int UserID, int militaryStatusId, string countryCode, string billingMonth)
        {
            return (new DeliveryEngineDAO()).MultipleLeadEdit(leadIdList, newDeliveryStatusId, newTrackId, editNotes, UserID, militaryStatusId, countryCode, billingMonth);
        }

        public bool AddToLeadHistory(int leadId, string OriginalLeadXmlString, string ModifiedLeadXmlString, int UserID)
        {
            return (new DeliveryEngineDAO()).AddToLeadHistory(leadId, OriginalLeadXmlString, ModifiedLeadXmlString, UserID);
        }

        public List<MilitaryStatus> GetMilitaryStatusList()
        {
            return (new DeliveryEngineDAO()).GetMilitaryStatusList();
        }

        public List<ProductEntityList> GetProductList(int leadId)
        {
            return (new DeliveryEngineDAO()).GetProductList(leadId);
        }

        public List<ProgramFilteredList> GetProgramFilteredList(int CRID, int ProductId, int CampusId )
        {
            return (new DeliveryEngineDAO()).GetProgramFilteredList(CRID, ProductId, CampusId);
        }

        public List<EducationLevel> GetEducationLevelSelectList()
        {
            return (new DeliveryEngineDAO()).GetEducationLevelSelectList();
        }

        public string GetTrackCampaignName(string TrackId)
        {
            return (new DeliveryEngineDAO()).GetTrackCampaignName(TrackId);
        }

        public List<RealTimeDeliveryStatusType> GetDeliveryStatusTypeList()
        {
            return (new DeliveryEngineDAO()).GetDeliveryStatusTypeList();
        }

        public List<ApplicationEntity> GetAllApplications()
        {
            return (new DeliveryEngineDAO()).GetAllApplications();        
        }

        public List<CountryEntity> GetCountryList()
        {
            return (new DeliveryEngineDAO()).GetCountryList();
        }

        #endregion

        
    }
}
