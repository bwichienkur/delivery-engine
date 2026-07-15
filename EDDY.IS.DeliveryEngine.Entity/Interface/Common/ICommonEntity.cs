using System;

namespace EDDY.IS.DeliveryEngine.Entity.Interface.Common
{
    public interface ICommonEntity
    {
        bool IsEnabled { get; set; }
        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        int UpdatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string CurrentUser { get; set; }
        int CurrentCSR { get; set; }
    }
}
