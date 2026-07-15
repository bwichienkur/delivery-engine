using System;
using System.Activities;
//using EDDY.Nexus.DataAccess.CoreBusiness;
//using EDDY.Nexus.DataAccess.Interface.CoreBusiness;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class GetRealtimeLeadData : CodeActivity
    {
        public InArgument<Int32> LeadId { get; set; }
        public OutArgument<DeliveryLeadData> LeadData { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                DeliveryLeadData deliveryLeadData = null;

                int leadId = LeadId.Get(context);

                deliveryLeadData = LeadDataService.LeadDAO.GetRealtimeDeliveryLeadDataByLeadId(leadId);

                //Console.WriteLine(leadId.ToString() + " - Found Lead Data. ProgramId is: " + deliveryLeadData.ProgramId.ToString() + " - Delivery Fields Count: " + deliveryLeadData.AllFields.Count.ToString());




                //store 
                LeadData.Set(context, deliveryLeadData);
            }
            catch (Exception ex)
            { 

            }
  
        }
    }
}
