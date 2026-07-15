using System;
using System.Activities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class RemoveRePost : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                int leadId = LeadId.Get(context);
                var result = DeliveryEngineDataService.DeliveryEngineDAO.RemoveRePostData(leadId);               
            }
            catch (Exception)
            {
                //do nothing
            }

        }
    }
}
