using System.Runtime.Serialization;
using EDDY.IS.DeliveryEngine.Entity.Interface;
//using EDDY.Nexus.Entity.DeliveryEngine.Interface;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliveryDefSearchParam : IDeliveryDefSearchParam
    {
        #region Constructor

        public DeliveryDefSearchParam()
        {
            StartPosition = 1;
            EndPosition = 35;
            Pagesize = 10000;
            IsActive = null;
        }

        #endregion
        #region Data Members
        [DataMember]
        public int StartPosition { get; set; }
        [DataMember]
        public int EndPosition { get; set; }
        [DataMember]
        public string SortField { get; set; }
        [DataMember]
        public int Pagesize { get; set; }
        [DataMember]
        public int DeliveryDefId { get; set; }

        [DataMember]
        public string NameStartsWith{ get; set; }
        [DataMember]
        public string NameContains { get; set; }
        [DataMember]
        public int CrId { get; set; }

        [DataMember]
        public int? IsActive { get; set; }
        
        #endregion
    }
}
