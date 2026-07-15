using System;
using System.Activities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
//using EDDY.Nexus.BusinessComponent.DataTransformation;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
//using EDDY.Nexus.DataAccess.DataTransformation;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DataTransformation;
//using EDDY.Nexus.Entity.DeliveryEngine;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation;
using EDDY.IS.DeliveryEngine.Entity.Common;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class TransformData : CodeActivity
    {
        public InArgument<DeliveryLeadData> RawLeadData { get; set; }
        public InOutArgument<DeliveryEndpoint> Endpoint { get; set; }
        public OutArgument<DeliveryLeadData> TransformedLeadData { get; set; }


        #region "Activity"

        protected override void Execute(CodeActivityContext context)
        {

                //intialize return object to input
                var returnLeadData = RawLeadData.Get(context);
                var endpoint = Endpoint.Get(context);
                var deliveryEndpointId = endpoint.DeliveryEndpointId;

               
                //Get Transformed Data
                var transformedNameValuePairs = GetTransformedNameValuePairs((int)deliveryEndpointId, returnLeadData);
                
                
               //Email Subject && Email routing transformations
                if((endpoint.DeliveryTypeId==2||endpoint.DeliveryTypeId==3) 
                    && transformedNameValuePairs.Count>0 
                    && transformedNameValuePairs.ContainsKey("EmailSubject"))
                {
                    endpoint = SwitchEmailSubject(ref transformedNameValuePairs, endpoint);
                    Endpoint.Set(context, endpoint);
                }
                
            if ((endpoint.DeliveryTypeId == 2 || endpoint.DeliveryTypeId == 3) 
                && transformedNameValuePairs.Count > 0 
                && transformedNameValuePairs.ContainsKey("LiveEmailTo"))
                {
                    endpoint = SwitchEmailRouting(ref transformedNameValuePairs, endpoint);
                    Endpoint.Set(context, endpoint);
                }

            if (transformedNameValuePairs.ContainsKey("LiveEmailTo"))
                transformedNameValuePairs.Remove("LiveEmailTo");

                
                //append Transformed Data to return object
                returnLeadData.TransformedNameValuePairs = transformedNameValuePairs;

                // Store the request in the OutArgument
                Endpoint.Set(context, endpoint);
                TransformedLeadData.Set(context, returnLeadData);

        }

        #endregion


        #region "Private Methods"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformedLeadData"></param>
        /// <param name="endpointTobeModified"></param>
        /// <returns></returns>
        private static DeliveryEndpoint SwitchEmailSubject(ref Dictionary<string, object> transformedLeadData,DeliveryEndpoint endpointTobeModified)
        {

                if(transformedLeadData.ContainsKey("EmailSubject"))
                {
                    if(endpointTobeModified.DeliveryTypeId==3)
                        ((BatchEmailEndpoint) endpointTobeModified).EmailSubject =
                            transformedLeadData["EmailSubject"].ToString();
                        
                    else
                        ((InstantEmailEndpoint)endpointTobeModified).EmailSubject =
                            transformedLeadData["EmailSubject"].ToString();

                        transformedLeadData.Remove("EmailSubject");
                        
                }
             
            
            return (DeliveryEndpoint) endpointTobeModified;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformedLeadData"></param>
        /// <param name="endpointTobeModified"></param>
        /// <returns></returns>
        private static DeliveryEndpoint SwitchEmailRouting(ref Dictionary<string, object> transformedLeadData, DeliveryEndpoint endpointTobeModified)
        {

            if (transformedLeadData.ContainsKey("LiveEmailTo"))
            {
                if (endpointTobeModified.DeliveryTypeId == 3)
                    ((BatchEmailEndpoint) endpointTobeModified).LiveEmailTo =
                    transformedLeadData["LiveEmailTo"].ToString();
                else
                    ((InstantEmailEndpoint) endpointTobeModified).LiveEmailTo =
                    transformedLeadData["LiveEmailTo"].ToString();
               

                transformedLeadData.Remove("LiveEmailTo");
            }


            return (DeliveryEndpoint)endpointTobeModified;
        }
        
        
        /// <summary>
        /// Gets the transformed name value pairs.
        /// </summary>
        /// <param name="deliveryEndpointId">The delivery endpoint id.</param>
        /// <param name="leadData"></param>
        /// <returns>Transformed values. If no transformation applies, original values returned.</returns>
        private static Dictionary<string, object> GetTransformedNameValuePairs(int deliveryEndpointId, DeliveryLeadData leadData)
        {

            var rawNameValuePairs = leadData.AllFields;
            Dictionary<string, object> retNameValuePairs;
            var retList = new List<KeyValueObject>();
            IDataTransformationBC dataTransformationBc = new DataTransformationBC();
            //var deDao = new DeliveryEngineDAO();
            var dtDao = new DataTransformationDAO();

            try
            {

               
                //Create Meta-Data Object to assist in Condition evaluation for DT
               var metaData = new List<KeyValueObject>
                                  {
                                      new KeyValueObject("CRId", leadData.CRId),
                                      new KeyValueObject("PSIId", leadData.PSIId),
                                      new KeyValueObject("ProgramId", leadData.ProgramId),
                                      new KeyValueObject("FormId",leadData.FormUniqueId)
                                  };

                var log = new StringBuilder();
                log.Append(" - Service Started :");

                //Converts the key/value string to dictionary
                var rawDataDictionary = rawNameValuePairs;
                var metaDataDictionary = metaData.ToDictionary(m => m.Key, m => m.Value);
                    
                try
                {
                    retNameValuePairs = dataTransformationBc.Transform(rawDataDictionary, metaDataDictionary, EventType.Delivery, deliveryEndpointId, ref log);
                }
                finally
                {
                    var logRecord = new LogEntity
                                        {
                                            CreatedBy = 1,
                                            CreatedDate = DateTime.Now,
                                            LeadId = leadData.LeadId,
                                            RowGuid = new Guid(),
                                            Message = log.ToString()

                                        };
                    dtDao.CreateLog(logRecord);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(leadData.LeadId.ToString(CultureInfo.InvariantCulture) + " - Exception in TransformData: " + ex);
                throw;
            }

            return retNameValuePairs;
        }

        #endregion


    }
}
