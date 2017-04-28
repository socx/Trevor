using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    public class SurveyMessage
    {
        public int SurveyID { get; set; }
        public string SiteID { get; set; }
        public string LogoFile { get; set; }
        public string Passcode { get; set; }
        public string Survey { get; set; }
        public string StartDate { get; set; }
        public string Description { get; set; }
        public string SurveyName { get; set; }
    }
}