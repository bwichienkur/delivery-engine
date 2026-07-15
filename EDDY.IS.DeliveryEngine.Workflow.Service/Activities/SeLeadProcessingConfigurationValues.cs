using System;
using System.Activities;
using System.Configuration;
using System.Globalization;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
using System.Text;
using System.Linq;
using EDDY.IS.DeliveryEngine.Entity;

namespace EDDY.IS.DeliveryEngine.Workflow.Service.Activities
{
    public sealed class SeLeadProcessingConfigurationValues : CodeActivity
    {
        public OutArgument<bool> ProcessCap { get; set; }
        public OutArgument<bool> ScoreLead { get; set; }
        public OutArgument<bool> DeliverLead { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            //set vals from web.config
            var processCap = false;// Convert.ToBoolean(ConfigurationManager.AppSettings["ProcessCap"]);
            var scoreLead = false;// Convert.ToBoolean(ConfigurationManager.AppSettings["ScoreLead"]);
            var deliverLead = Convert.ToBoolean(ConfigurationManager.AppSettings["DeliverLead"]);
            var leadData = LeadData.Get(context);
            
            //return values
            ProcessCap.Set(context, processCap);
            ScoreLead.Set(context, scoreLead);
            DeliverLead.Set(context, deliverLead);
        }
    }
}
