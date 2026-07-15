using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EDDY.IS.DeliveryEngine.DataAccess.Interface.Common
{
    public interface IBaseDataSource
    {
        Database Db { set; get; }
        Database EddyTransactionDb { set; get; }
        Database EddyLoggingDb { set; get; }
    }
}
