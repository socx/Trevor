using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    public class ConfidenceTrendMessage
    {
        public string Period { get; set; }
        public decimal? Previous { get; set; }
        public decimal? Current { get; set; }
    }
}