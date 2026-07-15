using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;


namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class RemoveFields : IDataTransformationTask
    {
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public RemoveFields()
        {
            //<Tasks>
            //    <RemoveFields>
            //        <TargetedFields>
            //        <TargetedField name='minitial' />
            //        <TargetedField name='gradYr' />
            //        </TargetedFields>
            //    </RemoveFields>
            //</Tasks>
        }
        #endregion

        # region Properties
        /// <summary>
        /// To get and set TargetedFields
        /// </summary>
        [XmlArray("TargetedFields")]
        [XmlArrayItem("TargetedField")]
        public TargetedField[] TargetedFields { get; set; }
        #endregion

        #region IDataTransformationTask Members
        /// <summary>
        /// Transforms the rawData dictionary with XML rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {
            try
            {
                log.Append(" - RemoveFields transformation end \r\n");                
                //Removes the fields from raw data
                foreach (TargetedField targetedField in TargetedFields)
                {
                    if (rawData.ContainsKey(targetedField.Name))
                    {
                        log.Append(" - " + targetedField.Name + " removed \r\n");
                        rawData.Remove(targetedField.Name);                        
                    }
                }
                log.Append(" - RemoveFields transformation end \r\n");
            }
            catch (Exception ex)
            {
                log.Append(" - RemoveFields transformation error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return rawData;
        }

        #endregion
    }
}
