using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity.Common
{
    [DataContract]
    public abstract class BaseXmlEntity : CommonEntity
    {
        #region constructor
        #endregion

        #region private functions
        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        } 

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        #endregion private

        #region public methods to Serialize and Deserialize
        
        /// <summary>
        /// Serialize an object into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="extracTypes"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, [Optional] Type[] extracTypes)
        {
            string xmlString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof (T), extracTypes ?? new Type[0]);
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
            {
                // xmlTextWriter.Settings.OmitXmlDeclaration = true;
                xs.Serialize(xmlTextWriter, obj);
                using(memoryStream = (MemoryStream) xmlTextWriter.BaseStream)
                {
                    xmlString = UTF8ByteArrayToString(memoryStream.ToArray());
                }
            }

            memoryStream.Dispose();
            return xmlString;
        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="extracTypes"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml, [Optional] Type[] extracTypes)
        {
    
            XmlSerializer xs = new XmlSerializer(typeof(T), extracTypes??new Type[0]);
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xml));

            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (T) xs.Deserialize(memoryStream);
                
            
            // xmlTextWriter.Settings.OmitXmlDeclaration = true;
           
        }


        /// <summary>
        /// Transforming object to XML
        /// </summary>
        /// <param name="extracTypes"></param>
        /// <returns></returns>
        public string ToXML([Optional] Type[] extracTypes)
        {
            string xmlString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(this.GetType(), extracTypes ?? new Type[0]);
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
            {
                xmlTextWriter.Settings.OmitXmlDeclaration = true;
                xs.Serialize(xmlTextWriter, this);
                using (memoryStream = (MemoryStream)xmlTextWriter.BaseStream)
                {
                    xmlString = UTF8ByteArrayToString(memoryStream.ToArray());
                }
            }
            memoryStream.Dispose();
            return xmlString;
        }

        /// <summary>
        /// Transforming XML to object
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="extracTypes"></param>
        /// <returns></returns>
        public Object ToObject(string xml, [Optional] Type[] extracTypes)
        {
            XmlSerializer xs = new XmlSerializer(this.GetType(), extracTypes??new Type[0]);
            using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xml)))
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    xmlTextWriter.Settings.OmitXmlDeclaration = true;
                }
                return xs.Deserialize(memoryStream);
            }
        }
        #endregion
    }
}
