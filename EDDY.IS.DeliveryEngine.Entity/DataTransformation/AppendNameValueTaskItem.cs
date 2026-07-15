using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.Interface.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
    public class AppendNameValueTaskItem:CommonEntity,IAppendNameValueTaskItem
    {
        [DataMember]
        public int SeqID { get; set; }
        [DataMember]
        public int RuleID { get; set; }
        [DataMember]
        public string RuleDesc { get; set; }
        [DataMember]
        public int ConditionID { get; set; }
        [DataMember]
        public string ConditionDesc { get; set; }

        [DataMember]
        public string ConditionXML { get; set; }

        [DataMember]
        public string TaskXML { get; set; }

        [DataMember]
        public int TaskXMLID { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public int TaskTypeID { get; set; }

        public AppendNameValueTaskItem()
        {
            

        }

        public AppendNameValueTaskItem(IDataReader dr)
        {
            this.SeqID = Convert.ToInt32(dr["RuleID"]);
            this.RuleID = Convert.ToInt32(dr["TaskID"]);
            this.TaskXMLID = Convert.ToInt32(dr["TaskXMLID"]);
            this.ConditionID = Convert.ToInt32(dr["ConditionXMLID"]);
            this.ConditionDesc = Convert.ToString(dr["ConditionDescription"]);
            this.RuleDesc = Convert.ToString(dr["TaskDescription"]);
            this.ConditionXML = Convert.ToString(dr["ConditionXML"]);
            this.TaskXML = Convert.ToString(dr["TaskXML"]);
            this.TotalCount = Convert.ToInt32(dr["TotalCount"]);
            this.TaskTypeID = dr.GetName(dr.FieldCount - 1).ToLower().Equals("tasktypeid") ? Convert.ToInt32(dr["TaskTypeID"]) : 0;
        }

    }
}
