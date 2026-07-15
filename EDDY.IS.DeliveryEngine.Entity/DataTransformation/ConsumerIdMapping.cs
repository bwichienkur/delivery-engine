using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
   [DataContract]
   [Serializable]
    public class ConsumerIdMapping
    {
        public ConsumerIdMapping(IDataReader dataReader)
        {
           
            ConsumerIdType = DatabaseUtilities.SetStringValue(dataReader[(int) ConsumerIdMappingTableField.ConsumerIdType]);
            ConsumerId = DatabaseUtilities.SetInt32Value(dataReader[(int) ConsumerIdMappingTableField.ConsumerId]);
            InternalId = DatabaseUtilities.SetInt32Value(dataReader[(int) ConsumerIdMappingTableField.InternalId]);
            IsExclusion = DatabaseUtilities.SetBooleanValue(dataReader[(int) ConsumerIdMappingTableField.IsExclusion]);
            EntityMetaName = DatabaseUtilities.SetStringValue(dataReader[(int) ConsumerIdMappingTableField.EntityMetaName]);
            //ELRFieldName = DatabaseUtilities.SetStringValue(dataReader[(int)ConsumerIdMappingTableField.ELRFieldName]);
          
        }
        public ConsumerIdMapping(IDataReader dataReader, bool blnElr)
        {

            ConsumerIdType = DatabaseUtilities.SetStringValue(dataReader[(int)ConsumerIdMappingTableField.ConsumerIdType]);
            ConsumerId = DatabaseUtilities.SetInt32Value(dataReader[(int)ConsumerIdMappingTableField.ConsumerId]);
            InternalId = DatabaseUtilities.SetInt32Value(dataReader[(int)ConsumerIdMappingTableField.InternalId]);
            IsExclusion = DatabaseUtilities.SetBooleanValue(dataReader[(int)ConsumerIdMappingTableField.IsExclusion]);
            EntityMetaName = DatabaseUtilities.SetStringValue(dataReader[(int)ConsumerIdMappingTableField.EntityMetaName]);
            ELRFieldName = DatabaseUtilities.SetStringValue(dataReader[(int)ConsumerIdMappingTableField.ELRFieldName]);
           
        }

       [DataMember]
        public string ConsumerIdType { get; set; }

       [DataMember] 
       public int ConsumerId { get; set; }

       [DataMember] 
       public int InternalId { get; set; }

       [DataMember] 
       public bool IsExclusion { get; set; }

       [DataMember] 
       public string EntityMetaName { get; set; }

       [DataMember] 
       public string ELRFieldName { get; set; }

        public enum ConsumerIdMappingTableField
        {
            ConsumerIdType,
            ConsumerId,
            InternalId,
            IsExclusion,
            EntityMetaName,
            ELRFieldName
        }
    }

    
}
