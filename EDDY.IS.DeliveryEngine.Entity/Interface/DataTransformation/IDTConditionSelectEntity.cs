using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{
    public interface IDTConditionSelectEntity
    {
        int TaskID { get; set; }
        int ConditionXMLID { get; set;}
        string ConditionXML { get; set; }
        string ConditionDesc { get; set; }
        string UpdatedByName { get; set;}
    }
}
