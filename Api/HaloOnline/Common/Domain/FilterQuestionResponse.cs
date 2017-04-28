using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Domain
{
    public class FilterQuestionResponse
    {
        public int RespondentID { get; set; }
        public DateTime CompletionDate { get; set; }
        public int ResponseID1 { get; set; }
        public int ValueResponse1 { get; set; }
        public int PromptID1 { get; set; }
        public string Prompt1 { get; set; }
        public string Option1 { get; set; }
        public int ResponseID2 { get; set; }
        public int ValueResponse2 { get; set; }
        public int PromptID2 { get; set; }
        public string Prompt2 { get; set; }
        public string Option2 { get; set; }
    }
}