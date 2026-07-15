using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class ApplyXsltForXml : IDataTransformationTask
    {
        #region Properties

        [XmlArray("TargetedFields")]
        [XmlArrayItem("TargetedField")]
        public TargetedField[] TargetedFields { get; set; }

        public string Xsl { get; set; }
        #endregion

        #region IDataTransformationTask Members

        /// <summary>
        /// Transforms the rawData dictionary with XML rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {

            //public delegate  Dictionary<string,object>() resultDel();

            var result = new Dictionary<string, object>();

            try
            {
                log.Append("ApplyXsltForXml transformation started. All other transformed values will be removed. ");
                var xml = ConvertRawDataToXml(rawData);

                //For loading file in local memory temporarily
                Random random = new Random();

                string currTime = DateTime.Now.Millisecond.ToString() + random.Next().ToString();
                var outputDir = @"C:\DeliveryTransformationTemp" + @"\DT_Temp";
                var xmlPath = outputDir + @"\DT" + currTime + ".xml";
                var xslPath = outputDir + @"\DT" + currTime + ".xsl";
                var outputXmlPath = outputDir + @"\DT_Output" + currTime + ".xsl";

                if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

                var succes = false;
                var maxRetries = 3;
                var currTry = 1;


                if (Directory.Exists(outputDir))
                {
                    while (!succes && currTry <= maxRetries)
                    {



                        using (StreamWriter sw = new StreamWriter(xmlPath))
                        {
                            sw.Write(xml.Trim());
                        }

                        using (StreamWriter sw = new StreamWriter(xslPath))
                        {
                            sw.Write(this.Xsl);
                        }

                        log.AppendFormat(" - Existing XML:{0} | ", xml);





                        succes = ProcessXSLT(ref result, ref log, xmlPath, xslPath, outputXmlPath);
                        currTry += 1;
                        System.Threading.Thread.Sleep(1000);
                    }

                    if (File.Exists(xmlPath)) File.Delete(xmlPath);
                    if (File.Exists(xslPath)) File.Delete(xslPath);
                    if (File.Exists(outputXmlPath)) File.Delete(outputXmlPath);
                    //if(Directory.Exists(outputDir))
                    //{                        
                    //    Directory.Delete(outputDir, true);
                    //}
                }
                log.Append("ApplyXsltForXml transformation end. ");
            }
            catch (Exception ex)
            {
                log.Append("ApplyXsltForXml transformation error." + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return result;
        }
        public bool ProcessXSLT(ref Dictionary<string, object> result, ref StringBuilder log, string xmlPath, string xslPath, string outputXmlPath)
        {
            try
            {

                if (File.Exists(xmlPath) && File.Exists(xslPath))
                {
                    //load the Xml doc
                    XPathDocument myXPathDoc = new XPathDocument(xmlPath);

                    XslCompiledTransform myXct = new XslCompiledTransform();

                    //load the Xsl                 
                    myXct.Load(xslPath);

                    //create the output stream
                    using (var myWriter = new XmlTextWriter(outputXmlPath, null))
                    {
                        myXct.Transform(myXPathDoc, null, myWriter);
                        myXct.TemporaryFiles.Delete();
                        myWriter.Close();

                    }


                    //Remove all other pairs..only leave xml
                    var deliveryLabel = string.Empty;
                    if (TargetedFields != null)
                    {
                        deliveryLabel = TargetedFields[0].OverwriteName;
                    }
                    deliveryLabel = (string.IsNullOrWhiteSpace(deliveryLabel) ? "TransformedXml" : deliveryLabel);

                    using (var sr = new StreamReader(outputXmlPath))
                    {
                        result.Add(deliveryLabel, sr.ReadToEnd().Trim());
                    }

                    log.AppendFormat(" - Transformed XML:{0} | ", result[deliveryLabel]);

                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Converts raw data to xml string
        /// </summary>
        /// <param name="rawData">raw data dictionary</param>
        /// <returns>xml string</returns>
        private static string ConvertRawDataToXml(Dictionary<string, object> rawData)
        {
            string outputXml;
            try
            {
                //Construct XML from raw data dictionary
                var sb = new StringBuilder();
                sb.Append("<xmlData><fields>");
                foreach (var item in rawData)
                {
                    sb.AppendFormat("<field name='{0}' value='{1}' />", item.Key, item.Value.ToString().Replace("'", "&apos;"));
                }
                sb.Append("</fields></xmlData>");
                outputXml = sb.ToString();
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return outputXml;
        }

        #endregion
    }
}