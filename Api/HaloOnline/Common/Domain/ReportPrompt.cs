using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Domain
{
    public class ReportPrompt
    {
        public int PromptID { get; set; }
        public string Prompt { get; set; }
        public string NegativePrompt { get; set; }
        public string PositivePrompt { get; set; }
        public int QuestionID { get; set; }
        public string QuestionType { get; set; }
    }
}