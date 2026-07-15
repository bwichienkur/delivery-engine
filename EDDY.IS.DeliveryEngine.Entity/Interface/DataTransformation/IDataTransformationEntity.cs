using System;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{

    public interface IDataTransformationEntity
    {
        #region Properties

        Int32 Id { get; set; }

        string Name { get; set; }
                
        Int32 DeliveryDefId { get; set; }

        Int32 FormValidationId { get; set; }

        int ConditionXmlId { get; set; }

        string ConditionXml { get; set; }

        int TaskXmlId { get; set; }

        string TaskXml { get; set; }

        Int32 SequenceNo { get; set; }
        #endregion
    }
}
