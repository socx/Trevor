using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    using Common;

    public class DirectCommentRequest : DataRequest
    {
        public int SurveyId { get; set; }
        public string Comment { get; set; }
    }
}