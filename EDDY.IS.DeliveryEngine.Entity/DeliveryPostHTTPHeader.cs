using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliveryPostHTTPHeader : CommonEntity
    {
        [DataMember]
        public int DeliveryEndpointHTTPHeader { get; set; }
        [DataMember]
        public int DeliveryEndPointID { get; set; }
        [DataMember]
        public string HTTPHeaderKey { get; set; }
        [DataMember]
        public string HTTPHeaderValue { get; set; }

        #region Constructor
        public DeliveryPostHTTPHeader()
        {

        }

        public DeliveryPostHTTPHeader(IDataReader dataReader)
        {

            if (dataReader != null)
            {
                DeliveryEndpointHTTPHeader = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointHTTPHeader"]);
                DeliveryEndPointID = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndPointID"]);
                HTTPHeaderKey = DatabaseUtilities.SetStringValue(dataReader["HTTPHeaderKey"]);
                HTTPHeaderValue = DatabaseUtilities.SetStringValue(dataReader["HTTPHeaderValue"]);
                IsEnabled = dataReader.IsColumnExists("IsEnabled") ? DatabaseUtilities.SetBooleanValue(dataReader["IsEnabled"]) : true;
            }
        }

        #endregion

    }
}
