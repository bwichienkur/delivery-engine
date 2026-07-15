using System.Data;
//using EDDY.Nexus.DataAccess.Interface.Common;
//using EDDY.Nexus.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.DataAccess.Interface.Common;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.DataAccess.Interface
{
    public interface IConditionDao : IBaseDataSource
    {
        #region Public Methods
        DataTable GetComparisonTable(string storedProcedureName, FtcParameter[] parameters); //Gets custom Stored Proc return data
        DataTable GetXmlTable(EventType eventType); //Gets DataTransformaiton data from SQL server
        
        # endregion
    }
}
