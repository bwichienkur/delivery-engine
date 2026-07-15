using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.Nexus.Common.Logging;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.DataAccess.Common;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity.Cap;
using EDDY.IS.DeliveryEngine.Entity;
//using EDDY.Nexus.DataAccess.Common;
//using EDDY.Nexus.DataAccess.Interface.CoreBusiness;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.DataAccess
{
    public class CapDistributionDAO : BaseDataSource, ICapDistributionDAO
    {

        public void CreateCAP(CapDistributionEntity capEntity)
        {
            EDDYLogger.LogMessage(1, LogLevel.General, "In CreateCAP DAO - Create CAP", capEntity.CurrentUser, new Dictionary<string, object> { { "ModuleAction", "Create" }, { "ModuleName", "Core Business" } });

            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("Eddy_CreateCap", Db);
            Db.AddInParameter(dbCommand, "@ClientID", DbType.Int32, capEntity.SchoolId);
            Db.AddInParameter(dbCommand, "@CapLevel", DbType.String, capEntity.CapLevel);
            Db.AddInParameter(dbCommand, "@CapValue", DbType.Int32, capEntity.CapValue);
            Db.AddInParameter(dbCommand, "@StartDate", DbType.DateTime, capEntity.StartDate);
            Db.AddInParameter(dbCommand, "@EndDate", DbType.DateTime, capEntity.EndDate);
           
            try
            {
                Db.ExecuteNonQuery(dbCommand); // Execute Stored Procedure


            }
           

            catch (Exception ex)
            {
                 ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
               
            }


        }

        public List<GetCapEntity> GetCap()
        {
            List<GetCapEntity> listCap = new List<GetCapEntity>();
            IDataReader dataReader = null;
            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("dbo.Eddy_GetCapDistribution", Db);

            try
            {
                dataReader = Db.ExecuteReader(dbCommand);

                while (dataReader.Read())
                {
                    listCap.Add(new GetCapEntity(dataReader));
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

            return listCap;
        }

        public void EditCap(CapDistributionEntity capEntity)
        {
            EDDYLogger.LogMessage(1, LogLevel.General, "In EditCap DAO - Update CAP", capEntity.CurrentUser, new Dictionary<string, object> { { "ModuleAction", "Update" }, { "ModuleName", "Core Business" } });

            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("Eddy_EditCap", Db);
            Db.AddInParameter(dbCommand, "@ClientID", DbType.Int32, capEntity.SchoolId);
            Db.AddInParameter(dbCommand, "@CapLevel", DbType.String, capEntity.CapLevel);
            Db.AddInParameter(dbCommand, "@CapValue", DbType.Int32, capEntity.CapValue);
            Db.AddInParameter(dbCommand, "@StartDate", DbType.DateTime, capEntity.StartDate);
            Db.AddInParameter(dbCommand, "@EndDate", DbType.DateTime, capEntity.EndDate);

            try
            {
                Db.ExecuteNonQuery(dbCommand); // Execute Stored Procedure


            }
           

            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
               
            }


        }

        public bool UpdateCapValueByCSRId(DeliveryLeadData deliveryData)
        {
            bool chkValue = false;

            EDDYLogger.LogMessage(1, LogLevel.General, "In UpdateCapValueByCSRId DAO - Update CAP value by taking delivery lead data as param", deliveryData.CurrentUser, new Dictionary<string, object> { { "ModuleAction", "Update" }, { "ModuleName", "Core Business" } });

            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("EDDY_UpdateCapValue", Db);
            Db.AddInParameter(dbCommand, "@ClientSchoolRelationID", DbType.Int32, deliveryData.CRId);

            try
            {
                Db.ExecuteNonQuery(dbCommand); // Execute Stored Procedure
                chkValue = true;

            }
            

            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
            }

            return chkValue;
        }

        public void CreateCapLevel(CapLevelEntity capLevelEntity)
        {
            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("Eddy_CreateCap", Db);

            EDDYLogger.LogMessage(1, LogLevel.General, "In CreateCapLevel DAO - ", capLevelEntity.CurrentUser, new Dictionary<string, object> { { "ModuleAction", "Create" }, { "ModuleName", "Core Business" } });

            Db.AddInParameter(dbCommand, "@CapId", DbType.Int32, capLevelEntity.CapId);
            Db.AddInParameter(dbCommand, "@CapLevel", DbType.String, capLevelEntity.CapLevel);
            Db.AddInParameter(dbCommand, "@EntityMetaId", DbType.Int32, capLevelEntity.EntityMetaId);
            Db.AddInParameter(dbCommand, "@EntityId", DbType.DateTime, capLevelEntity.EntityId);
            Db.AddInParameter(dbCommand, "@Status", DbType.DateTime, capLevelEntity.Status);

            try
            {
                Db.ExecuteNonQuery(dbCommand); // Execute Stored Procedure


            }
           

            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
              
            }


        }

        public void ProcessCap(DeliveryLeadData deliveryData)
        {

          
            try
            {
                int commandTimeOut = 120;

                if(ConfigurationManager.AppSettings["SQLCommandTimeOut"] != null)
                {
                    commandTimeOut =  Convert.ToInt32(ConfigurationManager.AppSettings["SQLCommandTimeOut"]);
                }

                DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("CAP_ExecuteCappingProcess", Db); 
                Db.AddInParameter(dbCommand, "@LeadID", DbType.Int32, deliveryData.LeadId);
                Db.AddInParameter(dbCommand, "@Debug", DbType.Int32, 1);
                dbCommand.CommandTimeout = commandTimeOut;
           
                Db.ExecuteNonQuery(dbCommand); // Execute Stored Procedure


            }
           

            catch (Exception ex)
            {

                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                
            }

        }

        public void TerminateOrfantCapDuration(int clientRelationshipId, int clientRelationshipProductMappingId)
        {
            DbCommand dbCommand = DatabaseUtilities.CreateDbCommand("Eddy_CAP_TerminateOrfantDuration", Db);



            Db.AddInParameter(dbCommand, "@CRProductId", DbType.Int32, clientRelationshipProductMappingId);
            Db.AddInParameter(dbCommand, "@CRId", DbType.String, clientRelationshipId);


            try
            {
                Db.ExecuteNonQuery(dbCommand); // Execute Stored Procedure

            }


            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);

            }

        }

        

    }
}
