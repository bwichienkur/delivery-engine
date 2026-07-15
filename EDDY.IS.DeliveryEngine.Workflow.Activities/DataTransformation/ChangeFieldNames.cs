using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class ChangeFieldNames : IDataTransformationTask
    {
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public ChangeFieldNames()
        {
            //<ChangeFieldNames>
            //  <MapValues>
            //    <MapValue oldValue='Id' newValue='AffLeadID' />
            //    <MapValue oldValue='QID' newValue='AffSubID' />
            //    <MapValue oldValue='CurrentDate' newValue='Date' />
            //  </MapValues>
            //</ChangeFieldNames>       
        }

        /// <summary>
        /// Constructor for populating XML as object
        /// </summary>
        /// <param name="ruleNode">XMLDocument will be parsed and values will be set to properties</param>
        //public ChangeFieldNames(XmlDocument ruleNode)
        //{

        //    //<ChangeFieldNames>
        //    //  <MapValues>
        //    //    <MapValue oldValue='Id' newValue='AffLeadID' />
        //    //    <MapValue oldValue='QID' newValue='AffSubID' />
        //    //    <MapValue oldValue='CurrentDate' newValue='Date' />
        //    //  </MapValues>
        //    //</ChangeFieldNames>

        //    int index = 0;            
        //    if (ruleNode != null)
        //    {
        //        XmlNodeList MapValueNodes = ruleNode.SelectNodes(@"ChangeFieldNames/MapValues/MapValue");

        //        string oldValue = string.Empty;
        //        string newValue = string.Empty;
        //        MapValues = new MapValue[MapValueNodes.Count];
        //        foreach (XmlNode node in MapValueNodes)
        //        {
        //            oldValue = MapValueNodes[index].Attributes["oldValue"].InnerXml.ToString();
        //            newValue = MapValueNodes[index].Attributes["newValue"].InnerXml.ToString();

        //            MapValues[index] = new MapValue(oldValue, newValue);
        //            index++;
        //        }
        //    }
        //}
        #endregion

        # region Properties
        /// <summary>
        /// To get and set MapValues
        /// </summary>
        [XmlArray("MapValues")]
        [XmlArrayItem("MapValue")]
        public MapValue[] MapValues { get; set;}
        #endregion

        # region IDataTransformationTask Methods
        /// <summary>
        /// Transforms the rawData dictionary with XML rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {
            try
            {
                log.Append("\r\n - ChangeFieldNames transformation started \r\n");
                foreach (MapValue mapValue in MapValues)
                {
                    //Replace the old value with new value
                    if (rawData.ContainsKey(mapValue.OldValue))
                    {
                        
                        object tempValue = rawData[mapValue.OldValue];
                        rawData.Remove(mapValue.OldValue);
                        if (!rawData.ContainsKey(mapValue.NewValue))
                        {
                            rawData.Add(mapValue.NewValue, tempValue);                            
                        }
                        else
                        {
                            rawData[mapValue.NewValue] = tempValue;                            
                        }
                        log.Append(" - " + mapValue.OldValue + " replaced with " + mapValue.NewValue + " \r\n");
                    }
                }
                log.Append(" - ChangeFieldNames transformation end \r\n");
            }
            catch (Exception ex)
            {
                log.Append(" - ChangeFieldNames transformation error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return rawData;
        }
        #endregion
    }
}
