using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EDDY.Nexus.Common.Utilities;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;
using System.Linq;
namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class CreateBatchLeadFile : CodeActivity
    {
        public InArgument<int> BatchDeliveryId { get; set; }
        public InArgument<DeliveryEndpoint> DeliveryEndpoint { get; set; }
        public InArgument<DeliveryLeadData[]> LeadArray { get; set; }
        public OutArgument<string> FilePath { get; set; }

        /// <summary>
        /// When implemented in a derived class, performs the execution of the activity.
        /// </summary>
        /// <param name="context">The execution context under which the activity executes.</param>
        protected override void Execute(CodeActivityContext context)
        {
            DeliveryEndpoint deliveryEndpoint = DeliveryEndpoint.Get(context);
            DeliveryLeadData[] leadArray = LeadArray.Get(context);
            DeliveryLeadData[] transformedLeadArray;
            int batchDeliveryId = BatchDeliveryId.Get(context);


            //Get Batch File Definition from Endpoint
            BatchFileDefinition batchFileDefinition = null;
            switch (deliveryEndpoint.DeliveryType)
            {
                case LeadDeliveryType.BatchEmail:
                    {
                        batchFileDefinition = ((BatchEmailEndpoint)deliveryEndpoint).BatchFileDefinition;
                        break;
                    }
                case LeadDeliveryType.BatchFtp:
                    {
                        batchFileDefinition = ((BatchFtpEndpoint)deliveryEndpoint).BatchFileDefinition;
                        break;
                    }
                default:
                    {
                        throw (new Exception("Incorrect Delivery Endpoint Type."));
                    }
            }

            //get snapshot of time for file naming
            DateTime dateTime = DateTime.Now.ToUniversalTime();

            //set file name, dir and full path
            string fileName = batchFileDefinition.FileNameRegExp + "." + batchFileDefinition.FileExtension;
            if (batchFileDefinition.FileNameRegExp.Equals("EDDYLeads-(MM-dd-yyyy-hhmmss)"))
                fileName = GetFileName(DateTime.Now.ToString(batchFileDefinition.FileNameRegExp), batchFileDefinition.FileExtension, dateTime);

            string fileDir = GetFileDir(batchDeliveryId, deliveryEndpoint, dateTime);

            //Creating File Name 'No_Leads-{0:MM-dd-yyyy-hhmmss}' formatt when there is no lead for the End Point
            if (leadArray == null || leadArray.Length == 0) fileName = fileName.Replace("EDDY", "No");

            string filePath = fileDir + "\\" + fileName;


            //Create Path if it does not exist)
            if (!System.IO.Directory.Exists(fileDir))
            {
                System.IO.Directory.CreateDirectory(fileDir);
            }

            //check file type and create
            switch (batchFileDefinition.BatchFileType)
            {
                case BatchFileType.Delimited:
                    {
                        //create Delimited File
                        CreateDelimitedFile(batchFileDefinition, leadArray, filePath);
                        break;
                    }
                case BatchFileType.XML:
                    {
                        //create XML File
                        CreateXmlFile(batchFileDefinition, leadArray, filePath, deliveryEndpoint.DeliveryEndpointId, batchDeliveryId);
                        break;
                    }
            }



            //return filePath 
            FilePath.Set(context, filePath);

        }

        #region Private Methods

        /// <summary>
        /// Creates the delimited file for a batch delivery.
        /// </summary>
        /// <param name="batchFileDefinition">The batch file definition.</param>
        /// <param name="leadArray">The lead array.</param>
        /// <param name="filePath">The file path.</param>
        private static void CreateDelimitedFile(BatchFileDefinition batchFileDefinition, IEnumerable<DeliveryLeadData> leadArray, string filePath)
        {

            // create a writer and open the file
            TextWriter tw = new StreamWriter(filePath);
            if (!batchFileDefinition.ShowHeading) //If the show heading is off
            {
                foreach (DeliveryLeadData lead in leadArray)
                {
                    int i = 0;
                    foreach (KeyValuePair<string, object> val in lead.TransformedNameValuePairs)
                    {
                        if (!string.IsNullOrEmpty(batchFileDefinition.TextQualifier)) tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));

                        tw.Write(val.Key.ToString() + "=" + val.Value.ToString());
                        if (!string.IsNullOrEmpty(batchFileDefinition.TextQualifier)) tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));
                        if (i < lead.TransformedNameValuePairs.Count - 1)
                            tw.Write(batchFileDefinition.FieldDelimiter);
                        i++;
                    }
                    //catch for /n for newline and replace
                    if (batchFileDefinition.RowDelimiter == "\\r\\n" || batchFileDefinition.RowDelimiter == "\\n")
                    {
                        batchFileDefinition.RowDelimiter = Environment.NewLine;
                    }
                    tw.Write(batchFileDefinition.RowDelimiter);
                }

                // close the stream
                tw.Close();
            }
            else
            {
                List<string> LeadColumns = new List<string>();
                if (leadArray.Count() > 0) //Store all the distinct lead columns in the list
                {
                    LeadColumns = leadArray.SelectMany(a => a.TransformedNameValuePairs.Keys).Distinct().ToList();

                }
                List<KeyValuePair<string, int>> leadColumnandPositions = new List<KeyValuePair<string, int>>();

                foreach (string s in LeadColumns)//create rank(based on how many times the column exist in the all the lead.) to position the order of the column heading
                {
                    int position = leadArray.Count(a => a.TransformedNameValuePairs.Keys.Contains(s));
                    leadColumnandPositions.Add(new KeyValuePair<string, int>(s, position));






                }
                leadColumnandPositions = leadColumnandPositions.OrderByDescending(a => a.Value).ToList();
                int j = 0;
                foreach (var a in leadColumnandPositions)//print column headers first
                {
                    tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));
                    tw.Write(a.Key.ToString());
                    tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));
                    if (j < leadColumnandPositions.Count() - 1)
                        tw.Write(batchFileDefinition.FieldDelimiter);
                    j++;

                }
                if (leadColumnandPositions.Count > 0)
                {
                    //catch for /n for newline and replace
                    if (batchFileDefinition.RowDelimiter == "\\r\\n" || batchFileDefinition.RowDelimiter == "\\n")
                    {
                        batchFileDefinition.RowDelimiter = Environment.NewLine;
                    }
                    tw.Write(batchFileDefinition.RowDelimiter);
                }
                foreach (DeliveryLeadData lead in leadArray)// print column values for all the leads
                {
                    int i = 0;
                    foreach (var col in leadColumnandPositions)
                    {
                        string colValue = string.Empty;
                        if (!string.IsNullOrEmpty(batchFileDefinition.TextQualifier)) tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));
                        if (lead.TransformedNameValuePairs.Any(a => a.Key.Equals(col.Key)))
                            colValue = lead.TransformedNameValuePairs.Where(a => a.Key == col.Key).FirstOrDefault().Value.ToString();
                        tw.Write(!string.IsNullOrWhiteSpace(colValue) ? colValue : " ");
                        if (!string.IsNullOrEmpty(batchFileDefinition.TextQualifier)) tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));
                        if (i < leadColumnandPositions.Count - 1) tw.Write(batchFileDefinition.FieldDelimiter);
                        i++;
                    }
                    //foreach (KeyValuePair<string, object> val in lead.TransformedNameValuePairs)
                    //{
                    //    if (!string.IsNullOrEmpty(batchFileDefinition.TextQualifier)) tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));
                    //    tw.Write(val.Key.ToString() + "=" + val.Value.ToString());
                    //    if (!string.IsNullOrEmpty(batchFileDefinition.TextQualifier)) tw.Write(batchFileDefinition.TextQualifier.Replace("&apos;", "'").Replace("&quot;", "\""));
                    //    if (i < lead.TransformedNameValuePairs.Count - 1)
                    //        tw.Write(batchFileDefinition.FieldDelimiter);
                    //    i++;
                    //}
                    //catch for /n for newline and replace
                    if (batchFileDefinition.RowDelimiter == "\\r\\n" || batchFileDefinition.RowDelimiter == "\\n")
                    {
                        batchFileDefinition.RowDelimiter = Environment.NewLine;
                    }
                    tw.Write(batchFileDefinition.RowDelimiter);
                }



                // close the stream
                tw.Close();
            }

        }

        /// <summary>
        /// Creates the XML file for a batch delivery.
        /// </summary>
        /// <param name="batchFileDefinition">The batch file definition.</param>
        /// <param name="leadArray">The lead array.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="deliveryEndpointId">The delivery endpoint id.</param>
        /// <param name="batchDeliveryId">The batch delivery id.</param>
        private static void CreateXmlFile(BatchFileDefinition batchFileDefinition, IEnumerable<DeliveryLeadData> leadArray, string filePath, int deliveryEndpointId, int batchDeliveryId)
        {

            //get xml
            string xml = GetBatchXml(leadArray, deliveryEndpointId, batchDeliveryId);

            //convert if there is an XSL specified
            if (batchFileDefinition.XSL != null)
            {
                xml = XmlUtilities.TransformXML(xml, batchFileDefinition.XSL);
            }

            // create file with xml content
            TextWriter tw = new StreamWriter(filePath);
            tw.Write(xml);
            tw.Close();



        }

        /// <summary>
        /// Gets the batch XML string to use for a batch delivery XML file.
        /// </summary>
        /// <param name="leadArray">The lead array.</param>
        /// <param name="deliveryEndpointId">The delivery endpoint id.</param>
        /// <param name="batchDeliveryId">The batch delivery id.</param>
        /// <returns>String of XML containing lead info for XML batch delivery.</returns>
        private static string GetBatchXml(IEnumerable<DeliveryLeadData> leadArray, int deliveryEndpointId, int batchDeliveryId)
        {
            //Construct XML from raw data dictionary
            StringBuilder sb = new StringBuilder();

            sb.Append(XmlUtilities.GetXMLOpenNode("Leads",
                                                  new Dictionary<string, string>
                                                      {
                                                          {"EndpointId", deliveryEndpointId.ToString()},
                                                          {"BatchDeliveryId", batchDeliveryId.ToString()}
                                                      }, false));

            //loop through leads
            foreach (DeliveryLeadData t in leadArray)
            {
                //open lead node
                sb.Append(XmlUtilities.GetXMLOpenNode("Lead",
                                                      new Dictionary<string, string> { { "LeadId", t.LeadId.ToString() } },
                                                      false));

                //open fields node
                sb.Append(XmlUtilities.GetXMLOpenNode("Fields", null, false));

                //loop through fields for current lead
                foreach (KeyValuePair<string, object> keyVal in t.TransformedNameValuePairs)
                {
                    //Append field
                    sb.Append(XmlUtilities.GetXMLOpenNode("Field",
                                                          new Dictionary<string, string> { { "Name", keyVal.Key }, { "Value", keyVal.Value.ToString() } },
                                                          true));
                }

                //close fields node
                sb.Append(XmlUtilities.GetXMLCloseNode("Fields"));

                //close lead node
                sb.Append(XmlUtilities.GetXMLCloseNode("Lead"));
            }

            //close leads node
            sb.Append(XmlUtilities.GetXMLCloseNode("Leads"));

            string outputXml = sb.ToString();


            return outputXml;
        }

        /// <summary>
        /// Gets the file directory.
        /// </summary>
        /// <param name="batchDeliveryId"></param>
        /// <param name="deliveryEndpoint">The delivery endpoint.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        private static string GetFileDir(int batchDeliveryId, DeliveryEndpoint deliveryEndpoint, DateTime dateTime)
        {
            //get root from configuration file
            string pathRoot = System.Configuration.ConfigurationManager.AppSettings["BatchFilePath"];

            //Set Folder names
            string yyyy = string.Format("{0:yyyy}", dateTime);
            string mm = string.Format("{0:MM}", dateTime);
            string dd = string.Format("{0:dd}", dateTime);

            //build path
            string returnString = pathRoot + "\\" + yyyy + "\\" + mm + "\\" + dd + "\\" + "ep-" +
                                  deliveryEndpoint.DeliveryEndpointId.ToString("0000") + "\\" + "bd-" +
                                  batchDeliveryId.ToString();



            return returnString;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="fileRegex">The file regex.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        private static string GetFileName(string fileRegex, string fileExtension, DateTime dateTime)
        {
            //default for Regex if not supplied
            //if (fileRegex == null)
            //{
            fileRegex = "EDDY_Leads_{0:MM-dd-yyyy-hhmmss}";
            //}

            //apply regex for name
            string returnFileName = string.Format(fileRegex, dateTime);

            //add extension
            returnFileName += ".";
            returnFileName += fileExtension;


            return returnFileName;
        }

        #endregion



    }
}