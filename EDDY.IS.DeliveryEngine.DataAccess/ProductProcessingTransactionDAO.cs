using System;
using System.Collections.Generic;
using System.Data;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.Nexus.Common.Logging;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.DataAccess.Common;
//using EDDY.Nexus.DataAccess.Interface.ProductProcessing;
//using EDDY.Nexus.Entity.ProductProcessing;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EDDY.IS.DeliveryEngine.DataAccess.Common;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity.ProductProcessing;

namespace EDDY.IS.DeliveryEngine.DataAccess
{
    public class ProductProcessingTransactionDAO : BaseDataSource, IProductProcessingTransactionDAO
    {

        Database EddyTracking = null;
        public ProductProcessingTransactionDAO()
        {   
            string EddyTrackingDb = ConfigurationManager.AppSettings["EddyTrackingDb"].ToString();
            EddyTracking = DatabaseUtilities.CreateDatabase(EddyTrackingDb);
        }
        #region Public Methods

        /// <summary>
        /// Insert an activity step into the transaction database transaction detail table
        /// </summary>
        /// <returns>bool true/false on successful insert</returns>

        public bool InsertTransactionDetail(string transactionId, int activityStepId, string additionalInfo)
        {
            bool transactionSaved = false;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In ProductProcessingTransaction DAO - calling SP EDDYT_PPT_TransactionDetail_Insert to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "InsertTransactionDetail" }, { "ModuleName", "ProductProcessingTransaction.DAO" } });
                using (DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDYT_PPT_TransactionDetail_Insert", EddyTracking))
                {
                    EddyTracking.AddInParameter(dbcommand, "@TransactionId", DbType.Guid, new Guid(transactionId));
                    EddyTracking.AddInParameter(dbcommand, "@ActivityStepId", DbType.Int32, activityStepId);
                    EddyTracking.AddInParameter(dbcommand, "@AdditionalInfo", DbType.String, additionalInfo);
                    int retval = EddyTracking.ExecuteNonQuery(dbcommand); // Execute Stored Procedure
                    if (retval == 1)
                        transactionSaved = true;

                    if (dbcommand.Connection.State == ConnectionState.Open)
                    {
                        dbcommand.Connection.Close();
                    }

                }
            }
            catch(Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }
            
            return transactionSaved;
        }

        /// <summary>
        /// Saves by Inserting or Updating the transaction database - tranasaction summary table
        /// </summary>
        /// <returns>bool true/false on successful insert</returns>

        public bool SaveTransactionSummary(string transactionId, string trackingId, Int64 rawPostDataId,
                                           Int32 formValidationId, Int32 formId, Int64 leadId)
        {
            bool transactionSaved = false;

            try
            {
                //EDDYLogger.LogMessage(1, LogLevel.General, "In ProductProcessingTransaction DAO - calling SP EDDYT_PPT_TransactionSummary_Save to insert a record into database", null, new Dictionary<string, object> { { "ModuleAction", "SaveTransactionSummary" }, { "ModuleName", "ProductProcessingTransaction.DAO" } });
               
                using (DbCommand dbcommand = DatabaseUtilities.CreateDbCommand("dbo.EDDYT_PPT_TransactionSummary_Save", EddyTracking))
                {
                    EddyTracking.AddInParameter(dbcommand, "@TransactionId", DbType.Guid, new Guid(transactionId));
                    EddyTracking.AddInParameter(dbcommand, "@TrackingId", DbType.Guid, new Guid(trackingId));
                    EddyTracking.AddInParameter(dbcommand, "@RawPostDataId", DbType.Int64, rawPostDataId);
                    EddyTracking.AddInParameter(dbcommand, "@FormValidationId", DbType.Int32, formValidationId);
                    EddyTracking.AddInParameter(dbcommand, "@FormId", DbType.Int32, formId);
                    EddyTracking.AddInParameter(dbcommand, "@LeadId", DbType.Int64, leadId);

                    int retval = EddyTracking.ExecuteNonQuery(dbcommand); // Execute Stored Procedure
                    if (retval == 1)
                        transactionSaved = true;

                    if (dbcommand.Connection.State == ConnectionState.Open)
                    {
                        dbcommand.Connection.Close();
                    }

                }
            }
            catch(Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

           
            return transactionSaved;
        }


        /// <summary>
        /// Get TransactionSummary Data from SQL Server
        /// </summary>
        /// <returns></returns>
        public List<TransactionSummary> GetTransactionSummary()
        {
            List<TransactionSummary> dtList = null;
            try
            {
                //set the stored procedure for the command
                TransactionSummary iDtEntity = null;
                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDYT_PPT_TransactionSummary_Get", EddyTracking))
                {
                    //Execute the command get all data
                    using (IDataReader dataReader = EddyTracking.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            dtList = new List<TransactionSummary>();
                            //Load each record to TransactionSummaryEntity class and add the class to dtList
                            while (dataReader.Read())
                            {
                                iDtEntity = new TransactionSummary(dataReader);
                                dtList.Add(iDtEntity);
                            }

                            dataReader.Close();
                        }
                    }

                    if (dbCommand.Connection.State == ConnectionState.Open)
                    {
                        dbCommand.Connection.Close();
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
        /// Get TransactionDetail Data from SQL Server based on transactionId
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public List<TransactionDetail> GetTransactionDetail(string transactionId)
        {
            List<TransactionDetail> dtList = null;

            try
            {
                //set the stored procedure for the command
                TransactionDetail iDtEntity = null;

                using (DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDYT_PPT_TransactionDetail_Get", EddyTracking))
                {
                    //Execute the command get all data
                    Db.AddInParameter(dbCommand, "@TransactionId", DbType.String, transactionId);
                    using (IDataReader dataReader = EddyTracking.ExecuteReader(dbCommand))
                    {
                        if (dataReader != null)
                        {
                            dtList = new List<TransactionDetail>();
                            //Load each record to TransactionSummaryEntity class and add the class to dtList
                            while (dataReader.Read())
                            {
                                iDtEntity = new TransactionDetail(dataReader);
                                dtList.Add(iDtEntity);
                            }

                            dataReader.Close();
                        }
                    }

                    if (dbCommand.Connection.State == ConnectionState.Open)
                    {
                        dbCommand.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

            return dtList;
        }


        #endregion

    }

}
