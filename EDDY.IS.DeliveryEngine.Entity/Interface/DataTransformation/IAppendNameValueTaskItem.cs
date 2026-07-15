using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{
    public interface IAppendNameValueTaskItem
    {
        int SeqID { get; set; }
        int RuleID { get; set; }
        int TaskXMLID { get; set; }
        string RuleDesc { get; set; }
        int ConditionID { get; set; }
        string ConditionDesc { get; set; }
        string ConditionXML { get; set; }
        string TaskXML { get; set; }
        int TotalCount { get; set; }
        int TaskTypeID { get; set; }
    }
}
