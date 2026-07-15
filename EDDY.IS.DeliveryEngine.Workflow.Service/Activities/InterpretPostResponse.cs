using System.Activities;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;
using System.Collections.Generic;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using System;



namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class InterpretPostResponse : CodeActivity
    {
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<string> PostResponse { get; set; }
        public InArgument<string> ServerResponse { get; set; }
        public OutArgument<bool> PostSuccess { get; set; }
        public OutArgument<int> SchoolValidationResponseStatus { get; set; }
        public OutArgument<int> RejectionReasonID { get; set; }
        public OutArgument<int> ReviewedStatusID { get; set; }
        
        private bool _postSuccess;
        private int schoolValidationResponseStatus = 0;
        


        protected override void Execute(CodeActivityContext context)
        {
            var ipe = Endpoint.Get(context) as InstantPostEndpoint;
            var ld = LeadData.Get(context);
            var postResponseString = PostResponse.Get(context);
            var serverResponse = ServerResponse.Get(context);
            var forceRepost = ipe.ForceRepostResponseString;
            var successRespString = ipe.SuccessResponseString;
            var failureResponseString = ipe.FailureResponseString;
            var duplicateResponseString = ipe.DuplicateResponseString;
            var matched = false;
            int rejectionReasonID = 0;
            int NewReviewedStatusID = 0;
            bool LeadStatusIDUpdated = false;

            schoolValidationResponseStatus = 200;

            if (serverResponse.StartsWith("2") 
                        || serverResponse.ToUpper() == "OK" 
                        || serverResponse.ToUpper() == "CREATED") 
                _postSuccess = true;
            else
            {
                        _postSuccess = false;
                        schoolValidationResponseStatus = 485;
            }
                       
            //success and no response
            if (_postSuccess && string.IsNullOrWhiteSpace(postResponseString))
            {
                _postSuccess = true;

            }
            else
            {

                //force repost
                if (_postSuccess && !string.IsNullOrWhiteSpace(forceRepost)
                    && (System.Text.RegularExpressions.Regex.IsMatch(postResponseString,
                                                                     forceRepost,
                                                                     System.Text.RegularExpressions.RegexOptions.
                                                                         IgnoreCase)))
                {
                    _postSuccess = false;
                    schoolValidationResponseStatus = 480;
                    matched = true;
                }


                //failure
                if (_postSuccess && !string.IsNullOrWhiteSpace(failureResponseString)
                    && (System.Text.RegularExpressions.Regex.IsMatch(postResponseString,
                                                                     failureResponseString,
                                                                     System.Text.RegularExpressions.RegexOptions.
                                                                         IgnoreCase)))
                {
                    _postSuccess = true;
                    schoolValidationResponseStatus = 460;
                    matched = true;
                }

                ////duplicate
                //else if (_postSuccess && !string.IsNullOrWhiteSpace(duplicateResponseString)
                //    && (System.Text.RegularExpressions.Regex.IsMatch(postResponseString,
                //                                                     duplicateResponseString,
                //                                                     System.Text.RegularExpressions.RegexOptions.
                //                                                         IgnoreCase)))
                //{
                //    _postSuccess = true;
                //    schoolValidationResponseStatus = 450;
                //    matched = true;
                //}

                //success doesnt match
                else if (_postSuccess && !string.IsNullOrWhiteSpace(successRespString)
                    && !(System.Text.RegularExpressions.Regex.IsMatch(postResponseString,
                                                                      successRespString,
                                                                      System.Text.RegularExpressions.RegexOptions.
                                                                          IgnoreCase)))
                {
                    _postSuccess = true;
                    schoolValidationResponseStatus = 475;
                    matched = true;
                }

               //success matches
                else if (_postSuccess && !string.IsNullOrWhiteSpace(successRespString)
                    && (System.Text.RegularExpressions.Regex.IsMatch(postResponseString,
                                                                      successRespString,
                                                                      System.Text.RegularExpressions.RegexOptions.
                                                                          IgnoreCase)))
                {
                    _postSuccess = true;
                    schoolValidationResponseStatus = 200;
                    matched = true;
                }

                if (_postSuccess &&
                    !string.IsNullOrWhiteSpace(forceRepost)
                    && !string.IsNullOrWhiteSpace(failureResponseString)                    
                    && !string.IsNullOrWhiteSpace(successRespString)
                    && !string.IsNullOrWhiteSpace(postResponseString)
                    && matched == false)
                {
                    _postSuccess = true;
                    schoolValidationResponseStatus = 490;
                    matched = false;
                }


            }
                                                        
            //AUTO REVIEW CODE -- ONLY FOR REJECTIONS
            var deDAO = new DeliveryEngineDAO();
            if (schoolValidationResponseStatus == 460 || schoolValidationResponseStatus == 475)
            {               
                List<DeliveryRejectionReasons> allRejectionReasons = new List<DeliveryRejectionReasons>();
                allRejectionReasons = ipe.DeliveryRejectionReasons;

                foreach (DeliveryRejectionReasons item in allRejectionReasons)
                {                    
                    if (_postSuccess & !string.IsNullOrWhiteSpace(item.RejectionResponse)
                    && (System.Text.RegularExpressions.Regex.IsMatch(postResponseString,                                                                                                    item.RejectionResponse, System.Text.RegularExpressions.RegexOptions.IgnoreCase)))
                    { 
                        //if rejection response matches what is pre-defined in the admin...then if the autoReview flag is set to (yes-"1")...
                        //1)find out new status based on (crid & reasonid) - sp will return new reviewed statusid (240-260)  
                        rejectionReasonID = item.ReasonID;

                        //duplicate checked moved down here
                        if (rejectionReasonID == 29 || rejectionReasonID == 30 || rejectionReasonID == 31)
                        {
                            _postSuccess = true;
                            duplicateResponseString = item.RejectionResponse;
                            schoolValidationResponseStatus = 450;
                            matched = true;
                        }
                        
                        if (item.AutoReviewID == 1)
                        {
                            NewReviewedStatusID = Convert.ToInt32(deDAO.LogNewReviewedStatusID(ld.LeadId.ToString(), rejectionReasonID, "Auto"));                            
                            break;                       
                        }                         
                   }                   
                }                  
            } 

            
            //END OF AUTO REVIEW CODE
            

            // Store the request in the OutArgument
            PostSuccess.Set(context, _postSuccess);
            SchoolValidationResponseStatus.Set(context, schoolValidationResponseStatus);
            RejectionReasonID.Set(context, rejectionReasonID);
            ReviewedStatusID.Set(context, NewReviewedStatusID);
        }
    }
}
