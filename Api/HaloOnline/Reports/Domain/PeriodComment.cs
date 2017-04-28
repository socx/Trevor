using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Domain
{

    public class PeriodComment
    {
        public string Comment { get; set; }
        public bool AmberFlag { get; set; }
        public bool RedFlag { get; set; }
        public int RespondentID { get; set; }
    }
}