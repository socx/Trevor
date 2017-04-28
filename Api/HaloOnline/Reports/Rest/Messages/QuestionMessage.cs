using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    public class QuestionMessage
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public string Summary { get; set; }
        public int QuestionTypeID { get; set; }
        public int PromptID { get; set; }
        public int OptionSetID { get; set; }
        public IEnumerable<ReportOption> Options { get; set; }
    }

    public class Option
    {
        public int OptionID { get; set; }
        public string OptionText { get; set; }
        public int QuestionID { get; set; }
    }

    public class ReportOption
    {
        public int Value { get; set; }
        public string Option { get; set; }
        public int Count { get; set; }
    }
}