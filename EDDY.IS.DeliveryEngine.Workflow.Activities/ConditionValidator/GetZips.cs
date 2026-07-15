using System.Collections.Generic;
using System;
//using EDDY.Nexus.BusinessComponent.Interface.ConditionValidator;
using EDDY.Nexus.Common.ExceptionHandler;
//using EDDY.Nexus.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator
{
    public class GetZips : IDataClass
    {
        #region Properties

        public DataType ReturnDataType { get; set; } //type of data being returned

        #endregion

        #region Public Methods

        /// <summary>
        /// TEST CUSTOM CONDITION CLASS - Returns a list of objects for condtion comparison
        /// </summary>
        /// <returns>List of objects</returns>
        List<object> IDataClass.GetData(FtcParameter[] parameters)
        {
            List<object> returnList;
            try
            {
                returnList = new List<object> {"07030", "55555", "07410", parameters == null ? "44444" : "90210"};
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return returnList;
        }

        #endregion
    }
}