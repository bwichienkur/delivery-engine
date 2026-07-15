using System;
using System.Activities;
using EDDY.Nexus.DataAccess.CoreBusiness;
using EDDY.Nexus.DataAccess.Interface.CoreBusiness;
using EDDY.Nexus.Entity.CoreBusiness;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class GetLeadData : CodeActivity
    {
        public InArgument<Int32> LeadId { get; set; }
        public OutArgument<DeliveryLeadData> LeadData { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DeliveryLeadData deliveryLeadData = null;

            int leadId = LeadId.Get(context);

           
                ILeadDAO leadDAO = new LeadDAO();
                deliveryLeadData = leadDAO.GetDeliveryLeadDataByLeadId(leadId);

                Console.WriteLine(leadId.ToString() + " - Found Lead Data. ProgramId is: " + deliveryLeadData.ProgramId.ToString() + " - Delivery Fields Count: " + deliveryLeadData.AllFields.Count.ToString());
           

           

            //store 
            LeadData.Set(context, deliveryLeadData);
  
        }
    }
}
