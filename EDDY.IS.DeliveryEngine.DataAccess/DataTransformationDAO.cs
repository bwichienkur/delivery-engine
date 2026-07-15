using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.Nexus.Common.Logging;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.DataAccess.Common;
//using EDDY.Nexus.DataAccess.Interface.DataTransformation;
using EDDY.IS.DeliveryEngine.DataAccess.Common;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.DataTransformation;

namespace EDDY.IS.DeliveryEngine.DataAccess
{
    public class DataTransformationDAO : BaseDataSource, IDataTransformationDAO
    {
        #region IDataTransformationDAO Members
        /// <summary>
        /// Creates a record in Database
        /// </summary>
        /// <param name="data">Data to be inserted in DB</param> 
        /// <returns>True if success else false</returns>
        bool IDataTransformationDAO.CreateData(DataTransformationEntity data)
        {
           
            bool result = false;
            try
            {                
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Insert", Db))
                {
                    //Pass parameters to command                    
                    Db.AddInParameter(dbCommand, "@Name", DbType.String, data.Name);                    
                    Db.AddInParameter(dbCommand, "@ConditionXML", DbType.Xml, data.ConditionXml);
                    Db.AddInParameter(dbCommand, "@DeliveryDefId", DbType.Int32, data.DeliveryDefId);
                    Db.AddInParameter(dbCommand, "@FormValidationId", DbType.Int32, data.FormValidationId);
                    Db.AddInParameter(dbCommand, "@TaskXML", DbType.Xml, data.TaskXml);
                    Db.AddInParameter(dbCommand, "@TaskXSL", DbType.Xml, data.XSL);
                    Db.AddInParameter(dbCommand, "@SequenceNo", DbType.Int32, data.SequenceNo);                    
                    Db.AddInParameter(dbCommand, "@CreatedBy", DbType.Int32, data.CreatedBy);

                    //Execute the command to insert records
                    int rowsAffected = Db.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }

        /// <summary>
        /// Creates a record in Database
        /// </summary>
        /// <param name="data">Log Data to be inserted in DB</param> 
        /// <returns>True if success else false</returns>
       public  bool CreateLog(LogEntity data)
        {
            bool result = false;
            try
            {                
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Log_Insert", Db))
                {
                    //Pass parameters to command                    
                    Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, data.LeadId);
                    Db.AddInParameter(dbCommand, "@RowLeadId", DbType.Int32, data.RowLeadId);
                    Db.AddInParameter(dbCommand, "@Message", DbType.String, data.Message);                    
                    Db.AddInParameter(dbCommand, "@CreatedBy", DbType.Int32, data.CreatedBy);

                    //Execute the command to insert records
                    int rowsAffected = Db.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }


        string IDataTransformationDAO.GetStoredProcResult(string procedureName, Int64 leadId)
        {
            string value="";
            try
            {

                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand(procedureName, Db))
                {
                    //Pass parameters to command                    
                    Db.AddInParameter(dbCommand, "@LeadId", DbType.Int32, leadId);
                    Db.AddOutParameter(dbCommand, "@Result", DbType.String, 4000);
                    object result = Db.ExecuteNonQuery(dbCommand);

                    value = Db.GetParameterValue(dbCommand, "@Result").ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return value;
        }

        /// <summary>
        /// Removes data from Database for the given id
        /// </summary>
        /// <param name="id">record id</param> 
        /// <returns>True if success else false</returns>
        bool IDataTransformationDAO.DeleteData(int Id)
        {
            bool result = false;
            try
            {                
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Delete", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@Id", DbType.Int32, Id);

                    //Execute the command to delete data
                    int rowsAffected = Db.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }



        
        /// <summary>
        /// Gets DataTransformaiton data from SQL server
        /// </summary>        
        /// <returns>Returns DataTable with DataTransformation data</returns>
        List<DataTransformationEntity> IDataTransformationDAO.GetAllData()
        {
            List<DataTransformationEntity> dtList = null;
            try
            {
                //set the stored procedure for the command
                DataTransformationEntity iDtEntity = null;
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Select", Db))
                {
                    //Execute the command get all data
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            dtList = new List<DataTransformationEntity>();
                            //Load each record to DataTransformationEntity class and add the class to dtList
                            while (dataReader.Read())
                            {
                                iDtEntity = new DataTransformationEntity(dataReader);
                                dtList.Add(iDtEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return dtList;
        }

        /// <summary>
        /// Get data for the id
        /// </summary>
        /// <param name="id">record id</param>
        /// <returns>IDataTransformationEntity</returns>
        DataTransformationEntity IDataTransformationDAO.GetData(int Id)
        {
            DataTransformationEntity data = null;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Select", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@Id", DbType.Int32, Id);

                    //Execute the command to get data based on Id
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                data = new DataTransformationEntity(dataReader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return data;
        }

        /// <summary>
        /// To get list of delivery defs
        /// </summary>
        /// <returns>List of delivery defs</returns>
        List<DeliveryDefsEntity> IDataTransformationDAO.GetDeliveryDefs()
        {
            List<DeliveryDefsEntity> data = null;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_GetDeliveryDefs", Db))
                {                    
                    //Execute the command to get data based on Id
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            DeliveryDefsEntity entity = new DeliveryDefsEntity();
                            data = new List<DeliveryDefsEntity>();

                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                entity = new DeliveryDefsEntity(dataReader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return data;
        }


        /// <summary>
        /// To get field names and populate in admin
        /// </summary>
        /// <returns>Key value dictionary</returns>
        Dictionary<string, string> IDataTransformationDAO.GetFieldNames()
        {
            Dictionary<string, string> result = null;
            try
            {    
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_GetFieldNames", Db))
                {
                    //Execute the command get all data
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            result = new Dictionary<string, string>();

                            //Load each record to dictionary
                            while (dataReader.Read())
                            {                                
                                string fieldName = DatabaseUtilities.SetStringValue(dataReader["FieldName"]);
                                result.Add(fieldName, fieldName);
                            }
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

        /// <summary>
        /// To get list of form validations
        /// </summary>
        /// <returns>List of form validations</returns>
        List<FormValidationEntity> IDataTransformationDAO.GetFormValidations()
        {
            List<FormValidationEntity> data = null;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_GetFormValidations", Db))
                {
                    //Execute the command to get data based on Id
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            FormValidationEntity entity = new FormValidationEntity();
                            data = new List<FormValidationEntity>();

                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                entity = new FormValidationEntity(dataReader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return data;
        }

        /// <summary>
        /// To get task data based on condition id
        /// </summary>        
        /// <param name="id">condition id</param> 
        /// <returns>Returns list of DataTransformation data</returns>
        List<DataTransformationEntity> IDataTransformationDAO.GetTasksByConditionId(int id)
        {
            List<DataTransformationEntity> dtList = null;
            try
            {
                //set the stored procedure for the command
                DataTransformationEntity iDtEntity = null;
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_SelectByConditionXMLId", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@ConditionXMLId", DbType.Int32, id);

                    //Execute the command get all data
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            dtList = new List<DataTransformationEntity>();
                            //Load each record to DataTransformationEntity class and add the class to dtList
                            while (dataReader.Read())
                            {
                                iDtEntity = new DataTransformationEntity(dataReader);
                                dtList.Add(iDtEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return dtList;
        }

        /// <summary>
        /// To get task data based on deliverydef id
        /// </summary>        
        /// <param name="id">deliverydef id</param> 
        /// <returns>Returns list of DataTransformation data</returns>
        List<DataTransformationEntity> IDataTransformationDAO.GetTasksByDeliveryDefId(int id)
        {
            List<DataTransformationEntity> dtList = null;
            try
            {
                //set the stored procedure for the command
                DataTransformationEntity iDtEntity = null;
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_SelectByDeliveryDefId", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@DeliveryDefId", DbType.Int32, id);

                    //Execute the command get all data
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            dtList = new List<DataTransformationEntity>();
                            //Load each record to DataTransformationEntity class and add the class to dtList
                            while (dataReader.Read())
                            {
                                iDtEntity = new DataTransformationEntity(dataReader);
                                dtList.Add(iDtEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return dtList;
        }

        /// <summary>
        /// To get task data based on form validation id
        /// </summary>        
        /// <param name="id">form validation id</param> 
        /// <returns>Returns list of DataTransformation data</returns>
        List<DataTransformationEntity> IDataTransformationDAO.GetTasksByFormValidationId(int id)
        {
            List<DataTransformationEntity> dtList = null;
            try
            {
                //set the stored procedure for the command
                DataTransformationEntity iDtEntity = null;
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_SelectByFormValidationId", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@FormValidationId", DbType.Int32, id);

                    //Execute the command get all data
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            dtList = new List<DataTransformationEntity>();
                            //Load each record to DataTransformationEntity class and add the class to dtList
                            while (dataReader.Read())
                            {
                                iDtEntity = new DataTransformationEntity(dataReader);
                                dtList.Add(iDtEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return dtList;
        }

        /// <summary>
        /// To get value code based on program id
        /// </summary>
        /// <param name="ProgramId">Program id based on which Value Code will be fatched</param>
        /// <returns>Value code string</returns>
        string IDataTransformationDAO.GetValueCodeFromProgramId(int ProgramId)
        {
            string valueCode = string.Empty;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_GetValueCodeFromProgramId", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@ProgramId", DbType.Int32, ProgramId);

                    //Execute the command to get value code
                    object result = Db.ExecuteScalar(dbCommand);
                    if (result != null)
                    {
                        valueCode = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return valueCode;
        }

        /// <summary>
        /// To get XSL string for the given id
        /// </summary>
        /// <param name="Id">Task id based on which xsl will be fetched</param>
        /// <returns>XSL string</returns>
        string IDataTransformationDAO.GetXSLById(int id)
        {
            string xslString = string.Empty;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_GetXSLById", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@Id", DbType.Int32, id);

                    //Execute the command to get xsl string
                    object result = Db.ExecuteScalar(dbCommand);

                    //if result not null then set the result
                    if (result != null)
                    {
                        xslString = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return xslString;
        }

        /// <summary>
        /// Gets DataTransformaiton data from SQL server
        /// </summary>
        /// <param name="eventType">Filter based on event type</param>
        /// <returns>Returns DataTable with DataTransformation data</returns>
        DataTable IDataTransformationDAO.LoadDataTransformationTable(EventType eventType, int eventId)        
        {
            DataTable dataTable = null;
            try
            {
                if (eventType == EventType.Delivery)
                {
                    //set the stored procedure for the command
                    using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_SelectByDeliveryDefId", Db))
                    {
                        //Pass parameters to command   
                        Db.AddInParameter(dbCommand, "@DeliveryDefId", DbType.Int32, eventId);

                        //Execute the command select data based on event id
                        using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                        {
                            //Convert the datareader to data table
                            if (dataReader != null)
                            {
                                dataTable = new DataTable();
                                dataTable.Load(dataReader);
                            }
                        }
                    }
                }
                else if (eventType == EventType.Validation)
                {
                    //set the stored procedure for the command
                    using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_SelectByFormValidationId", Db))
                    {
                        //Pass parameters to command   
                        Db.AddInParameter(dbCommand, "@FormValidationId", DbType.Int32, eventId);

                        //Execute the command select data based on event id
                        using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                        {
                            //Convert the datareader to data table
                            if (dataReader != null)
                            {
                                dataTable = new DataTable();
                                dataTable.Load(dataReader);
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return dataTable;            
        }

        /// <summary>
        /// Updates condition data into Database
        /// </summary>
        /// <param name="ConditionXMLEntity">Data to be updated</param>        
        /// <returns>returns TRUE if success, else FALSE</returns>
        bool IDataTransformationDAO.UpdateConditionXml(ConditionXmlEntity data)
        {
            bool result = false;
            try
            {                
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_ConditionXML_Update", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@ConditionXMLId", DbType.Int32, data.ConditionXmlId);
                    Db.AddInParameter(dbCommand, "@ConditionXML", DbType.Xml, data.ConditionXml);
                    Db.AddInParameter(dbCommand, "@ConditionDescription", DbType.String, data.ConditionDescription);
                    Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Boolean, data.IsEnabled);
                    Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, data.UpdatedBy);

                    //Execute the command to update data
                    int rowsAffected = Db.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }


        /// <summary>
        /// Saves data into Database
        /// </summary>
        /// <param name="IDataTransformationEntity">Data to be updated</param>        
        /// <returns>returns TRUE if success, else FALSE</returns>
        bool IDataTransformationDAO.UpdateData(DataTransformationEntity data)
        {
            bool result = false;
            try
            {                
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Update", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@Id", DbType.Int32, data.Id);
                    Db.AddInParameter(dbCommand, "@Name", DbType.String, data.Name);
                    Db.AddInParameter(dbCommand, "@DeliveryDefId", DbType.Int32, data.DeliveryDefId);
                    Db.AddInParameter(dbCommand, "@FormValidationId", DbType.Int32, data.FormValidationId);
                    Db.AddInParameter(dbCommand, "@TaskXSL", DbType.Xml, data.XSL);
                    Db.AddInParameter(dbCommand, "@SequenceNo", DbType.Int32, data.SequenceNo);
                    Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Boolean, data.IsEnabled);
                    Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, data.UpdatedBy);

                    //Execute the command to update data
                    int rowsAffected = Db.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }

        /// <summary>
        /// To update sequence number
        /// </summary>
        /// <param name="data">Data to be updated</param>        
        /// <returns>returns TRUE if success, else FALSE</returns>
        bool IDataTransformationDAO.UpdateSequenceNo(string data)
        {
            bool result = false;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Reorder", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@IdOrder", DbType.String, data);

                    //Execute the command to update data
                    int rowsAffected = Db.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }

        /// <summary>
        /// Updates task data into Database
        /// </summary>
        /// <param name="TaskXMLEntity">Data to be updated</param>        
        /// <returns>returns TRUE if success, else FALSE</returns>
        bool IDataTransformationDAO.UpdateTaskXml(TaskXmlEntity data)
        {
            bool result = false;
            try
            {                
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_TaskXML_Update", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@TaskXMLId", DbType.Int32, data.TaskXmlId);
                    Db.AddInParameter(dbCommand, "@TaskXML", DbType.Xml, data.TaskXml);
                    Db.AddInParameter(dbCommand, "@TaskDescription", DbType.String, data.TaskDescription);
                    Db.AddInParameter(dbCommand, "@IsEnabled", DbType.Boolean, data.IsEnabled);
                    Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, data.UpdatedBy);

                    //Execute the command to update data
                    int rowsAffected = Db.ExecuteNonQuery(dbCommand);
                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return result;
        }
        #endregion

        public List<ConsumerIdMapping> GetConsumerIdMapping(DataTable table, long serviceTransactionId)
        {
            DateTime startDateTime = DateTime.Now;

            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.[EDDY_GetCorrespondingNexusIds]", Db);
            dbCommand.CommandTimeout = 1000;
            Db.AddInParameter(dbCommand, "@ELRParam", DbType.Object, table);

            SqlParameter sparam = (SqlParameter)dbCommand.Parameters["@ELRParam"];
            sparam.SqlDbType = SqlDbType.Structured;
            sparam.TypeName = "dbo.EDDY_NexusMappingType";

            dbCommand.Parameters["@ELRParam"] = sparam;
            dbCommand.CommandType = CommandType.StoredProcedure;
            List<ConsumerIdMapping> consumerIdMappingList = new List<ConsumerIdMapping>();
            
            try
            {
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        ConsumerIdMapping consumerIdMapping = new ConsumerIdMapping(dataReader);
                        consumerIdMappingList.Add(consumerIdMapping);
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
                if (table != null)
                {
                    table.Clear();
                    table.Dispose();
                }
            }

            decimal executionTime = Convert.ToDecimal((DateTime.Now - startDateTime).TotalMilliseconds / 1000);
            string message = string.Format("Req:{0} TranId:{1} {2} - ExecTime: {3} ", "", serviceTransactionId, "GetConsumerIdMapping", executionTime);
            return consumerIdMappingList;
        }

        public DataTable GetELRNexusMappingData(DataTable table)
        {
            
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ELRType", Type.GetType("System.String"));
            dataTable.Columns.Add("ELRId", Type.GetType("System.Int32"));
            dataTable.Columns.Add("NexusId", Type.GetType("System.Int32"));
            dataTable.Columns.Add("IsExclusion", Type.GetType("System.Boolean"));

            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.[EDDY_GetCorrespondingNexusIds]", Db);
            dbCommand.CommandTimeout = 1000;
            Db.AddInParameter(dbCommand, "@ELRParam", DbType.Object, table);

            var sparam = (SqlParameter)dbCommand.Parameters["@ELRParam"];
            sparam.SqlDbType = SqlDbType.Structured;
            sparam.TypeName = "dbo.EDDY_NexusMappingType";

            dbCommand.Parameters["@ELRParam"] = sparam;
            dbCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        row["ELRType"] = dataReader[0];
                        row["ELRId"] = dataReader[1];
                        row["NexusId"] = dataReader[2];
                        row["IsExclusion"] = dataReader[3];
                        dataTable.Rows.Add(row);

                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

            return dataTable;
        }
        #region Added By Bala for New Data Transformation
        DataTransformationEntityNew IDataTransformationDAO.GetDataTransformationTask(DTConditionparamEntity cp)
        {
            DataTransformationEntityNew data = null;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Select_New", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@EndPointId", DbType.Int32, cp.EndPointID);
                    Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, cp.TaskTypeID);
                    Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, cp.TaskID);
                    //Execute the command to get data based on Id)
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                data = new DataTransformationEntityNew(dataReader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return data;
        }
        ConditionSelectEntity IDataTransformationDAO.GetDataTransformationCondition(DTConditionparamEntity ce)
        {
            ConditionSelectEntity data = null;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_GetDataTransformationTaskConditions", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@ConditionXMLID", DbType.Int32, ce.ConditionXMLID);
                    Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                    Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                    //Execute the command to get data based on Id
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                data = new ConditionSelectEntity(dataReader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return data;

        }
        public DTConditionparamEntity AddUpdateXsltXmlTasks(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;

            DTConditionparamEntity rtnEntity = new DTConditionparamEntity();
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_AddUpdateXsltXml", Db);
                Db.AddInParameter(dbCommand, "@TaskXML", DbType.String, ce.DeliveryLabelXml);
                Db.AddInParameter(dbCommand, "@TaskXMLID", DbType.Int32, ce.TaskXMLID);
                Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, ce.TaskID);
                Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                Db.AddInParameter(dbCommand, "@TaskDesc", DbType.String, ce.TaskDesc);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, ce.UpdatedBy);
                Db.AddInParameter(dbCommand, "@SeqNo", DbType.Int32, ce.SequenceNo);
                Db.AddInParameter(dbCommand, "@XSLT", DbType.String, ce.TaskXML);
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            rtnEntity.EndPointID = (int)dataReader["EndPointID"];
                            rtnEntity.TaskTypeID = (int)dataReader["TaskTypeID"];
                            rtnEntity.TaskID = (int)dataReader["TaskID"];
                            rtnEntity.TaskXMLID = (int)dataReader["TaskXMLID"];
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return rtnEntity;
        }
        XsltXmlEntity IDataTransformationDAO.GetXsltXmlTask(DTConditionparamEntity ce)
        {
            XsltXmlEntity data = null;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_GetXsltXmlTask", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@TaskXMLID", DbType.Int32, ce.TaskXMLID);
                    Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                    Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                    //Execute the command to get data based on Id
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                data = new XsltXmlEntity(dataReader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return data;

        }

        TaskSelectEntity IDataTransformationDAO.GetDataTransformationAppendNameValueTask(DTConditionparamEntity ce)
        {
            TaskSelectEntity data = null;
            try
            {
                //set the stored procedure for the command
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_GetAppendNameValuePairs", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@TaskXMLID", DbType.Int32, ce.TaskXMLID);
                    Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                    Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                    //Execute the command to get data based on Id
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                data = new TaskSelectEntity(dataReader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            return data;

        }
        public bool RemoveDataTransformationCondition(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;
            IDataReader dataReader = null;
            bool blnDeldef = false;
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_RemoveDataTransformationCondition", Db);
                Db.AddInParameter(dbCommand, "@ConditionXML", DbType.String, ce.ConditionXML);
                Db.AddInParameter(dbCommand, "@ConditionXMLID", DbType.Int32, ce.ConditionXMLID);
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
        public DTConditionparamEntity AddUpdateDataTransformationCondition(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;

            DTConditionparamEntity rtnEntity =new  DTConditionparamEntity();
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_AddUpdateDataTransformationTaskConditions", Db);
                Db.AddInParameter(dbCommand, "@ConditionXML", DbType.String, ce.ConditionXML);
                Db.AddInParameter(dbCommand, "@ConditionXMLID", DbType.Int32, ce.ConditionXMLID);
                Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, ce.TaskID);
                Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                Db.AddInParameter(dbCommand, "@ConditionDesc", DbType.String, ce.ConditionDesc);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, ce.UpdatedBy);
                Db.AddInParameter(dbCommand, "@SeqNo", DbType.Int32, ce.SequenceNo);
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            rtnEntity.EndPointID = (int)dataReader["EndPointID"];
                            rtnEntity.TaskTypeID = (int)dataReader["TaskTypeID"];
                            rtnEntity.TaskID = (int)dataReader["TaskID"];
                            rtnEntity.ConditionXMLID = (int)dataReader["ConditionXMLID"];
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return rtnEntity;
        }
        public DTConditionparamEntity AddUpdateDataTransformationTasks(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;

            DTConditionparamEntity rtnEntity = new DTConditionparamEntity();
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_AddUpdateTasks", Db);
                Db.AddInParameter(dbCommand, "@TaskXML", DbType.String, ce.TaskXML);
                Db.AddInParameter(dbCommand, "@TaskXMLID", DbType.Int32, ce.TaskXMLID);
                Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, ce.TaskID);
                Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                Db.AddInParameter(dbCommand, "@TaskDesc", DbType.String, ce.TaskDesc);
                Db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, ce.UpdatedBy);
                Db.AddInParameter(dbCommand, "@SeqNo", DbType.Int32, ce.SequenceNo);
                //int RecordsAffected = Db.ExecuteNonQuery(dbCommand);
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            rtnEntity.EndPointID = (int)dataReader["EndPointID"];
                            rtnEntity.TaskTypeID = (int)dataReader["TaskTypeID"];
                            rtnEntity.TaskID = (int)dataReader["TaskID"];
                            rtnEntity.TaskXMLID = (int)dataReader["TaskXMLID"];
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return rtnEntity;
        }
        public List<AppendNameValueTaskItem> GetAppendNameValueTasks(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;
            List<AppendNameValueTaskItem> appendNameValueTaskItems = new List<AppendNameValueTaskItem>();
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_GetAppnedNameValueTasks", Db);
                Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, ce.TaskID);
                Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                Db.AddInParameter(dbCommand, "@StartPosition", DbType.Int32, ce.StartPosition);
                Db.AddInParameter(dbCommand, "@EndPosition", DbType.Int32, ce.EndPosition);
                Db.AddInParameter(dbCommand, "@SortField", DbType.String, ce.SortField);
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            AppendNameValueTaskItem item=new AppendNameValueTaskItem(dataReader);
                            appendNameValueTaskItems.Add(item);
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return appendNameValueTaskItems;

        }
        public List<AppendNameValueTaskItem> GetFieldFormatTasks(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;
            List<AppendNameValueTaskItem> appendNameValueTaskItems = new List<AppendNameValueTaskItem>();
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("[EDDY_DT_Task_GetFieldFormatTasks]", Db);
                Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, ce.TaskID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                Db.AddInParameter(dbCommand, "@StartPosition", DbType.Int32, ce.StartPosition);
                Db.AddInParameter(dbCommand, "@EndPosition", DbType.Int32, ce.EndPosition);
                Db.AddInParameter(dbCommand, "@SortField", DbType.String, ce.SortField);
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            AppendNameValueTaskItem item = new AppendNameValueTaskItem(dataReader);
                            appendNameValueTaskItems.Add(item);
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return appendNameValueTaskItems;

        }
        public Dictionary<string, string> GetDataTranformationFormats(string DateOrPhone)
        {
            Dictionary<string, string> formatTypes = new Dictionary<string, string>();
            System.Data.Common.DbCommand dbCommand = null;
            
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("[EDDY_DT_GetDataTranformationFormats]", Db);
                Db.AddInParameter(dbCommand, "@DateOrPhone", DbType.String, DateOrPhone);
               
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            formatTypes.Add(dataReader["FormatType"].ToString(), dataReader["FormatTypeDesc"].ToString());
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }

            return formatTypes;
        }
        public bool DTCheckDuplicateSequence(DTConditionparamEntity ce)
        {
            bool IsSequenceExists = false;
            try
            {
                
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_TaskCheckDuplicateSequenceNo", Db))
                {
                    //Pass parameters to command   
                    Db.AddInParameter(dbCommand, "@TaskXMLID", DbType.Int32, ce.TaskXMLID);
                    Db.AddInParameter(dbCommand, "@TaskTypeID", DbType.Int32, ce.TaskTypeID);
                    Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                    Db.AddInParameter(dbCommand, "@SequenceID", DbType.Int32, ce.EndPointID);
                    //Execute the command to get data based on Id);
                    using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            //Load the data to DataTransformationEntity to return
                            while (dataReader.Read())
                            {
                                IsSequenceExists = (int)dataReader["IsSequenceExists"]==0?false:true;
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
                //DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return IsSequenceExists;
        }
        public List<AppendNameValueTaskItem> GetXsltXmlTasks(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;
            List<AppendNameValueTaskItem> appendNameValueTaskItems = new List<AppendNameValueTaskItem>();
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("[EDDY_DT_Task_GetXsltXmlTasks]", Db);
                Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, ce.TaskID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                Db.AddInParameter(dbCommand, "@StartPosition", DbType.Int32, ce.StartPosition);
                Db.AddInParameter(dbCommand, "@EndPosition", DbType.Int32, ce.EndPosition);
                Db.AddInParameter(dbCommand, "@SortField", DbType.String, ce.SortField);
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            AppendNameValueTaskItem item = new AppendNameValueTaskItem(dataReader);
                            appendNameValueTaskItems.Add(item);
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return appendNameValueTaskItems;

        }
        public List<AppendNameValueTaskItem> GetEmailSubjectAndEmailToTasks(DTConditionparamEntity ce)
        {
            System.Data.Common.DbCommand dbCommand = null;
            List<AppendNameValueTaskItem> appendNameValueTaskItems = new List<AppendNameValueTaskItem>();
            try
            {

                dbCommand = DatabaseUtilities.CreateDbCommand("[EDDY_DT_Task_GetEmailSubjectAndEmailToTasks]", Db);
                Db.AddInParameter(dbCommand, "@TaskID", DbType.Int32, ce.TaskID);
                Db.AddInParameter(dbCommand, "@EndPointID", DbType.Int32, ce.EndPointID);
                Db.AddInParameter(dbCommand, "@StartPosition", DbType.Int32, ce.StartPosition);
                Db.AddInParameter(dbCommand, "@EndPosition", DbType.Int32, ce.EndPosition);
                Db.AddInParameter(dbCommand, "@SortField", DbType.String, ce.SortField);
                using (IDataReader dataReader = Db.ExecuteReader(dbCommand))
                {
                    if (dataReader != null)
                    {
                        //Load the data to DataTransformationEntity to return
                        while (dataReader.Read())
                        {
                            AppendNameValueTaskItem item = new AppendNameValueTaskItem(dataReader);
                            appendNameValueTaskItems.Add(item);
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
                DatabaseUtilities.CloseAndDispose(ref dbCommand);
            }
            return appendNameValueTaskItems;

        }
        
        #endregion
    }
}
