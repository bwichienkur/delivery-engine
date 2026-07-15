using System.Runtime.Serialization;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface;
//using EDDY.Nexus.Entity.Interface.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
    public class DTConditionparamEntity : IDTConditionparamEntity
    {
        [DataMember]
        public int TaskID { get; set; }

        [DataMember]
        public int ConditionXMLID { get; set; }

        [DataMember]
        public int TaskTypeID { get; set; }

        [DataMember]
        public int EndPointID { get; set; }

        [DataMember]
        public string ConditionXML { get; set; }

        [DataMember]
        public string ConditionDesc { get; set; }

        [DataMember]
        public int UpdatedBy { get; set; }

        [DataMember]
        public int SequenceNo { get; set; }

        [DataMember]
        public int TaskXMLID { get; set; }

        [DataMember]
        public string TaskXML { get; set; }

        [DataMember]
        public string TaskDesc { get; set; }

        [DataMember]
        public string ClassName { get; set;}

        [DataMember]
        public int StartPosition { get; set; }

        [DataMember]
        public int EndPosition { get; set; }

        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public int Pagesize { get; set; }

        [DataMember]
        public string DeliveryLabelXml { get; set; }

        public DTConditionparamEntity()
        {
            StartPosition = 1;
            EndPosition = 35;
            Pagesize = 35;
            //SortField = "TaskID ASC";

        }
    }
}
