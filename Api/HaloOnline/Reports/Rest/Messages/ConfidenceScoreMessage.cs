using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    public class ConfidenceScoreMessage
    {
        public ConfidenceScore Current { get; set; }
        public ConfidenceScore Previous { get; set; }
        public ConfidenceScore AllTime { get; set; }
    }

    public class ConfidenceScore
    {
        public string Period { get; set; }
        public int Respondents { get; set; }
        public decimal VeryConfident { get; set; }
        public decimal FairlyConfident { get; set; }
        public decimal NotVeryConfident { get; set; }
        public decimal NotAtAllConfident { get; set; }

    }
}