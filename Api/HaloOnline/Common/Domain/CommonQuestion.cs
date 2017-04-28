using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Domain
{
    public class CommonQuestion
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public string Summary { get; set; }
        public int QuestionTypeID { get; set; }
        public int PromptID { get; set; }
        public int OptionSetID { get; set; }
        public IEnumerable<CommonOption> Options { get; set; }
    }
}