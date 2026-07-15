using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;
using EDDY.Nexus.Entity.CoreBusiness;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class RemoveRePost : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                int leadId = LeadId.Get(context);
                IDeliveryEngineDAO deDao = new DeliveryEngineDAO();
                var result = deDao.RemoveRePostData(leadId);
                
               
            }
            catch (Exception)
            {
                //do nothing
            }

        }
    }
}
