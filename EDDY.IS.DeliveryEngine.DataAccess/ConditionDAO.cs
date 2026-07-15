using System;
using System.Data;
using System.Data.Common;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.DataAccess.Common;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DataAccess.Common;
//using EDDY.Nexus.DataAccess.Interface.ConditionValidator;
//using EDDY.Nexus.Entity.ConditionValidator;

namespace EDDY.IS.DeliveryEngine.DataAccess
{
    public class ConditionDao : BaseDataSource, IConditionDao
    {
        #region Public Methods
        /// <summary>
        /// Gets comparison data from custom stored procedure SQL server
        /// </summary>
        /// <param name="storedProcedureName">name of stored procedure</param>
        /// <param name="parameters">parameters collection</param>
        /// <returns>Returns DataTable with DataTransformation data</returns>
        DataTable IConditionDao.GetComparisonTable(string storedProcedureName, FtcParameter[] parameters)
        {
            DataTable dataTable = null;
            DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                //create command using stored proc name
                dbCommand = DatabaseUtilities.CreateDbCommand(storedProcedureName, Db);

                //Add parameters
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        Db.AddInParameter(dbCommand, parameter.Name,
                                            EntityTypeUtility.getDBTypeFromEntityDataType(parameter.DataType),
                                            EntityTypeUtility.getObjectFromFTCParameter(parameter));
                    }
                }

                //execute command and return data table
                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    dataTable = new DataTable();
                    dataTable.Load(dataReader);
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }

            return dataTable;
        }



        /// <summary>
        /// Gets DataTransformaiton data from SQL server
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns>Returns DataTable with DataTransformation data</returns>
        DataTable IConditionDao.GetXmlTable(EventType eventType)
        {
            DataTable dataTable = null;
            DbCommand dbCommand = null;
            IDataReader dataReader = null;

            try
            {
                dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_DT_Task_Select", Db);                
                dataReader = Db.ExecuteReader(dbCommand);
                if (dataReader != null)
                {
                    dataTable = new DataTable();
                    dataTable.Load(dataReader);
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            finally
            {
                DatabaseUtilities.CloseAndDispose(ref dbCommand, ref dataReader);
            }
            return dataTable;
        }
        
        #endregion
    }
}
