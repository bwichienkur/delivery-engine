using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
    [DataContract]
    public class KeyValueObject
    {
        public KeyValueObject(string key, object value)
        {
            Key = key;
            Value = value;
        }

        #region Properties
        /// <summary>
        /// To get and set Key
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// To get and set Value
        /// </summary>
        [DataMember]
        public object Value { get; set; }
        #endregion
    }
}
