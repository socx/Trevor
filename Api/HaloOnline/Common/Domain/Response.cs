using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Domain
{
    public class Response
    {
        public int ResponseID { get; set; }
        public int RespondentID { get; set; }
        public int QuestionID { get; set; }
        public int OptionID { get; set; }
        public string ResponseText { get; set; }

        public bool AmberFlag { get; set; }

    }
}