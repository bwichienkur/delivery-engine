using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.DataAccess.Interface.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EDDY.IS.DeliveryEngine.DataAccess.Interface.Common;

namespace EDDY.IS.DeliveryEngine.DataAccess.Common
{
    public abstract class BaseDataSource : IBaseDataSource
    {        
        public Database Db { set; get; }
        public Database EddyTransactionDb { set; get; }
        public Database EddyLoggingDb { set; get; }

        public BaseDataSource()
        {   // constructor
            Db = DatabaseUtilities.CreateDatabase();
        }
    }
}
