using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    public class ReportResponseMessage
    {
        public ReportResponse Current { get; set; }
        public ReportResponse Previous { get; set; }
    }

    public class ReportResponse
    {
        public int Respondents { get; set; }
        public string Period { get; set; }
    }
}