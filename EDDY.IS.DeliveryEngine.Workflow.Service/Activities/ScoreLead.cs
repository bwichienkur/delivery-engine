using System;
using System.Activities;
using System.Diagnostics;
//using EDDY.Nexus.BusinessComponent.CoreBusiness;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.Workflow.Activities.LeadScoring;

namespace EDDY.IS.DeliveryEngine.Workflow.Service.Activities
{

    public sealed class ScoreLead : CodeActivity
    {
        public InArgument<DeliveryLeadData> inLeadData { get; set; }
        public OutArgument<int> outLeadScore { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DeliveryLeadData _leadData = inLeadData.Get(context);
            int _leadScore = 0;

            try
            {
                LeadScoringBusinessComponent lsbc = new LeadScoringBusinessComponent();
                _leadScore = lsbc.ScoreLead(_leadData);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
            }

            //return filePath 
            outLeadScore.Set(context, _leadScore);

        }
    }
}
