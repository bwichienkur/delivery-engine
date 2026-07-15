using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{
    public interface IDTConditionparamEntity
    {
        int TaskID { get; set; }
        int ConditionXMLID { get; set;}
        int TaskTypeID { get; set; }
        int EndPointID { get; set; }
        string ConditionXML { get; set; }
        string ConditionDesc { get; set;}
        int UpdatedBy { get; set; }
        int SequenceNo { get; set; }
        int TaskXMLID { get; set; }
        string TaskXML { get; set; }
        string TaskDesc { get; set; }
        string ClassName { get; set; }
        int StartPosition { get; set;}
        int EndPosition { get; set; }
        string SortField { get; set;}
        int Pagesize { get; set; }
    }
}
