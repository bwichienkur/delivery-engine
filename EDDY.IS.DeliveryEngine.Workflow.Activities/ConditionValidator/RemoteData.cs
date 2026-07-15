using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
//using EDDY.Nexus.BusinessComponent.Interface.ConditionValidator;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.ConditionValidator;
//using EDDY.Nexus.DataAccess.Interface.ConditionValidator;
//using EDDY.Nexus.Entity.ConditionValidator;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator
{
    public class RemoteData : IRemoteData
    {


        #region Constructor
        /// <summary>
        /// To initialize members
        /// </summary>
        /// <param name="remoteDataObjectName"></param>
        /// <param name="remoteDataType"></param>
        /// <param name="parameters"></param>
        /// <param name="returnDataType"></param>
        public RemoteData(string remoteDataObjectName, DataSourceType remoteDataType, FtcParameter[] parameters, DataType returnDataType)
        {
            RemoteDataObjectName = remoteDataObjectName;
            RemoteDataType = remoteDataType;
            Parameters = parameters;
            ReturnDataType = returnDataType;
        }

        #endregion


        #region Properties
        public FtcParameter[] Parameters { get; set; }  //collection of parameters for the sp/ws/class
        public string RemoteDataObjectName { get; set; } //Name of storedProc, webService or Class
        public DataSourceType RemoteDataType { get; set; } //storedProc, webService or Class?
        public DataType ReturnDataType { get; set; } //type of data being returned

        #endregion

        #region Public Methods
        /// <summary>
        /// To create instance of an IDataClass based object using reflection
        /// </summary>
        /// <param name="className">class to instantiate</param>
        /// <returns>Returns IDataClass instance</returns>
        private static IDataClass CreateRemoteClassFromAssembly(string className)
        {
            var classNameWithNamespace = "EDDY.Nexus.BusinessComponent.ConditionValidator." + className;

            try
            {
                //get remote conditions assembly classes path
                //string assemblyPath = System.AppDomain.CurrentDomain.RelativeSearchPath + @"\EDDY.Cheetah.ConditionValidator.BusinessComponent.dll";

                string assemblyPath = ConfigurationManager.AppSettings["ReferenceLibraryPath"] + @"\EDDY.Nexus.BusinessComponent.dll";
                
                //load assembly
                Assembly asm = Assembly.LoadFile(assemblyPath);

                // create the actual type
                Type classType = asm.GetType(classNameWithNamespace, true, false);

                // Create an instance of this type and verify that it exists
                var retObj = (IDataClass)Activator.CreateInstance(classType);

                //return created object
                return retObj;
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
        }

        /// <summary>
        /// Returns an object (list or single value) for condtion comparison
        /// </summary>
        /// <returns>object</returns>
        public object GetData() 
        {
            object returnObject = null;

            try
            {

                switch (RemoteDataType)
                {
                    case DataSourceType.Class:
                        {
                            returnObject = GetClassList();
                            break;
                        }
                    case DataSourceType.StoredProcedure:
                        {
                            returnObject = GetStoredProcedureComparisonObject();
                            break;
                        }
                    case DataSourceType.Webservice:
                        {
                            returnObject = GetWebserviceList();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

            return returnObject;
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Returns an object (list or single value) from a custom class for condition comparison
        /// </summary>
        /// <returns>object</returns>
        private object GetClassList()
        {
            try
            {
                //invoke object using name for remoteData class from ftc
                IDataClass dataClass = CreateRemoteClassFromAssembly(RemoteDataObjectName);
                return dataClass.GetData(Parameters);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
        }

        /// <summary>
        /// Returns an object (list or single value) from a dataTable
        /// </summary>
        /// <returns>object</returns>
        private static object GetComparisonObjectFromTable(DataType returnDataType, DataTable storedProcResultsTable)
        {
            object returnObject = null;

            try
            {
                //make sure dataType is provided
                if (returnDataType == DataType.Other || returnDataType == DataType.Unassigned)
                {
                    return returnObject;
                }

                //make sure Data is provided
                if (storedProcResultsTable == null || storedProcResultsTable.Rows.Count == 0)
                {
                    return returnObject;
                }

                if (storedProcResultsTable.Rows.Count == 1) //check if its a single value
                {
                    switch (returnDataType)
                    {
                        case DataType.StringData:
                            {
                                returnObject = (storedProcResultsTable.Rows[0][0]).ToString();
                                break;
                            }
                        case DataType.IntData:
                            {
                                returnObject = Convert.ToInt32(storedProcResultsTable.Rows[0][0]);
                                break;
                            }
                        //case DataType.DoubleData:
                        //    {
                        //        returnObject = Convert.ToDouble(storedProcResultsTable.Rows[0][0]);
                        //        break;
                        //    }
                        case DataType.DecimalData:
                            {
                                returnObject = Convert.ToDecimal(storedProcResultsTable.Rows[0][0]);
                                break;
                            }
                        case DataType.BooleanData:
                            {
                                returnObject = Convert.ToBoolean(storedProcResultsTable.Rows[0][0]);
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
                else //otherwise its a list of values
                {
                    var objList = new List<object>();

                    switch (returnDataType)
                    {
                        case DataType.StringData:
                            {
                                for (int index = 0; index < storedProcResultsTable.Rows.Count - 1; index++)
                                {
                                    objList.Add(storedProcResultsTable.Rows[index][0].ToString());
                                }
                                break;
                            }
                        case DataType.IntData:
                            {
                                for (int index = 0; index < storedProcResultsTable.Rows.Count - 1; index++)
                                {
                                    objList.Add(Convert.ToInt32(storedProcResultsTable.Rows[index][0]));
                                }
                                break;
                            }
                        //case DataType.DoubleData:
                        //    {
                        //        for (int index = 0; index < storedProcResultsTable.Rows.Count - 1; index++)
                        //        {
                        //            objList.Add(Convert.ToDouble(storedProcResultsTable.Rows[index][0]));
                        //        }
                        //        break;
                        //    }
                        case DataType.DecimalData:
                            {
                                for (int index = 0; index < storedProcResultsTable.Rows.Count - 1; index++)
                                {
                                    objList.Add(Convert.ToDecimal(storedProcResultsTable.Rows[index][0]));
                                }
                                break;
                            }
                        case DataType.BooleanData:
                            {
                                for (int index = 0; index < storedProcResultsTable.Rows.Count - 1; index++)
                                {
                                    objList.Add(Convert.ToBoolean(storedProcResultsTable.Rows[index][0]));
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
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

            return returnObject;
        }

        /// <summary>
        /// Returns an object (list or single value) from a stored proc for condition comparison
        /// </summary>
        /// <returns>object</returns>
        private object GetStoredProcedureComparisonObject()
        {
            //object to return
            object returnObject;

            try
            {
                //call stored procedure and get data table (storedProcResultsTable)
                //return table from stored Proc
                DataTable storedProcResultsTable = ConditionDataService.ConditionDao.GetComparisonTable(RemoteDataObjectName, Parameters);

                //Get object to return from the table
                returnObject = GetComparisonObjectFromTable(ReturnDataType, storedProcResultsTable);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }

            return returnObject;
        }


        /// <summary>
        /// Returns an object (list or single value) from a web service for condition comparison
        /// </summary>
        /// <returns>object</returns>
        private object GetWebserviceList()
        {
            object returnObject;

            try
            {
                //return table from stored Proc
                var webServiceResultsTable = new DataTable();

                //ToDo: invoke web service and return object.

                //Get object to return from the table
                returnObject = GetComparisonObjectFromTable(ReturnDataType, webServiceResultsTable);
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }

            return returnObject;
        }
        

        #endregion

    }
}
