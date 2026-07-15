using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
//using EDDY.Nexus.BusinessComponent.ConditionValidator;
//using EDDY.Nexus.BusinessComponent.Interface.ConditionValidator;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity.Common;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DataTransformation;
//using EDDY.Nexus.DataAccess.Interface.DataTransformation;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.ConditionValidator;
//using EDDY.Nexus.Entity.DataTransformation;
//using EventType = EDDY.Nexus.Entity.DataTransformation.EventType;


namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class DataTransformationBC  : IDataTransformationBC
    {
        #region Constructor
        /// <summary>
        ///  Constructor to initialise DataTransformationDAO
        /// </summary>
        public DataTransformationBC()
        {            
        }
        # endregion

        #region IDataTransformationBC Members
        ///// <summary>
        ///// Converts the dictionary object to Key/Value object
        ///// </summary>
        ///// <param name="dicValue">key value dictionary object</param>
        ///// <returns>KeyValueObject<string, object/></returns>
        //List<KeyValueObject> IDataTransformationBC.ConvertDictionaryToKeyValueObject(Dictionary<string, object> dicValue)
        //{
        //    List<KeyValueObject> output = null;
        //    try
        //    {
        //        if (dicValue == null)
        //        {
        //            return output;
        //        }

        //        output = new List<KeyValueObject>();
        //        KeyValueObject keyValue = null;

        //        //Add the items to dictionary
        //        foreach (KeyValuePair<string, object> item in dicValue)
        //        {
        //            keyValue = new KeyValueObject(item.Key, item.Value);
        //            output.Add(keyValue);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);                
        //    }

        //    return output;
        //}

        ///// <summary>
        ///// Converts the Key/Value object to a dictionary objects
        ///// </summary>
        ///// <param name="keyValue">key value object list</param>
        ///// <returns>Dictionary<string, object></returns>
        //Dictionary<string, object> IDataTransformationBC.ConvertKeyValueToDictionary(List<KeyValueObject> keyValue)
        //{
        //    Dictionary<string, object> keyValuePair = null;
        //    try
        //    {
        //        if (keyValue == null)
        //        {
        //            return keyValuePair;
        //        }

        //        keyValuePair = new Dictionary<string, object>();

        //        //Add the items to dictionary
        //        foreach (KeyValueObject item in keyValue)
        //        {
        //            if (keyValuePair.ContainsKey(item.Key))
        //                keyValuePair[item.Key] = item.Value;
        //            else
        //                keyValuePair.Add(item.Key, item.Value);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
        //    }

        //    return keyValuePair;
        //}

        ///// <summary>
        ///// Converts the Key/Value string to a dictionary objects
        ///// </summary>
        ///// <param name="leadDataString">String value containing Name/Value pair</param>
        ///// <param name="seperator">Char seperator to split the Name/Value pairs</param>
        ///// <returns>Dictionary<string, object></returns>
        //Dictionary<string, object> IDataTransformationBC.ConvertKeyValueToDictionary(string leadDataString, char seperator)
        //{
        //    Dictionary<string, object> keyValuePair = null;
        //    try
        //    {
        //        if (string.IsNullOrEmpty(leadDataString))
        //        {
        //            return keyValuePair;
        //        }

        //        //If no seperator found return the whole string a one item
        //        if (!leadDataString.Contains(seperator))
        //        {
        //            keyValuePair = new Dictionary<string, object>();
        //            keyValuePair.Add(leadDataString, "");
        //            return keyValuePair;
        //        }

        //        keyValuePair = new Dictionary<string, object>();
        //        //Split data based on seperator
        //        string[] arrTargetString = leadDataString.Split(seperator);

        //        if (arrTargetString.Count() == 0)
        //        {
        //            return null;
        //        }

        //        //Add the items to dictionary
        //        for (int index = 0; index < arrTargetString.Length; index++)
        //        {
        //            string[] arrKeyValue = arrTargetString[index].Split('=');
        //            keyValuePair.Add(arrKeyValue[0], arrKeyValue[1]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
        //    }

        //    return keyValuePair;
        //}

        ///// <summary>
        ///// Creates a record in Database
        ///// </summary>
        ///// <param name="data">Data to be inserted in DB</param> 
        ///// <returns>True if success else false</returns>
        //bool IDataTransformationBC.CreateData(DataTransformationEntity data)
        //{
        //    return _DataTransformationDAO.CreateData(data);
        //}

        ///// <summary>
        ///// To create a record in DB
        ///// </summary>
        ///// <param name="leadId"></param>
        ///// <param name="rowLeadId"></param>
        ///// <param name="message"></param>
        ///// <param name="createBy"></param>
        ///// <returns>true if success else false</returns>
        //bool IDataTransformationBC.CreateLog(int? leadId, int? rowLeadId, StringBuilder message, int createBy)
        //{
        //    bool result = false;
        //    try
        //    {
        //        LogEntity data = new LogEntity();
        //        data.LeadId = leadId;
        //        data.RowLeadId = rowLeadId;
        //        data.Message = message.ToString();
        //        data.CreatedBy = createBy;

        //        result = _DataTransformationDAO.CreateLog(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Removes data from Database for the given id
        ///// </summary>
        ///// <param name="Id">record id</param> 
        ///// <returns>True if success else false</returns>
        //bool IDataTransformationBC.DeleteData(int Id)
        //{
        //    return _DataTransformationDAO.DeleteData(Id);
        //}
                
        ///// <summary>
        ///// Gets DataTransformaiton data from SQL server
        ///// </summary>
        ///// <returns>Returns DataTable with DataTransformation data</returns>
        //List<DataTransformationEntity> IDataTransformationBC.GetAllData()
        //{
        //    return _DataTransformationDAO.GetAllData();
        //}

        ///// <summary>
        ///// Get data for the id
        ///// </summary>
        ///// <param name="Id">record id</param>
        ///// <returns>IDataTransformationEntity</returns>
        //DataTransformationEntity IDataTransformationBC.GetData(int Id)
        //{
        //    return _DataTransformationDAO.GetData(Id);
        //}

        ///// <summary>
        ///// To get list of delivery defs
        ///// </summary>
        ///// <returns>List of delivery defs</returns>
        //List<DeliveryDefsEntity> IDataTransformationBC.GetDeliveryDefs()
        //{
        //    return _DataTransformationDAO.GetDeliveryDefs();
        //}

        ///// <summary>
        ///// To get field names and populate in admin
        ///// </summary>
        ///// <returns>Key value dictionary</returns>
        //Dictionary<string, string> IDataTransformationBC.GetFieldNames()
        //{
        //    return _DataTransformationDAO.GetFieldNames();
        //}

        ///// <summary>
        ///// To get list of form validations
        ///// </summary>
        ///// <returns>List of form validations</returns>
        //List<FormValidationEntity> IDataTransformationBC.GetFormValidations()
        //{
        //    return _DataTransformationDAO.GetFormValidations();  
        //}

        ///// <summary>
        ///// Converts the input dictionary to Key/Value string by splitting based on seperator
        ///// </summary>
        ///// <param name="nameVals">Dictionary Name/Value pairs</param>
        ///// <param name="seperator">Char seperator to be applied</param>
        ///// <returns>String of Name/Value pair with seperator</returns>
        //string IDataTransformationBC.GetNameValueString(Dictionary<string, object> nameVals, char seperator)
        //{
        //    string outputString = string.Empty;
        //    try
        //    {
        //        if (nameVals == null)
        //        {
        //            return string.Empty;
        //        }

        //        if (nameVals.Count == 0)
        //        {
        //            return string.Empty;
        //        }
                
        //        //Read the each item in dictionary and add as key/value to a string
        //        foreach (KeyValuePair<string, object> keyValeuPair in nameVals)
        //        {
        //            outputString += keyValeuPair.Key + "=" + keyValeuPair.Value + seperator;
        //        }
        //        outputString = outputString.Substring(0, outputString.Length - 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
        //    }
        //    return outputString;
        //}

        ///// <summary>
        ///// To get task data based on condition id
        ///// </summary>        
        ///// <param name="id">condition id</param> 
        ///// <returns>Returns list of DataTransformation data</returns>
        //List<DataTransformationEntity> IDataTransformationBC.GetTasksByConditionId(int id)
        //{
        //    return _DataTransformationDAO.GetTasksByConditionId(id);
        //}

        ///// <summary>
        ///// To get task data based on deliverydef id
        ///// </summary>        
        ///// <param name="id">deliverydef id</param> 
        ///// <returns>Returns list of DataTransformation data</returns>
        //List<DataTransformationEntity> IDataTransformationBC.GetTasksByDeliveryDefId(int id)
        //{
        //    return _DataTransformationDAO.GetTasksByDeliveryDefId(id);
        //}

        ///// <summary>
        ///// To get task data based on form validation id
        ///// </summary>        
        ///// <param name="id">form validation id</param> 
        ///// <returns>Returns list of DataTransformation data</returns>
        //List<DataTransformationEntity> IDataTransformationBC.GetTasksByFormValidationId(int id)
        //{
        //    return _DataTransformationDAO.GetTasksByFormValidationId(id);
        //}

        ///// <summary>
        ///// To get XSL string for the given id
        ///// </summary>
        ///// <param name="id">Task id based on which xsl will be fetched</param>
        ///// <returns>XSL string</returns>
        //string IDataTransformationBC.GetXSLById(int id)
        //{
        //    return _DataTransformationDAO.GetXSLById(id); 
        //}

        /// <summary>
        /// Transforms the leadData dictionary with XML rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <param name="leadData">Dictionary containing input for DataTransformation</param>
        /// <param name="eventType"></param>
        /// <param name="eventId">type of event</param>
        /// <param name="log">Log information</param>
        /// <returns>Transformed dictionary objects</returns>        
        Dictionary<string, object> IDataTransformationBC.Transform(Dictionary<string, object> rawData, Dictionary<string, object> leadData, EventType eventType, int eventId, ref StringBuilder log)
        {
            Dictionary<string, object> outputDictionary = null;
            try
            {
                //log.Append(" - Merging raw data and lead data \r\n");
                Dictionary<string, object> conditionData = MergeRawDataWithLeadData(rawData, leadData);
                //log.Append(" - Merged \r\n\r\n");

                if (conditionData == null)
                {
                    log.Append(" - Raw Data and Lead Data are null \r\n");
                    return rawData;
                }

                if (conditionData.Count == 0)
                {
                    log.Append(" - Raw Data and Lead Data are Empty \r\n");
                    return rawData;
                }

                //Creates a filtered operation dictionary
                Dictionary<int, IDataTransformationTask> operationDictionary = CreateOperationDictionary(conditionData, eventType, eventId, ref log);

                if (operationDictionary == null)
                {
                    log.Append("\r\n - There are no Tasks to transform, because all conditions failed \r\n");
                    return rawData;
                }

                if (operationDictionary.Count == 0)
                {
                    log.Append("\r\n - There are no Tasks to transform, because all conditions failed \r\n");
                    return rawData;
                }

                //Raw data cannot be null
                if (rawData == null)
                {
                    outputDictionary = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
                }
                else
                {
                    outputDictionary = rawData;
                }

                if (outputDictionary != null)
                {
                    log.Append(" Raw data before transformation: ");
                    foreach (KeyValuePair<string, object> item in outputDictionary)
                    {
                        log.Append(item.Key + " = " + item.Value + "|");
                    }
                }


                //Merge all append tasks into ine addfields task and replace in the operation dictionary.
                AddFields newTask = new AddFields();
                bool foundAppendTask = false;
                int indexFound = 0;
                newTask.TargetedFields = new List<TargetedField>();

                for (var index = 0; index < operationDictionary.Count; index++)
                {
                    if (operationDictionary[index].GetType() == typeof(AppendNameValuePair))
                    {
                        if (!foundAppendTask) indexFound = index;
                        foundAppendTask = true;
                        newTask.TargetedFields.AddRange(((AppendNameValuePair)operationDictionary[index]).TargetedFields);
                        ((AppendNameValuePair)operationDictionary[index]).TargetedFields = new TargetedField[0];
                    }
                }
                //if (foundAppendTask) operationDictionary[indexFound] = newTask;
                if (!foundAppendTask)
                    throw new ArgumentException("At least one append task must be defined with passing conditions");

                operationDictionary[indexFound] = newTask;


                //To perform all the transformations
                for (var index = 0; index < operationDictionary.Count; index++)
                {
                    IDataTransformationTask task = operationDictionary[index];
                    outputDictionary = task.Transform(outputDictionary, ref log);
                }



                //log.Append("\r\n Removing fields \r\n");
                //((IDataTransformationTask)newTask).Transform(outputDictionary, ref log);
                //log.Append("\r\n - Executed all the Tasks \r\n");

                if (outputDictionary != null)
                {
                    log.Append("Raw data after transformation:");
                    foreach (KeyValuePair<string, object> item in outputDictionary)
                    {
                        log.Append(item.Key + " = " + item.Value + "|");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Append(" - Error while transforming data, " + ex.Message + "\r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return outputDictionary;
        }

        ///// <summary>
        ///// Updates condition data into Database
        ///// </summary>
        ///// <param name="ConditionXMLEntity">Data to be updated</param>        
        ///// <returns>returns TRUE if success, else FALSE</returns>
        //bool IDataTransformationBC.UpdateConditionXml(ConditionXmlEntity data)
        //{
        //    return _DataTransformationDAO.UpdateConditionXml(data);
        //}

        ///// <summary>
        ///// Saves data into Database
        ///// </summary>
        ///// <param name="IDataTransformationEntity">Data to be updated</param>        
        ///// <returns>returns TRUE if success, else FALSE</returns>
        //bool IDataTransformationBC.UpdateData(DataTransformationEntity data)
        //{
        //    return _DataTransformationDAO.UpdateData(data);
        //}

        ///// <summary>
        ///// To update sequence number
        ///// </summary>
        ///// <param name="data">Data to be updated</param>        
        ///// <returns>returns TRUE if success, else FALSE</returns>
        //bool IDataTransformationBC.UpdateSequenceNo(string data)
        //{
        //    return _DataTransformationDAO.UpdateSequenceNo(data);
        //}

        ///// <summary>
        ///// Updates task data into Database
        ///// </summary>
        ///// <param name="TaskXMLEntity">Data to be updated</param>        
        ///// <returns>returns TRUE if success, else FALSE</returns>
        //bool IDataTransformationBC.UpdateTaskXml(TaskXmlEntity data)
        //{
        //    return _DataTransformationDAO.UpdateTaskXml(data);
        //}

        #endregion

        # region Private Methods

        /// <summary>
        /// After retrieving XML Conditions and XML Tasks from database.
        /// Based on condition XML result, task XML will be passed to created task instance.
        /// Created task instance will be added to operation dictionary 
        /// </summary>
        /// <param name="leadData">LeadData dictionary will be passed as input</param>
        /// <returns>Created operation dictionary will be returned</returns>
        private Dictionary<int, IDataTransformationTask> CreateOperationDictionary(Dictionary<string, object> conditionData, EventType eventType, int eventId, ref StringBuilder log)
        {
            Dictionary<int, IDataTransformationTask> operationDictionary = null;
            int index = 0;
            try
            {
                if (conditionData == null)
                {
                    return operationDictionary;
                }

                //log.Append(" - Fetching data from DB \r\n");
                //Get all data from database as data table                
                DataTable dataTable = DataTransformationDataService.DataTransformationDAO.LoadDataTransformationTable(eventType, eventId);
                //log.Append(" - Data Fetched \r\n");

                if (dataTable == null || conditionData.Count == 0)
                {
                    log.Append(" - No data found in DB \r\n");
                    return operationDictionary;
                }

                //IConditionService conditionService = new ConditionService();
                IConditionBc conditionService = new ConditionBc();
                operationDictionary = new Dictionary<int, IDataTransformationTask>();
                foreach (DataRow row in dataTable.Rows)
                {
                    //Check the condition pass for each task if not leave the task else add to operationDictionary
                    bool isConditionMet = false;
                    Result result = null;

                    //ServiceCall(wcfIE => result = wcfIE.IsConditionMet(conditionData, row["ConditionXML"].ToString()));
                    result = conditionService.IsConditionMet(conditionData, row["ConditionXML"].ToString());

                    if (result != null)
                    {
                        isConditionMet = result.IsConditionMet;
                        if (isConditionMet)
                        {
                            log.Append(result.Log.ToString());
                        }
                    }

                    if (isConditionMet)
                    {
                        //log.Append(" - Condition passed \r\n\r\n");
                        log.Append("Task Id " + row["Id"] + ", Condition passed.Task added. \r\n");

                        string taskXmlString = row["TaskXML"].ToString();

                        string className = GetXmlNode(taskXmlString).FirstChild.Name;
                        taskXmlString = "<Tasks>" + taskXmlString + "</Tasks>";
                        Tasks tasks = BaseXmlEntity.Deserialize<Tasks>(taskXmlString);
                        IDataTransformationTask task = null;

                        //Depending on the task (class name) add different task  to tasks
                        switch (className.ToLower())
                        {
                            case "appendnamevaluepair":
                                task = (IDataTransformationTask)tasks.AppendNameValuePair;
                                break;

                            case "changefieldnames":
                                task = (IDataTransformationTask)tasks.ChangeFieldNames;
                                break;

                            case "fieldvaluemapping":
                                task = (IDataTransformationTask)tasks.FieldValueMapping;
                                break;

                            case "removefields":
                                task = (IDataTransformationTask)tasks.RemoveFields;
                                break;

                            case "formattelephonenumber":
                                task = (IDataTransformationTask)tasks.FormatTelephoneNumber;
                                break;
                            case "formatdatetime":
                                task = (IDataTransformationTask)tasks.FormatDateTime;
                                break;
                            case "applyxsltforxml":
                                int id = (int)row["Id"];
                                tasks.ApplyXsltForXml.Xsl = DataTransformationDataService.DataTransformationDAO.GetXSLById(id);
                                task = (IDataTransformationTask)tasks.ApplyXsltForXml;
                                break;
                            case "setemailto":
                                task = (IDataTransformationTask)tasks.SetEmailTo;
                                break;
                            case "setemailsubject":
                                task = (IDataTransformationTask)tasks.SetEmailSubject;
                                break;
                        }

                        if (task != null)
                        {
                            //add to dictionary
                            operationDictionary.Add(index, task);
                            //log.Append(" - " + className + " class added to operationDictionary \r\n");
                            index++;
                        }
                    }
                    else
                    {
                        log.Append("Task Id " + row["Id"] + ", Condition failed \r\n");
                    }
                }

            }
            catch (Exception ex)
            {
                log.Append(" - Error occured while processing conditions \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }

            return operationDictionary;
        }

        /// <summary>
        /// Converts the XML string to XML Node
        /// </summary>
        /// <param name="xmlString">XML string will be passed as input</param>
        /// <returns>XMLDocument will be returned</returns>
        private XmlDocument GetXmlNode(string xmlString)
        {
            XmlDocument taskNode = null;
            try
            {
                if (!string.IsNullOrEmpty(xmlString))
                {
                    taskNode = new XmlDocument();
                    taskNode.LoadXml(xmlString);
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }

            return taskNode;
        }

        /// <summary>
        /// To merge raw data with lead data
        /// </summary>
        /// <param name="rawData">raw data dictionary</param>
        /// <param name="leadData">lead data dictionary</param>
        /// <returns>Merged dictionary</returns>
        private Dictionary<string, object> MergeRawDataWithLeadData(Dictionary<string, object> rawData, Dictionary<string, object> leadData)
        {
            Dictionary<string, object> output = null;
            try
            {

                if (rawData != null && leadData != null)
                {
                    if (rawData.Count == 0 && leadData.Count == 0)
                    {
                        return null;
                    }

                    if (rawData.Count != 0 && leadData.Count == 0)
                    {
                        output = rawData;
                    }
                    else if (rawData.Count == 0 && leadData.Count != 0)
                    {
                        output = leadData;
                    }
                    else
                    {
                        //if Key already present in Lead data then will leave as it is
                        output = leadData;
                        foreach (KeyValuePair<string, object> item in rawData)
                        {
                            if (!output.ContainsKey(item.Key))
                            {
                                output.Add(item.Key, item.Value);
                            }
                        }
                    }
                }
                else
                {
                    if (rawData == null && leadData != null)
                    {
                        return leadData;
                    }
                    if (rawData != null)
                    {
                        return rawData;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return output;
        }

        ///// <summary>
        ///// To call WCF service thru Channel Factory method
        ///// </summary>
        ///// <param name="codeBlock"></param>
        ///*private void ServiceCall(UseServiceDelegate<IConditionService> codeBlock)
        //{
        //    try
        //    {
        //        //Calls the WCF service thru Channel Factory method
        //        (new WCFProxyHelper<IConditionService>()).Call(codeBlock, "BasicHttpBinding_IConditionService");
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(
        //            CheetahExceptionHelper.AddArgumentsToException(
        //            ex, 1, MessageType.FatalError,
        //            codeBlock),
        //        CheetahExceptionPolicyHelper.BUSINESS_POLICY);
        //        throw;
        //    }
        //}*/

        //public DataTable GetELRNexusMappingData(DataTable datatable)
        //{
        //    DataTable dataTable = null;
        //    try
        //    {
        //        IDataTransformationDAO DataTransformationDAO = new DataTransformationDAO();
        //        dataTable =  DataTransformationDAO.GetELRNexusMappingData(datatable);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
        //        throw;
        //    }
        //    return dataTable;
        //}

        #endregion

        #region Added By Bala on 08/19/2011
        //DataTransformationEntityNew IDataTransformationBC.GetDataTransformationTask(DTConditionparamEntity cp)
        //{
        //    return _DataTransformationDAO.GetDataTransformationTask(cp);
        //}
        //public ConditionSelectEntity GetDataTransformationCondition(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.GetDataTransformationCondition(ce);
        //}
        //public bool RemoveDataTransformationCondition(DTConditionparamEntity ce)
        //{

        //    return _DataTransformationDAO.RemoveDataTransformationCondition(ce);
        //}
        //public DTConditionparamEntity AddUpdateDataTransformationCondition(DTConditionparamEntity ce)
        //{

        //    return _DataTransformationDAO.AddUpdateDataTransformationCondition(ce);
        //}
        //public DTConditionparamEntity AddUpdateDataTransformationTasks(DTConditionparamEntity ce)
        //{

        //    return _DataTransformationDAO.AddUpdateDataTransformationTasks(ce);
        //}
        //public List<AppendNameValueTaskItem> GetAppendNameValueTasks(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.GetAppendNameValueTasks(ce);
        //}
        //public TaskSelectEntity GetDataTransformationAppendNameValueTask(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.GetDataTransformationAppendNameValueTask(ce);

        //}
        //public bool DTCheckDuplicateSequence(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.DTCheckDuplicateSequence(ce);
        //}
        //public List<AppendNameValueTaskItem> GetFieldFormatTasks(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.GetFieldFormatTasks(ce);
        //}
        //public Dictionary<string, string> GetDataTranformationFormats(string DateOrPhone)
        //{
        //    return _DataTransformationDAO.GetDataTranformationFormats(DateOrPhone);
        //}
        //public List<AppendNameValueTaskItem> GetXsltXmlTasks(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.GetXsltXmlTasks(ce);
        //}

        //public DTConditionparamEntity AddUpdateXsltXmlTasks(DTConditionparamEntity ce)
        //{

        //    return _DataTransformationDAO.AddUpdateXsltXmlTasks(ce);
        //}
        //public XsltXmlEntity GetXsltXmlTask(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.GetXsltXmlTask(ce);
        //}
        //public List<AppendNameValueTaskItem> GetEmailSubjectAndEmailToTasks(DTConditionparamEntity ce)
        //{
        //    return _DataTransformationDAO.GetEmailSubjectAndEmailToTasks(ce);
        //}
        #endregion

    }
}
