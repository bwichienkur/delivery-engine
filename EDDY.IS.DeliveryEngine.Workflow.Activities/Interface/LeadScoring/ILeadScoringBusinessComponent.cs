//using EDDY.Nexus.Entity.CoreBusiness;

using EDDY.IS.DeliveryEngine.Entity;
namespace EDDY.IS.DeliveryEngine.Workflow.Activities.LeadScoring.Interface
{
    public interface ILeadScoringBusinessComponent
    {
        int ScoreLead(DeliveryLeadData deliveryData);
    }
}
