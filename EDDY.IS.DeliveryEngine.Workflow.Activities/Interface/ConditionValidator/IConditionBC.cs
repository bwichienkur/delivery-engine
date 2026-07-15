using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.ConditionValidator;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator.Interface
{
    public interface IConditionBc
    {
        #region Public Methods

        List<MatchingCondition> GetMatchingConditions(Dictionary<string, object> leadData, EventType eventType); //Checks a given data list against a all XML conditions in the database.
        Result IsConditionMet(Dictionary<string, object> leadData, string xmlNode);  //Checks a given data list against an XML condition.
        
        #endregion
    }
}
