using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDY.IS.DeliveryEngine.DataAccess.DataService
{
    public static class DeliveryEngineDataService
    {
        public static IDeliveryEngineDAO DeliveryEngineDAO { get; set; }

        static DeliveryEngineDataService()
        {
            DeliveryEngineDAO = new DeliveryEngineDAO();
        }
    }
}
