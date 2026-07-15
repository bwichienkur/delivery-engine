using System.Collections.Generic;
using System.Text;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface
{


    public interface IDataTransformationTask
    {
        # region Public Methods
        Dictionary<string, object> Transform(Dictionary<string, object> rawData, ref StringBuilder log); //Transforms the rawData dictionary with XML rules        
        # endregion
    }
}
