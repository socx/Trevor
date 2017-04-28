using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    using Common;

    public class ReportRequest : DataRequest
    {
        public int SurveyID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AllWithOrWithoutFilters { get; set; }
        public string LocationLevel1 { get; set; }
        public string LocationLevel2 { get; set; }
        public string LocationLevel3 { get; set; }
        public string LocationLevel4 { get; set; }
        public string LocationLevel5 { get; set; }
        public string LocationLevel6 { get; set; }
        public string LocationLevel7 { get; set; }
        public string LocationLevel8 { get; set; }
        public string SLA { get; set; }
        public string SLABreach { get; set; }
        public string SLAContract { get; set; }
        public string SLAService { get; set; }
        public string SLAServiceType { get; set; }
        public string Cause { get; set; }
        public string Failure { get; set; }
        public string Problem { get; set; }
        public string Remedy { get; set; }
        public string PrintHtml { get; set; }
        public string FilterQuestionPromptID1 { get; set; }
        public string FilterQuestionPromptID2 { get; set; }
        public string FilterQuestionResponse1 { get; set; }
        public string FilterQuestionResponse2 { get; set; }
        public string HasFilterQuestion { get; set; }
        
    }
}