using System;
using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
    [DataContract]
    public class DataTransformationExceptions
    {

        #region Properties
        /// <summary>
        /// To get and set ServiceException
        /// </summary>
        [DataMember]
        public Exception ServiceException { get; set; }
        #endregion
    }
}
