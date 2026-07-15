using System;
using System.Activities;
using EDDY.Nexus.Common.Logging;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.ProductProcessing;
//using EDDY.Nexus.DataAccess.ProductProcessing;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class WriteToRealtimeDELog : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<int> DeliveryEngineEventId { get; set; }
        public InArgument<int> DeliveryEndpointId { get; set; }
        public InArgument<string> Message { get; set; }
        public InArgument<Int32> Verbosity { get; set; }
        public InArgument<int> ActivityStepId { get; set; }
        public OutArgument<bool> WriteToDELogStatus { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            int leadId = LeadId.Get(context);
            DeliveryLeadData leadData = LeadData.Get(context);
            int deliveryEngineEventId = DeliveryEngineEventId.Get(context);
            int deliveryEndpointId = DeliveryEndpointId.Get(context);
            string message = Message.Get(context);
            int verbosity = Verbosity.Get(context);
            int activityStepId = ActivityStepId.Get(context);
            var dao = new DeliveryEngineDAO();
            bool IsBeta = leadData != null ? leadData.IsBeta : false;
            try
            {
                
                //Site verbosity setting? (default to 1)
                int configVerbosityLevel = 1;
                if (System.Configuration.ConfigurationManager.AppSettings["DE_LogVerbosityLevel"] != null)
                {
                    configVerbosityLevel = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DE_LogVerbosityLevel"]);
                }
                //Truncate message
                message=message.Length > 1000 ? message.Substring(0, 1000) : message;

                //Temp for debug
                if (configVerbosityLevel == 10)
                {
                    dao.TempLog(leadId, deliveryEngineEventId, deliveryEndpointId,
                                    "DT activityStepId: " + activityStepId.ToString(), 1, IsBeta);
                    dao.TempLog(leadId, deliveryEngineEventId, deliveryEndpointId,
                                    "DT verbosity: " + verbosity.ToString(), 1, IsBeta);
                }

                // If this is a regular item, or if we are in verbose mode (or both) - write to log
                if (configVerbosityLevel >= verbosity)
                {
                    dao.TempLog(leadId, deliveryEngineEventId, deliveryEndpointId,
                                message, 1, IsBeta);

                    //Write to Logging App
                    EDDYLogger.LogMessage(1, LogLevel.Debug, message);
                }

                //Also write to transaction log
                if (activityStepId > 0)
                {
                    ProductProcessingTransactionDataService.ProductProcessingTransactionDAO.InsertTransactionDetail(leadData != null ? leadData.TransactionId.ToString() : new Guid().ToString(), activityStepId, message);
                }

                WriteToDELogStatus.Set(context, true);
            }
            catch (Exception ex)
            {
                dao.TempLog(leadId, deliveryEngineEventId, deliveryEndpointId, ex.ToString(), 1, IsBeta);
            }
            WriteToDELogStatus.Set(context, true);
        }
    }
}


