using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.ViewData.Rest.Messages
{
    public class ViewDataMessage
    {
        public List<SurveyResponseColumn> Columns { get; set; }
        public List<string[]> Data { get; set; }
    }

    public class SurveyResponseColumn
    {
        public string Title { get; set; }
        public string ToolTip { get; set; }
    }
}