using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{
    public interface IDataTransformationEntityNew
    {
        Int32 Id { get; set; }

        string Name { get; set; }

        Int32 DeliveryDefId { get; set; }

        Int32 FormValidationId { get; set; }

        int ConditionXmlId { get; set; }

        string ConditionXml { get; set; }

        int TaskXmlId { get; set; }

        string TaskXml { get; set; }

        Int32 SequenceNo { get; set; }

        DateTime TaskCreatedDate { get; set; }

        DateTime TaskUpdatedDate { get; set; }

        string TaskUpdatedByName { get; set; }

        int TaskCreatedBy { get; set; }

        DateTime ConditionCreatedDate { get; set; }

        DateTime ConditionUpdatedDate { get; set; }

        string ConditionUpdatedByName { get; set; }

        int ConditionCreatedBy { get; set; }

        

    }
}
