using System;
using System.Activities;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class GetDeliveryEndpoint : CodeActivity
    {
        public InArgument<Int32> DeliveryEndpointId { get; set; }
        public OutArgument<DeliveryEndpoint> DeliveryEndpoint { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DeliveryEndpoint returnEndpoint = null;

            int deliveryEndpointId = DeliveryEndpointId.Get(context);
            returnEndpoint = DeliveryEngineDataService.DeliveryEngineDAO.GetEndpointById(deliveryEndpointId);

            //store 
            DeliveryEndpoint.Set(context, returnEndpoint);

        }
    }
}