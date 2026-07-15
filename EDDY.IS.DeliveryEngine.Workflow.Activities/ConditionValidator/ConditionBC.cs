using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
//using EDDY.Nexus.BusinessComponent.Interface.ConditionValidator;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity.Common;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
using EDDY.Nexus.Common.Logging;
//using EDDY.Nexus.DataAccess.ConditionValidator;
//using EDDY.Nexus.DataAccess.Interface.ConditionValidator;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.ConditionValidator;


namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator
{
    public class ConditionBc : IConditionBc
    {
        #region Private Variables
                
        readonly StringBuilder _log;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor to initialize ConditionDAO
        /// </summary>
        public ConditionBc()
        {            
            _log = new StringBuilder();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks a given data list against a all XML conditions in the database.
        /// </summary>
        /// <param name="leadData">String value dictionary containing data to validate against.</param>
        /// <param name="eventType">Event Type upon which to filter Conditions.</param> 
        /// <returns>DataTable: Contains a record for each rule that applies.</returns>
        List<MatchingCondition> IConditionBc.GetMatchingConditions(Dictionary<string, object> leadData, EventType eventType)
        {
            List<MatchingCondition> matchingConditions = null;

            try
            {               
                if (leadData == null)
                {
                    return matchingConditions;
                }

                //Filter records using leadData object
                DataTable dataTable = GetXmlTable(eventType);

                if (dataTable == null)
                {
                    return matchingConditions;
                }

                matchingConditions = new List<MatchingCondition>();
                //Build Return Table with a row for each condition that does not apply
                foreach (DataRow row in dataTable.Rows)
                {
                    var condition = BaseXmlEntity.Deserialize<Condition>(row["ConditionXML"].ToString(), null);
                    bool isConditionMet = AreCondtionsMet(condition.FieldsToCompare, leadData);
                                       
                    if (isConditionMet)
                    {
                        var item = new MatchingCondition
                                       {
                                           ConditionId = (int) row[0],
                                           ConditionName = row[1].ToString(),
                                           ConditionEventId = (int) row[2],
                                           ConditionXML = row[3].ToString(),
                                           ConditionMet = true
                                       };

                        matchingConditions.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
              
            }
            return matchingConditions;
        }

        /// <summary>
        /// Checks a given data list against an XML condition.
        /// </summary>
        /// <param name="leadData">String value dictionary containing data to validate against.</param>
        /// <param name="xmlNode">XML string defining condition to check.</param>
        /// <returns>bool: TRUE if condition met, else FALSE</returns>
        Result IConditionBc.IsConditionMet(Dictionary<string, object> leadData, string xmlNode)
        {
            Result result;
            try
            {
                //_Log.Append(" - Condition check started \r\n");
                result = new Result();

                //Check for empty string or NULL condition (always true)
                if (string.IsNullOrEmpty(xmlNode))
                {
                    result.IsConditionMet = true;
                    _log.Append(" - No Condition supplied. IsConditionMet returning True \r\n");
                    result.Log = _log;
                }
                else //we have a condtion to test
                {
                    var condition = BaseXmlEntity.Deserialize<Condition>(xmlNode, null);

                    //bool blnResult = AreCondtionsMet(condition.FieldsToCompare, leadData);

                    /* The above line is commented and Modified as below By Bala */

                    bool blnResult = true;
                    if(condition.FieldsToCompare!=null) blnResult = AreCondtionsMet(condition.FieldsToCompare, leadData);
                    else _log.Append(" - No Condition supplied. IsConditionMet returning True \r\n");

                    /*Modification End */


                    //_Log.Append(" - Condition check end \r\n");
                    result.IsConditionMet = blnResult;
                    result.Log = _log;
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return result;
        }
        
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks whether all conditions are met or not
        /// </summary>
        /// <param name="fieldsToCompare"></param>
        /// <param name="leadData"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool AreCondtionsMet(FieldToCompare[] fieldsToCompare, IDictionary<string, object> leadData)
        {
            bool logDebug = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDebugMode"], out logDebug);
            string leadDataStr = string.Empty;

            if (logDebug)
            {
                foreach (KeyValuePair<string, object> keyValuePair in leadData)
                {
                    leadDataStr += keyValuePair.Key + "-" + (keyValuePair.Value == null ? string.Empty : keyValuePair.Value.ToString()) + "|";
                }
            }

            bool result = false;
            try
            {
                foreach (FieldToCompare fieldToCompare in fieldsToCompare)
                {

                    //Hack to convert csr to cr
                    if (fieldToCompare.FieldName.ToLower() == "csrid") fieldToCompare.FieldName = "CRId";
                    result = CheckFieldToCompareAgainstData(fieldToCompare, leadData);

                    if (result == false)
                    {
                        if (logDebug)
                            DeliveryEngineDataService.DeliveryEngineDAO.TempLog(leadData != null ? Int32.Parse(leadData["LeadId"].ToString()) : 0, 10, 0,
                                   "Failed to match condition: " + fieldToCompare.FieldName.ToLower() + " with lead data: " + leadDataStr, 1, false);

                        return false;
                    }

                    if (logDebug)
                        DeliveryEngineDataService.DeliveryEngineDAO.TempLog(leadData != null ? Int32.Parse(leadData["LeadId"].ToString()) : 0, 10, 0,
                                   "Matched condition: " + fieldToCompare.FieldName.ToLower() + " with lead data: " + leadDataStr, 1, false);
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }
            return result;
        }

        /// <summary>
        /// To decide which type of comparison to use
        /// </summary>
        /// <param name="fieldToCompare">FieldToCompare class instance</param>
        /// <param name="leadDataDictionary">LeadData to compare</param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool CheckFieldToCompareAgainstData(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            bool result = false;
            try
            {
                switch (fieldToCompare.RelOperator)
                {
                    case RelOperator.Equals:
                        {
                            result = ValueEqualToData(fieldToCompare, leadDataDictionary);
                            break;
                        }
                    case RelOperator.In:
                        {
                            result = ValueExistsInData(fieldToCompare, leadDataDictionary);
                            break;
                        }
                    case RelOperator.LessThan:
                        {
                            result = ValueLessThanData(fieldToCompare, leadDataDictionary);
                            break;
                        }
                    case RelOperator.LessThanOrEqualTo:
                        {
                            result = ValueLessThanOrEqualToData(fieldToCompare, leadDataDictionary);
                            break;
                        }
                    case RelOperator.GreaterThan:
                        {
                            result = ValueGreaterThanData(fieldToCompare, leadDataDictionary);
                            break;
                        }
                    case RelOperator.GreaterThanOrEqualTo:
                        {
                            result = ValueGreaterThanOrEqualToData(fieldToCompare, leadDataDictionary);
                            break;
                        }
                    case RelOperator.RegularExpression:
                        {
                            result = (bool)leadDataDictionary["IsRepost"] || ValueMatchesRegularExpression(fieldToCompare, leadDataDictionary);
                            break;
                        }
                    case RelOperator.Other:
                        {
                            break;
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
        /// Decides local/remote datasource type
        /// </summary>
        /// <param name="ftc">FieldToCompare instance containing datasource type</param>
        /// <returns>object</returns>
        private object GetFieldToCompareObject(FieldToCompare ftc)
        {
            //this method will return an object, or list of objects based on the dataType and Data

            try
            {
                switch (ftc.DataSourceType)
                {
                    case DataSourceType.Local:
                        {
                            return(GetFieldToCompareObject_Local(ftc));
                        }
                    case DataSourceType.StoredProcedure:
                    case DataSourceType.Webservice:
                    case DataSourceType.Class:
                        {
                            return(GetFieldToCompareObject_Remote(ftc));
                        }
                    default:
                        {
                            return null;
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                //return null;
                throw;
            }
        }

        /// <summary>
        /// Gets object from given data in XML.
        /// </summary>
        /// <param name="ftc"></param>
        /// <returns>object</returns>
        private object GetFieldToCompareObject_Local(FieldToCompare ftc)
        {
            //this method will return an object, or list of objects based on the dataType and Data from the XML
            object returnObject = null;

            try
            {
                //make sure dataType is provided
                if (ftc.DataType == DataType.Other || ftc.DataType == DataType.Unassigned)
                {
                    return returnObject;
                }

                //make sure Data is provided
                if (string.IsNullOrEmpty(ftc.Data))
                {
                    return returnObject;
                }

                if (!ftc.HasMultipleDataVals) //check if its a single values (no commas)
                {
                    switch (ftc.DataType)
                    {
                        case DataType.StringData:
                            {
                                returnObject = ftc.Data;
                                break;
                            }
                        case DataType.IntData:
                            {
                                returnObject = Convert.ToInt32(ftc.Data);
                                break;
                            }
                        //case DataType.DoubleData:
                        //    {
                        //        returnObject = Convert.ToDouble(ftc.Data);
                        //        break;
                        //    }
                        case DataType.DecimalData:
                            {
                                returnObject = Convert.ToDecimal(ftc.Data);
                                break;
                            }
                        case DataType.BooleanData:
                            {
                                returnObject = Convert.ToBoolean(ftc.Data);
                                break;
                            }
                        //case DataType.Other:
                        //case DataType.Unassigned:
                        default:
                            {
                                break;
                            }
                    }
                }
                else //otherwise its a list of values (contains at least one comma)
                {
                    string[] dataStrings = null;

                    dataStrings = ftc.Data.Split(ftc.Data.Contains("|") ? '|' : ',');
                    var objList = new List<object>();

                    switch (ftc.DataType)
                    {
                        case DataType.StringData:
                            {
                                for (int index = 0; index < dataStrings.Length; index++)
                                {
                                    objList.Add(dataStrings[index]);
                                }
                                break;
                            }
                        case DataType.IntData:
                            {
                                for (int index = 0; index < dataStrings.Length; index++)
                                {
                                    objList.Add(Convert.ToInt32(dataStrings[index]));
                                }
                                break;
                            }
                        //case DataType.DoubleData:
                        //    {
                        //        for (int index = 0; index < dataStrings.Length; index++)
                        //        {
                        //            objList.Add(Convert.ToDouble(dataStrings[index]));
                        //        }
                        //        break;
                        //    }
                        case DataType.DecimalData:
                            {
                                for (int index = 0; index < dataStrings.Length; index++)
                                {
                                    objList.Add(Convert.ToDecimal(dataStrings[index]));
                                }
                                break;
                            }
                        case DataType.BooleanData:
                            {
                                for (int index = 0; index < dataStrings.Length; index++)
                                {
                                    objList.Add(Convert.ToBoolean(dataStrings[index]));
                                }
                                break;
                            }
                        //case DataType.Other:
                        //case DataType.Unassigned:
                        default:
                            {
                                break;
                            }
                    }

                    returnObject = objList;
                }

            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);
                
            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }

            return returnObject;
        }

        /// <summary>
        /// Gets object from remote data source (i.e. class, stored procedure or web service)
        /// </summary>
        /// <param name="ftc"></param>
        /// <returns>object</returns>
        private static object GetFieldToCompareObject_Remote(FieldToCompare ftc)
        {
            //this method will return an object, or list of objects based on the dataType and Data returned from the specified class
            object returnObject;

            try
            {
                //instansiate RemoteData class and pass fieldToCompare vals
                var remotedata = new RemoteData(ftc.Data, ftc.DataSourceType, ftc.FTCParameters, ftc.DataType);

                //call getdata and assign results to returnObject
                returnObject = remotedata.GetData();
                
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }

            return returnObject;
        }

        /// <summary>
        /// Gets DataTransformaiton data from SQL server
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetXmlTable(EventType eventType)
        {
            try
            {
                return ConditionDataService.ConditionDao.GetXmlTable(eventType);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
        }
        
        /// <summary>
        /// Checks if value in leadDataDictionary is equal to fieldToCompare value
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool ValueEqualToData(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            try
            {
                //FieldToCompare Cannot Be Null
                if (fieldToCompare == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //leadDataDictionary Cannot Be Null
                if (leadDataDictionary == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //first see if key exists in dictionary
                if (!leadDataDictionary.ContainsKey(fieldToCompare.FieldName))
                {
                    return false;
                }

                //This Operation cannot not compare to multiple values
                if (fieldToCompare.HasMultipleDataVals)
                {
                    //throw exception? Log error?
                    return false;
                }

                //get object value from dictionary to compare
                object dataFromDictionary = leadDataDictionary[fieldToCompare.FieldName];



                //convert FieldToCompare to the correct type, and compare
                switch (fieldToCompare.DataType)
                {
                    case DataType.BooleanData:
                        {
                            return (Convert.ToBoolean(dataFromDictionary) == (bool)GetFieldToCompareObject(fieldToCompare));
                        }
                    case DataType.StringData:
                        {
                            return (dataFromDictionary.ToString() == (string)GetFieldToCompareObject(fieldToCompare));
                        }
                    case DataType.IntData:
                        {
                            return (Convert.ToInt32(dataFromDictionary) == (int)GetFieldToCompareObject(fieldToCompare));
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        return (Convert.ToDouble(dataFromDictionary) == (double)GetFieldToCompareObject(fieldToCompare));
                    //    }
                    case DataType.DecimalData:
                        {
                            return (Convert.ToDecimal(dataFromDictionary) == (decimal)GetFieldToCompareObject(fieldToCompare));
                        }
                    case DataType.Other:
                    case DataType.Unassigned:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                }
            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);
                
            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }
            return false;
        }

        /// <summary>
        /// Checks if value exists in fieldToCompare values
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool ValueExistsInData(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            try
            {
                //FieldToCompare Cannot Be Null
                if (fieldToCompare == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //leadDataDictionary Cannot Be Null
                if (leadDataDictionary == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //first see if key exists in dictionary
                if (!leadDataDictionary.ContainsKey(fieldToCompare.FieldName))
                {
                    return false;
                }

                //get object value from dictionary to compare
                object itemFromLeadDataDictionary = leadDataDictionary[fieldToCompare.FieldName];

                //Get object list
                List<object> objList;

                //both multiple local val and remote data will return list
                if (fieldToCompare.HasMultipleDataVals || fieldToCompare.DataSourceType != DataSourceType.Local) 
                {
                    objList = (List<object>)GetFieldToCompareObject(fieldToCompare);
                }
                else //singular object, put it in a new list
                {
                    objList = new List<object> {GetFieldToCompareObject(fieldToCompare)};
                }
                

                switch (fieldToCompare.DataType)
                {
                    case DataType.BooleanData:
                        {
                            //check for string - may have to convert
                            if (itemFromLeadDataDictionary.GetType() == typeof(string))
                            {
                                itemFromLeadDataDictionary = Convert.ToBoolean(itemFromLeadDataDictionary);
                            }
                            //check for value
                            return objList.Contains((bool)itemFromLeadDataDictionary);
                        }
                    case DataType.StringData:
                        {
                            //check for value
                            return objList.Contains(itemFromLeadDataDictionary.ToString());
                        }
                    case DataType.IntData:
                        {
                            //check for string - may have to convert
                            if (itemFromLeadDataDictionary.GetType() == typeof(string))
                            {
                                itemFromLeadDataDictionary = Convert.ToInt32(itemFromLeadDataDictionary);
                            }
                            //check for value
                            return objList.Contains((int)itemFromLeadDataDictionary);
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        //check for string - may have to convert
                    //        if (itemFromLeadDataDictionary.GetType() == typeof(double))
                    //        {
                    //            itemFromLeadDataDictionary = Convert.ToDouble(itemFromLeadDataDictionary);
                    //        }
                    //        //check for value
                    //        return objList.Contains((int)itemFromLeadDataDictionary);
                    //    }
                    case DataType.DecimalData:
                        {
                            //check for string - may have to convert
                            if (itemFromLeadDataDictionary.GetType() == typeof(decimal))
                            {
                                itemFromLeadDataDictionary = Convert.ToDecimal(itemFromLeadDataDictionary);
                            }
                            //check for value
                            return objList.Contains((decimal)itemFromLeadDataDictionary);
                        }
                    case DataType.Other:
                    case DataType.Unassigned:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                }

            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);
                
            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }

            return false;
        }

        /// <summary>
        /// Checks if value in leadDataDictionary is greater than fieldToCompare value
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool ValueGreaterThanData(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            try
            {
                //FieldToCompare Cannot Be Null
                if (fieldToCompare == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //leadDataDictionary Cannot Be Null
                if (leadDataDictionary == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //first see if key exists in dictionary
                if (!leadDataDictionary.ContainsKey(fieldToCompare.FieldName))
                {
                    return false;
                }

                //This Operation cannot not compare to multiple values
                if (fieldToCompare.HasMultipleDataVals)
                {
                    //throw exception? Log error?
                    return false;
                }

                //get object value from dictionary to compare
                object dataFromDictionary = leadDataDictionary[fieldToCompare.FieldName];

                //convert FieldToCompare to the correct type, and compare
                switch (fieldToCompare.DataType)
                {
                    case DataType.BooleanData:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                    case DataType.StringData:
                        {
                            //throw exception? Log error? check alphabetical order for ValueGreaterThanData?
                            return false;
                        }
                    case DataType.IntData:
                        {
                            return (Convert.ToInt32(dataFromDictionary) > (int)GetFieldToCompareObject(fieldToCompare));
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        return (Convert.ToDouble(dataFromDictionary) > (double)GetFieldToCompareObject(fieldToCompare));
                    //    }
                    case DataType.DecimalData:
                        {
                            return (Convert.ToDecimal(dataFromDictionary) > (decimal)GetFieldToCompareObject(fieldToCompare));
                        }
                    case DataType.Other:
                    case DataType.Unassigned:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                }
            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);
                
            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }
            return false;
        }

        /// <summary>
        /// Checks if value in leadDataDictionary is greater than or equal to fieldToCompare value
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool ValueGreaterThanOrEqualToData(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            try
            {
                //FieldToCompare Cannot Be Null
                if (fieldToCompare == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //leadDataDictionary Cannot Be Null
                if (leadDataDictionary == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //first see if key exists in dictionary
                if (!leadDataDictionary.ContainsKey(fieldToCompare.FieldName))
                {
                    return false;
                }

                //This Operation cannot not compare to multiple values
                if (fieldToCompare.HasMultipleDataVals)
                {
                    //throw exception? Log error?
                    return false;
                }

                //get object value from dictionary to compare
                object dataFromDictionary = leadDataDictionary[fieldToCompare.FieldName];

                //convert FieldToCompare to the correct type, and compare
                switch (fieldToCompare.DataType)
                {
                    case DataType.BooleanData:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                    case DataType.StringData:
                        {
                            //throw exception? Log error? check alphabetical order for ValueGreaterThanOrEqualToData?
                            return false;
                        }
                    case DataType.IntData:
                        {
                            return (Convert.ToInt32(dataFromDictionary) >= (int)GetFieldToCompareObject(fieldToCompare));
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        return (Convert.ToDouble(dataFromDictionary) >= (double)GetFieldToCompareObject(fieldToCompare));
                    //    }
                    case DataType.DecimalData:
                        {
                            return (Convert.ToDecimal(dataFromDictionary) >= (decimal)GetFieldToCompareObject(fieldToCompare));
                        }
                    case DataType.Other:
                    case DataType.Unassigned:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                }
            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);
                
            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }
            return false;
        }

        /// <summary>
        /// Checks if value in leadDataDictionary is less than fieldToCompare value
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool ValueLessThanData(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            try
            {
                //FieldToCompare Cannot Be Null
                if (fieldToCompare == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //leadDataDictionary Cannot Be Null
                if (leadDataDictionary == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //first see if key exists in dictionary
                if (!leadDataDictionary.ContainsKey(fieldToCompare.FieldName))
                {
                    return false;
                }

                //This Operation cannot not compare to multiple values
                if (fieldToCompare.HasMultipleDataVals)
                {
                    //throw exception? Log error?
                    return false;
                }

                //get object value from dictionary to compare
                object dataFromDictionary = leadDataDictionary[fieldToCompare.FieldName];

                //convert FieldToCompare to the correct type, and compare
                switch (fieldToCompare.DataType)
                {
                    case DataType.BooleanData:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                    case DataType.StringData:
                        {
                            //throw exception? Log error? check alphabetical order for ValueLessThanData?
                            return false;
                        }
                    case DataType.IntData:
                        {
                            return (Convert.ToInt32(dataFromDictionary) < (int)GetFieldToCompareObject(fieldToCompare));
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        return (Convert.ToDouble(dataFromDictionary) < (double)GetFieldToCompareObject(fieldToCompare));
                    //    }
                    case DataType.DecimalData:
                        {
                            return (Convert.ToDecimal(dataFromDictionary) < (decimal)GetFieldToCompareObject(fieldToCompare));
                        }
                    case DataType.Other:
                    case DataType.Unassigned:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                }
            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);
                
            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }
            return false;
        }

        /// <summary>
        /// Checks if value in leadDataDictionary is less than or equal to fieldToCompare value
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool ValueLessThanOrEqualToData(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            try
            {
                //FieldToCompare Cannot Be Null
                if (fieldToCompare == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //leadDataDictionary Cannot Be Null
                if (leadDataDictionary == null)
                {
                    //throw exception? Log error?
                    return false;
                }

                //first see if key exists in dictionary
                if (!leadDataDictionary.ContainsKey(fieldToCompare.FieldName))
                {
                    return false;
                }

                //This Operation cannot not compare to multiple values
                if (fieldToCompare.HasMultipleDataVals)
                {
                    //throw exception? Log error?
                    return false;
                }

                //get object value from dictionary to compare
                object dataFromDictionary = leadDataDictionary[fieldToCompare.FieldName];

                //convert FieldToCompare to the correct type, and compare
                switch (fieldToCompare.DataType)
                {
                    case DataType.BooleanData:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                    case DataType.StringData:
                        {
                            //throw exception? Log error? check alphabetical order for ValueLessThanOrEqualToData?
                            return false;
                        }
                    case DataType.IntData:
                        {
                            return (Convert.ToInt32(dataFromDictionary) <= (int)GetFieldToCompareObject(fieldToCompare));
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        return (Convert.ToDouble(dataFromDictionary) <= (double)GetFieldToCompareObject(fieldToCompare));
                    //    }
                    case DataType.DecimalData:
                        {
                            return (Convert.ToDecimal(dataFromDictionary) <= (decimal)GetFieldToCompareObject(fieldToCompare));
                        }
                    case DataType.Other:
                    case DataType.Unassigned:
                        {
                            //throw exception? Log error?
                            return false;
                        }
                }
            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);
                
            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }
            return false;
        }

        /// <summary>
        /// Checks if value in leadDataDictionary is equal to the regular expression value
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool ValueMatchesRegularExpression(FieldToCompare fieldToCompare, IDictionary<string, object> leadDataDictionary)
        {
            try
            {                

                object dataFromDictionary = leadDataDictionary[fieldToCompare.FieldName];


                System.Text.RegularExpressions.Regex regExp = new System.Text.RegularExpressions.Regex(fieldToCompare.Data, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                return regExp.IsMatch(dataFromDictionary.ToString());
            }
            catch (InvalidCastException castEx)
            {
                _log.Append(" - Condition check type casting error  \r\n" + castEx.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(castEx, Policies.DATA_ACCESS_POLICY);

            }
            catch (Exception ex)
            {
                _log.Append(" - Condition check error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);

            }
            return false;
        }

        #endregion

    }
}
