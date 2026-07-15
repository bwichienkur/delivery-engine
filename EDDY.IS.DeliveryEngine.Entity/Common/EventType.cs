using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity.Common
{
    #region Enum
    /// <summary>
    /// To populate event type
    /// </summary>
    [DataContract]
    public enum EventType : int
    {
        [EnumMember]
        Validation = 1,
        [EnumMember]
        Database,
        [EnumMember]
        Delivery
    }
    #endregion
    
}
