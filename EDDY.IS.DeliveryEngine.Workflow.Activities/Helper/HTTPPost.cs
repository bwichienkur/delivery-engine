using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
//using EDDY.Nexus.Entity.DeliveryEngine;
using System.Security.Cryptography.X509Certificates;
using EDDY.IS.DeliveryEngine.Entity;
using System.Collections.Generic;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.Helper
{



    /// <summary>
    /// Class to avoid self issued certificat issues for ssl
    /// </summary>
    public class TrustAllCert
    {
        public TrustAllCert()
        {
        }

        public bool OnValidationCallback (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors )
        {
            return true;
        }
    }

    
    
    /// <summary>
    /// Class that holds HTTP POST Response message
    /// </summary>
    public class WebResponse
    {
        public string ResponseHeader { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeDescription { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }



    public class HttpPost
    {
        // Overloaded
        /// <summary>
        /// Sends the HTTP post.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="postMethod">The post method.</param>
        /// <param name="postContentType">Type of the post content.</param>
        /// <returns></returns>
        public WebResponse SendHttpPost(string uri, string parameters, PostMethod postMethod, PostContentType postContentType)
        {
            return SendHttpPost(uri, parameters, postMethod, postContentType, null);
        }

        /// <summary>
        /// Sends the HTTP post.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="postMethod">The post method.</param>
        /// <param name="postContentType">Type of the post content.</param>
        /// <param name="soapAction">The SOAP action.</param>
        /// <returns></returns>
        public WebResponse SendHttpPost(string uri, string parameters, PostMethod postMethod, PostContentType postContentType, string soapAction)
        {

            var returnResponse = new WebResponse();
             

            //Trust all certificates
            var trust = new TrustAllCert();
            ServicePointManager.ServerCertificateValidationCallback = new
                System.Net.Security.RemoteCertificateValidationCallback(trust.OnValidationCallback);


            // Set Post Method
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            switch (postMethod)
            {
                case PostMethod.HTTPGET:
                    {
                        uri += "?" + parameters;
                        webRequest = (HttpWebRequest)WebRequest.Create(uri);
                        webRequest.Method = "GET";
                        webRequest.Referer = "www.google.com";
                        webRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                        break;
                    }
                case PostMethod.HTTPPOST:
                    {
                        if (!string.IsNullOrWhiteSpace(soapAction) )
                            return SendSoapRequest(uri, parameters, postMethod, postContentType,soapAction );

                        webRequest.Method = "POST";

                        // Set Content Type (add SOAP Action if XML)
                        switch (postContentType)
                        {
                            case PostContentType.Form:
                                {
                                    webRequest.ContentType = "application/x-www-form-urlencoded";
                                    break;
                                }
                            case PostContentType.XML:
                                {
                                    webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                                    if (!string.IsNullOrWhiteSpace(soapAction))
                                    {
                                        webRequest.Headers.Add("SOAPAction", soapAction);
                                    }
                                    //if (parameters.IndexOf("<?xml version=\"1.0\" encoding=\"utf-8\"?>")<0)
                                    //{
                                    //    parameters = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + parameters;
                                    //}
                                    break;
                                }
                        }

                        Stream os = null;
                        try
                        {
                            var encoding = new UTF8Encoding();
                            var bytes = encoding.GetBytes(parameters);
                            webRequest.ContentLength = bytes.Length;
                            os = webRequest.GetRequestStream();
                            os.Write(bytes, 0, bytes.Length);
                        }
                        catch (WebException ex)
                        {

                            var response = ex.Response as HttpWebResponse;

                            if (null == response)

                                return new WebResponse
                                           {
                                               HttpStatusCode = HttpStatusCode.InternalServerError,
                                               ResponseHeader = "",
                                               ResponseCode = HttpStatusCode.InternalServerError.ToString(),
                                               ResponseMessage = ex.Message

                                           };

                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {

                                var error = reader.ReadToEnd();
                                return new WebResponse
                                           {
                                               HttpStatusCode = response.StatusCode,//HttpStatusCode.InternalServerError,
                                               ResponseHeader = response.Headers.ToString(),
                                               ResponseCode = response.StatusCode.ToString(),//HttpStatusCode.InternalServerError.ToString(),
                                               ResponseMessage = error,
                                               ResponseCodeDescription = response.StatusDescription
                                           };

                            }

                        }
                        finally
                        {
                            if (os != null)
                            {
                                os.Close();
                            }
                        }

                        break;
                    }
            }

            // Send Request and Get Response
            try
            {
                var httpWebResponse = (HttpWebResponse) webRequest.GetResponse();

                //Build return object
                using (var sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    returnResponse.HttpStatusCode = httpWebResponse.StatusCode;
                    returnResponse.ResponseHeader = httpWebResponse.Headers.ToString();
                    returnResponse.ResponseCode = returnResponse.HttpStatusCode.ToString();
                    returnResponse.ResponseMessage = sr.ReadToEnd().Trim();
                }
                returnResponse.ResponseCodeDescription = httpWebResponse.StatusDescription;
                return returnResponse;
            }
            catch (WebException ex)
            {

                var response = ex.Response as HttpWebResponse;

                if (null == response)

                    return new WebResponse
                               {
                                   HttpStatusCode = HttpStatusCode.InternalServerError,
                                   ResponseHeader = "",
                                   ResponseCode = HttpStatusCode.InternalServerError.ToString(),
                                   ResponseMessage = ex.Message

                               };

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    var error = reader.ReadToEnd();

                    return new WebResponse
                               {
                                   HttpStatusCode = response.StatusCode,//HttpStatusCode.InternalServerError,
                                   ResponseHeader = response.Headers.ToString(),
                                   ResponseCode = response.StatusCode.ToString(),//HttpStatusCode.InternalServerError.ToString(),
                                   ResponseMessage = error,
                                   ResponseCodeDescription = response.StatusDescription
                               };

                }

            }
            //return null;
        }


        public WebResponse SendSoapRequest(string uri, string parameters, PostMethod postMethod, PostContentType postContentType, string soapAction)
        {

            var returnResponse = new WebResponse();
            try
            {
                var soapEnvelopeXml = CreateSoapEnvelope(parameters);
                var webRequest = CreateWebRequest(uri, soapAction);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                var asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();

                using (var webresponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult))
                using (var rd = new StreamReader(webresponse.GetResponseStream()))
                {
                    returnResponse.HttpStatusCode = webresponse.StatusCode;
                    returnResponse.ResponseHeader = webresponse.Headers.ToString();
                    returnResponse.ResponseCode = returnResponse.HttpStatusCode.ToString();
                    returnResponse.ResponseMessage = rd.ReadToEnd().Trim();
                    returnResponse.ResponseCodeDescription = webresponse.StatusDescription;
                }

                return returnResponse;
            }
            catch (WebException ex)
            {

                var response = ex.Response as HttpWebResponse;

                if (null == response)

                    return new WebResponse
                    {
                        HttpStatusCode = HttpStatusCode.InternalServerError,
                        ResponseHeader = "",
                        ResponseCode = HttpStatusCode.InternalServerError.ToString(),
                        ResponseMessage = ex.Message

                    };

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    var error = reader.ReadToEnd();

                    return new WebResponse
                    {
                        HttpStatusCode = response.StatusCode,//HttpStatusCode.InternalServerError,
                        ResponseHeader = response.Headers.ToString(),
                        ResponseCode = response.StatusCode.ToString(),//HttpStatusCode.InternalServerError.ToString(),
                        ResponseMessage = error,
                        ResponseCodeDescription = response.StatusDescription
                    };

                }

            }
        }

        private  static HttpWebRequest CreateWebRequest(string url, string action)
        {
            var webRequest = (HttpWebRequest) WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.Method = "POST";
            webRequest.Accept = "text/xml";
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            return webRequest;
        }


        private static System.Xml.XmlDocument CreateSoapEnvelope(string soapMessage)
        {
            var soapEnvelope = new System.Xml.XmlDocument();
            soapEnvelope.LoadXml(soapMessage);
            return soapEnvelope;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(System.Xml.XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (var stream=webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }



        #region Code-PostRoman

        /// <summary>
        /// Sends the HTTP post.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="postMethod">The post method.</param>
        /// <param name="postContentType">Type of the post content.</param>
        /// <param name="soapAction">The SOAP action.</param>
        /// <returns></returns>
        public WebResponse SendHttpPost(string uri, string parameters, PostMethod postMethod, PostContentType postContentType, string soapAction, List<DeliveryPostHTTPHeader> DeliveryPostHTTPHeaderList)
        {

            var returnResponse = new WebResponse();


            //Trust all certificates
            var trust = new TrustAllCert();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(trust.OnValidationCallback);

            // Set Post Method
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            switch (postMethod)
            {
                case PostMethod.HTTPGET:
                    {
                        uri += "?" + parameters;
                        webRequest = (HttpWebRequest)WebRequest.Create(uri);
                        webRequest.Method = "GET";

                        //Add custom headers set from the ui
                        WebHeaderCollection myWebHeaderCollection = webRequest.Headers;
                        foreach (DeliveryPostHTTPHeader deliveryPostHTTPHeader in DeliveryPostHTTPHeaderList)
                        {
                            myWebHeaderCollection.Add(deliveryPostHTTPHeader.HTTPHeaderKey, deliveryPostHTTPHeader.HTTPHeaderValue);
                        }

                        webRequest.Referer = "www.google.com";
                        webRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                        break;
                    }
                case PostMethod.HTTPPOST:
                    {
                        if (!string.IsNullOrWhiteSpace(soapAction))
                            return SendSoapRequest(uri, parameters, postMethod, postContentType, soapAction);

                        webRequest.Method = "POST";

                        //Add custom headers set from the ui
                        WebHeaderCollection myWebHeaderCollection = webRequest.Headers;
                        foreach (DeliveryPostHTTPHeader deliveryPostHTTPHeader in DeliveryPostHTTPHeaderList)
                        {
                            myWebHeaderCollection.Add(deliveryPostHTTPHeader.HTTPHeaderKey, deliveryPostHTTPHeader.HTTPHeaderValue);
                        }

                        // Set Content Type (add SOAP Action if XML)
                        switch (postContentType)
                        {
                            case PostContentType.Form:
                                {
                                    webRequest.ContentType = "application/x-www-form-urlencoded";
                                    break;
                                }
                            case PostContentType.XML:
                                {
                                    webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                                    if (!string.IsNullOrWhiteSpace(soapAction)) webRequest.Headers.Add("SOAPAction", soapAction);
                                    break;
                                }
                            case PostContentType.JSON:
                                {
                                    webRequest.ContentType = "application/json";
                                    break;
                                }
                        }

                        Stream os = null;
                        try
                        {
                            var encoding = new UTF8Encoding();
                            var bytes = encoding.GetBytes(parameters);
                            webRequest.ContentLength = bytes.Length;
                            os = webRequest.GetRequestStream();
                            os.Write(bytes, 0, bytes.Length);
                        }
                        catch (WebException ex)
                        {

                            var response = ex.Response as HttpWebResponse;

                            if (null == response)
                                return new WebResponse
                                {
                                    HttpStatusCode = HttpStatusCode.InternalServerError,
                                    ResponseHeader = "",
                                    ResponseCode = HttpStatusCode.InternalServerError.ToString(),
                                    ResponseMessage = ex.Message
                                };

                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                var error = reader.ReadToEnd();
                                return new WebResponse
                                {
                                    HttpStatusCode = response.StatusCode,//HttpStatusCode.InternalServerError,
                                    ResponseHeader = response.Headers.ToString(),
                                    ResponseCode = response.StatusCode.ToString(),//HttpStatusCode.InternalServerError.ToString(),
                                    ResponseMessage = error,
                                    ResponseCodeDescription = response.StatusDescription
                                };
                            }
                        }
                        finally { if (os != null) os.Close(); }

                        break;
                    }
            }

            // Send Request and Get Response
            try
            {
                var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                //Build return object
                using (var sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    returnResponse.HttpStatusCode = httpWebResponse.StatusCode;
                    returnResponse.ResponseHeader = httpWebResponse.Headers.ToString();
                    returnResponse.ResponseCode = returnResponse.HttpStatusCode.ToString();
                    returnResponse.ResponseMessage = sr.ReadToEnd().Trim();
                }
                returnResponse.ResponseCodeDescription = httpWebResponse.StatusDescription;
                return returnResponse;
            }
            catch (WebException ex)
            {

                var response = ex.Response as HttpWebResponse;

                if (null == response)

                    return new WebResponse
                    {
                        HttpStatusCode = HttpStatusCode.InternalServerError,
                        ResponseHeader = "",
                        ResponseCode = HttpStatusCode.InternalServerError.ToString(),
                        ResponseMessage = ex.Message

                    };

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    var error = reader.ReadToEnd();

                    return new WebResponse
                    {
                        HttpStatusCode = response.StatusCode,//HttpStatusCode.InternalServerError,
                        ResponseHeader = response.Headers.ToString(),
                        ResponseCode = response.StatusCode.ToString(),//HttpStatusCode.InternalServerError.ToString(),
                        ResponseMessage = error,
                        ResponseCodeDescription = response.StatusDescription
                    };

                }

            }
            //return null;
        }

        #endregion

    }
}
