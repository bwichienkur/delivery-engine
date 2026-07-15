using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    [KnownType(typeof(LeadProcessingEntity))]
    [KnownType(typeof(DeliveryLeadData))] 
    [DataContract]
    public class LeadEntity : CommonEntity
    {

        #region constructors

        /// <summary>
        /// constructor for LeadEntity
        /// </summary>
        /// 
        public LeadEntity()
        {
            _additionalFields = new Dictionary<string, object>();
        }

        public LeadEntity(IDataReader dataReader)
        {
            if (dataReader != null)
            {
                SessionInternalId = DatabaseUtilities.SetInt32Value(dataReader[(int)DatabaseTableField.SessionInternalId]);
                VisitorInternalId = DatabaseUtilities.SetInt32Value(dataReader[(int)DatabaseTableField.VisitorInternalId]);
                FirstName = DatabaseUtilities.SetStringValue(dataReader[(int)DatabaseTableField.FirstName]);
                LastName = DatabaseUtilities.SetStringValue(dataReader[(int)DatabaseTableField.LastName]);
            }
        }

        public LeadEntity(IDataReader dataReader,bool IsLeadViewer)
        {
            if (dataReader == null || !IsLeadViewer) return;
            
            LeadId = DatabaseUtilities.SetInt32Value(dataReader["LeadId"]);
            ProductId = DatabaseUtilities.SetInt32Value(dataReader["ProductId"]);
            FirstName = DatabaseUtilities.SetStringValue(dataReader["FirstName"]);
            LastName = DatabaseUtilities.SetStringValue(dataReader["LastName"]);
            MiddleName = DatabaseUtilities.SetStringValue(dataReader["MiddleName"]);
            Address1 = DatabaseUtilities.SetStringValue(dataReader["Address1"]);
            Address2 = DatabaseUtilities.SetStringValue(dataReader["Address2"]);
            City = DatabaseUtilities.SetStringValue(dataReader["City"]);
            ZipCode = DatabaseUtilities.SetStringValue(dataReader["ZipCode"]);
            StateProvince = DatabaseUtilities.SetStringValue(dataReader["StateProvince"]);
            CountryCode = DatabaseUtilities.SetStringValue(dataReader["CountryCode"]);
            EmailAddress = DatabaseUtilities.SetStringValue(dataReader["EmailAddress"]);
            Phone1 = DatabaseUtilities.SetStringValue(dataReader["Phone1"]);
            Phone2 = DatabaseUtilities.SetStringValue(dataReader["Phone2"]);
            TimeToStartInWeeks = DatabaseUtilities.SetInt32Value(dataReader["TimeToStartInWeeks"]);
            RealTimeDeliveryStatusId = DatabaseUtilities.SetInt32Value(dataReader["RealTimeDeliveryStatusId"]);
            PSIId = DatabaseUtilities.SetInt32Value(dataReader["PsiId"]);
            ProgramId = DatabaseUtilities.SetInt32Value(dataReader["ProgramId"]);
            ApplicationId = DatabaseUtilities.SetInt32Value(dataReader["ApplicationId"]);
            ProgramCode = DatabaseUtilities.SetStringValue(dataReader["ProgramCode"]);
            PsiName = DatabaseUtilities.SetStringValue(dataReader["PsiName"]);
            ProgramName = DatabaseUtilities.SetStringValue(dataReader["ProgramName"]);
            CampusId = DatabaseUtilities.SetInt32Value(dataReader["CampusId"]);
            CampusValue = DatabaseUtilities.SetStringValue(dataReader["CampusValue"]);
            Prefix = DatabaseUtilities.SetStringValue(dataReader["Prefix"]);
            Age = DatabaseUtilities.SetInt32Value(dataReader["Age"]);
            YearHighestEduCompleted = DatabaseUtilities.SetInt32Value(dataReader["YearHighestEduCompleted"]);
            HighestLevelOfEdu = DatabaseUtilities.SetStringValue(dataReader["HighestLevelOfEdu"]);
            Military = DatabaseUtilities.SetStringValue(dataReader["Military"]);
            MilitaryStatusId = DatabaseUtilities.SetInt32Value(dataReader["MilitaryStatusId"]);
            StartDate = DatabaseUtilities.SetStringValue(dataReader["StartDate"]);
            TrackingCampaignName = DatabaseUtilities.SetStringValue(dataReader["TrackingCampaignName"]);
            TrackId = DatabaseUtilities.SetStringValue(dataReader["TrackId"].ToString());
            ProgramProductId = DatabaseUtilities.SetInt32Value(dataReader["ProgramProductId"]);
            CRId = DatabaseUtilities.SetInt32Value(dataReader["CRId"]);
            CampusName = DatabaseUtilities.SetStringValue(dataReader["CampusName"]);
            EduLevelId = DatabaseUtilities.SetInt32Value(dataReader["EduLevelId"]);
            LeadEditNoteId = DatabaseUtilities.SetInt32Value(dataReader["LeadEditNoteId"]);
            LeadEditNoteText = DatabaseUtilities.SetStringValue(dataReader["LeadEditNote"]);
            CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
            AllowRepost = DatabaseUtilities.SetBooleanValue(dataReader["AllowRepost"]);
            AllowRepostTimeHours = DatabaseUtilities.SetInt32Value(dataReader["AllowRepostTimeHours"]);
            BillingDate = Convert.ToDateTime(DatabaseUtilities.SetDateTimeValueWithNull(dataReader["BillingDate"], false));
            BetaLead = DatabaseUtilities.SetStringValue(dataReader["BetaLead"]);
            LeadCreationTypeId = DatabaseUtilities.SetInt32Value(dataReader["LeadCreationTypeId"]);
        }

        #endregion

        #region Data Members
                 
        [DataMember]
        public int LeadId { get; set; }
        [DataMember]
        public long RawPostDataId { get; set; }
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public string VisitorId { get; set; }
        [DataMember]
        public int SessionInternalId { get; set; }
        [DataMember]
        public int VisitorInternalId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public int FormUniqueId { get; set; }
        [DataMember]
        public int CRId { get; set; }
        [DataMember]
        public int PSIId { get; set; }
        [DataMember]
        public int ProgramId { get; set; }
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string StateProvince { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Phone1 { get; set; }
        [DataMember]
        public string Phone2 { get; set; }
        [DataMember]
        public int TimeToStartInWeeks { get; set; }
        [DataMember]
        public string AdditionalFieldXml { get; set; }
        [DataMember]
        public int RealTimeDeliveryStatusId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int ApplicationId { get; set; }
        [DataMember]
        public string ProgramCode { get; set; }
        [DataMember]
        public string PsiName { get; set; }
        [DataMember]
        public string ProgramName { get; set;}
        [DataMember]
        public bool IsBeta { get; set; } 
        [DataMember]
        public List<AdditionalLeadData> AdditionalLeadDataList { get; set; }
        [DataMember]
        public string Prefix { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public int YearHighestEduCompleted { get; set; }
        [DataMember]
        public string HighestLevelOfEdu { get; set; }
        [DataMember]
        public string Military { get; set; }
        [DataMember]
        public int MilitaryStatusId { get; set; }
        [DataMember]
        public string MethodOfContact { get; set; }
        [DataMember]
        public string LegacyLeadId { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string InitialLeadId { get; set; }
        [DataMember]
        public string TrackId {get; set;}
        [DataMember]
        public string CMID { get; set; }
        [DataMember]
        public string AffiliateId { get; set; }
        [DataMember]
        public string UID { get; set; }
        [DataMember]
        public string ExternalLeadId { get; set; }
        [DataMember]
        public int CampusId { get; set; }
        [DataMember]
        public int ProgramProductId { get; set; }
        [DataMember]
        public int CampusApplicationId { get; set; }
        [DataMember]
        public int CampusProgramId { get; set; }
        [DataMember]
        public int ClientCampusRelationshipId { get; set; }
        [DataMember]
        public int InstitutionApplicationId { get; set; }
        [DataMember]
        public int ProgramApplicationId { get; set; }
        [DataMember]
        public string Tsource { get; set; }
        [DataMember]
        public string TrackingSessionGUID { get; set; }
        [DataMember]
        public string CampusValue { get; set; } 
        [DataMember]
        public string TrackingCampaignName { get; set; }
        [DataMember]
        public string RealTimeDeliveryStatusName { get; set; }
        [DataMember]
        public string CampusName { get; set; }
        [DataMember]
        public int EduLevelId { get; set; }
        [DataMember]
        public int LeadEditNoteId { get; set; }
        [DataMember]
        public string LeadEditNoteText { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public bool AllowRepost { get; set; }
        [DataMember]
        public int AllowRepostTimeHours { get; set; }
        [DataMember]
        public DateTime BillingDate { get; set; }
        [DataMember]
        public string BetaLead { get; set; }
        [DataMember]
        public int LeadCreationTypeId { get; set; }


        //AdditionalFields from XML in data
        private Dictionary<string, object> _additionalFields;
        public Dictionary<string, object> AdditionalFields
        {
            get { return _additionalFields; }
            set { _additionalFields = value; } 
        }

        [DataMember]
        public string AdditionalFieldsDictionaryXml
        {
            get
            {
                var dcs = new DataContractSerializer(typeof(Dictionary<string, object>));
                var sbDictionary = new StringBuilder();
                using (var xw = XmlWriter.Create(sbDictionary))
                {
                    dcs.WriteObject(xw, _additionalFields);
                }
                return sbDictionary.ToString();
            }

            set
            {
                var dcs = new DataContractSerializer(typeof(Dictionary<string, object>));

                using (var reader = new XmlTextReader(new StringReader(value)))
                {
                    _additionalFields = (Dictionary<string, object>)dcs.ReadObject(reader);
                }
            }
        }


        //read-only gets a list of all properties + additional fields in key/value form
        Dictionary<string, object> _allFields;
        public Dictionary<string, object> AllFields     
        {
            get 
            {
                _allFields = new Dictionary<string, object>();

                ////Add Standard Fields
                if (LeadId != 0) { _allFields.Add("LeadId", LeadId); }
                if (SessionInternalId != 0) { _allFields.Add("SessionInternalId", SessionInternalId); }
                if (VisitorInternalId != 0) { _allFields.Add("VisitorInternalId", VisitorInternalId); }
                if (FirstName != null) { _allFields.Add("FirstName", FirstName); }
                if (LastName != null) { _allFields.Add("LastName", LastName); }
                if (MiddleName != null) { _allFields.Add("MiddleName", MiddleName); }
                if (FormUniqueId != 0) { _allFields.Add("FormUniqueId", FormUniqueId); }
                if (CRId != 0) { _allFields.Add("CRId", CRId); }
                if (PSIId != 0) { _allFields.Add("PSIId", PSIId); }
                if (ProgramId != 0) { _allFields.Add("ProgramId", ProgramId); }
                if (CategoryId != 0) { _allFields.Add("CategoryId", CategoryId); }
                if (Address1 != null) { _allFields.Add("Address1", Address1); }
                if (Address2 != null) { _allFields.Add("Address2", Address2); }
                if (City != null) { _allFields.Add("City", City); }
                if (ZipCode != null) { _allFields.Add("ZipCode", ZipCode); }
                if (StateProvince != null) { _allFields.Add("StateProvince", StateProvince); }
                if (CountryCode != null) { _allFields.Add("CountryCode", CountryCode); }
                if (EmailAddress != null) { _allFields.Add("EmailAddress", EmailAddress); }
                if (Phone1 != null) { _allFields.Add("Phone1", Phone1); }
                if (Phone2 != null) { _allFields.Add("Phone2", Phone2); }
                if (TimeToStartInWeeks != 0) { _allFields.Add("TimeToStartInWeeks", TimeToStartInWeeks); }
                if (Prefix != null) { _allFields.Add("Prefix", Prefix); }
                if (Age != 0) { _allFields.Add("Age", Age); }
                if (YearHighestEduCompleted != 0) { _allFields.Add("YearHighestEduCompleted", YearHighestEduCompleted); }
                if (HighestLevelOfEdu != null) { _allFields.Add("HighestLevelOfEdu", HighestLevelOfEdu); }
                if (Military != null) { _allFields.Add("Military", Military); }
                if (MilitaryStatusId != null) { _allFields.Add("MilitaryStatusId", MilitaryStatusId); }
                if (MethodOfContact != null) { _allFields.Add("MethodOfContact", MethodOfContact); }
                if (StartDate != null) { _allFields.Add("StartDate", StartDate); }
                if (LegacyLeadId != null) { _allFields.Add("LegacyLeadId", LegacyLeadId); }
                if (RealTimeDeliveryStatusId != 0) { _allFields.Add("RealTimeDeliveryStatusId", RealTimeDeliveryStatusId); }
                if (ApplicationId != 0) { _allFields.Add("ApplicationId", ApplicationId); }
                if (ProductId != 0) { _allFields.Add("ProductId", ProductId); }
                if (CampusId != 0) { _allFields.Add("CampusId", CampusId); }
                if (ProgramProductId != 0) { _allFields.Add("ProgramProductId", ProgramProductId); }
                if (Tsource != null) { _allFields.Add("Tsource", Tsource); }
                if (CampusValue != null) { _allFields.Add("CampusValue", CampusValue); }
                if (CampusName != null) { _allFields.Add("CampusName", CampusName); }
                if (TrackId != null) { _allFields.Add("TrackId", TrackId); }
                if (EduLevelId != 0) { _allFields.Add("EduLevelId", EduLevelId); }  
                if (LeadEditNoteId != 0) { _allFields.Add("LeadEditNoteId", LeadEditNoteId); }                
                if (LeadEditNoteText != null) { _allFields.Add("LeadEditNoteText", LeadEditNoteText); }
                if (CreatedDate != null) { _allFields.Add("CreatedDate", CreatedDate); }
                _allFields.Add("AllowRepost",AllowRepost);
                if (AllowRepostTimeHours != null) { _allFields.Add("AllowRepostTimeHours", AllowRepostTimeHours); }
                if (BillingDate != null) { _allFields.Add("BillingDate", BillingDate); }
                if (LeadCreationTypeId != 0) { _allFields.Add("LeadCreationTypeId", LeadCreationTypeId); }

                _allFields.Add("BetaLead", BetaLead); 
                _allFields.Add("IsBeta", IsBeta);

                //Add Additional Fields
                if (AdditionalFields != null)
                {
                    foreach (var item in
                        AdditionalFields.Where(item => !string.IsNullOrWhiteSpace(item.Key)).Where(item => !_allFields.ContainsKey(item.Key)))
                    {
                        _allFields.Add(item.Key, item.Value);
                    }
                }
                
                return _allFields;
            } 
        }

        #endregion

        #region enums

        public enum DatabaseTableField
        {
            SessionInternalId,
            VisitorInternalId,
            FirstName,
            LastName
        }

        public enum LeadDataField
        {
            LeadId,
            ProductId,
            FirstName,
            LastName,
            MiddleName,
            Address1,
            Address2,
            City,
            ZipCode,
            StateProvince,
            CountryCode,
            EmailAddress,
            Phone1,
            Phone2,
            TimeToStartInWeeks,
            RealTimeDeliveryStatusId,
            PsiId,
            ProgramId,
            ProgramInstanceId,
            ApplicationId,
            ProgramCode,
            PsiName,
            ProgramName,            
            Prefix,
            Age,
            YearhHighestEduCompleted,
            HighestLevelOfEdu,
            Military,
            MilitaryStatusId,
            MethodOfContact,
            StartDate,
            TrackId,
            TrackingCampaignName,
            CampusId,
            ProgramProductId,
            CRId,
            CampusName,
            EduLevelId,
            LeadEditNoteId,
            LeadEditNoteText,
            CreatedDate,
            AllowRepost,
            AllowRepostTimeHours,
            BillingDate,
            BetaLead,
            LeadCreationTypeId
        }

        #endregion
    }

    [KnownType(typeof(DeliveryLeadData))]
    [DataContract]
    public class LeadProcessingEntity : LeadEntity
    {
        //From CapLeadEntity
        [DataMember]
        public string TrackingId { get; set; }
        [DataMember]
        public int ApplicationId { get; set; }
        [DataMember]
        public int SiteId { get; set; }
        [DataMember]
        public int ConsumerLeadId { get; set; }
        [DataMember]
        public int ProgramTypeId { get; set; }
        [DataMember]
        public int SchoolId { get; set; }
        [DataMember]
        public int ProfileId { get; set; }
        [DataMember]
        public int BusinessCategoryId { get; set; }
        [DataMember]
        public int ChannelId { get; set; }
        [DataMember]
        public int VendorId { get; set; }
        [DataMember]
        public int CampaignId { get; set; }
        [DataMember]
        public int BusinessModelId { get; set; }
        [DataMember]
        public int ProgramLevelId { get; set; }
        [DataMember]
        public int SubjectId { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int LeadCreationTypeId { get; set; }

        [DataMember]
        public ICollection<UserExperience> UserExperienceList { get; set; }

        [DataMember]
        public ICollection<UserData> UserDataList { get; set; }

        Dictionary<string, object> _allFields;
        public Dictionary<string, object> AllFields
        {
            get
            {
                _allFields = new Dictionary<string, object>();

                ////Add Standard Fields
                if (LeadId != 0) { _allFields.Add("LeadId", LeadId); }
                if (SessionInternalId != 0) { _allFields.Add("SessionInternalId", SessionInternalId); }
                if (VisitorInternalId != 0) { _allFields.Add("VisitorInternalId", VisitorInternalId); }
                if (FirstName != null) { _allFields.Add("FirstName", FirstName); }
                if (LastName != null) { _allFields.Add("LastName", LastName); }
                if (MiddleName != null) { _allFields.Add("MiddleName", MiddleName); }
                if (FormUniqueId != 0) { _allFields.Add("FormUniqueId", FormUniqueId); }
                if (CRId != 0) { _allFields.Add("CRId", CRId); }
                if (PSIId != 0) { _allFields.Add("PSIId", PSIId); }
                if (ProgramId != 0) { _allFields.Add("ProgramId", ProgramId); }
                if (CategoryId != 0) { _allFields.Add("CategoryId", CategoryId); }
                if (Address1 != null) { _allFields.Add("Address1", Address1); }
                if (Address2 != null) { _allFields.Add("Address2", Address2); }
                if (City != null) { _allFields.Add("City", City); }
                if (ZipCode != null) { _allFields.Add("ZipCode", ZipCode); }
                if (StateProvince != null) { _allFields.Add("StateProvince", StateProvince); }
                if (CountryCode != null) { _allFields.Add("CountryCode", CountryCode); }
                if (EmailAddress != null) { _allFields.Add("EmailAddress", EmailAddress); }
                if (Phone1 != null) { _allFields.Add("Phone1", Phone1); }
                if (Phone2 != null) { _allFields.Add("Phone2", Phone2); }
                if (TimeToStartInWeeks != 0) { _allFields.Add("TimeToStartInWeeks", TimeToStartInWeeks); }
                if (Prefix != null) { _allFields.Add("Prefix", Prefix); }
                if (Age != 0) { _allFields.Add("Age", Age); }
                if (YearHighestEduCompleted != 0) { _allFields.Add("YearHighestEduCompleted", YearHighestEduCompleted); }
                if (HighestLevelOfEdu != null) { _allFields.Add("HighestLevelOfEdu", HighestLevelOfEdu); }
                if (Military != null) { _allFields.Add("Military", Military); }
                if (MilitaryStatusId != null) { _allFields.Add("MilitaryStatusId", MilitaryStatusId); }
                if (MethodOfContact != null) { _allFields.Add("MethodOfContact", MethodOfContact); }
                if (StartDate != null) { _allFields.Add("StartDate", StartDate); }
                if (LegacyLeadId != null) { _allFields.Add("LegacyLeadId", LegacyLeadId); }
                if (RealTimeDeliveryStatusId != 0) { _allFields.Add("RealTimeDeliveryStatusId", RealTimeDeliveryStatusId); }
                if (ApplicationId != 0) { _allFields.Add("ApplicationId", ApplicationId); }
                if (ProductId != 0) { _allFields.Add("ProductId", ProductId); }
                if (CampusId != 0) { _allFields.Add("CampusId", CampusId); }
                if (UID != null) { _allFields.Add("UID", UID); }
                if (ProgramProductId != 0) { _allFields.Add("ProgramProductId", ProgramProductId); }
                if (CampusValue != null) { _allFields.Add("CampusValue", CampusValue); }
                if (CampusName != null) { _allFields.Add("CampusName", CampusName); }
                if (TrackId != null) {_allFields.Add("TrackId",TrackId);}
                if (TrackingCampaignName != null) {_allFields.Add("TrackingCampaignName",TrackingCampaignName);}
                if (EduLevelId != 0) { _allFields.Add("EduLevelId", EduLevelId); }
                if (LeadEditNoteId != 0) { _allFields.Add("LeadEditNoteId", LeadEditNoteId); }
                if (LeadEditNoteText != null) { _allFields.Add("LeadEditNoteText", LeadEditNoteText); }
                if (CreatedDate != null) { _allFields.Add("CreatedDate", CreatedDate); }
                _allFields.Add("AllowRepost", AllowRepost);
                if (AllowRepostTimeHours != null) { _allFields.Add("AllowRepostTimeHours", CreatedDate); }
                if (BillingDate != null) { _allFields.Add("BillingDate", BillingDate); }
                if (LeadCreationTypeId != 0) { _allFields.Add("LeadCreationTypeId", LeadCreationTypeId); }


                _allFields.Add("BetaLead", BetaLead); 
                _allFields.Add("IsBeta", IsBeta);
                
                //Add Additional Fields
                if (AdditionalFields != null)
                {
                    foreach (var item in
                        AdditionalFields.Where(item => !string.IsNullOrWhiteSpace(item.Key)).Where(item => !_allFields.ContainsKey(item.Key)))
                    {
                        _allFields.Add(item.Key, item.Value);
                    }
                }

                return _allFields;
            }
        }

      
    }

    [DataContract]
    public class DeliveryLeadData : LeadProcessingEntity
    {

        public Dictionary<string, object> TransformedNameValuePairs { get; set; }

        [DataMember]
        public string TransformedNameValuePairsDictionaryXML
        {
            get
            {
                var dcs = new DataContractSerializer(typeof(Dictionary<string, object>));
                var sbDictionary = new StringBuilder();
                using (var xw = XmlWriter.Create(sbDictionary))
                {
                    dcs.WriteObject(xw, TransformedNameValuePairs);
                }
                return sbDictionary.ToString();
            }

            set
            {
                var dcs = new DataContractSerializer(typeof(Dictionary<string, object>));

                using (var reader = new XmlTextReader(new StringReader(value)))
                {
                    TransformedNameValuePairs = (Dictionary<string, object>)dcs.ReadObject(reader);
                }
            }
        }


        Dictionary<string, object> _allFields;
        public new Dictionary<string, object> AllFields
        {

            get
            {
                _allFields = new Dictionary<string, object>();

                ////Add Standard Fields
                if (LeadId != 0) { _allFields.Add("LeadId", LeadId); }
                if (SessionInternalId != 0) { _allFields.Add("SessionInternalId", SessionInternalId); }
                if (VisitorInternalId != 0) { _allFields.Add("VisitorInternalId", VisitorInternalId); }
                if (FirstName != null) { _allFields.Add("FirstName", FirstName); }
                if (LastName != null) { _allFields.Add("LastName", LastName); }
                if (MiddleName != null) { _allFields.Add("MiddleName", MiddleName); }
                if (FormUniqueId != 0) { _allFields.Add("FormUniqueId", FormUniqueId); }
                if (CRId != 0) { _allFields.Add("CRId", CRId); }
                if (PSIId != 0) { _allFields.Add("PSIId", PSIId); }
                if (ProgramId != 0) { _allFields.Add("ProgramId", ProgramId); }
                if (CategoryId != 0) { _allFields.Add("CategoryId", CategoryId); }
                if (Address1 != null) { _allFields.Add("Address1", Address1); }
                if (Address2 != null) { _allFields.Add("Address2", Address2); }
                if (City != null) { _allFields.Add("City", City); }
                if (ZipCode != null) { _allFields.Add("ZipCode", ZipCode); }
                if (StateProvince != null) { _allFields.Add("StateProvince", StateProvince); }
                if (CountryCode != null) { _allFields.Add("CountryCode", CountryCode); }
                if (EmailAddress != null) { _allFields.Add("EmailAddress", EmailAddress); }
                if (Phone1 != null) { _allFields.Add("Phone1", Phone1); }
                if (Phone2 != null) { _allFields.Add("Phone2", Phone2); }
                if (TimeToStartInWeeks != 0) { _allFields.Add("TimeToStartInWeeks", TimeToStartInWeeks); }
                if (Prefix != null) { _allFields.Add("Prefix", Prefix); }
                if (Age != 0) { _allFields.Add("Age", Age); }
                if (YearHighestEduCompleted != 0) { _allFields.Add("YearHighestEduCompleted", YearHighestEduCompleted); }
                if (HighestLevelOfEdu != null) { _allFields.Add("HighestLevelOfEdu", HighestLevelOfEdu); }
                if (Military != null) { _allFields.Add("Military", Military); }
                if (MilitaryStatusId != null) { _allFields.Add("MilitaryStatusId", MilitaryStatusId); }
                if (MethodOfContact != null) { _allFields.Add("MethodOfContact", MethodOfContact); }
                if (StartDate != null) { _allFields.Add("StartDate", StartDate); }
                if (LegacyLeadId != null) { _allFields.Add("LegacyLeadId", LegacyLeadId); }
                if (RealTimeDeliveryStatusId != 0) { _allFields.Add("RealTimeDeliveryStatusId", RealTimeDeliveryStatusId); }
                if (ApplicationId != 0) { _allFields.Add("ApplicationId", ApplicationId); }
                if (ProductId != 0) { _allFields.Add("ProductId", ProductId); }
                if (CampusId != 0) { _allFields.Add("CampusId", CampusId); }
                if (UID != null) { _allFields.Add("UID", UID); }
                if (ProgramProductId != 0) { _allFields.Add("ProgramProductId", ProgramProductId); }
                if (Tsource != null) { _allFields.Add("Tsource", Tsource); }
                if (CampusValue != null) { _allFields.Add("CampusValue", CampusValue); }
                if (TrackId != null) {_allFields.Add("TrackId",TrackId);}
                if (TrackingCampaignName != null) {_allFields.Add("TrackingCampaignName",TrackingCampaignName);}
                if (CampusName != null) { _allFields.Add("CampusName", CampusName); }
                if (EduLevelId != 0) { _allFields.Add("EduLevelId", EduLevelId); }
                if (LeadEditNoteId != 0) { _allFields.Add("LeadEditNoteId", LeadEditNoteId); }
                if (LeadEditNoteText != null) { _allFields.Add("LeadEditNoteText", LeadEditNoteText); }
                if (CreatedDate != null) { _allFields.Add("CreatedDate", CreatedDate); }
                _allFields.Add("AllowRepost",AllowRepost);
                if (AllowRepostTimeHours != null) { _allFields.Add("AllowRepostTimeHours", AllowRepostTimeHours); }
                if (BillingDate != null) { _allFields.Add("BillingDate", BillingDate); }
                if (LeadCreationTypeId != 0) { _allFields.Add("LeadCreationTypeId", LeadCreationTypeId); }
                if (ChannelId != 0) { _allFields.Add("ChannelId", ChannelId); }

                _allFields.Add("IsRepost", IsRepost);

                _allFields.Add("BetaLead", BetaLead); 
                _allFields.Add("IsBeta", IsBeta);

                //Add Additional Fields
                if (AdditionalFields != null)
                {
                    foreach (var item in
                        AdditionalFields.Where(item => !string.IsNullOrWhiteSpace(item.Key)).Where(item => !_allFields.ContainsKey(item.Key)))
                    {
                        _allFields.Add(item.Key, item.Value);
                    }
                }

                return _allFields;
            }
        }

        [DataMember]
        public bool IsRepost { get; set; }
    }

    [DataContract]
    public class AdditionalLeadData 
    {

       //From CapLeadEntity
        [DataMember]
        public string ControlId { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string Value { get; set; }

        public AdditionalLeadData()
        {
            
        }

        public AdditionalLeadData(IDataReader dataReader)
        {
            if (dataReader == null) return;

            ControlId = DatabaseUtilities.SetStringValue(dataReader["ControlId"]);
            Label = DatabaseUtilities.SetStringValue(dataReader["Label"]);
            Value = DatabaseUtilities.SetStringValue(dataReader["Value"]);
        }
        
    }

    

}
