using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Domain
{
    public class ReportsResponse
    {
        public int ResponseID { get; set; }
        public int RespondentID { get; set; }
        public int QuestionID { get; set; }
        public int OptionID { get; set; }
        public string ResponseText { get; set; }
        public bool AmberFlag { get; set; }
    }
}