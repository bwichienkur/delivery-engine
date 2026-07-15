using System.Collections.Generic;
//using EDDY.Nexus.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator.Interface
{
    public interface IDataClass
    {
            #region Properties

            DataType ReturnDataType { get; } //type of data being returned

            #endregion

            #region Public Methods

            List<object> GetData(FtcParameter[] parameters);  //Returns a list of objects for condtion comparison

            #endregion

    }
}
