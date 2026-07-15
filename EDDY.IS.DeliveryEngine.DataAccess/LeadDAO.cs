using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Xml;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.Nexus.Common.Logging;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.DataAccess.Common;
//using EDDY.Nexus.DataAccess.Interface.CoreBusiness;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.DataAccess.Common;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;

namespace EDDY.IS.DeliveryEngine.DataAccess
{
	public class LeadDAO : BaseDataSource, ILeadDAO
	{        
        public DeliveryLeadData GetDeliveryLeadDataByLeadId(int leadId)
        {
            DeliveryLeadData returnLeadEntity = new DeliveryLeadData();

            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.Eddy_GetLeadById", Db);
            Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);

            try
            {
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        returnLeadEntity.LeadId = leadId;
                        returnLeadEntity.SessionInternalId = DatabaseUtilities.GetDataReaderInt(dataReader, "SessionInternalId", 0);
                        returnLeadEntity.VisitorInternalId = DatabaseUtilities.GetDataReaderInt(dataReader, "VisitorInternalId", 0);
                        returnLeadEntity.FormUniqueId = DatabaseUtilities.GetDataReaderInt(dataReader, "FormUniqueId", 0);
                        returnLeadEntity.CRId = DatabaseUtilities.GetDataReaderInt(dataReader, "CRId", 0);
                        returnLeadEntity.PSIId = DatabaseUtilities.GetDataReaderInt(dataReader, "PSIId", 0);
                        returnLeadEntity.ProgramId = DatabaseUtilities.GetDataReaderInt(dataReader, "ProgramId", 0);
                        returnLeadEntity.CategoryId = DatabaseUtilities.GetDataReaderInt(dataReader, "CategoryId", 0);
                        returnLeadEntity.FirstName = DatabaseUtilities.GetDataReaderString(dataReader, "FirstName", null);
                        returnLeadEntity.LastName = DatabaseUtilities.GetDataReaderString(dataReader, "LastName", null);
                        returnLeadEntity.MiddleName = DatabaseUtilities.GetDataReaderString(dataReader, "MiddleName", null);
                        returnLeadEntity.Address1 = DatabaseUtilities.GetDataReaderString(dataReader, "Address1", null);
                        returnLeadEntity.Address2 = DatabaseUtilities.GetDataReaderString(dataReader, "Address2", null);
                        returnLeadEntity.City = DatabaseUtilities.GetDataReaderString(dataReader, "City", null);
                        returnLeadEntity.ZipCode = DatabaseUtilities.GetDataReaderString(dataReader, "ZipCode", null);
                        returnLeadEntity.StateProvince = DatabaseUtilities.GetDataReaderString(dataReader, "StateProvince", null);
                        returnLeadEntity.CountryCode = DatabaseUtilities.GetDataReaderString(dataReader, "CountryCode", null);
                        returnLeadEntity.EmailAddress = DatabaseUtilities.GetDataReaderString(dataReader, "EmailAddress", null);
                        returnLeadEntity.Phone1 = DatabaseUtilities.GetDataReaderString(dataReader, "Phone1", null);
                        returnLeadEntity.Phone2 = DatabaseUtilities.GetDataReaderString(dataReader, "Phone2", null);
                        returnLeadEntity.TimeToStartInWeeks = DatabaseUtilities.GetDataReaderInt(dataReader, "TimeToStartInWeeks", 0);
                        returnLeadEntity.RawPostDataId = DatabaseUtilities.GetDataReaderInt64(dataReader, "RawPostDataId", 0);
                        returnLeadEntity.TransactionId = DatabaseUtilities.GetDataReaderGuid(dataReader, "TransactionId").ToString();
                        
                        returnLeadEntity.ApplicationId = DatabaseUtilities.GetDataReaderInt(dataReader, "ApplicationId", 0);
                        returnLeadEntity.ChannelId = DatabaseUtilities.GetDataReaderInt(dataReader, "ChannelId", 0);
                        returnLeadEntity.VendorId = DatabaseUtilities.GetDataReaderInt(dataReader, "VendorId", 0);
                        returnLeadEntity.BusinessModelId = DatabaseUtilities.GetDataReaderInt(dataReader, "BusinessModelId", 0);

                        returnLeadEntity.RealTimeDeliveryStatusId = DatabaseUtilities.GetDataReaderInt(dataReader, "RealTimeDeliveryStatusId", 0);
                        returnLeadEntity.Prefix = DatabaseUtilities.GetDataReaderString(dataReader, "Prefix", null);
                        returnLeadEntity.Age = DatabaseUtilities.GetDataReaderInt(dataReader, "Age", 0);
                        returnLeadEntity.YearHighestEduCompleted = DatabaseUtilities.GetDataReaderInt(dataReader, "YearHighestEduCompleted", 0);
                        returnLeadEntity.HighestLevelOfEdu = DatabaseUtilities.GetDataReaderString(dataReader, "HighestLevelOfEdu", null);
                        returnLeadEntity.Military = DatabaseUtilities.GetDataReaderString(dataReader, "Military", null);
                        returnLeadEntity.MethodOfContact = DatabaseUtilities.GetDataReaderString(dataReader, "MethodOfContact", null);
                        returnLeadEntity.StartDate = DatabaseUtilities.GetDataReaderString(dataReader, "StartDate", null);
                        returnLeadEntity.LegacyLeadId = DatabaseUtilities.GetDataReaderString(dataReader, "LegacyLeadId", null);
                        returnLeadEntity.CampusId = DatabaseUtilities.GetDataReaderInt(dataReader, "CampusId", 0);
                        returnLeadEntity.ProductId = DatabaseUtilities.GetDataReaderInt(dataReader, "ProductId", 0);
                        returnLeadEntity.CampusValue = DatabaseUtilities.GetDataReaderString(dataReader, "CampusValue", null);
                        returnLeadEntity.LeadCreationTypeId = DatabaseUtilities.GetDataReaderInt(dataReader, "LeadCreationTypeId", 0);

                        string xml = DatabaseUtilities.GetDataReaderString(dataReader, "AdditionalFields", null);
                        returnLeadEntity.AdditionalFields = GetAdditionalFieldsDictionaryFromXML(xml);
                    }
                    else
                    {
                        return null;
                    }

                }

            }

            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);

            }

            return returnLeadEntity;
        }

        public DeliveryLeadData GetRealtimeDeliveryLeadDataByLeadId(int leadId)
        {
            DeliveryLeadData returnLeadEntity = new DeliveryLeadData();

            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_GetRealtimeLeadById", Db);
            Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);

            try
            {
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader.Read())
                    {
                        returnLeadEntity.LeadId = leadId;
                        returnLeadEntity.SessionInternalId = DatabaseUtilities.GetDataReaderInt(dataReader, "SessionInternalId", 0);
                        returnLeadEntity.VisitorInternalId = DatabaseUtilities.GetDataReaderInt(dataReader, "VisitorInternalId", 0);
                        returnLeadEntity.FormUniqueId = DatabaseUtilities.GetDataReaderInt(dataReader, "FormUniqueId", 0);
                        returnLeadEntity.CRId = DatabaseUtilities.GetDataReaderInt(dataReader, "CRId", 0);
                        returnLeadEntity.PSIId = DatabaseUtilities.GetDataReaderInt(dataReader, "PSIId", 0);
                        returnLeadEntity.ProgramId = DatabaseUtilities.GetDataReaderInt(dataReader, "ProgramId", 0);
                        returnLeadEntity.CategoryId = DatabaseUtilities.GetDataReaderInt(dataReader, "CategoryId", 0);
                        returnLeadEntity.FirstName = DatabaseUtilities.GetDataReaderString(dataReader, "FirstName", null);
                        returnLeadEntity.LastName = DatabaseUtilities.GetDataReaderString(dataReader, "LastName", null);
                        returnLeadEntity.MiddleName = DatabaseUtilities.GetDataReaderString(dataReader, "MiddleName", null);
                        returnLeadEntity.Address1 = DatabaseUtilities.GetDataReaderString(dataReader, "Address1", null);
                        returnLeadEntity.Address2 = DatabaseUtilities.GetDataReaderString(dataReader, "Address2", null);
                        returnLeadEntity.City = DatabaseUtilities.GetDataReaderString(dataReader, "City", null);
                        returnLeadEntity.ZipCode = DatabaseUtilities.GetDataReaderString(dataReader, "ZipCode", null);
                        returnLeadEntity.StateProvince = DatabaseUtilities.GetDataReaderString(dataReader, "StateProvince", null);
                        returnLeadEntity.CountryCode = DatabaseUtilities.GetDataReaderString(dataReader, "CountryCode", null);
                        returnLeadEntity.EmailAddress = DatabaseUtilities.GetDataReaderString(dataReader, "EmailAddress", null);
                        returnLeadEntity.Phone1 = DatabaseUtilities.GetDataReaderString(dataReader, "Phone1", null);
                        returnLeadEntity.Phone2 = DatabaseUtilities.GetDataReaderString(dataReader, "Phone2", null);
                        returnLeadEntity.TimeToStartInWeeks = DatabaseUtilities.GetDataReaderInt(dataReader, "TimeToStartInWeeks", 0);
                        returnLeadEntity.RawPostDataId = DatabaseUtilities.GetDataReaderInt64(dataReader, "RawPostDataId", 0);
                        returnLeadEntity.TransactionId = DatabaseUtilities.GetDataReaderGuid(dataReader, "TransactionId").ToString();

                        returnLeadEntity.ApplicationId = DatabaseUtilities.GetDataReaderInt(dataReader, "ApplicationId", 0);
                        returnLeadEntity.ChannelId = DatabaseUtilities.GetDataReaderInt(dataReader, "ChannelId", 0);
                        returnLeadEntity.VendorId = DatabaseUtilities.GetDataReaderInt(dataReader, "VendorId", 0);
                        returnLeadEntity.BusinessModelId = DatabaseUtilities.GetDataReaderInt(dataReader, "BusinessModelId", 0);

                        returnLeadEntity.RealTimeDeliveryStatusId = DatabaseUtilities.GetDataReaderInt(dataReader, "RealTimeDeliveryStatusId", 0);
                        returnLeadEntity.Prefix = DatabaseUtilities.GetDataReaderString(dataReader, "Prefix", null);
                        returnLeadEntity.Age = DatabaseUtilities.GetDataReaderInt(dataReader, "Age", 0);
                        returnLeadEntity.YearHighestEduCompleted = DatabaseUtilities.GetDataReaderInt(dataReader, "YearHighestEduCompleted", 0);
                        returnLeadEntity.HighestLevelOfEdu = DatabaseUtilities.GetDataReaderString(dataReader, "HighestLevelOfEdu", null);
                        returnLeadEntity.Military = DatabaseUtilities.GetDataReaderString(dataReader, "Military", null);
                        returnLeadEntity.MethodOfContact = DatabaseUtilities.GetDataReaderString(dataReader, "MethodOfContact", null);
                        returnLeadEntity.StartDate = DatabaseUtilities.GetDataReaderString(dataReader, "StartDate", null);
                        returnLeadEntity.LegacyLeadId = DatabaseUtilities.GetDataReaderString(dataReader, "LegacyLeadId", null);
                        returnLeadEntity.CampusId = DatabaseUtilities.GetDataReaderInt(dataReader, "CampusId", 0);
                        returnLeadEntity.ProductId = DatabaseUtilities.GetDataReaderInt(dataReader, "ProductId", 0);
                        returnLeadEntity.CampusValue = DatabaseUtilities.GetDataReaderString(dataReader, "CampusValue", null);
                        returnLeadEntity.LeadCreationTypeId = DatabaseUtilities.GetDataReaderInt(dataReader, "LeadCreationTypeId", 0);

                        string xml = DatabaseUtilities.GetDataReaderString(dataReader, "AdditionalFields", null);
                        returnLeadEntity.AdditionalFields = GetAdditionalFieldsDictionaryFromXML(xml);
                    }
                    else
                    {
                        return null;
                    }

                }

            }

            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);

            }

            return returnLeadEntity;
        }
        
        Dictionary<string, object> ILeadDAO.GetAdditionalFieldsDictionaryFromXML(string additionalFieldsXML)
        {
            return GetAdditionalFieldsDictionaryFromXML(additionalFieldsXML);
        }
        
        private static Dictionary<string, object> GetAdditionalFieldsDictionaryFromXML(string additionalFieldsXML)
        {
            Dictionary<string, object> returnDictionary = null;
            MemoryStream ms;

            try
            {
                if (additionalFieldsXML != null)
                {
                    // Encode the XML string in a UTF-8 byte array 
                    byte[] encodedString = Encoding.UTF8.GetBytes(additionalFieldsXML);

                    //Put the byte array into a stream and rewind it to the beginning 
                    ms = new MemoryStream(encodedString);
                    ms.Flush();
                    ms.Position = 0;


                    returnDictionary = new Dictionary<string, object>();

                    XmlDocument doc = new XmlDocument();
                    doc.Load(ms);
                    XmlNodeList xmlNodeList = doc.SelectNodes("LeadAdditionalFields/Fields/Field");

                    foreach (XmlNode node in xmlNodeList)
                    {
                        //get key
                        XmlAttribute fieldNameAttribute = node.Attributes["FieldName"];
                        string key = fieldNameAttribute.InnerText;

                        //get value
                        XmlAttribute valueAttribute = node.Attributes["Value"];
                        string value = valueAttribute.InnerText;

                        //add key/value to return dictionary
                        returnDictionary.Add(key, value);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

            return returnDictionary;

        }
    }
}
