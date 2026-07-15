using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DataTransformation;
//using EDDY.Nexus.DataAccess.Interface.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class AppendNameValuePair : IDataTransformationTask
    {
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public AppendNameValuePair()
        {
            //<AppendNameValuePair>
            //  <TargetedFields>
            //    <TargetedField name='Address2' value='' dataType='string' />
            //    <TargetedField name='Middle Initial' value='' dataType='string' />
            //  </TargetedFields>
            //</AppendNameValuePair>
        }

        /// <summary>
        /// Constructor for populating XML as object
        /// </summary>
        /// <param name="ruleNode">XMLDocument will be parsed and values will be set to properties</param>
        //public AppendNameValuePair(XmlDocument ruleNode)
        //{
        //    //<AppendNameValuePair>
        //    //  <TargetedFields>
        //    //    <TargetedField name='Address2' value='' dataType='string' />
        //    //    <TargetedField name='Middle Initial' value='' dataType='string' />
        //    //  </TargetedFields>
        //    //</AppendNameValuePair>

        //    int index = 0;            
        //    if (ruleNode != null)
        //    {
        //        XmlNodeList TargetedFieldNodes = ruleNode.SelectNodes(@"AppendNameValuePair/TargetedFields/TargetedField");

        //        TargetedFields = new TargetedField[TargetedFieldNodes.Count];
        //        foreach (XmlNode node in TargetedFieldNodes)
        //        {
        //            string name = TargetedFieldNodes[index].Attributes["name"].InnerXml.ToString();
        //            string dataType = TargetedFieldNodes[index].Attributes["dataType"].InnerXml.ToString(); ;
        //            string value = TargetedFieldNodes[index].Attributes["value"].InnerXml.ToString(); ;
        //            TargetedFields[index] = new TargetedField(name, dataType, value);
        //            index++;
        //        }
        //    }
        //}
        #endregion

        # region Properties
        /// <summary>
        /// To get and set TargetedFields
        /// </summary>
        [XmlArray("TargetedFields")]
        [XmlArrayItem("TargetedField")]
        public TargetedField[] TargetedFields { get; set; }
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
                log.Append("AppendNameValuePair transformation started. ");
                foreach (TargetedField targetedField in TargetedFields)
                {
                   
                    //If datatype is storedprocedure then the result should come from db. Field value is the sp name
                    if (targetedField.DataType == "storedprocedure")
                    {
                        Int64 leadId;
                        string spName = targetedField.Value;

                        log.Append(string.Format (" - AppendNameValuePair stored procedure transformation: FieldName: SP Name: |", targetedField.Name,targetedField.Value));

                        //validate
                        Int64.TryParse(rawData["LeadId"].ToString() , out leadId);
                        if (leadId == 0) throw new ArgumentException("AppendNameValuePair for sp transform error. LeadId is not found in the raw lead data");
                        if (String.IsNullOrEmpty(spName)) throw new ArgumentException("AppendNameValuePair for sp transform error. SP name is not defined.");

                        //Get value
                        string returnedValue = DataTransformationDataService.DataTransformationDAO.GetStoredProcResult(targetedField.Value, leadId);
                        targetedField.Value = returnedValue;
                    }

                    //If key already present in raw data 
                    //overwrite the new value else add the key/value to raw data
                    if (rawData.ContainsKey(targetedField.Name))
                    {
                        log.Append(" - " + targetedField.Name + " = " + rawData[targetedField.Name] + " changed to as => " + targetedField.Name + " = " + targetedField.Value + " | ");
                        rawData[targetedField.Name] = targetedField.Value;
                    }
                    else
                    {
                        log.Append(" - Added a new item => " + targetedField.Name + " = " + targetedField.Value + " | ");
                        rawData.Add(targetedField.Name, targetedField.Value);
                    }
                }
                log.Append("AppendNameValuePair transformation end. ");
            }
            catch (Exception ex)
            {
                log.Append(" - AppendNameValuePair transformation error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return rawData;
        }
        # endregion

    }
}
