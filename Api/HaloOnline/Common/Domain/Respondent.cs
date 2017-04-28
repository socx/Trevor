using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Domain
{
    public class Respondent
    {
        public int RespondentID { get; set; }
        public int SurveyID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool RedFlag { get; set; }

        public List<Response> Responses { get; set; }

        public CommonWorkOrder WorkOrder { get; set; }
        public string Extra { get; set; }
        public string ResponseDate { get; set; }
    }
}