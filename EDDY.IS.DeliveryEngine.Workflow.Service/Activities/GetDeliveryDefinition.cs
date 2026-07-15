using System.Activities;
using System.Collections.Generic;
//using EDDY.Nexus.BusinessComponent.ConditionValidator;
//using EDDY.Nexus.BusinessComponent.Interface.ConditionValidator;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.ConditionValidator;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class GetDeliveryDefinition : CodeActivity
    {
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public OutArgument<DeliveryDefinition> DeliveryDefinition { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DeliveryLeadData leadData = LeadData.Get(context);

            //definition to return
            DeliveryDefinition definitionToUse = null;

            IDeliveryEngineDAO dao = new DeliveryEngineDAO();

            //keeps track of highest priority found for all matching defs
            int highestPriority = -1;

            //Get All Defs for CSR
            List<DeliveryDefinition> defs = dao.GetCRDeliveryDefinitionsForLead(leadData.CRId, leadData.LeadId);

            //Loop through all Defs and get one that applies to this lead
            foreach (DeliveryDefinition def in defs)
            {
                if (DoesConditionApply(leadData, def.ConditionXML))
                {
                    //check current definitionToUse priority 
                    if (def.Active==1)
                    {
                        if (def.Priority > highestPriority)
                        {
                            definitionToUse = def;
                            highestPriority = def.Priority;
                        }
                    }
                }
            }

            if (definitionToUse == null)
            {
                //throw new Exception("No delivery definition found for lead id: " + leadData.LeadId.ToString());
            }
            else
            {
                //Add Endpoints to complete definition
                definitionToUse.EndPoints = dao.GetDefinitionEndpoints(definitionToUse);
            }

            // Store the request in the OutArgument
            DeliveryDefinition.Set(context, definitionToUse);
        }

        /// <summary>
        /// Checks if the XML condition applies to leadData.
        /// </summary>
        /// <param name="leadData">The lead data.</param>
        /// <param name="xml">The Condition XML.</param>
        /// <returns>bool: true if condition applies, else false</returns>
        private bool DoesConditionApply(DeliveryLeadData leadData, string xml)
        {
            //return var
            bool applies = false;

            //if there is no condition xml, then return true
            if (xml == null)
            {
                return true;
            }

           
                //holds return params from conditon method
                Result result;

                //call condition dll with leadData and XML to get a bool 
                IConditionBc conditionBC = new ConditionBc();

                //call method and get results
                result = conditionBC.IsConditionMet(leadData.AllFields, xml);

                //set applies
                applies = result.IsConditionMet;
           

            return applies;
        }

    }
}
