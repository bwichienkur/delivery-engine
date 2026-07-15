using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.Entity.Common;
using System;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class LeadViewerResultEntity:CommonEntity
    {
        [DataMember]
        public int LeadId { get; set; }
        [DataMember]
        public int ClientRelationshipID { get; set; }
        [DataMember]
        public int DeliveryTypeId { get; set; }
        [DataMember]
        public int RealTimeDeliveryStatusId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int DeliveryEndpointId { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public int DeliveryDefinitionId { get; set; }
        [DataMember]
        public string RealTimeDeliveryStatusName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string DeliveryTypeDesc { get; set; }
        [DataMember]
        public string TrackId { get; set; }
        [DataMember]
        public string TrackingCampaignName { get; set; }
        [DataMember]
        public int VendorId { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public int SubchannelId { get; set; }
        [DataMember]
        public string SubchannelName { get; set; }
        [DataMember]
        public string SchoolName { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public bool AllowRepost { get; set; }
        [DataMember]
        public int AllowRepostTimeHours { get; set; }
        [DataMember]
        public string LastestPostResponse { get; set; }
        [DataMember]
        public string BetaLead { get; set; }    
        
        public List<LVRealtimeLogEntity> realtimelogs { get; set; }


        public LeadViewerResultEntity()
        {
           

        }


        public LeadViewerResultEntity(IDataReader dataReader)
            : this()
        {

            if (dataReader != null)
            {
                LeadId = DatabaseUtilities.SetInt32Value(dataReader["LeadId"]);
                ClientRelationshipID = DatabaseUtilities.SetInt32Value(dataReader["ClientRelationshipId"]);
                DeliveryTypeId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryTypeId"]);
                DeliveryDefinitionId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryDefinitionId"]);
                //DeliveryEndpointId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointId"]);
                RealTimeDeliveryStatusId = DatabaseUtilities.SetInt32Value(dataReader["RealtimeDeliveryStatusId"]);
                RealTimeDeliveryStatusName = DatabaseUtilities.SetStringValue(dataReader["RealTimeDeliveryStatusName"]);                
                ApplicationID = DatabaseUtilities.SetInt32Value(dataReader["ApplicationID"]);
                FirstName = DatabaseUtilities.SetStringValue(dataReader["FirstName"]);
                LastName = DatabaseUtilities.SetStringValue(dataReader["LastName"]);
                EmailAddress = DatabaseUtilities.SetStringValue(dataReader["EmailAddress"]);
                TotalCount = DatabaseUtilities.SetInt32Value(dataReader["TotalCount"]);
                ProductName = DatabaseUtilities.SetStringValue(dataReader["ProductName"]);
                DeliveryTypeDesc = DatabaseUtilities.SetStringValue(dataReader["DeliveryTypeDesc"]);
                TrackId = DatabaseUtilities.SetStringValue(dataReader["TrackID"]);
                TrackingCampaignName = DatabaseUtilities.SetStringValue(dataReader["TrackingCampaignName"]);
                VendorId = DatabaseUtilities.SetInt32Value(dataReader["VendorID"]);
                VendorName = DatabaseUtilities.SetStringValue(dataReader["VendorName"]);
                SubchannelId = DatabaseUtilities.SetInt32Value(dataReader["SubchannelID"]);
                SubchannelName = DatabaseUtilities.SetStringValue(dataReader["SubchannelName"]);
                SchoolName = DatabaseUtilities.SetStringValue(dataReader["SchoolName"]);                
                CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["CreatedDate"], true);
                AllowRepost = DatabaseUtilities.SetBooleanValue(dataReader["AllowRepost"]);
                AllowRepostTimeHours = DatabaseUtilities.SetInt32Value(dataReader["AllowRepostTimeHours"]);
                LastestPostResponse = DatabaseUtilities.SetStringValue(dataReader["LastestPostResponse"]);
                BetaLead = DatabaseUtilities.SetStringValue(dataReader["BetaLead"]);
            }

        }



        public LeadViewerResultEntity(DataRow row)
            : this()
        {

            if (row != null)
            {
                LeadId = DatabaseUtilities.SetInt32Value(row["LeadId"]);
                ClientRelationshipID = DatabaseUtilities.SetInt32Value(row["ClientRelationshipId"]);
                DeliveryTypeId = DatabaseUtilities.SetInt32Value(row["DeliveryTypeId"]);
                DeliveryDefinitionId = DatabaseUtilities.SetInt32Value(row["DeliveryDefinitionId"]);
                //DeliveryEndpointId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointId"]);
                RealTimeDeliveryStatusId = DatabaseUtilities.SetInt32Value(row["RealtimeDeliveryStatusId"]);
                RealTimeDeliveryStatusName = DatabaseUtilities.SetStringValue(row["RealTimeDeliveryStatusName"]);
                ApplicationID = DatabaseUtilities.SetInt32Value(row["ApplicationID"]);
                FirstName = DatabaseUtilities.SetStringValue(row["FirstName"]);
                LastName = DatabaseUtilities.SetStringValue(row["LastName"]);
                EmailAddress = DatabaseUtilities.SetStringValue(row["EmailAddress"]);
                TotalCount = DatabaseUtilities.SetInt32Value(row["TotalCount"]);
                ProductName = DatabaseUtilities.SetStringValue(row["ProductName"]);
                DeliveryTypeDesc = DatabaseUtilities.SetStringValue(row["DeliveryTypeDesc"]);
                TrackId = DatabaseUtilities.SetStringValue(row["TrackID"]);
                TrackingCampaignName = DatabaseUtilities.SetStringValue(row["TrackingCampaignName"]);
                VendorId = DatabaseUtilities.SetInt32Value(row["VendorID"]);
                VendorName = DatabaseUtilities.SetStringValue(row["VendorName"]);
                SubchannelId = DatabaseUtilities.SetInt32Value(row["SubchannelID"]);
                SubchannelName = DatabaseUtilities.SetStringValue(row["SubchannelName"]);
                SchoolName = DatabaseUtilities.SetStringValue(row["SchoolName"]);
                CreatedDate = DatabaseUtilities.SetDateTimeValue(row["CreatedDate"], true);
                AllowRepost = DatabaseUtilities.SetBooleanValue(row["AllowRepost"]);
                AllowRepostTimeHours = DatabaseUtilities.SetInt32Value(row["AllowRepostTimeHours"]);
                LastestPostResponse = DatabaseUtilities.SetStringValue(row["LastestPostResponse"]);
                BetaLead = DatabaseUtilities.SetStringValue(row["BetaLead"]);
            }

        }

    }
}
