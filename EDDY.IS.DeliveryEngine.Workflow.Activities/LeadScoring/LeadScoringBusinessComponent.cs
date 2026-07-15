//using EDDY.Nexus.BusinessComponent.Interface.CoreBusiness;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Workflow.Activities.LeadScoring.Interface;
using EDDY.IS.DeliveryEngine.Entity;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.LeadScoring
{
    public class LeadScoringBusinessComponent : ILeadScoringBusinessComponent
    {
        public int ScoreLead(DeliveryLeadData deliveryData)
        {
            return 100;
        }
    }

}
