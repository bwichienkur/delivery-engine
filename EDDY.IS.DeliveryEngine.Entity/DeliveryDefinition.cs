using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public enum LeadDeliveryType
    {
        Undefined = 0,
        InstantPost = 1,
        InstantEmail = 2,
        BatchEmail = 3,
        BatchFtp = 4
    }

    public enum PostMethod
    {
        HTTPPOST,
        HTTPGET
    }

    public enum PostContentType
    {
        Form,
        XML,
        JSON
    }

    public enum EmailBodyType
    { 
        Text,
        HTML
    }

    public enum LeadDelivery
    {
        Post = 1,
        Response = 2
    }

    [DataContract]
    public class DeliveryDefinition : CommonEntity
    {
        public DeliveryDefinition()
        {
            EndPoints = new List<DeliveryEndpoint>();
        }

        [DataMember]
        public int DeliveryDefinitionId { get; set; }

        [DataMember]
        public int CRId { get; set; }

        [DataMember]
        public string DeliveryName { get; set; }

        [DataMember]
        public List<DeliveryEndpoint> EndPoints { get; set; }

        [DataMember]
        public string ConditionXML {get; set;}

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public int Active { get; set;}

        [DataMember]
        public string UpdatedByName { get; set; }
    }

    public class DeliveryUtility
    {
        public static PostMethod GetPostMethodByName(string name)
        {
            switch (name.ToLower())
            {
                case "post":
                default:
                    {
                        return PostMethod.HTTPPOST;
                    }
                case "get":
                    {
                        return PostMethod.HTTPGET;
                    }
            }
        }

        public static PostContentType GetPostContentTypeByName(string name)
        {
            switch (name.ToLower())
            {
                case "form":
                default:
                    {
                        return PostContentType.Form;
                    }
                case "xml":
                    {
                        return PostContentType.XML;
                    }
                case "json":
                    {
                        return PostContentType.JSON;
                    }
            }
        }
    }

    [KnownType(typeof(InstantPostEndpoint))]
    [KnownType(typeof(InstantEmailEndpoint))]
    [KnownType(typeof(BatchEmailEndpoint))]
    [KnownType(typeof(BatchFtpEndpoint))]
    [DataContract]
    public abstract class DeliveryEndpoint //: CommonEntity
    {

        protected DeliveryEndpoint()
        {
            BlackoutPeriods = new List<DeliveryBlackoutPeriod>();
            DeliveryRejectionReasons = new List<DeliveryRejectionReasons>();
        }

        [DataMember]
        public int DeliveryEndpointId { get; set; }

        [DataMember]
        public int DeliveryDefinitionId { get; set; }

        [DataMember]
        public int CSRId { get; set; }

        [DataMember]
        public string EndpointDetailXML { get; set; }

        [DataMember]
        public string EndpointName { get; set; }

        [DataMember]
        public int DeliveryTypeId { get; set; }

        [DataMember]
        public int DataTransformationId { get; set; }

        [DataMember]
        public bool IsScrub { get; set; }
        
        [DataMember]
        public bool IsRealtime
        {
            get
            {
                //true if instant email or instant post
                return (this.DeliveryType == LeadDeliveryType.InstantEmail || this.DeliveryType == LeadDeliveryType.InstantPost);
            }
            set
            {//just putting a set statement because wcf needs it
                string s = "IsRealtime";
            }
        }

        [DataMember]
        public LeadDeliveryType DeliveryType
        {
            get
            {
                switch (this.DeliveryTypeId)
                {
                    case 1:
                        {
                            return LeadDeliveryType.InstantPost;
                        }
                    case 2:
                        {
                            return LeadDeliveryType.InstantEmail;
                        }
                    case 3:
                        {
                            return LeadDeliveryType.BatchEmail;
                        }
                    case 4:
                        {
                            return LeadDeliveryType.BatchFtp;
                        }
                    default:
                        {
                            return LeadDeliveryType.Undefined;
                        }
                }
            }
            set
            {//just putting a set statement because wcf needs it
                string s = "IsRealtime";
            }
        }

        [DataMember]
        public int MaxRetryAttempts { get; set; }

        [DataMember]
        public int RetryDelayInHours { get; set; }

        [DataMember]
        public DateTime EffectiveDateTime { get; set; }

        [DataMember]
        public bool IsTest { get; set; }

        [DataMember]
        public List<DeliveryBlackoutPeriod> BlackoutPeriods { get; set; }
        
        public bool IsTestModeOverRide(DeliveryLeadData leadData)
        {
            bool isTestMode = this.IsTest;

            //no check if endpoint is test
            if(isTestMode) return true;

            //check server name if endpoint is live
            isTestMode = !Regex.IsMatch(Environment.MachineName.Substring(0, 1), @"^\d+$");
            if (Environment.MachineName.Equals("ISTASK01", StringComparison.InvariantCultureIgnoreCase))
                isTestMode = false;

            if (isTestMode) return true;

            //check lead data first name
            if (leadData !=null)
            {
                 if (!string.IsNullOrWhiteSpace(leadData.FirstName) && leadData.FirstName.ToLower().Equals("test"))
                    isTestMode = true;
            }


            return isTestMode;

        }

        public bool IsTestModeOverRide()
        {

            return IsTestModeOverRide(new DeliveryLeadData());
        }

        [DataMember]
        public bool IsPrimary { get; set;}

        [DataMember]
        public List<DeliveryRejectionReasons> DeliveryRejectionReasons { get; set; }

        [DataMember]
        public Dictionary<string, string> EndPointDetailDictionary { get; set; }
    }


        [DataContract]
    public class InstantPostEndpoint : DeliveryEndpoint
    {
        [DataMember]
        public string LivePostUrl { get; set; }

        [DataMember]
        public string TestPostUrl { get; set; }

        [DataMember]
        public PostMethod PostMethod { get; set; }

        [DataMember]
        public PostContentType PostContentType { get; set; }

        [DataMember]
        public string SuccessResponseString { get; set; }

        [DataMember]
        public string FailureResponseString { get; set; }

        [DataMember]
        public string DuplicateResponseString { get; set; }

        [DataMember]
        public string ForceRepostResponseString { get; set; }
        
        [DataMember]
        public string SoapAction {get;set;}

       
    }

    [DataContract]
    public class InstantEmailEndpoint : DeliveryEndpoint
    {
        [DataMember]
        public string EmailSubject { get; set; }

        [DataMember]
        public string LiveEmailTo { get; set; }

        [DataMember]
        public string LiveEmailCC { get; set; }

        [DataMember]
        public string LiveEmailBCC { get; set; }

        [DataMember]
        public string TestEmailTo { get; set; }

        [DataMember]
        public string TestEmailCC { get; set; }

        [DataMember]
        public string TestEmailBCC { get; set; }

        [DataMember]
        public EmailBodyType BodyType { get; set; }

        [DataMember]
        public string FieldDelimiter { get; set; }

        [DataMember]
        public string LineDelimiter { get; set; }

        [DataMember]
        public string BodyHeader { get; set; }

        [DataMember]
        public string BodyFooter { get; set; }

        [DataMember]
        public bool IsEmailTemplate { get; set; }

        [DataMember]
        public string EmailTemplate { get; set; }
        
    }

    [DataContract]
    public class BatchEmailEndpoint : DeliveryEndpoint
    {
        [DataMember]
        public string EmailSubject { get; set; }

        [DataMember]
        public string LiveEmailTo { get; set; }

        [DataMember]
        public string LiveEmailCC { get; set; }

        [DataMember]
        public string LiveEmailBCC { get; set; }

        [DataMember]
        public string TestEmailTo { get; set; }

        [DataMember]
        public string TestEmailCC { get; set; }

        [DataMember]
        public string TestEmailBCC { get; set; }

        [DataMember]
        public EmailBodyType BodyType { get; set; }

        [DataMember]
        public string EmailBody { get; set; }

        [DataMember]
        public BatchFileDefinition BatchFileDefinition { get; set; }

        [DataMember]
        public string[] DaysToDeliver { get; set; }

        [DataMember]
        public string TimeToDeliver { get; set; }
    }

    [DataContract]
    public class BatchFtpEndpoint : DeliveryEndpoint
    {
        [DataMember]
        public string FileProtocol { get; set; }

        [DataMember]
        public bool SecureProtocol { get; set; }

        [DataMember]
        public string URI { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string RemoteFolder { get; set; }
        [DataMember]
        public string HostKey { get; set; }

        [DataMember]
        public bool ActiveMode { get; set; }

        [DataMember]
        public BatchFileDefinition BatchFileDefinition { get; set; }

        [DataMember]
        public  string[] DaysToDeliver { get; set; }

        [DataMember]
        public string TimeToDeliver { get; set; }

        [DataMember]
        public string TestURL { get; set; }        
    }


}
