using System;
using System.Runtime.Serialization;
using EDDY.IS.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity.Cap
{
    [DataContract]
    public class CapDistributionEntity : RequestHeader
    {

        #region constructors

        /// <summary>
        /// constructor for ClientEntity
        /// </summary>

        public CapDistributionEntity()
        {
        }

        #endregion

        #region Data Members

        [DataMember(Order = 5)]
        public int SchoolId { get; set; }
        [DataMember(Order = 3)]
        public DateTime StartDate { get; set; }
        [DataMember(Order = 4)]
        public DateTime EndDate { get; set; }
        [DataMember(Order = 0)]
        public string ClientId { get; set; }
        [DataMember(Order = 1)]
        public string CapLevel { get; set; }
        [DataMember(Order = 2)]
        public string CapValue { get; set; }
        [DataMember(Order = 6)]
        public bool AutoRecurring { get; set; }
        [DataMember(Order = 7)]
        public int AutoRecurringCapValue { get; set; }
        [DataMember(Order = 8)]
        public DateTime AutoRecurringEndDate { get; set; }
        [DataMember(Order = 9)]
        public string AutoRecurringTimePeriod { get; set; }

        #endregion


    }
}




