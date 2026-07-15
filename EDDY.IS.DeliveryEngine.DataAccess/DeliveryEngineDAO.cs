using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data.SqlTypes;
using System.Linq;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.Nexus.Common.Logging;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.DataAccess.Common;
//using EDDY.Nexus.DataAccess.CoreBusiness;
//using EDDY.Nexus.DataAccess.Interface.CoreBusiness;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DataTransformation;
//using EDDY.Nexus.Entity.DeliveryEngine;
using System.Xml;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Common;
using EDDY.Nexus.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.DataAccess
{
    //public class DeliveryEngineDAO : BaseDataSource, IDeliveryEngineDAO
    public class DeliveryEngineDAO : BaseDataSource, IDeliveryEngineDAO
    {

        #region Public Methods

        private static readonly Object padLock = new object();

        public int DefaultMaxRetryAttempts
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["DefaultPOSTRetryAttempts"] != null)
                {
                    return Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["DefaultPOSTRetryAttempts"]);
                }
                else
                {
                    return 5;
                }
            }
        }

        public int DefaultRetryDelay
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["DefaultRetryDelay"] != null)
                {
                    return Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["DefaultRetryDelay"]);
                }
                else
                {
                    return 6;
                }
            }
        }
        public CheckActiveEndPoint CheckActiveEndPoint(int CSRID, int DelDefID, int EndPointID)
        {

            CheckActiveEndPoint activeendPoint = new CheckActiveEndPoint();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {


                dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_CheckActiveEndPoints", Db);
                Db.AddInParameter(dbCommand, "@CSRId", DbType.Int32, CSRID);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, DelDefID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, EndPointID);
                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        //create definition
                        activeendPoint.Active = Convert.ToInt32(dataReader["Active"]);
                        activeendPoint.IsActiveEndPointExists = Convert.ToString(dataReader["ActiveExists"]);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return activeendPoint;

        }

        public List<DeliveryDefinition> GetCRDeliveryDefinitions(int crId)
        {
            List<DeliveryDefinition> returnDefs = new List<DeliveryDefinition>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetCRDeliveryDefinitions", Db);
                Db.AddInParameter(dbCommand, "@CRId", DbType.Int32, crId);
                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        //create definition
                        def = new DeliveryDefinition();
                        def.DeliveryDefinitionId = Convert.ToInt32(dataReader["DeliveryDefinitionId"]);
                        def.CRId = crId;

                        def.DeliveryName = GetDBString(dataReader, "DeliveryName", null);
                        def.ConditionXML = GetDBString(dataReader, "ConditionXML", null);
                        def.Active = GetDBInt(dataReader, "Active", 0);
                        def.Priority = GetDBInt(dataReader, "Priority", -1);

                        //add to list
                        returnDefs.Add(def);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnDefs;
        }


        public List<DeliveryDefinition> GetCRDeliveryDefinitionsForLead(int crId, int leadId)
        {
            List<DeliveryDefinition> returnDefs = new List<DeliveryDefinition>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetCRDeliveryDefinitionsForLead", Db);
                Db.AddInParameter(dbCommand, "@CRId", DbType.Int32, crId);
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        //create definition
                        def = new DeliveryDefinition();
                        def.DeliveryDefinitionId = Convert.ToInt32(dataReader["DeliveryDefinitionId"]);
                        def.CRId = crId;

                        def.DeliveryName = GetDBString(dataReader, "DeliveryName", null);
                        def.ConditionXML = GetDBString(dataReader, "ConditionXML", null);
                        def.Active = GetDBInt(dataReader, "Active", 0);
                        def.Priority = GetDBInt(dataReader, "Priority", -1);

                        //add to list
                        returnDefs.Add(def);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnDefs;
        }



        public List<DeliveryDefinition> GetCRDeliveryDefinitions(DeliveryDefSearchParam deliveryparam)
        {
            List<DeliveryDefinition> returnDefs = new List<DeliveryDefinition>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetCRDeliveryDefinitions", Db);
                Db.AddInParameter(dbCommand, "@CRId", DbType.Int32, deliveryparam.CrId);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, deliveryparam.DeliveryDefId);
                Db.AddInParameter(dbCommand, "@NameStartsWith", DbType.String, deliveryparam.NameStartsWith);
                Db.AddInParameter(dbCommand, "@NameContains", DbType.String, deliveryparam.NameContains);
                Db.AddInParameter(dbCommand, "@SortField", DbType.String, deliveryparam.SortField);
                Db.AddInParameter(dbCommand, "@IsActive", DbType.Int32, deliveryparam.IsActive);

                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        //create definition
                        def = new DeliveryDefinition();
                        def.DeliveryDefinitionId = Convert.ToInt32(dataReader["DeliveryDefinitionId"]);
                        def.CRId = deliveryparam.CrId;

                        def.DeliveryName = GetDBString(dataReader, "DeliveryName", null);
                        def.ConditionXML = GetDBString(dataReader, "ConditionXML", null);
                        def.Active = GetDBInt(dataReader, "Active", 0);
                        def.Priority = GetDBInt(dataReader, "Priority", -1);
                        def.CreatedDate = GetDBDateTime(dataReader, "CreatedDate", DateTime.UtcNow);
                        def.UpdatedDate = GetDBDateTime(dataReader, "UpdatedDate", DateTime.UtcNow);
                        def.UpdatedByName = GetDBString(dataReader, "UpdatedByName", string.Empty);
                        //add to list
                        returnDefs.Add(def);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnDefs;
        }
        public bool UpdateDeliveryDefinition(DeliveryDefinition deliveryDefinition)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            bool blnDeldef = false;
            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_UpdateDeliveryDefinition", Db);
                Db.AddInParameter(dbCommand, "@CSRId", DbType.Int32, deliveryDefinition.CRId);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, deliveryDefinition.DeliveryDefinitionId);
                Db.AddInParameter(dbCommand, "@Priority", DbType.Int32, deliveryDefinition.Priority);
                Db.AddInParameter(dbCommand, "@DeliveryName", DbType.String, deliveryDefinition.DeliveryName);
                Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Boolean, deliveryDefinition.IsEnabled);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, deliveryDefinition.UpdatedBy);

                int RecordsAffected = Db.ExecuteNonQuery(dbCommand);


                if (RecordsAffected != -1)
                {
                    blnDeldef = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return blnDeldef;

        }

        public bool UpdateDeliveryCondition(int CsrID, int DelDefID, string ConditionXML)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            bool blnDeldef = false;
            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_UpdateDeliveryConditions", Db);
                Db.AddInParameter(dbCommand, "@CsrID", DbType.Int32, CsrID);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, DelDefID);
                Db.AddInParameter(dbCommand, "@ConditionXML", DbType.String, ConditionXML);
                int RecordsAffected = Db.ExecuteNonQuery(dbCommand);


                if (RecordsAffected != -1)
                {
                    blnDeldef = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return blnDeldef;

        }
        public bool CloneDeliveryDefinition(int DelDefID)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            bool blnDeldef = false;
            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_CloneDeliveryDefinition", Db);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, DelDefID);
                int RecordsAffected = Db.ExecuteNonQuery(dbCommand);


                if (RecordsAffected != -1)
                {
                    blnDeldef = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return blnDeldef;

        }
        public bool DeleteNonActiveDeliveryDefinition(int DelDefID)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            bool blnDeldef = false;
            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_DeleteDeliveryDefinition", Db);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, DelDefID);
                int RecordsAffected = Db.ExecuteNonQuery(dbCommand);


                if (RecordsAffected != -1)
                {
                    blnDeldef = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return blnDeldef;

        }

        public List<ConditionDataTypeItem> GetDeliveryConditionWithType()
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            List<ConditionDataTypeItem> conds = new List<ConditionDataTypeItem>();
            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDeliveryConditionWithType", Db);
                using (dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        ConditionDataTypeItem condDataItem = new ConditionDataTypeItem(dataReader);
                        conds.Add(condDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return conds;
        }

        public List<int> GetDistinctCSRIds()
        {
            List<int> CSRIDs = new List<int>();
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDistinctCSRIds", Db);

                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        //add to list
                        CSRIDs.Add(GetDBInt(dataReader, "csrid", -1));
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }


            return CSRIDs;
        }

        public bool TempLog(int leadId, int deliveryEngineEventId, int deliveryEndpointId, string message, int userId,bool IsBeta)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_TempDeliveryEngineLog_Insert to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "TempLog" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_TempDeliveryEngineLog_Insert", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@DeliveryEngineEventId", DbType.Int32, deliveryEngineEventId);
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);
                Db.AddInParameter(dbCommand, "@Message", DbType.String, message);
                Db.AddInParameter(dbCommand, "@EventDateTime", DbType.DateTime, DateTime.Now.ToUniversalTime());
                Db.AddInParameter(dbCommand, "@IsBeta", DbType.Boolean, IsBeta);
                Db.AddInParameter(dbCommand, "@CreatedBy", DbType.Int32, userId);
                


                Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY_NO_RETHROW );
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }

            return success;
        }

        public List<int> GetDistinctPSIIds()
        {
            List<int> PSIIDs = new List<int>();
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDistinctPSIIds", Db);

                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        //add to list
                        PSIIDs.Add(GetDBInt(dataReader, "psiid", -1));
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }


            return PSIIDs;
        }

        public long GetProgramIDFromCSRId(int csrId)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetProgramIDFromCSRId", Db);

                Db.AddInParameter(dbCommand, "@CSRId", DbType.Int32, csrId);

                dataReader = Db.ExecuteReader(dbCommand);

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        //add to list
                        return GetDBBigInt(dataReader, "programid", -1);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return -1;
        }

        public void InsertLead(int RawPostDataId, decimal SessionInternalId, decimal VisitorInternalId, decimal FormUniqueId,
                                int CSRId, int PSIId, long ProgramId, int CategoryId, string FirstName,
                                string MiddleName, string LastName, string Address1, string Address2,
                                string City, string ZipCode, string StateProvince, string CountryCode,
                                string EmailAddress, string Phone1, string Phone2, int TimeToStartInWeeks,
                                SqlXml AdditionalFields, int DeliveryDefinitionId, int RealtimeDeliveryStatusId,
                                Guid DeliveryEngineMachineKey, int userId)
        {
            //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_InsertLead to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "InsertLead" }, { "ModuleName", "DeliveryEngine.DAO" } });
            //Insert Lead Record
            var dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_InsertLead", Db);
            dbCommand.CommandTimeout = 120;
            Db.AddInParameter(dbCommand, "@RawPostDataId", DbType.Int32, RawPostDataId);
            Db.AddInParameter(dbCommand, "@SessionInternalId", DbType.Decimal, SessionInternalId);
            Db.AddInParameter(dbCommand, "@VisitorInternalId", DbType.Decimal, VisitorInternalId);
            Db.AddInParameter(dbCommand, "@FormUniqueId", DbType.Decimal, FormUniqueId);
            Db.AddInParameter(dbCommand, "@CSRId", DbType.Int32, (CSRId == 0) ? SqlInt32.Null : CSRId);
            Db.AddInParameter(dbCommand, "@PSIId", DbType.Int32, (PSIId == 0) ? SqlInt32.Null : PSIId);
            Db.AddInParameter(dbCommand, "@ProgramId", DbType.Int64, (ProgramId == 0) ? SqlInt64.Null : ProgramId);
            Db.AddInParameter(dbCommand, "@CategoryId", DbType.Int32, (CategoryId == 0) ? SqlInt32.Null : CategoryId);
            Db.AddInParameter(dbCommand, "@FirstName", DbType.String, FirstName);
            Db.AddInParameter(dbCommand, "@MiddleName", DbType.String, MiddleName);
            Db.AddInParameter(dbCommand, "@LastName", DbType.String, LastName);
            Db.AddInParameter(dbCommand, "@Address1", DbType.String, Address1);
            Db.AddInParameter(dbCommand, "@Address2", DbType.String, Address2);
            Db.AddInParameter(dbCommand, "@City", DbType.String, City);
            Db.AddInParameter(dbCommand, "@ZipCode", DbType.String, ZipCode);
            Db.AddInParameter(dbCommand, "@StateProvince", DbType.String, StateProvince);
            Db.AddInParameter(dbCommand, "@CountryCode", DbType.String, CountryCode);
            Db.AddInParameter(dbCommand, "@EmailAddress", DbType.String, EmailAddress);
            Db.AddInParameter(dbCommand, "@Phone1", DbType.String, Phone1);
            Db.AddInParameter(dbCommand, "@Phone2", DbType.String, Phone2);
            Db.AddInParameter(dbCommand, "@TimeToStartInWeeks", DbType.Int32, (TimeToStartInWeeks == 0) ? SqlInt32.Null : TimeToStartInWeeks);
            Db.AddInParameter(dbCommand, "@AdditionalFields", DbType.Xml, AdditionalFields);
            Db.AddInParameter(dbCommand, "@DeliveryDefinitionId", DbType.Int32, (DeliveryDefinitionId == 0) ? SqlInt32.Null : DeliveryDefinitionId);
            Db.AddInParameter(dbCommand, "@RealtimeDeliveryStatusId", DbType.Int32, (RealtimeDeliveryStatusId == 0) ? SqlInt32.Null : RealtimeDeliveryStatusId);
            Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.Guid, DeliveryEngineMachineKey);
            Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);
            // Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, userId);


            try
            {
                int iAffectedRows = Db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {

                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }

            return;
        }

        DeliveryDefinition IDeliveryEngineDAO.GetDeliveryDefinition(int deliveryDefinitionId)
        {
            DeliveryDefinition returnDefinition = new DeliveryDefinition();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDeliveryDefinition", Db);
                Db.AddInParameter(dbCommand, "@DeliveryDefinitionId", DbType.Int32, deliveryDefinitionId);
                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    //get result set 1 - definitions
                    while (dataReader.Read())
                    {
                        returnDefinition.DeliveryDefinitionId = deliveryDefinitionId;
                        returnDefinition.CRId = GetDBInt(dataReader, "CSRId", 0);
                        returnDefinition.DeliveryName = GetDBString(dataReader, "DeliveryName", null);
                        returnDefinition.ConditionXML = GetDBString(dataReader, "ConditionXML", null);
                        returnDefinition.Priority = GetDBInt(dataReader, "Priority", -1);
                    }


                    //get result set 2 - endpoints
                    dataReader.NextResult();

                    DeliveryEndpoint endpointToAdd;
                    int deliveryTypeId;
                    string endpointDetailXML;
                    Dictionary<string, string> endPointDetailDictionary;

                    //create each endpoint
                    while (dataReader.Read())
                    {
                        //get typeId to handle for each type of DeliveryEndpoint
                        deliveryTypeId = Convert.ToInt32(dataReader["DeliveryTypeId"]);

                        //get endpintDetailXML
                        endpointDetailXML = GetDBString(dataReader, "EndpointDetailXML", null);

                        //convert XML to Dictionary
                        endPointDetailDictionary = GetDictionaryFromEndpointDetailXML(endpointDetailXML);

                        //check type of endpoint (change this to factory when there is time)
                        switch (deliveryTypeId)
                        {
                            case 1:
                                {
                                    endpointToAdd = new InstantPostEndpoint();
                                    AddInstantPostEndpointProperties((InstantPostEndpoint)endpointToAdd, endPointDetailDictionary);
                                    break;
                                }
                            case 2:
                                {
                                    endpointToAdd = new InstantEmailEndpoint();
                                    AddInstantEmailEndpointProperties((InstantEmailEndpoint)endpointToAdd, endPointDetailDictionary);
                                    break;
                                }
                            case 3:
                                {
                                    endpointToAdd = new BatchEmailEndpoint();
                                    AddBatchEmailEndpointProperties((BatchEmailEndpoint)endpointToAdd, endPointDetailDictionary);
                                    break;
                                }
                            case 4:
                            default:
                                {
                                    endpointToAdd = new BatchFtpEndpoint();
                                    AddBatchFTPEndpointProperties((BatchFtpEndpoint)endpointToAdd, endPointDetailDictionary);
                                    break;
                                }
                        }

                        //add common endpoint fields
                        endpointToAdd.DeliveryDefinitionId = returnDefinition.DeliveryDefinitionId;
                        endpointToAdd.CSRId = returnDefinition.CRId;
                        endpointToAdd.DeliveryEndpointId = GetDBInt(dataReader, "DeliveryEndpointId", 0);
                        endpointToAdd.EndpointName = GetDBString(dataReader, "EndpointName", null);
                        endpointToAdd.DeliveryTypeId = deliveryTypeId;
                        endpointToAdd.DataTransformationId = GetDBInt(dataReader, "DataTransformationId", 0);

                        //add current endpoint to the list
                        returnDefinition.EndPoints.Add(endpointToAdd);
                    }


                    //get result set 3 - blackouts
                    dataReader.NextResult();
                    int currentEndpointId;
                    int blackoutEndpointId;
                    DeliveryBlackoutPeriod dbpToAdd;
                    DateTime defaultDateTime = Convert.ToDateTime("01-01-1900");
                    int currentBlackoutTypeInt;
                    BlackoutType currentBlackoutType;
                    string recurringBlackoutDataXML;

                    //loop through all blackouts
                    while (dataReader.Read())
                    {
                        //for each endpoint check for match
                        foreach (DeliveryEndpoint currentEndpoint in returnDefinition.EndPoints)
                        {
                            currentEndpointId = currentEndpoint.DeliveryEndpointId;

                            blackoutEndpointId = GetDBInt(dataReader, "DeliveryEndpointId", 0);
                            //if match - add new blackout to endpoint
                            if (currentEndpointId == blackoutEndpointId)
                            {
                                //Get the blackout type int from Database
                                currentBlackoutTypeInt = GetDBInt(dataReader, "DeliveryBlackoutType", 1);
                                //Get Enum from the int val
                                currentBlackoutType = (BlackoutType)Enum.ToObject(typeof(BlackoutType), currentBlackoutTypeInt);
                                //Get recurringBlackoutDataXML
                                recurringBlackoutDataXML = GetDBString(dataReader, "RecurringBlackoutXML", null);
                                //call factory method to get appropriate blackout object
                                dbpToAdd = BlackoutFactory.GetDeliveryBlackoutPeriodObject(currentBlackoutType, recurringBlackoutDataXML);

                                //add common properties
                                dbpToAdd.AbsloluteStartDateTime = GetDBDateTime(dataReader, "StartDateTime", defaultDateTime);
                                dbpToAdd.AbsloluteEndDateTime = GetDBDateTime(dataReader, "EndDateTime", defaultDateTime);
                                dbpToAdd.UTCHourOffset = GetDBInt(dataReader, "UTCHourOffset", 0);

                                //add blackout to endpoint
                                currentEndpoint.BlackoutPeriods.Add(dbpToAdd);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnDefinition;

        }

        List<DeliveryEndpoint> IDeliveryEngineDAO.GetDefinitionEndpoints(DeliveryDefinition deliveryDefinition)
        {
            return GetDefinitionEndpoints(deliveryDefinition);
        }

        List<DeliveryEndpoint> IDeliveryEngineDAO.GetDefinitionEndpoints(int deliveryDefinitionId)
        {
            return GetDefinitionEndpoints(deliveryDefinitionId);
        }

        DeliveryEndpoint IDeliveryEngineDAO.GetEndpointById(int deliveryEndpointId)
        {
            DeliveryEndpoint returnEndpoint = null;
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                //get endpoint list from DB
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDeliveryEndpoint", Db);
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);
                dataReader = Db.ExecuteReader(dbCommand);

                dataReader.Read();
                returnEndpoint = GetDefinitionEndpoint(dataReader);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnEndpoint;
        }
        
        List<string> IDeliveryEngineDAO.GetDTStoredProcedureNames()
        {
            List<string> returnData= new List<string>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDTStoredProcedureNames", Db);
                dataReader = Db.ExecuteReader(dbCommand);

                while (dataReader.Read())
                {
                    returnData.Add(dataReader["SPName"].ToString());
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
               // clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnData;
        }

        List<DeliveryLeadData> IDeliveryEngineDAO.GetLeadsForBatch(int batchDeliveryId)
        {
            List<DeliveryLeadData> returnLeadData = new List<DeliveryLeadData>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                //get lead list from DB
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetLeadsForBatch", Db);
                Db.AddInParameter(dbCommand, "@BatchDeliveryId", DbType.Int32, batchDeliveryId);
                dataReader = Db.ExecuteReader(dbCommand);
                returnLeadData = GetLeadDataListFromReader(dataReader);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                if (dbCommand != null)
                    if (dataReader != null) DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);

            }

            return returnLeadData;
        }

        int IDeliveryEngineDAO.CreateNewBatchDelivery(int deliveryEndpointId, int userId, int productId, string machineName)
        {
            int newBatchDeliveryId = -1;
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_Batch_Insert to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "CreateNewBatchDelivery" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_Batch_Insert", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);
                Db.AddInParameter(dbCommand, "@ProductId", DbType.Int32, productId);
                Db.AddInParameter(dbCommand, "@MachineName", DbType.String, machineName);
                //ececute and return new rownumber
                newBatchDeliveryId = (int)Db.ExecuteScalar(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return newBatchDeliveryId;
        }

        bool IDeliveryEngineDAO.LogBatchDeliveryComplete(int batchDeilveryId, DeliveryEndpoint deliveryEndpoint, string batchFileName, int leadCount, int userId)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_Batch_Update to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "LogBatchDeliveryComplete" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_Batch_Update", Db);
                Db.AddInParameter(dbCommand, "@BatchDeliveryId", DbType.Int32, batchDeilveryId);
                Db.AddInParameter(dbCommand, "@DeliveryDate", DbType.DateTime, DateTime.Now.ToUniversalTime());
                Db.AddInParameter(dbCommand, "@BatchFileName", DbType.String, batchFileName);
                Db.AddInParameter(dbCommand, "@LeadCount", DbType.Int32, leadCount);
                Db.AddInParameter(dbCommand, "@DeliveryTypeId", DbType.Int32, deliveryEndpoint.DeliveryTypeId);
                Db.AddInParameter(dbCommand, "@EndpointDetailXML", DbType.Xml, deliveryEndpoint.EndpointDetailXML);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

                int commandResult = Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;
        }

        bool IDeliveryEngineDAO.ResetRealtimeDeliveryQueueForMachine(string machineKey)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_ResetRealtimeQueueForMachine", Db);

                Guid guidMachineKey = new System.Guid(machineKey);

                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.Guid, guidMachineKey);

                int iRecordsAffected = Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;
        }
        bool IDeliveryEngineDAO.LogPreviewDeliveryComplete(int leadid, int deliveryEndpointId, string leadData, string endpointDetailXML, bool deliverySuccess, DateTime deliveryDateTime, int attemptNumber, string serverResponse, string postResponse, Guid machineKey, int userId)
        {
             bool success = false;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_PreviewDeliveryLog_Insert to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "LogPreviewDeliveryComplete" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_PreviewDeliveryLog_Insert", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadid);
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);
                Db.AddInParameter(dbCommand, "@LeadData", DbType.String, leadData);
                Db.AddInParameter(dbCommand, "@EndpointDetailXML", DbType.Xml, endpointDetailXML);
                Db.AddInParameter(dbCommand, "@Success", DbType.Boolean, deliverySuccess);
                Db.AddInParameter(dbCommand, "@DeliveryDateTime", DbType.DateTime, deliveryDateTime);
                Db.AddInParameter(dbCommand, "@AttemptNumber", DbType.Int32, attemptNumber);

                if (serverResponse != null)
                {
                    Db.AddInParameter(dbCommand, "@ServerResponse", DbType.String, serverResponse);
                }

                if (postResponse != null)
                {
                    Db.AddInParameter(dbCommand, "@PostResponse", DbType.String, postResponse);
                }

                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.Guid, machineKey);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

                int commandResult = Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;
        }
        bool IDeliveryEngineDAO.LogRealtimeDeliveryComplete(int leadid, int deliveryEndpointId, string leadData, string endpointDetailXML, bool deliverySuccess, DateTime deliveryDateTime, int attemptNumber, string serverResponse, string postResponse, Guid machineKey, int userId,bool IsBeta,string Detination, int rejectionReasonId, int reviewedStatusId, int schoolValidationResponseStatusID)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_RealtimeDeliveryLog_Insert to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "LogRealtimeDeliveryComplete" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_RealtimeDeliveryLog_Insert", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadid);
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);
                Db.AddInParameter(dbCommand, "@LeadData", DbType.String, leadData);
                Db.AddInParameter(dbCommand, "@EndpointDetailXML", DbType.Xml, endpointDetailXML);
                Db.AddInParameter(dbCommand, "@Success", DbType.Boolean, deliverySuccess);
                Db.AddInParameter(dbCommand, "@DeliveryDateTime", DbType.DateTime, deliveryDateTime);
                Db.AddInParameter(dbCommand, "@AttemptNumber", DbType.Int32, attemptNumber);
                Db.AddInParameter(dbCommand, "@IsBeta", DbType.Boolean, IsBeta);
                Db.AddInParameter(dbCommand, "@Destination", DbType.String, Detination);
                Db.AddInParameter(dbCommand, "@RejectionReasonID", DbType.Int32, rejectionReasonId);
                Db.AddInParameter(dbCommand, "@ReviewedStatusID", DbType.Int32, reviewedStatusId);
                Db.AddInParameter(dbCommand, "@schoolValidationResponseStatusID", DbType.Int32, schoolValidationResponseStatusID);

                if (serverResponse != null)
                {
                    Db.AddInParameter(dbCommand, "@ServerResponse", DbType.String, serverResponse);
                }

                if (postResponse != null)
                {
                    Db.AddInParameter(dbCommand, "@PostResponse", DbType.String, postResponse);
                }

                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.Guid, machineKey);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

                int commandResult = Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;
        }

        RealtimeDeliveryQueueItem IDeliveryEngineDAO.CreateRDQItem(int leadId, int deliveryEndpointId, Guid machinekey, int userId)
        {
            //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_RealtimeDeliveryQueue_Insert to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "CreateRDQItem" }, { "ModuleName", "DeliveryEngine.DAO" } });
            //Create return RDQ Item and set properties 
            RealtimeDeliveryQueueItem returnRDQ = new RealtimeDeliveryQueueItem();
            returnRDQ.LeadId = leadId;
            returnRDQ.EndpointId = deliveryEndpointId;
            returnRDQ.DeliveryEngineMachineKey = machinekey;
            returnRDQ.CurrentDeliveryAttempts = 0;

            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //Create RDQ Record and reutrn RDQ object
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_RealtimeDeliveryQueue_Insert", Db);
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);
                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.Guid, machinekey);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

                returnRDQ.RealtimeDeliveryQueueId = Convert.ToInt32(Db.ExecuteScalar(dbCommand));

            }
            catch (Exception ex)
            {
                //throw (ex);
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }

            return returnRDQ;
        }

        bool IDeliveryEngineDAO.UpdateRealtimeDeliveryQueue(int realtimeDeliveryQueueId, int deliveryStatusId, Guid machineKey, int currentDeliveryAttempts, DateTime lastDeliveryAttemptDatetime, DateTime nextDeliveryAttemptDatetime, int userId)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_RealtimeDeliveryQueue_Update to update a record into database", null, new Dictionary<string, object> { { "ModuleAction", "UpdateRealtimeDeliveryQueue" }, { "ModuleName", "DeliveryEngine.DAO" } });

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_RealtimeDeliveryQueue_Update", Db);
                Db.AddInParameter(dbCommand, "@RealtimeDeliveryQueueId", DbType.Int32, realtimeDeliveryQueueId);
                Db.AddInParameter(dbCommand, "@DeliveryStatusId", DbType.Int32, deliveryStatusId);
                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.Guid, machineKey);
                Db.AddInParameter(dbCommand, "@CurrentDeliveryAttempts", DbType.Int32, currentDeliveryAttempts);
                Db.AddInParameter(dbCommand, "@LastDeliveryAttemptDatetime", DbType.DateTime, lastDeliveryAttemptDatetime);
                Db.AddInParameter(dbCommand, "@NextDeliveryAttemptDatetime", DbType.DateTime, nextDeliveryAttemptDatetime);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

                int commandResult = Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;
        }

        List<RealtimeDeliveryQueueItem> IDeliveryEngineDAO.GetRDQsForProcessingReposts(int returnRecordsCount, string machineName, bool updateStatus)
        {
            List<RealtimeDeliveryQueueItem> returnList = new List<RealtimeDeliveryQueueItem>();
            RealtimeDeliveryQueueItem rdqToAdd;

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                //get RDQ item list from DB
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetRepostsForDelivery", Db);
                Db.AddInParameter(dbCommand, "@ReturnRecordCount", DbType.Int32, returnRecordsCount);
                //Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.Guid, machineKey);
                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineName", DbType.String, machineName);
                Db.AddInParameter(dbCommand, "@UpdateStatusToProcessing", DbType.Boolean, updateStatus);

                lock (padLock)
                {
                    dataReader = Db.ExecuteReader(dbCommand);
                }

                while (dataReader.Read())
                {
                    //Fill properties from Data
                    rdqToAdd = new RealtimeDeliveryQueueItem();
                    rdqToAdd.RealtimeDeliveryQueueId = DatabaseUtilities.GetDataReaderInt(dataReader, "RealtimeDeliveryQueueId", 0);
                    rdqToAdd.LeadId = DatabaseUtilities.GetDataReaderInt(dataReader, "LeadId", 0);
                    rdqToAdd.EndpointId = DatabaseUtilities.GetDataReaderInt(dataReader, "DeliveryEndpointId", 0);
                    rdqToAdd.DeliveryStatusId = DatabaseUtilities.GetDataReaderInt(dataReader, "DeliveryStatusId", 0);
                    rdqToAdd.DeliveryEngineMachineName = machineName;
                    rdqToAdd.CurrentDeliveryAttempts = DatabaseUtilities.GetDataReaderInt(dataReader, "CurrentDeliveryAttempts", 0);
                    rdqToAdd.LastDeliveryAttemptDatetime = DatabaseUtilities.GetDataReaderDateTime(dataReader, "LastDeliveryAttemptDatetime", Convert.ToDateTime("01-01-1900"));
                    rdqToAdd.NextDeliveryAttemptDatetime = DatabaseUtilities.GetDataReaderDateTime(dataReader, "NextDeliveryAttemptDateTime", Convert.ToDateTime("01-01-1900"));

                    //Add to return list
                    returnList.Add(rdqToAdd);
                }

            }
            catch (Exception ex)
            {
                throw;
                //ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY_NO_RETHROW);
            }
            finally
            {
                //clean up reader and command
                if (dbCommand != null)
                    if (dataReader != null) DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);

            }


            return returnList;
        }

        List<DeliveryLeadData> IDeliveryEngineDAO.GetNewLeadsForProcessing(int returnRecordsCount, string machineName, bool updateStatus, bool isBeta)
        {
            var returnList = new List<DeliveryLeadData>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetNewLeadsForProcessing_Slate", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@ReturnRecordCount", DbType.Int32, returnRecordsCount);
                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineKey", DbType.String, machineName);
                Db.AddInParameter(dbCommand, "@UpdateStatusToProcessing", DbType.Boolean, updateStatus);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int16, 1);
                Db.AddInParameter(dbCommand, "@IsBeta", DbType.Boolean, isBeta);

                lock (padLock)
                {
                    dataReader = Db.ExecuteReader(dbCommand);
                }

                //TEST
                //throw new SqlNullValueException("Test Error");
                returnList = GetLeadDataListFromReader(dataReader);
            }

            catch (Exception ex)
            {
                throw;
                //ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY_NO_RETHROW);
            }
            finally
            {
                //clean up reader and command
                if (dbCommand != null)
                    if (dataReader != null) DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);

            }

            return returnList;

        }

        List<DeliveryLeadData> IDeliveryEngineDAO.GetLeadForPreview(int LeadPreviewID)
        {
            var returnList = new List<DeliveryLeadData>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetLeadForPreview", Db);
                Db.AddInParameter(dbCommand, "@LeadPreviewID", DbType.Int32, LeadPreviewID);

                lock (padLock)
                {
                    dataReader = Db.ExecuteReader(dbCommand);
                }

                //TEST
                //throw new SqlNullValueException("Test Error");
                returnList = GetLeadDataListFromReader(dataReader);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY_NO_RETHROW);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnList;

        }

        bool IDeliveryEngineDAO.DeleteRealtimeDeliveryQueueRecord(int realtimeDeliveryQueueId)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_RealtimeDeliveryQueue_Delete to delete a record into database", null, new Dictionary<string, object> { { "ModuleAction", "DeleteRealtimeDeliveryQueueRecord" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_RealtimeDeliveryQueue_Delete", Db);
                Db.AddInParameter(dbCommand, "@RealtimeDeliveryQueueId", DbType.Int32, realtimeDeliveryQueueId);

                int commandResult = Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;

        }

        bool IDeliveryEngineDAO.UpdateLeadDeliveryDefinition(int leadId, int deliveryDefinitionId, int userId)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_Lead_UpdateDeliveryDefinition to update a record into database", null, new Dictionary<string, object> { { "ModuleAction", "UpdateLeadDeliveryDefinition" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_Lead_UpdateDeliveryDefinition", Db);
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@DeliveryDefinitionId", DbType.Int32, deliveryDefinitionId);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

                int commandResult = Db.ExecuteNonQuery(dbCommand);

                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;
        }
        
        int IDeliveryEngineDAO.CreateLeadDeliveryRecords(int leadId, int deliveryDefinitionId, int userId,bool IsBeta)
        {
            int numRowsAdded = 0;
            System.Data.Common.DbCommand dbCommand = null;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_UpdateLeadDelivery to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "CreateLeadDeliveryRecords" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_UpdateLeadDelivery", Db);
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@DeliveryDefinitionId", DbType.Int32, deliveryDefinitionId);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);
                Db.AddInParameter(dbCommand, "@IsBeta", DbType.Boolean, IsBeta);
                numRowsAdded = Db.ExecuteNonQuery(dbCommand);

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }

            return numRowsAdded;
        }

        void IDeliveryEngineDAO.LogBatchLead(int leadId, int batchid, int statusid)
        {
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_LogBatchLead to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "CreateLeadDeliveryRecords" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_LogBatchLead", Db);
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@BatchId", DbType.Int32,batchid);
                Db.AddInParameter(dbCommand, "@StatusId", DbType.Int32, statusid);

                Db.ExecuteNonQuery(dbCommand);

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }

        }

        bool IDeliveryEngineDAO.FinalizeLeadDelivery(int leadId, int leadRealtimeDeliveryStatus, int userId)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_FinalizeLeadDelivery", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@LeadRealtimeDeliveryStatusId", DbType.Int32, leadRealtimeDeliveryStatus);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);   
                Db.ExecuteNonQuery(dbCommand);
                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }

            return success;
        }


        //bool IDeliveryEngineDAO.FinalizeLeadPreview(int leadId, int leadRealtimeDeliveryStatus, int userId)
        //{
        //    bool success = false;
        //    System.Data.Common.DbCommand dbCommand = null;
        //    try
        //    {

        //        dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_FinalizeLeadDelivery", Db);
        //        Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
        //        Db.AddInParameter(dbCommand, "@LeadRealtimeDeliveryStatusId", DbType.Int32, leadRealtimeDeliveryStatus);
        //        Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);

        //        Db.ExecuteNonQuery(dbCommand);

        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
        //    }
        //    finally
        //    {
        //        //clean up command
        //        DatabaseUtilities.CloseAndDispose(ref dbCommand);
        //    }

        //    return success;
        //}

        bool IDeliveryEngineDAO.UpdateLeadStatusIfLastRealtimeDelivery(int leadId, int status, int userId, bool isTest, bool isPrimary, bool isBeta)
        {
            bool wasLastLead = false;
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_UpdateLeadStatusIfLastRealtimeDelivery to update a record into database", null, new Dictionary<string, object> { { "ModuleAction", "UpdateLeadStatusIfLastRealtimeDelivery" }, { "ModuleName", "DeliveryEngine.DAO" } });
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_UpdateLeadStatusIfLastRealtimeDelivery", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@StatusId", DbType.Int32, status);
                Db.AddInParameter(dbCommand, "@IsTest", DbType.Boolean ,isTest);
                Db.AddInParameter(dbCommand, "@IsPrimary", DbType.Boolean, isPrimary);
                Db.AddInParameter(dbCommand, "@IsBeta", DbType.Boolean, isBeta);
                //Db.AddInParameter(dbCommand, "@IsScrub", DbType.Boolean, isScrub);
                //ececute and return TRUE if this was the last realtime lead to be delivered and Lead table was updated with "Complete" status
                wasLastLead = (bool)Db.ExecuteScalar(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return wasLastLead;
        }
        /*
         * Added By Bala:04/19/2011
         
         */

        public List<DeliverySelectItem> GetDeliveryDataTypes()
        {
            List<DeliverySelectItem> delDataTypes = new List<DeliverySelectItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryDataTypeTypes", Db);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliverySelectItem delDataItem = new DeliverySelectItem(dataReader);
                        delDataTypes.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return delDataTypes;

        }
        public int AddUpdateDeliveryPostInstructions(DeliveryPI delPI)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            int EndPointID = 0;
            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_AddUpdateDeliveryPostInstructions", Db);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, delPI.DeliveryEndpointId);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, delPI.DelDefID);
                Db.AddInParameter(dbCommand, "@DeliveryTypeId", DbType.Int32, delPI.DeliveryTypeId);
                Db.AddInParameter(dbCommand, "@EndPointName", DbType.String, delPI.EndPointName);
                Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Int32, delPI.IsEnabled);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, delPI.UpdatedBy);
                Db.AddInParameter(dbCommand, "@EndPointDetailXML", DbType.String, delPI.EndpointDetailXML);
                Db.AddInParameter(dbCommand, "@EffectiveDate", DbType.DateTime, delPI.EffectiveDate);
                Db.AddInParameter(dbCommand, "@BatchFileXSL", DbType.String, delPI.BatchFileXSL);
                if (delPI.ExpirationDate != DateTime.MinValue)
                    Db.AddInParameter(dbCommand, "@ExpirationDate", DbType.DateTime, delPI.ExpirationDate);

                using (dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        EndPointID = (int)dataReader["EndPointID"];
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }
            return EndPointID;

        }

        public int AddUpdateDeliveryPostInstructions(DeliveryPI delPI, bool createDefaultTransformations)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            int EndPointID = 0;
            try
            {
                DeliveryDefinition def;

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_AddUpdateDeliveryPostInstructions", Db);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, delPI.DeliveryEndpointId);
                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, delPI.DelDefID);
                Db.AddInParameter(dbCommand, "@DeliveryTypeId", DbType.Int32, delPI.DeliveryTypeId);
                Db.AddInParameter(dbCommand, "@EndPointName", DbType.String, delPI.EndPointName);
                Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Int32, delPI.IsEnabled);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, delPI.UpdatedBy);
                Db.AddInParameter(dbCommand, "@EndPointDetailXML", DbType.String, delPI.EndpointDetailXML);
                Db.AddInParameter(dbCommand, "@EffectiveDate", DbType.DateTime, delPI.EffectiveDate);
                Db.AddInParameter(dbCommand, "@BatchFileXSL", DbType.String, delPI.BatchFileXSL);
                Db.AddInParameter(dbCommand, "@CreateDefaultTransformations", DbType.Boolean, createDefaultTransformations);
                if (delPI.ExpirationDate != DateTime.MinValue)
                    Db.AddInParameter(dbCommand, "@ExpirationDate", DbType.DateTime, delPI.ExpirationDate);

                using (dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        EndPointID = (int)dataReader["EndPointID"];
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }
            return EndPointID;

        }

        public void AddDefaultDataTransformationFields(int crId, int endPointId, int userId, int DefaultPaidTemplateProductId)
        {
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                //If GSPaidTemplate(59) or InternationalPaidPtemplate(64)
                if (DefaultPaidTemplateProductId == 17) 
                {
                    dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_AddDefaultDTFields_Gradschools", Db);
                }
                else if (DefaultPaidTemplateProductId == 4)
                {
                    dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_AddDefaultDTFields_International", Db);
                }

                Db.AddInParameter(dbCommand, "@CRId", DbType.Int32, crId);
                Db.AddInParameter(dbCommand, "@EndPointId", DbType.Int32, endPointId);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);
                Db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
           
        }

        public DeliveryPI GetDeliveryPostInstructions(int EndPointID)
        {
            DeliveryPI delPI = new DeliveryPI();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryPostInstructions", Db);
            Db.AddInParameter(dbcommand, "@EndPointID", DbType.Int32, EndPointID);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {

                        delPI.DeliveryEndpointId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointId"]);
                        delPI.DeliveryTypeId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryTypeId"]);
                        delPI.DeliveryName = DatabaseUtilities.SetStringValue(dataReader["DeliveryName"]);
                        delPI.EndpointDetailXML = DatabaseUtilities.SetStringValue(dataReader["EndpointDetailXML"].ToString());
                        delPI.UpdatedByName = DatabaseUtilities.SetStringValue(dataReader["UpdatedByName"]);
                        delPI.CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["CreatedDate"], true);
                        delPI.UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["UpdatedDate"], true);
                        delPI.EffectiveDate = DatabaseUtilities.SetDateTimeValue(dataReader["EffectiveDate"], false);
                        //if (DateTime.MinValue != dataReader["ExpirationDate"])
                        //{
                        delPI.ExpirationDate = DatabaseUtilities.SetDateTimeValue(dataReader["ExpirationDate"],
                                                                                  false);
                        //}
                        delPI.IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader["IsEnabled"]);
                        delPI.BatchFileXSL = DatabaseUtilities.SetStringValue(dataReader["BatchFileXSL"]);
                        delPI.DefaultPaidTemplateProductId = DatabaseUtilities.SetInt32Value(dataReader["DefaultPaidTemplateProductId"]);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return delPI;
        }
        public List<DeliverySelectItem> GetDeliveryConditionTypes(int DelDefID)
        {
            List<DeliverySelectItem> delConditionTypes = new List<DeliverySelectItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryConditionTypes", Db);
            Db.AddInParameter(dbcommand, "@DelDefID", DbType.Int32, DelDefID);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliverySelectItem delDataItem = new DeliverySelectItem(dataReader);
                        delConditionTypes.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return delConditionTypes;

        }

        public List<DeliveryEndPointItem> GetDeliveryEndPointList(int DelDefID, int DeliveryTypeID)
        {
            List<DeliveryEndPointItem> endpoints = new List<DeliveryEndPointItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetEndPointList", Db);
            Db.AddInParameter(dbcommand, "@DeliveryDefID", DbType.Int32, DelDefID);
            Db.AddInParameter(dbcommand, "@DeliveryTypeID", DbType.Int32, DeliveryTypeID);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliveryEndPointItem delDataItem = new DeliveryEndPointItem(dataReader);
                        endpoints.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return endpoints;
        }

        public List<DeliveryBlackoutItem> GetDeliveryBlackOutList(int EndPointID)
        {
            List<DeliveryBlackoutItem> blackouts = new List<DeliveryBlackoutItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryBlackOutList", Db);
            Db.AddInParameter(dbcommand, "@EndPointID", DbType.Int32, EndPointID);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliveryBlackoutItem delDataItem = new DeliveryBlackoutItem(dataReader);
                        blackouts.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return blackouts;

        }
        public List<DeliverySelectItem> GetDeliveryFrequencies()
        {
            List<DeliverySelectItem> blackoutFreqs = new List<DeliverySelectItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryFrequencies", Db);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliverySelectItem delDataItem = new DeliverySelectItem(dataReader);
                        blackoutFreqs.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return blackoutFreqs;
        }
        public DeliveryBlackoutItem GetDeliveryBlackOut(int BlackoutID)
        {
            DeliveryBlackoutItem blackout = new DeliveryBlackoutItem();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryBlackOut", Db);
            Db.AddInParameter(dbcommand, "@BlackoutID", DbType.Int32, BlackoutID);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        if (dataReader != null)
                        {
                            blackout.DeliveryBlackoutId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryBlackoutId"]);
                            blackout.DeliveryEndpointId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointId"]);
                            blackout.DeliveryBlackoutType = DatabaseUtilities.SetInt32Value(dataReader["DeliveryBlackoutType"]);
                            blackout.StartDateTime = DatabaseUtilities.SetDateTimeValue(dataReader["StartDateTime"], false);
                            blackout.EndDateTime = DatabaseUtilities.SetDateTimeValue(dataReader["EndDateTime"], false);
                            blackout.UtcHourOffset = DatabaseUtilities.SetInt32Value(dataReader["UtcHourOffset"]);
                            blackout.Frequency = DatabaseUtilities.SetStringValue(dataReader["Frequency"]);
                            blackout.BlackoutPeriodName = DatabaseUtilities.SetStringValue(dataReader["BlackoutPeriodName"]);

                            blackout.DailyEndDateTime = blackout.DeliveryBlackoutType > 1 ? Convert.ToDateTime(string.Empty + dataReader["DailyEndDateTime"].ToString()) : blackout.EndDateTime;
                            blackout.IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader["IsEnabled"]);
                            blackout.CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["CreatedDate"], false);
                            blackout.UtcHourOffset = DatabaseUtilities.SetInt32Value(dataReader["UtcHourOffset"]);
                            //blackout.CreatedBy = DatabaseUtilities.SetInt32Value(dataReader["CreatedBy"]);
                            //blackout.UpdatedBy = DatabaseUtilities.SetInt32Value(dataReader["UpdatedBy"]);
                            blackout.UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["UpdatedDate"], false);
                            blackout.UpdatedByName = DatabaseUtilities.SetStringValue(dataReader["UpdatedByName"]);
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return blackout;

        }
        public int GetCrOffset(int crId)
        {
            var timezoneOffSet = 0;
            var dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetCsrOffset", Db);
            Db.AddInParameter(dbcommand, "@CsrID", DbType.Int32, crId);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        {
                            timezoneOffSet = Convert.ToInt32(dataReader["CsrOffSet"]);
                           
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }
            return timezoneOffSet;

        }

        public int HasActiveEndpoints(int DelDefID)
         {
            int EPExist = 0;
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_HasActiveEndpoints", Db);
            Db.AddInParameter(dbcommand, "@DelDefID", DbType.Int32, DelDefID);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        if (dataReader != null)
                        {
                            EPExist = Convert.ToInt32(dataReader["EPExist"]);
                           
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }
            return EPExist;

        }
        
        public int IsDeactivatingDefinition(int DelDefID, int EndPointID)
        {
            int WillDeactivate = 0;
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_IsDeactivatingDefinition", Db);
            Db.AddInParameter(dbcommand, "@DelDefID", DbType.Int32, DelDefID);
            Db.AddInParameter(dbcommand, "@EndPointID", DbType.Int32, EndPointID);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        if (dataReader != null)
                        {
                            WillDeactivate = Convert.ToInt32(dataReader["WillDeactivate"]);

                        }


                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }
            return WillDeactivate;

        }

        public bool AddUpdateDeliveryBlackOut(DeliveryBlackoutItem blackout)
        {
            System.Data.Common.DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_AddUpdateDeliveryBlackOut", Db);
            bool blBlackout = false;
            try
            {
                DeliveryDefinition def;

                Db.AddInParameter(dbCommand, "@DeliveryBlackoutId", DbType.Int32, blackout.DeliveryBlackoutId);
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, blackout.DeliveryEndpointId);
                Db.AddInParameter(dbCommand, "@DeliveryBlackoutType", DbType.Int32, blackout.DeliveryBlackoutType);
                Db.AddInParameter(dbCommand, "@StartDateTime", DbType.DateTime, blackout.StartDateTime);
                if (blackout.EndDateTime != DateTime.MinValue)
                {
                    Db.AddInParameter(dbCommand, "@EndDateTime", DbType.DateTime, blackout.EndDateTime);
                }
                Db.AddInParameter(dbCommand, "@BlackoutPeriodName", DbType.String, blackout.BlackoutPeriodName);
                Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Int32, blackout.IsEnabled);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, blackout.UpdatedBy);
                Db.AddInParameter(dbCommand, "@RecurringBlackoutXML", DbType.String, blackout.RecurringBlackoutXML);
                Db.AddInParameter(dbCommand, "@UtcHourOffset", DbType.Int32, blackout.UtcHourOffset);

                int RecordsAffected = Db.ExecuteNonQuery(dbCommand);
                if (RecordsAffected != -1)
                {
                    blBlackout = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return blBlackout;

        }

        public List<DeliverySelectItem> GetBloackOutDateTimeOffSets()
        {
            List<DeliverySelectItem> blackoutdtoffsets = new List<DeliverySelectItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetBloackOutDateTimeOffSets", Db);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliverySelectItem delDataItem = new DeliverySelectItem(dataReader);
                        blackoutdtoffsets.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return blackoutdtoffsets;

        }
        public string GetDeliveryName(int deldefId)
        {
            string DeliveryName = string.Empty;
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryName", Db);
            Db.AddInParameter(dbcommand, "@DelDefID", DbType.Int32, deldefId);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliveryName = dataReader["DeliveryName"].ToString();

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return DeliveryName;
        }

        public List<EmailTemplate> GetEmailTemplateNames()
        {
            List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DT_GetEmailTemplates", Db);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        EmailTemplate delDataItem = new EmailTemplate(dataReader);
                        emailTemplates.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return emailTemplates;

        }
        public List<DeliverySelectItem> GetDeliveryExpressionTypes()
        {
            List<DeliverySelectItem> delExpressionTypes = new List<DeliverySelectItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryExpressionTypes", Db);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        DeliverySelectItem delDataItem = new DeliverySelectItem(dataReader);
                        delExpressionTypes.Add(delDataItem);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return delExpressionTypes;
        }

        public DeliveryDefinition GetDeliveryConditions(int CsrID, int DelDefID)
        {
            DeliveryDefinition deldef = new DeliveryDefinition();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryConditions", Db);
            Db.AddInParameter(dbcommand, "@CsrID", DbType.Int32, CsrID);
            Db.AddInParameter(dbcommand, "@DelDefID", DbType.Int32, DelDefID);

            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        deldef.ConditionXML = dataReader["ConditionXML"].ToString();
                        deldef.UpdatedByName = dataReader["UpdatedByName"].ToString();
                        deldef.UpdatedDate = (DateTime)dataReader["UpdatedDate"];
                        deldef.CreatedDate = (DateTime)dataReader["CreatedDate"];
                        deldef.DeliveryName = dataReader["DeliveryName"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return deldef;
        }

        public bool CheckDuplicatePriority(int CsrID, int Priority, int DelDefID)
        {
            bool blnDuplicate = false;
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_CheckDuplicatePriority", Db);
            Db.AddInParameter(dbcommand, "@CsrID", DbType.Int32, CsrID);
            Db.AddInParameter(dbcommand, "@Priority", DbType.Int32, Priority);
            Db.AddInParameter(dbcommand, "@DelDefID", DbType.Int32, DelDefID);

            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        blnDuplicate = (bool)dataReader["Duplicate"];

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }

            return blnDuplicate;
        }

        public int InsertDeliveryDefinition(DeliveryDefinitionItem deldef)
        {
            int DelDefID = 0;
            IDataReader dataReader = null;
            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_InsertDeliveryDefinition", Db);
            try
            {

                Db.AddInParameter(dbCommand, "@CsrID", DbType.Int32, deldef.CrID);
                Db.AddInParameter(dbCommand, "@Priority", DbType.Int32, deldef.Priority);
                Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Boolean, deldef.IsEnabled);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, deldef.UpdatedBy);
                Db.AddInParameter(dbCommand, "@DeliveryName", DbType.String, deldef.DeliveryName);
                if (!string.IsNullOrWhiteSpace(deldef.ConditionXML))
                    Db.AddInParameter(dbCommand, "@ConditionXML", DbType.String, deldef.ConditionXML);

                using (dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        DelDefID = Convert.ToInt32(dataReader["DelDefID"]);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return DelDefID;
        }

        public string DeliveryHasConditions(int DelDefID)
        {

            IDataReader dataReader = null;
            string strConditionsExist = string.Empty;
            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_DeliveryHasConditions", Db);
            try
            {

                Db.AddInParameter(dbCommand, "@DelDefID", DbType.Int32, DelDefID);
                
                using (dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        strConditionsExist = dataReader["ConditionsExist"].ToString();

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }
            return strConditionsExist;

        }

        public bool UpdateBatchStatusIfPrimaryDelivery(int leadId, int deliveryEndpointId, bool IsSuccess)
        {
            bool blnBatchStatusUpdated = false;
            System.Data.Common.DbCommand dbCommand = null;
            try
            {                              
                dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_UpdateBatchStatusIfPrimaryDelivery", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);   
                Db.AddInParameter(dbCommand, "@IsSuccess", DbType.Boolean, IsSuccess);

                //execute and update the realtime status for the batch lead delivered ONLY if the batch delivery is set as the primary delivery.
                int RecordsAffected = Db.ExecuteNonQuery(dbCommand);
                
                if (RecordsAffected != -1)
                {
                    blnBatchStatusUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return blnBatchStatusUpdated;
        }

        public bool FinalizeLeadDeliveryOnError(int leadId,  int userId)
        {
            bool bLeadStatusUpdated = false;
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_FinalizeLeadDeliveryOnError", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);
               
                int RecordsAffected = Db.ExecuteNonQuery(dbCommand);

                if (RecordsAffected != -1)
                {
                    bLeadStatusUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY_NO_RETHROW);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return bLeadStatusUpdated;
        }
        public bool GetCountAgainstCapStatus(int realtimeDeliveryStatusId)
        {
            var status = false;
            try
            {
                //DbCommand dbcommand = null;
                using (var dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetCountAgainstCapStatus", Db))
                {
                    Db.AddInParameter(dbcommand, "@RealtimeDeliveryStatusId", DbType.Int32, realtimeDeliveryStatusId);
                    status = (bool)Db.ExecuteScalar(dbcommand);
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return status;

        }

        public int RemoveRePostData(int leadId)
        {
            var recordsAffected = 0;
            try
            {
                //DbCommand dbcommand = null;
                using (var dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_RemoveRePostData", Db))
                {
                    Db.AddInParameter(dbcommand, "@LeadId", DbType.Int32, leadId);
                    recordsAffected = Db.ExecuteNonQuery(dbcommand);
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return recordsAffected;

        }

        public Dictionary<string, string> LoadGSDataForLead(int leadId)
        {

            var dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_LoadGSData", Db);
            var result = new Dictionary<string, string>();

            try
            {

                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);

                using (var dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        for (int lp = 0; lp < dataReader.FieldCount; lp++)
                        {
                            result.Add(dataReader.GetName(lp), dataReader.GetValue(lp).ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }

        public List<DeliveryLeadData> GetThirdPartyLeadsForProcessing(int returnRecordsCount, string machineName, bool isBeta)
        {
            var returnList = new List<DeliveryLeadData>();

            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetThirdPartyLeadsForProcessing", Db);
                Db.AddInParameter(dbCommand, "@ReturnRecordCount", DbType.Int32, returnRecordsCount);
                Db.AddInParameter(dbCommand, "@DeliveryEngineMachineName", DbType.String, machineName);
                Db.AddInParameter(dbCommand, "@UpdateStatusToProcessing", DbType.Boolean, true);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int16, 1);
                Db.AddInParameter(dbCommand, "@IsBeta", DbType.Boolean, isBeta);

                lock (padLock)
                {
                    dataReader = Db.ExecuteReader(dbCommand);
                }

                //TEST
                //throw new SqlNullValueException("Test Error");
                returnList = GetLeadDataListFromReader(dataReader);
            }

            catch (Exception ex)
            {
                throw;
                //ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY_NO_RETHROW);
            }
            finally
            {
                //clean up reader and command
                if (dbCommand != null)
                    if (dataReader != null) DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);

            }

            return returnList;
        }

        public List<LeadRejectionReasonItem> GetleadRejectionReasons()
        {
            List<LeadRejectionReasonItem> RejectionReasonList = new List<LeadRejectionReasonItem>();
            DbCommand dbCommand = null;
            IDataReader dataReader = null;
            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetLeadRejectionReason", Db);
                dataReader = Db.ExecuteReader(dbCommand);
                while (dataReader.Read())
                {
                    LeadRejectionReasonItem reasonItem = new LeadRejectionReasonItem(dataReader);
                    RejectionReasonList.Add(reasonItem);
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                // clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }
            return RejectionReasonList;
        }

        public List<DeliveryRejectionReasons> GetListDeliveryRejectionReasons(string endpointDetailXML)
        {
            List<DeliveryRejectionReasons> rejectionList = new List<DeliveryRejectionReasons>();
            if (endpointDetailXML != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(endpointDetailXML);
                XmlNode xmlNode = xmlDoc.SelectSingleNode("/DeliveryEndpointDetail/Fields/RejectionReasons");
                XmlNodeList RejectionReasonsXmlNodeList = null;

                if (xmlNode != null)
                    RejectionReasonsXmlNodeList = xmlNode.SelectNodes("RejectionRule");

                if (RejectionReasonsXmlNodeList != null)
                {
                    string key;
                    string value;
                    foreach (XmlNode aNode in RejectionReasonsXmlNodeList)
                    {
                        DeliveryRejectionReasons rejectionItem = new DeliveryRejectionReasons(aNode);
                        rejectionList.Add(rejectionItem);
                    }
                }
            }
            return rejectionList;
        }

        public int GetNewReviewedStatusID(int CRID, int RejectionReasonID, string ReviewTypeFlag = "Auto")
        {
            int NewReviewedStatusID = 0;
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetNewReviewedStatusID", Db);
            Db.AddInParameter(dbcommand, "@CRID", DbType.Int32, CRID);
            Db.AddInParameter(dbcommand, "@ReasonID", DbType.Int32, RejectionReasonID);
            Db.AddInParameter(dbcommand, "@ReviewTypeFlag", DbType.String, ReviewTypeFlag);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        if (dataReader != null)
                        {
                            NewReviewedStatusID = Convert.ToInt32(dataReader["NewReviewedStatusID"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }
            return NewReviewedStatusID;
        }

        public string LogNewReviewedStatusID(string LeadIdList, int RejectionReasonID, string ReviewTypeFlag, int UserId = 1)
        {
            string ReturnMsg = "";
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                EDDYLogger.LogMessage(1, LogLevel.General, "In DeliveryEngine DAO - calling SP EDDY_DE_LogNewReviewedStatusID to update the RealtimeStatusID", null, new Dictionary<string, object> { { "ModuleAction", "LogNewReviewedStatusID" }, { "ModuleName", "DeliveryEngine.DAO" } });

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_LogNewReviewedStatusID", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadIdList", DbType.String, LeadIdList);               
                Db.AddInParameter(dbCommand, "@rejectionReasonID", DbType.Int32, RejectionReasonID);                
                Db.AddInParameter(dbCommand, "@ReviewTypeFlag", DbType.String, ReviewTypeFlag);
                Db.AddInParameter(dbCommand, "@UserID", DbType.Int32, UserId); 

                var result = Db.ExecuteScalar(dbCommand);
                if (result != null)
                {
                    ReturnMsg = result.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return ReturnMsg;
        }


        bool IDeliveryEngineDAO.LogForLeadViewerHistory(int leadId, string originalStatusIDxml, string modifiedStatusIDxml)
        {
            bool success = false;
            System.Data.Common.DbCommand dbCommand = null;
            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_AddToLeadHistory", Db);
                dbCommand.CommandTimeout = 120;
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                Db.AddInParameter(dbCommand, "@OriginalLeadXmlString", DbType.String, originalStatusIDxml);
                Db.AddInParameter(dbCommand, "@ModifiedLeadXmlString", DbType.String, modifiedStatusIDxml);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, 1);
                Db.ExecuteNonQuery(dbCommand);
                success = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return success;
        }
        
        #endregion

        #region Private Methods

        private List<DeliveryLeadData> GetLeadDataListFromReader(IDataReader dataReader)
        {            
            List<DeliveryLeadData> returnList = new List<DeliveryLeadData>();
            DeliveryLeadData dld;

            while (dataReader.Read())
            {
                //Create new lead object
                dld = new DeliveryLeadData();

                //Fill Properties
                dld.LeadId = DatabaseUtilities.GetDataReaderInt(dataReader, "LeadId", 0);
                dld.SessionInternalId = DatabaseUtilities.GetDataReaderInt(dataReader, "SessionInternalId", 0);
                dld.VisitorInternalId = DatabaseUtilities.GetDataReaderInt(dataReader, "VisitorInternalId", 0);
                dld.FormUniqueId = DatabaseUtilities.GetDataReaderInt(dataReader, "FormUniqueId", 0);
                dld.CRId = DatabaseUtilities.GetDataReaderInt(dataReader, "CRId", 0);
                dld.PSIId = DatabaseUtilities.GetDataReaderInt(dataReader, "PSIId", 0);
                dld.ProgramId = DatabaseUtilities.GetDataReaderInt(dataReader, "ProgramId", 0);
                dld.CategoryId = DatabaseUtilities.GetDataReaderInt(dataReader, "CategoryId", 0);
                dld.ProductId = DatabaseUtilities.GetDataReaderInt(dataReader, "ProductId", 0);
                dld.FirstName = DatabaseUtilities.GetDataReaderString(dataReader, "FirstName", null);
                dld.LastName = DatabaseUtilities.GetDataReaderString(dataReader, "LastName", null);
                dld.MiddleName = DatabaseUtilities.GetDataReaderString(dataReader, "MiddleName", null);
                dld.Address1 = DatabaseUtilities.GetDataReaderString(dataReader, "Address1", null);
                dld.Address2 = DatabaseUtilities.GetDataReaderString(dataReader, "Address2", null);
                dld.City = DatabaseUtilities.GetDataReaderString(dataReader, "City", null);
                dld.ZipCode = DatabaseUtilities.GetDataReaderString(dataReader, "ZipCode", null);
                dld.StateProvince = DatabaseUtilities.GetDataReaderString(dataReader, "StateProvince", null);
                dld.CountryCode = DatabaseUtilities.GetDataReaderString(dataReader, "CountryCode", null);
                dld.EmailAddress = DatabaseUtilities.GetDataReaderString(dataReader, "EmailAddress", null);
                dld.Phone1 = DatabaseUtilities.GetDataReaderString(dataReader, "Phone1", null);
                dld.Phone2 = DatabaseUtilities.GetDataReaderString(dataReader, "Phone2", null);
                dld.TimeToStartInWeeks = DatabaseUtilities.GetDataReaderInt(dataReader, "TimeToStartInWeeks", 0);
                dld.RawPostDataId = DatabaseUtilities.GetDataReaderInt64(dataReader, "RawPostDataId", 0);
                dld.TransactionId = DatabaseUtilities.GetDataReaderGuid(dataReader, "TransactionId").ToString();

                //New For Cap Processing
                dld.ApplicationId = DatabaseUtilities.GetDataReaderInt(dataReader, "ApplicationId", 0);
                dld.ChannelId = DatabaseUtilities.GetDataReaderInt(dataReader, "ChannelId", 0);
                dld.VendorId = DatabaseUtilities.GetDataReaderInt(dataReader, "VendorId", 0);
                dld.BusinessModelId = DatabaseUtilities.GetDataReaderInt(dataReader, "BusinessModelId", 0);

                dld.RealTimeDeliveryStatusId = DatabaseUtilities.GetDataReaderInt(dataReader, "RealTimeDeliveryStatusId", 0);
                dld.IsBeta = DatabaseUtilities.GetDataReaderBool(dataReader, "IsBeta", false);
                //newly added fields
                dld.Age = DatabaseUtilities.GetDataReaderInt(dataReader, "Age", 0);
                dld.Prefix = DatabaseUtilities.GetDataReaderString(dataReader, "Prefix", null);
                dld.YearHighestEduCompleted = DatabaseUtilities.GetDataReaderInt(dataReader, "YearHighestEduCompleted", 0);
                dld.HighestLevelOfEdu = DatabaseUtilities.GetDataReaderString(dataReader, "HighestLevelOfEdu", null);
                dld.Military = DatabaseUtilities.GetDataReaderString(dataReader, "Military", null);
                dld.MethodOfContact = DatabaseUtilities.GetDataReaderString(dataReader, "MethodOfContact", null);
                dld.StartDate = DatabaseUtilities.GetDataReaderString(dataReader, "StartDate", null);
                dld.LegacyLeadId = DatabaseUtilities.GetDataReaderString(dataReader, "LegacyLeadId", null);
                dld.LeadCreationTypeId = DatabaseUtilities.GetDataReaderInt(dataReader, "LeadCreationTypeId", 0);
                dld.TrackId = DatabaseUtilities.GetDataReaderGuid(dataReader, "TrackId").ToString();

                dld.CampusId = DatabaseUtilities.GetDataReaderInt(dataReader, "CampusId", 0);
                dld.CampusValue = DatabaseUtilities.GetDataReaderString(dataReader, "CampusValue", null);

                //Add Additional fields from XML to lead
                string xml = DatabaseUtilities.GetDataReaderString(dataReader, "AdditionalFields", null);
                dld.AdditionalFields = LeadDataService.LeadDAO.GetAdditionalFieldsDictionaryFromXML(xml);

                dld.IsRepost = dataReader.IsColumnExists("IsRepost") ? DatabaseUtilities.GetDataReaderBool(dataReader, "IsRepost", false) : false;

                //Append To return List
                returnList.Add(dld);
            }

            dataReader.Close();

            return returnList;
        }

        private DeliveryEndpoint GetDefinitionEndpoint(IDataReader dataReader)
        {
            DeliveryEndpoint endpointToReturn = null;
            string endpointDetailXML;
            int deliveryTypeId;
            Dictionary<string, string> endPointDetailDictionary;
            //string xsl;

            try
            {
                //get typeId to handle for each type of DeliveryEndpoint
                deliveryTypeId = Convert.ToInt32(dataReader["DeliveryTypeId"]);

                //get endpintDetailXML
                endpointDetailXML = GetDBString(dataReader, "EndpointDetailXML", null);

                //convert XML to Dictionary
                endPointDetailDictionary = GetDictionaryFromEndpointDetailXML(endpointDetailXML);

                //check type of endpoint
                switch (deliveryTypeId)
                {
                    case 1:
                        {
                            endpointToReturn = new InstantPostEndpoint();                            
                            endpointToReturn.DeliveryRejectionReasons = GetDeliveryRejectionReasons(endpointDetailXML);
                            AddInstantPostEndpointProperties((InstantPostEndpoint)endpointToReturn, endPointDetailDictionary);
                            break;
                        }
                    case 2:
                        {
                            endpointToReturn = new InstantEmailEndpoint();
                            AddInstantEmailEndpointProperties((InstantEmailEndpoint)endpointToReturn, endPointDetailDictionary);
                            break;
                        }
                    case 3:
                        {
                            endpointToReturn = new BatchEmailEndpoint();
                            AddBatchEmailEndpointProperties((BatchEmailEndpoint)endpointToReturn, endPointDetailDictionary);
                            ((BatchEmailEndpoint)endpointToReturn).BatchFileDefinition.XSL = GetDBString(dataReader, "BatchFileXSL", null);
                            break;
                        }
                    case 4:
                    default:
                        {
                            endpointToReturn = new BatchFtpEndpoint();
                            AddBatchFTPEndpointProperties((BatchFtpEndpoint)endpointToReturn, endPointDetailDictionary);
                            ((BatchFtpEndpoint)endpointToReturn).BatchFileDefinition.XSL = GetDBString(dataReader, "BatchFileXSL", null);
                            break;
                        }
                }

                //add common endpoint fields
                endpointToReturn.EndPointDetailDictionary = endPointDetailDictionary;
                endpointToReturn.EndpointDetailXML = endpointDetailXML;
                endpointToReturn.DeliveryDefinitionId = GetDBInt(dataReader, "DeliveryDefinitionId", 0);
                endpointToReturn.CSRId = GetDBInt(dataReader, "CSRId", 0);
                endpointToReturn.DataTransformationId = GetDBInt(dataReader, "DataTransformationId", 0);
                endpointToReturn.DeliveryEndpointId = GetDBInt(dataReader, "DeliveryEndpointId", 0);
                endpointToReturn.EndpointName = GetDBString(dataReader, "EndpointName", null);
                endpointToReturn.DeliveryTypeId = deliveryTypeId;
                endpointToReturn.BlackoutPeriods = GetEndPointBlackouts(endpointToReturn.DeliveryEndpointId).ToList();
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            //We are not done with the reader, should not close. RK
            //finally
            //{
            //    DatabaseUtilities.CloseAndDispose(ref dataReader);
            //}

            return endpointToReturn;
        }

        private IEnumerable<DeliveryBlackoutPeriod> GetEndPointBlackouts(int deliveryEndpointId)
        {
            IDataReader dataReader = null;
            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetEndpointBlackouts", Db);

            Db.AddInParameter(dbCommand, "@DeliveryEndpointId", DbType.Int32, deliveryEndpointId);
            using (dataReader = Db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    yield return GetDeliveryBlackOutPeriodFromReader(dataReader);
                }
            }
        }

        private DeliveryBlackoutPeriod GetDeliveryBlackOutPeriodFromReader(IDataReader dataReader)
        {
            int blackoutTypeId = GetDBInt(dataReader, "DeliveryBlackoutType", 1);
            string blackoutXml = GetDBString(dataReader, "RecurringBlackoutXML", "");
            DeliveryBlackoutPeriod blackoutPeriod;

            switch (blackoutTypeId)
            {
                case 1:
                    {
                        blackoutPeriod = new OnetimeDeliveryBlackoutPeriod();
                        break;
                    }
                case 2:
                    {
                        blackoutPeriod = new DailyDeliveryBlackoutPeriod(blackoutXml);
                        break;
                    }
                case 3:
                    {
                        blackoutPeriod = new WeeklyDeliveryBlackoutPeriod(blackoutXml);
                        break;
                    }
                default:
                    {
                        blackoutPeriod =
                            new MonthlyDeliveryBlackoutPeriod(blackoutXml);
                        break;
                    }

            }
            blackoutPeriod.AbsloluteStartDateTime =
                GetDBDateTime(dataReader, "StartDateTime", DateTime.MinValue);
            blackoutPeriod.AbsloluteEndDateTime =
                GetDBDateTime(dataReader, "EndDateTime", DateTime.MaxValue);
            blackoutPeriod.UTCHourOffset = GetDBInt(dataReader, "UtcHourOffset", 0);

            //dataReader.Close();
            return blackoutPeriod;
        }

        private List<DeliveryEndpoint> GetDefinitionEndpoints(DeliveryDefinition deliveryDefinition)
        {
            return GetDefinitionEndpoints(deliveryDefinition.DeliveryDefinitionId);
        }

        private List<DeliveryEndpoint> GetDefinitionEndpoints(int deliveryDefinitionId)
        {
            List<DeliveryEndpoint> returnEndpointList = new List<DeliveryEndpoint>();
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                //get endpoint list from DB
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDefinitionEndpoints", Db);
                Db.AddInParameter(dbCommand, "@DeliveryDefinitionId", DbType.Int32, deliveryDefinitionId);
                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    //create each endpoint
                    while (dataReader.Read())
                    {
                        returnEndpointList.Add(GetDefinitionEndpoint(dataReader));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnEndpointList;
        }

        private string GetEmailEndPointTemplate(int endPointId)
        {

            string returnValue = String.Empty;
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetEmailTemplate", Db);
                Db.AddInParameter(dbCommand, "@EmailTemplateId", DbType.Int32, endPointId);
                dataReader = Db.ExecuteReader(dbCommand);
                using (dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    { 
                        returnValue = dataReader["TemplateContent"].ToString();

                    }
                }
            }


            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return returnValue;
        }
        
        private Dictionary<string, string> GetDictionaryFromEndpointDetailXML(string endpointDetailXML)
        {
            return EDDY.Nexus.Common.Utilities.XmlUtilities.GetDictionaryFromFieldNameValueXML(endpointDetailXML, "DeliveryEndpointDetail");            
        }

        private void AddInstantPostEndpointProperties(InstantPostEndpoint endpoint, Dictionary<string, string> endPointDetailDictionary)
        {
            try
            {
                endpoint.LivePostUrl = GetDictionaryString(endPointDetailDictionary, "LivePostUrl", null);
                endpoint.TestPostUrl = GetDictionaryString(endPointDetailDictionary, "TestPostUrl", null);
                endpoint.PostMethod = DeliveryUtility.GetPostMethodByName(GetDictionaryString(endPointDetailDictionary, "PostMethod", null));
                endpoint.PostContentType = DeliveryUtility.GetPostContentTypeByName(GetDictionaryString(endPointDetailDictionary, "PostContentType", null));
                endpoint.SuccessResponseString = GetDictionaryString(endPointDetailDictionary, "SuccessResponseString", null);
                endpoint.FailureResponseString = GetDictionaryString(endPointDetailDictionary, "FailureResponseString", null);
                endpoint.DuplicateResponseString = GetDictionaryString(endPointDetailDictionary, "DuplicateResponseString", null);
                endpoint.IsTest = GetDictionaryBool(endPointDetailDictionary, "IsTestURL", false);
                endpoint.MaxRetryAttempts = GetDictionaryInt(endPointDetailDictionary, "MaxRetryAttempts", DefaultMaxRetryAttempts);
                endpoint.RetryDelayInHours = GetDictionaryInt(endPointDetailDictionary, "RetryDelayInHours", DefaultRetryDelay);
                endpoint.ForceRepostResponseString = GetDictionaryString(endPointDetailDictionary, "ForceRespostString", null);
                endpoint.SoapAction = GetDictionaryString(endPointDetailDictionary, "SoapAction", null);
                endpoint.IsPrimary = GetDictionaryBool(endPointDetailDictionary, "IsPrimary", true);
                endpoint.IsScrub = GetDictionaryBool(endPointDetailDictionary, "IsInternalDelivery", false);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
        }

        private void AddInstantEmailEndpointProperties(InstantEmailEndpoint endpoint, Dictionary<string, string> endPointDetailDictionary)
        {
            try
            {
                endpoint.EmailSubject = GetDictionaryString(endPointDetailDictionary, "EmailSubject", null);
                endpoint.LiveEmailTo = GetDictionaryString(endPointDetailDictionary, "LiveEmailTo", null);
                endpoint.LiveEmailCC = GetDictionaryString(endPointDetailDictionary, "LiveEmailCC", null);
                endpoint.LiveEmailBCC = GetDictionaryString(endPointDetailDictionary, "LiveEmailBCC", null);
                endpoint.TestEmailTo = GetDictionaryString(endPointDetailDictionary, "TestEmailTo", null);
                endpoint.TestEmailCC = GetDictionaryString(endPointDetailDictionary, "TestEmailCC", null);
                endpoint.TestEmailBCC = GetDictionaryString(endPointDetailDictionary, "TestEmailBCC", null);
                endpoint.IsTest = GetDictionaryBool(endPointDetailDictionary, "IsTestEmail", false);
                string bodyTypeString = GetDictionaryString(endPointDetailDictionary, "BodyType", "Text");
                if (bodyTypeString.ToUpper().Trim() == "HTML")
                {
                    endpoint.BodyType = EmailBodyType.HTML;
                }
                else
                {
                    endpoint.BodyType = EmailBodyType.Text;
                }

                endpoint.FieldDelimiter = GetDictionaryString(endPointDetailDictionary, "FieldDelimiter", null);
                endpoint.LineDelimiter = GetDictionaryString(endPointDetailDictionary, "LineDelimiter", null);
                endpoint.BodyHeader = GetDictionaryString(endPointDetailDictionary, "BodyHeader", null);
                endpoint.BodyFooter = GetDictionaryString(endPointDetailDictionary, "BodyFooter", null);
                endpoint.IsEmailTemplate = (GetDictionaryInt(endPointDetailDictionary, "IsEmailTemplate", 0) == 1);

                if (endpoint.IsEmailTemplate)
                {
                    int emailTemplateId = GetDictionaryInt(endPointDetailDictionary, "EmailTemplateId", 0);
                    endpoint.EmailTemplate = GetEmailEndPointTemplate(emailTemplateId);
                }
                endpoint.IsPrimary = GetDictionaryBool(endPointDetailDictionary, "IsPrimary", true);
                endpoint.IsScrub = GetDictionaryBool(endPointDetailDictionary, "IsInternalDelivery", false);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
        }

        private void AddBatchEmailEndpointProperties(BatchEmailEndpoint endpoint, Dictionary<string, string> endPointDetailDictionary)
        {
            try
            {
                endpoint.EmailSubject = GetDictionaryString(endPointDetailDictionary, "EmailSubject", null);
                endpoint.EmailBody = GetDictionaryString(endPointDetailDictionary, "EmailBody", null);
                endpoint.LiveEmailTo = GetDictionaryString(endPointDetailDictionary, "LiveEmailTo", null);
                endpoint.LiveEmailCC = GetDictionaryString(endPointDetailDictionary, "LiveEmailCC", null);
                endpoint.LiveEmailBCC = GetDictionaryString(endPointDetailDictionary, "LiveEmailBCC", null);
                endpoint.TestEmailTo = GetDictionaryString(endPointDetailDictionary, "TestEmailTo", null);
                endpoint.TestEmailCC = GetDictionaryString(endPointDetailDictionary, "TestEmailCC", null);
                endpoint.TestEmailBCC = GetDictionaryString(endPointDetailDictionary, "TestEmailBCC", null);
                endpoint.IsTest = GetDictionaryBool(endPointDetailDictionary, "IsTestEmail", false);
                string bodyTypeString = GetDictionaryString(endPointDetailDictionary, "BodyType", "Text");
                if (bodyTypeString.ToUpper().Trim() == "HTML")
                {
                    endpoint.BodyType = EmailBodyType.HTML;
                }
                else
                {
                    endpoint.BodyType = EmailBodyType.Text;
                }

                endpoint.BatchFileDefinition = GetBatchFileDefinition(endPointDetailDictionary);
                endpoint.TimeToDeliver = GetDictionaryString(endPointDetailDictionary, "TimeToDeliver", null);
                endpoint.DaysToDeliver = GetDaysToDeliver(endPointDetailDictionary, "DaysToDeliver");
                endpoint.IsPrimary = GetDictionaryBool(endPointDetailDictionary, "IsPrimary", true);
                endpoint.IsScrub = GetDictionaryBool(endPointDetailDictionary, "IsInternalDelivery", false);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
        }

        private void AddBatchFTPEndpointProperties(BatchFtpEndpoint endpoint, Dictionary<string, string> endPointDetailDictionary)
        {
            try
            {
                endpoint.FileProtocol = GetDictionaryString(endPointDetailDictionary, "FileProtocol", "FTP");
                endpoint.ActiveMode = Convert.ToBoolean(GetDictionaryString(endPointDetailDictionary, "ActiveMode", null));
                endpoint.URI = GetDictionaryString(endPointDetailDictionary, "URI", null);
                endpoint.Port = Convert.ToInt16(GetDictionaryString(endPointDetailDictionary, "Port", null));
                endpoint.UserName = GetDictionaryString(endPointDetailDictionary, "UserName", null);
                endpoint.Password = GetDictionaryString(endPointDetailDictionary, "Password", null);
                endpoint.RemoteFolder = GetDictionaryString(endPointDetailDictionary, "RemoteFolder", null);
                endpoint.HostKey = GetDictionaryString(endPointDetailDictionary, "HostKey", null);
                endpoint.ActiveMode = Convert.ToBoolean(GetDictionaryString(endPointDetailDictionary, "ActiveMode", null));
                endpoint.IsTest = GetDictionaryBool(endPointDetailDictionary, "IsTestFtp", false);
                endpoint.BatchFileDefinition = GetBatchFileDefinition(endPointDetailDictionary);
                endpoint.TimeToDeliver = GetDictionaryString(endPointDetailDictionary, "TimeToDeliver", null);
                endpoint.DaysToDeliver = GetDaysToDeliver(endPointDetailDictionary, "DaysToDeliver");
                endpoint.TestURL = GetDictionaryString(endPointDetailDictionary, "TestURL", null);
                endpoint.IsPrimary = GetDictionaryBool(endPointDetailDictionary, "IsPrimary", true);
                endpoint.IsScrub = GetDictionaryBool(endPointDetailDictionary, "IsInternalDelivery", false);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
        }

        private BatchFileDefinition GetBatchFileDefinition(Dictionary<string, string> endPointDetailDictionary)
        {
            BatchFileDefinition returnBatchFileDefinition = new BatchFileDefinition();

            try
            {
                returnBatchFileDefinition.BatchFileTypeString = GetDictionaryString(endPointDetailDictionary, "BatchFileType", null);
                returnBatchFileDefinition.FileExtension = GetDictionaryString(endPointDetailDictionary, "FileExtension", null);
                returnBatchFileDefinition.FileNameRegExp = GetDictionaryString(endPointDetailDictionary, "FileNameRegExp", null);
                returnBatchFileDefinition.FieldDelimiter = GetDictionaryString(endPointDetailDictionary, "FieldDelimiter", null);
                returnBatchFileDefinition.RowDelimiter = GetDictionaryString(endPointDetailDictionary, "RowDelimiter", null);
                returnBatchFileDefinition.TextQualifier = GetDictionaryString(endPointDetailDictionary, "TextQualifier", null);
                returnBatchFileDefinition.ShowHeading = Convert.ToBoolean(GetDictionaryString(endPointDetailDictionary, "ShowHeading", null));
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

            return returnBatchFileDefinition;
        }

        private string GetDBString(IDataReader dataReader, string key, string defaultValue)
        {
            if (dataReader[key] != DBNull.Value)
            {
                return (string)dataReader[key];
            }
            else
            {
                return defaultValue;
            }
        }

        private int GetDBInt(IDataReader dataReader, string key, int defaultValue)
        {
            if (dataReader[key] != DBNull.Value)
            {
                return Convert.ToInt32(dataReader[key]);
            }
            else
            {
                return defaultValue;
            }
        }

        private Int64 GetDBBigInt(IDataReader dataReader, string key, int defaultValue)
        {
            if (dataReader[key] != DBNull.Value)
            {
                return Convert.ToInt64(dataReader[key]);
            }
            else
            {
                return defaultValue;
            }
        }
        
        private DateTime GetDBDateTime(IDataReader dataReader, string key, DateTime defaultValue)
        {
            if (dataReader[key] != DBNull.Value)
            {
                return Convert.ToDateTime(dataReader[key]);
            }
            else
            {
                return defaultValue;
            }
        }

        private bool GetDBBool(IDataReader dataReader, string key, bool defaultValue)
        {
            if (dataReader[key] != DBNull.Value)
            {
                return Convert.ToBoolean(dataReader[key]);
            }
            else
            {
                return defaultValue;
            }
        }

        private int GetDictionaryInt(Dictionary<string, string> dictionary, string key, int defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                string stringValue = dictionary[key];
                return Convert.ToInt32(stringValue);
            }
            else
            {
                return defaultValue;
            }
        }

        private string GetDictionaryString(Dictionary<string, string> dictionary, string key, string defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                return (string)dictionary[key];
            }
            else
            {
                return defaultValue;
            }
        }

        private bool GetDictionaryBool(Dictionary<string, string> dictionary, string key, bool defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                bool result;
                bool.TryParse(dictionary[key], out result);
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
        private string[] GetDaysToDeliver(Dictionary<string, string> dictionary, string key)
        {
            if (dictionary.ContainsKey(key) && !string.IsNullOrWhiteSpace(dictionary[key]))
            {
                string[] daysArr = dictionary[key].Split(',');
                return daysArr;
            }
            else
            {
                return new string[] {};
            }

        }

        private List<DeliveryRejectionReasons> GetDeliveryRejectionReasons(string endpointDetailXML)
        {
            return GetListDeliveryRejectionReasons(endpointDetailXML);
        }



        #endregion


        #region Lead Viewer

        public List<LeadViewerResultEntity> GetLeadViewerSearchResult(LeadViewerSearchParam lvsearch)
        {
            List<LeadViewerResultEntity> leadsforViewer = new List<LeadViewerResultEntity>();
            string SPName = lvsearch.IsBeta
                                ? "dbo.EDDY_DE_GetBETALeadViewerSearchResultsDynamic"
                                : "dbo.EDDY_DE_GetLeadViewerSearchResultsDynamic";
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand(SPName, Db);
            dbcommand.CommandTimeout = 1000;
            Db.AddInParameter(dbcommand, "@CRID", DbType.Int32, lvsearch.CRID);
            Db.AddInParameter(dbcommand, "@CRIDType", DbType.String, lvsearch.CRIDType);
            Db.AddInParameter(dbcommand, "@ApplicationId", DbType.Int32, lvsearch.ApplicationId);
            Db.AddInParameter(dbcommand, "@DeliveryTypeId", DbType.Int32, lvsearch.DeliveryTypeId);
            Db.AddInParameter(dbcommand, "@StartPosition", DbType.Int32, lvsearch.StartPosition);
            Db.AddInParameter(dbcommand, "@EndPosition", DbType.Int32, lvsearch.EndPosition);
            Db.AddInParameter(dbcommand, "@SortField", DbType.String, lvsearch.SortField);
            Db.AddInParameter(dbcommand, "@FirstNameMatchTypeStartsWith", DbType.String, lvsearch.FirstNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@FirstNameMatchTypeContains", DbType.String, lvsearch.FirstNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@LastNameMatchTypeStartsWith", DbType.String, lvsearch.LastNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@LastNameMatchTypeContains", DbType.String, lvsearch.LastNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@PsiNameMatchTypeStartsWith", DbType.String, lvsearch.PsiNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@PsiNameMatchTypeContains", DbType.String, lvsearch.PsiNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@ProgramID", DbType.Int32, Convert.ToInt32(lvsearch.ProgramID));
            Db.AddInParameter(dbcommand, "@DeliveryURL", DbType.String, lvsearch.DeliveryURL);
            Db.AddInParameter(dbcommand, "@StartDate", DbType.DateTime, lvsearch.StartDateWithTime);
            Db.AddInParameter(dbcommand, "@EndDate", DbType.DateTime, lvsearch.EndDateWithTime);
            Db.AddInParameter(dbcommand, "@ProductId", DbType.Int32, lvsearch.ProductId);
            Db.AddInParameter(dbcommand, "@SubChannelId", DbType.Int32, lvsearch.SubChannelID);
            Db.AddInParameter(dbcommand, "@VendorId", DbType.Int32, lvsearch.VendorID);
            Db.AddInParameter(dbcommand, "@LeadID", DbType.String, lvsearch.LeadID);
            Db.AddInParameter(dbcommand, "@MultipleEmails", DbType.String, lvsearch.Email);
            Db.AddInParameter(dbcommand, "@TrackID", DbType.String, lvsearch.TrackID);
            Db.AddInParameter(dbcommand, "@MultipleLeadStatuses", DbType.String, lvsearch.SelectedStatuses);
            Db.AddInParameter(dbcommand, "@AccountManagerId", DbType.Int32, lvsearch.AccountManagerId);
            Db.AddInParameter(dbcommand, "@DeliveryStatusType", DbType.Int32, lvsearch.DeliveryStatusTypeId);
            Db.AddInParameter(dbcommand, "@LeadPaidStatusTypeId", DbType.Int32, lvsearch.LeadPaidStatusTypeId);
            try
            {
                DataSet dsLVResults = new DataSet();
                DataTable dtDeliveryRecord = new DataTable();
                DataTable dtDeliveryLogrecord = new DataTable();
                Int32 iLeadId;
                Int32 iDeliveryTypeId;

                //Call sql scrit
                dsLVResults = Db.ExecuteDataSet(dbcommand);

                if (dsLVResults.Tables.Count > 0)
                {
                    dtDeliveryRecord = dsLVResults.Tables[0];

                    if (dtDeliveryRecord.Rows.Count > 0)
                    {

                        foreach (DataRow row in dtDeliveryRecord.Rows)
                        {
                            iLeadId = DatabaseUtilities.SetInt32Value(row["LeadId"]);
                            iDeliveryTypeId = DatabaseUtilities.SetInt32Value(row["DeliveryTypeId"]);

                            LeadViewerResultEntity leadItem = new LeadViewerResultEntity(row);
                            leadsforViewer.Add(leadItem);


                            if (dsLVResults.Tables.Count == 2)
                            {
                                dtDeliveryLogrecord = dsLVResults.Tables[1];

                                if (dtDeliveryLogrecord.Rows.Count > 0)
                                {
                                    EnumerableRowCollection<DataRow> query = from DeliveryLogrecord in dtDeliveryLogrecord.AsEnumerable()
                                                                             where DeliveryLogrecord.Field<Int32>("LeadId") == iLeadId && DeliveryLogrecord.Field<Int32>("DeliveryTypeId") == iDeliveryTypeId
                                                                             select DeliveryLogrecord;
                                    DataView view = query.AsDataView();
                                    foreach (DataRowView drv in view)
                                    {
                                        if (leadItem != null && leadItem.realtimelogs != null && leadItem.realtimelogs.Count() > 0)
                                        {
                                            leadItem.realtimelogs.Add(new LVRealtimeLogEntity(drv));
                                        }
                                        else
                                        {
                                            leadItem.realtimelogs = new List<LVRealtimeLogEntity>();
                                            leadItem.realtimelogs.Add(new LVRealtimeLogEntity(drv));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand);

            }
            return leadsforViewer;
        }

        /// <summary>
        /// Get RealtimeDeliveryStaus List
        /// </summary>
        /// <returns></returns>
        public List<RealtimeDeliveryStatus> GetRealtimeDeliveryStatusList(int DeliveryStatusTypeId = 0)
        {
            List<RealtimeDeliveryStatus> realtimeDeliveryStatusList = new List<RealtimeDeliveryStatus>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetLeadDeliveryStatus", Db);
            Db.AddInParameter(dbcommand, "@DeliveryStatusTypeId", DbType.Int16, DeliveryStatusTypeId);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        RealtimeDeliveryStatus leadStatus = new RealtimeDeliveryStatus(dataReader);
                        realtimeDeliveryStatusList.Add(leadStatus);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);

            }
            return realtimeDeliveryStatusList;
        }

        /// <summary>
        /// Get Lead Data by LeadId
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public LeadEntity GetLeadDataByLeadId(int leadId)
        {
            LeadEntity leadEntity = new LeadEntity();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetLeadDataByLeadId", Db);
            Db.AddInParameter(dbcommand, "@LeadID", DbType.Int32, leadId);

            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                         leadEntity = new LeadEntity(dataReader,true);
                    }
                }
                                
                leadEntity.AdditionalLeadDataList = GetAdditionalLeadDataByLeadId(leadId,leadEntity.BetaLead);
                
                
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);

            }
            return leadEntity;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>       
        public string UpdateLeadData(LeadEntity leadEntity, int userid)
        {
            DbCommand dbCommand = null;            
            string ReturnMsg = "";
            try
            {
                string SPName = "dbo.EDDY_DE_UpdateLeadData";
                if (leadEntity.BetaLead == "true")
                {
                    SPName = "dbo.EDDY_DE_UpdateBetaLeadData";
                }                  
                dbCommand = DatabaseUtilities.CreateDbCommand(SPName, Db);
                Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadEntity.LeadId);
                Db.AddInParameter(dbCommand, "@FirstName", DbType.String, leadEntity.FirstName);
                Db.AddInParameter(dbCommand, "@MiddleName", DbType.String, leadEntity.MiddleName);
                Db.AddInParameter(dbCommand, "@LastName", DbType.String, leadEntity.LastName);
                Db.AddInParameter(dbCommand, "@Address1", DbType.String, leadEntity.Address1);
                Db.AddInParameter(dbCommand, "@Address2", DbType.String, leadEntity.Address2);
                Db.AddInParameter(dbCommand, "@City", DbType.String, leadEntity.City);
                Db.AddInParameter(dbCommand, "@ZipCode", DbType.String, leadEntity.ZipCode);
                Db.AddInParameter(dbCommand, "@StateProvince", DbType.String, leadEntity.StateProvince);
                Db.AddInParameter(dbCommand, "@CountryCode", DbType.String, leadEntity.CountryCode);
                Db.AddInParameter(dbCommand, "@EmailAddress", DbType.String, leadEntity.EmailAddress);
                Db.AddInParameter(dbCommand, "@Phone1", DbType.String, leadEntity.Phone1);
                Db.AddInParameter(dbCommand, "@Phone2", DbType.String, leadEntity.Phone2);
                Db.AddInParameter(dbCommand, "@TimeToStartInWeeks", DbType.Int32, leadEntity.TimeToStartInWeeks);                
                Db.AddInParameter(dbCommand, "@RealtimeDeliveryStatusId", DbType.Int32, leadEntity.RealTimeDeliveryStatusId);
                Db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userid);               
                Db.AddInParameter(dbCommand, "@Prefix", DbType.String, leadEntity.Prefix);
                Db.AddInParameter(dbCommand, "@Age", DbType.Int32, leadEntity.Age);
                Db.AddInParameter(dbCommand, "@YearHighestEduCompleted", DbType.Int32, leadEntity.YearHighestEduCompleted);
                Db.AddInParameter(dbCommand, "@HighestLevelOfEdu", DbType.String, leadEntity.EduLevelId.ToString());
                Db.AddInParameter(dbCommand, "@Military", DbType.String, leadEntity.MilitaryStatusId.ToString());
                Db.AddInParameter(dbCommand, "@MethodOfContact", DbType.String, leadEntity.MethodOfContact);
                Db.AddInParameter(dbCommand, "@StartDate", DbType.String, leadEntity.StartDate);
                Db.AddInParameter(dbCommand, "@TrackId", DbType.String, leadEntity.TrackId.ToString());
                Db.AddInParameter(dbCommand, "@CampusId", DbType.Int32, leadEntity.CampusId);
                Db.AddInParameter(dbCommand, "@ProductId", DbType.Int32, leadEntity.ProductId);
                Db.AddInParameter(dbCommand, "@ProgramId", DbType.Int32, leadEntity.ProgramId);
                Db.AddInParameter(dbCommand, "@CRID", DbType.Int32, leadEntity.CRId);
                Db.AddInParameter(dbCommand, "@PSIID", DbType.Int32, leadEntity.PSIId);
                Db.AddInParameter(dbCommand, "@LeadEditNoteText", DbType.String, leadEntity.LeadEditNoteText);
                Db.AddInParameter(dbCommand, "@BillingDate", DbType.DateTime, leadEntity.BillingDate);
               
                var result = Db.ExecuteScalar(dbCommand);
                if (result != null)
                {
                    ReturnMsg = result.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }            
            return ReturnMsg;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public List<AdditionalLeadData> GetAdditionalLeadDataByLeadId(int leadId, string BetaLead="false")
        {
            string SPName = "dbo.EDDY_DE_GetAdditionalLeadDataByLeadId";
            if (BetaLead == "true"){ 
                SPName = "dbo.EDDY_DE_GetAdditionalBetaLeadDataByLeadId";
            }            
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand(SPName, Db);
            List<AdditionalLeadData> additionalLeadDataList = new List<AdditionalLeadData>();            
            Db.AddInParameter(dbcommand, "@LeadID", DbType.Int32, leadId);

            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        AdditionalLeadData additionalLeadData = new AdditionalLeadData(dataReader);
                        additionalLeadDataList.Add(additionalLeadData);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);

            }
            return additionalLeadDataList;
        }

        
        public List<LeadViewerHistoryItem> GetleadViewerHistory(int LeadID)
        {
            List<LeadViewerHistoryItem> leadViewerHistoryEntity = new List<LeadViewerHistoryItem>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetLeadViewerHistory", Db);
            Db.AddInParameter(dbcommand, "@LeadID", DbType.Int32, LeadID);

            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        leadViewerHistoryEntity.Add(new LeadViewerHistoryItem(dataReader));

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);

            }
            return leadViewerHistoryEntity;

        }
       
        public bool CheckCRhasDeliveryDefinitions(int crId)
        {    
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetCSRDeliveryDefinitions", Db);
            Db.AddInParameter(dbcommand, "CSRId", DbType.Int32, crId);
            bool HasDeliveryDef = false;
            IDataReader dataReader = null;           
            try  
            {                
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        HasDeliveryDef = Convert.ToBoolean(Convert.ToInt16(dataReader[0]));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }
            return HasDeliveryDef;
        }
       
        public string UpdateSelectedLeadsforRepost(string LeadsIDs, int UserID, string isBetaLead) 
        {

            string SPName = "dbo.EDDY_DE_UpdateSelectedLeadsforRepost";
            if (isBetaLead == "true")
            {
                SPName = "dbo.EDDY_DE_UpdateSelectedBetaLeadsforRepost";
            }
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand(SPName, Db);
            Db.AddInParameter(dbcommand, "@LeadIDs", DbType.String, LeadsIDs);
            Db.AddInParameter(dbcommand, "@UserID", DbType.String, UserID);
            string ReturnMsg = ""; 
            IDataReader dataReader = null;
            try
            {
                var result = Db.ExecuteScalar(dbcommand);
                if (result != null)
                {
                    ReturnMsg = result.ToString();
                }      
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbcommand);
            }
            return ReturnMsg;
        }

        public List<BatchLeadViewerEntity> GetLeadViewerSearchResultForBatch(LeadViewerSearchParam lvsearch)
        {
            List<BatchLeadViewerEntity> leadsforViewer = new List<BatchLeadViewerEntity>();
            string SPName = lvsearch.IsBeta
                                ? "dbo.EDDY_DE_GetBetaLeadViewerSearchResults"
                                : "dbo.EDDY_DE_GetLeadViewerSearchResultsForBatch";
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand(SPName, Db);
            dbcommand.CommandTimeout = 1000;
            Db.AddInParameter(dbcommand, "@CsrID", DbType.Int32, lvsearch.CRID);
            Db.AddInParameter(dbcommand, "@ApplicationId", DbType.Int32, lvsearch.ApplicationId);
            Db.AddInParameter(dbcommand, "@DeliveryTypeId", DbType.Int32, lvsearch.DeliveryTypeId);
            Db.AddInParameter(dbcommand, "@LeadStatusId", DbType.String, lvsearch.SelectedStatuses);
            Db.AddInParameter(dbcommand, "@StartPosition", DbType.Int32, lvsearch.StartPosition);
            Db.AddInParameter(dbcommand, "@EndPosition", DbType.Int32, lvsearch.EndPosition);
            Db.AddInParameter(dbcommand, "@SortField", DbType.String, lvsearch.SortField);
            Db.AddInParameter(dbcommand, "@FirstNameMatchTypeStartsWith", DbType.String, lvsearch.FirstNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@FirstNameMatchTypeContains", DbType.String, lvsearch.FirstNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@LastNameMatchTypeStartsWith", DbType.String, lvsearch.LastNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@LastNameMatchTypeContains", DbType.String, lvsearch.LastNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@PsiNameMatchTypeStartsWith", DbType.String, lvsearch.PsiNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@PsiNameMatchTypeContains", DbType.String, lvsearch.PsiNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@Email", DbType.String, lvsearch.Email);
            Db.AddInParameter(dbcommand, "@ProgramID", DbType.Int32, Convert.ToInt32(lvsearch.ProgramID));
            Db.AddInParameter(dbcommand, "@DeliveryURL", DbType.String, lvsearch.DeliveryURL);
            Db.AddInParameter(dbcommand, "@StartDate", DbType.DateTime, lvsearch.StartDateWithTime);
            Db.AddInParameter(dbcommand, "@EndDate", DbType.DateTime, lvsearch.EndDateWithTime);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        if (dataReader.GetName(0).ToLower().Equals("batchfilename"))
                        {
                            BatchLeadViewerEntity leadItem = new BatchLeadViewerEntity(dataReader);
                            leadsforViewer.Add(leadItem);

                        }
                    }
                    dataReader.NextResult();
                    while (dataReader.Read())
                    {
                        int BatchId = Convert.ToInt32(dataReader[1]);
                        if (leadsforViewer.Any(a => a.BatchID == BatchId))
                        {
                            BatchLeadViewerEntity leadViewerResultEntity =
                                leadsforViewer.Where(a => a.BatchID == BatchId).FirstOrDefault();

                            if (leadViewerResultEntity != null && leadViewerResultEntity.BatchLogEntries != null && leadViewerResultEntity.BatchLogEntries.Count() > 0)
                            {
                                leadViewerResultEntity.BatchLogEntries.Add(new LVBatchLogEntity(dataReader));
                            }
                            else
                            {
                                leadViewerResultEntity.BatchLogEntries = new List<LVBatchLogEntity>();
                                leadViewerResultEntity.BatchLogEntries.Add(new LVBatchLogEntity(dataReader));
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);

            }
            return leadsforViewer;
        }

        public int CheckLeadViewerSearchResultsExceeds3KRows(LeadViewerSearchParam lvsearch)
        {
            int ResultsCount = 0;
            string SPName = lvsearch.IsBeta ? "dbo.EDDY_DE_CheckBetaLeadViewerSearchResultsExceeds3KRows" : "dbo.EDDY_DE_CheckLeadViewerSearchResultsExceeds3KRows";
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand(SPName, Db);
            dbcommand.CommandTimeout = 1000;
            Db.AddInParameter(dbcommand, "@CsrID", DbType.Int32, lvsearch.CRID);
            Db.AddInParameter(dbcommand, "@ApplicationId", DbType.Int32, lvsearch.ApplicationId);
            Db.AddInParameter(dbcommand, "@DeliveryTypeId", DbType.Int32, lvsearch.DeliveryTypeId);
            Db.AddInParameter(dbcommand, "@LeadStatusId", DbType.String, lvsearch.SelectedStatuses);
            Db.AddInParameter(dbcommand, "@StartPosition", DbType.Int32, lvsearch.StartPosition);
            Db.AddInParameter(dbcommand, "@EndPosition", DbType.Int32, lvsearch.EndPosition);
            Db.AddInParameter(dbcommand, "@SortField", DbType.String, lvsearch.SortField);
            Db.AddInParameter(dbcommand, "@FirstNameMatchTypeStartsWith", DbType.String, lvsearch.FirstNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@FirstNameMatchTypeContains", DbType.String, lvsearch.FirstNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@LastNameMatchTypeStartsWith", DbType.String, lvsearch.LastNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@LastNameMatchTypeContains", DbType.String, lvsearch.LastNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@PsiNameMatchTypeStartsWith", DbType.String, lvsearch.PsiNameMatchTypeStartsWith);
            Db.AddInParameter(dbcommand, "@PsiNameMatchTypeContains", DbType.String, lvsearch.PsiNameMatchTypeContains);
            Db.AddInParameter(dbcommand, "@Email", DbType.String, lvsearch.Email);
            Db.AddInParameter(dbcommand, "@ProgramID", DbType.Int32, Convert.ToInt32(lvsearch.ProgramID));
            Db.AddInParameter(dbcommand, "@DeliveryURL", DbType.String, lvsearch.DeliveryURL);
            Db.AddInParameter(dbcommand, "@StartDate", DbType.DateTime, lvsearch.StartDateWithTime);
            Db.AddInParameter(dbcommand, "@EndDate", DbType.DateTime, lvsearch.EndDateWithTime);
            //Db.AddInParameter(dbcommand, "@IsBeta",DbType.Boolean, lvsearch.IsBeta);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        ResultsCount = Convert.ToInt16(dataReader[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);

            }
            return ResultsCount;

        }

        public string ScrubLeadFromLeadViewer(string LeadIDList, int UserID)
        {
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_FailLeadFromLeadViewer", Db);
            Db.AddInParameter(dbcommand, "@LeadIDList", DbType.String, LeadIDList);
            Db.AddInParameter(dbcommand, "@UserID", DbType.Int32, UserID);
            string ReturnMsg = "";  
            try
            {
                var result = Db.ExecuteScalar(dbcommand);
                if (result != null)
                {
                    ReturnMsg = result.ToString();
                }              

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbcommand);
            }
            return ReturnMsg;
        }

        public string MultipleLeadEdit(string leadIdList, int newDeliveryStatusId, string newTrackId, string editNotes, int UserID, int MilitaryStatusID, string CountryCode, string BillingMonth)
        {
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_MultipleLeadEdit", Db);
            Db.AddInParameter(dbcommand, "@LeadIDList", DbType.String, leadIdList);
            Db.AddInParameter(dbcommand, "@NewDeliveryStatusId", DbType.Int32, newDeliveryStatusId);
            Db.AddInParameter(dbcommand, "@NewTrackId", DbType.String, newTrackId);
            Db.AddInParameter(dbcommand, "@LeadEditNoteText", DbType.String, editNotes);
            Db.AddInParameter(dbcommand, "@UserId", DbType.Int32, UserID);
            Db.AddInParameter(dbcommand, "@NewMilitaryStatusId", DbType.Int32, MilitaryStatusID);
            Db.AddInParameter(dbcommand, "@NewCountryCode", DbType.String, CountryCode);
            Db.AddInParameter(dbcommand, "@NewBillingMonth", DbType.String, BillingMonth);
           
            string ReturnMsg = "";           
            try
            {
                var result = Db.ExecuteScalar(dbcommand);
                if (result != null)
                {
                    ReturnMsg = result.ToString();
                }              
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbcommand);
            }
            return ReturnMsg;
        }


        public bool AddToLeadHistory(int leadId, string OriginalLeadXmlString, string ModifiedLeadXmlString, int UserID)
        {
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_AddToLeadHistory", Db);
            Db.AddInParameter(dbcommand, "@LeadId", DbType.Int32, leadId);
            Db.AddInParameter(dbcommand, "@OriginalLeadXmlString", DbType.Xml, OriginalLeadXmlString);
            Db.AddInParameter(dbcommand, "@ModifiedLeadXmlString", DbType.Xml, ModifiedLeadXmlString);
            Db.AddInParameter(dbcommand, "@UserID", DbType.Int32, UserID);
            bool isUpdated = false;
            int RecordsAffected = 0;

            try
            {
                RecordsAffected = Db.ExecuteNonQuery(dbcommand);
                if (RecordsAffected > 0) isUpdated = true;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbcommand);
            }
            return isUpdated;
        }


        public string GetTrackCampaignName(string TrackId)
        {
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetTrackCampaignName", Db);
            Db.AddInParameter(dbcommand, "@TrackId", DbType.String, TrackId);
            string TrackCampaignName="";
            
            try
            {
                TrackCampaignName = Db.ExecuteScalar(dbcommand).ToString();                
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbcommand);
            }
            return TrackCampaignName;
        }


        public List<RealTimeDeliveryStatusType> GetDeliveryStatusTypeList()
        {
            List<RealTimeDeliveryStatusType> deliveryStatusTypeList = new List<RealTimeDeliveryStatusType>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryStatusType", Db);
            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                    {
                        RealTimeDeliveryStatusType leadStatusType = new RealTimeDeliveryStatusType(dataReader);
                        deliveryStatusTypeList.Add(leadStatusType);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                //clean up reader and command
                DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader);
            }
            return deliveryStatusTypeList;
        }
        
        public int GetDefaultPaidTemplateProductId(int EmailTemplateId)
        {
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("EDDY_DE_GetDefaultPaidTemplateProductId", Db);
            Db.AddInParameter(dbcommand, "@EmailTemplateId", DbType.Int32, EmailTemplateId);
            string DefaultPaidTemplateProductId = "0";

            try
            {
                DefaultPaidTemplateProductId = Db.ExecuteScalar(dbcommand).ToString();
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbcommand);
            }
            return Convert.ToInt32(DefaultPaidTemplateProductId);
        }

        #endregion



        #region Code-PostRoman

        public List<DeliveryPostHTTPHeader> GetHTTPHeaders(int DeliveryEndPointID)
        {
            List<DeliveryPostHTTPHeader> DeliveryPostHTTPHeaderList = new List<DeliveryPostHTTPHeader>();
            DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDY_DE_GetDeliveryEndpointHTTPHeaders", Db);
            Db.AddInParameter(dbcommand, "@DeliveryEndPointID", DbType.Int32, DeliveryEndPointID);

            IDataReader dataReader = null;
            try
            {
                using (dataReader = Db.ExecuteReader(dbcommand))
                {
                    while (dataReader.Read())
                        DeliveryPostHTTPHeaderList.Add(new DeliveryPostHTTPHeader(dataReader));
                }

            }
            catch (Exception ex) { ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY); }
            finally { DatabaseUtilities.CloseAndDispose(ref dbcommand, ref dataReader); }

            return DeliveryPostHTTPHeaderList;
        }

        #endregion
    }
}
