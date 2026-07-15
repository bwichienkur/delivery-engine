using System;
using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class LeadViewerSearchParam
    {
        #region Constructor

        public LeadViewerSearchParam()
        {
            StartPosition = 1;
            EndPosition = 25;
            Pagesize = 25;
            PageNumber = 1;
        }

        #endregion

        #region Data Members

        [DataMember]
        public int CRID { get; set; }

        [DataMember]
        public string CRIDType { get; set; }

        [DataMember]
        public int ApplicationId { get; set; }

        [DataMember]
        public int DeliveryTypeId { get; set; } 

        [DataMember]
        public int LeadStatusId { get; set; }

        [DataMember]
        public string FirstNameMatchType { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string FirstNameMatchTypeStartsWith { get; set; }

        [DataMember]
        public string FirstNameMatchTypeContains { get; set; }

        [DataMember]
        public string LastNameMatchType { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string LastNameMatchTypeStartsWith { get; set; }

        [DataMember]
        public string LastNameMatchTypeContains { get; set; }

        [DataMember]
        public string PsiName { get; set; }

        [DataMember]
        public string PsiNameMatchType { get; set; }

        [DataMember]
        public string PsiNameMatchTypeStartsWith { get; set; }

        [DataMember]
        public string PsiNameMatchTypeContains { get; set; }

        [DataMember]
        public string Email { get; set; }
       
        [DataMember]
        public int? ProgramID { get; set; }

        [DataMember]
        public string DeliveryURL { get; set; }

        [DataMember]
        public string StartDate { get; set; }

        [DataMember]
        public string EndDate { get; set; }

        [DataMember]
        public DateTime StartDateWithTime { get; set; }

        [DataMember]
        public DateTime EndDateWithTime { get; set; }

        [DataMember]
        public int StartPosition { get; set; }

        [DataMember]
        public int EndPosition { get; set; }

        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public int Pagesize { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public string Sthr { get; set; }

        [DataMember]
        public string Stmin { get; set; }

        [DataMember]
        public string Stsec { get; set; }

        [DataMember]
        public string Endhr { get; set; }

        [DataMember]
        public string Endmin { get; set; }

        [DataMember]
        public string Endsec { get; set; }

        [DataMember]
        public string Stampm { get; set; }

        [DataMember]
        public string Endampm { get; set; }

        [DataMember]
        public string SelectedStatuses { get; set; }

        [DataMember]
        public bool IsBeta { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string LeadID { get; set; }

        [DataMember]
        public int SubChannelID { get; set; }

        [DataMember]
        public int VendorID { get; set; }

        [DataMember]
        public string TrackID { get; set; }

        [DataMember]
        public int AccountManagerId { get; set; }

        [DataMember]
        public int DeliveryStatusTypeId { get; set; }

        [DataMember]
        public string FiltersDisplay { get; set; }

        [DataMember]
        public int LeadPaidStatusTypeId { get; set; }

        [DataMember]
        public bool DateRangeValidForBillingMonthUpdate { get; set; }


        #endregion
    }
}
