using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class GetDeliveryEndpoint : CodeActivity
    {
        public InArgument<Int32> DeliveryEndpointId { get; set; }
        public OutArgument<DeliveryEndpoint> DeliveryEndpoint { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DeliveryEndpoint returnEndpoint = null;

            int deliveryEndpointId = DeliveryEndpointId.Get(context);

          
                IDeliveryEngineDAO ddDAO = new DeliveryEngineDAO();
                returnEndpoint = ddDAO.GetEndpointById(deliveryEndpointId);
            

           

            //store 
            DeliveryEndpoint.Set(context, returnEndpoint);

        }
    }
}