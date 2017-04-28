using HaloOnline.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.ViewData.Rest.Messages
{
    public class ViewDataRequest : DataRequest
    {
        public int SurveyID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ImagePath { get; set; }
    }
}