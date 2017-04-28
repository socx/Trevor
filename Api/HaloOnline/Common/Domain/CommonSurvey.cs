using HaloOnline.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Domain
{

    public class CommonSurvey
    {
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public string Description { get; set; }
        public int SiteID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        /// <summary>
        /// Total number of survey responses
        /// </summary>
        public int TotalNumberOfRespondents { get; set; }
        /// <summary>
        /// Number of survey responses with  filters
        /// </summary>
        public int WorkOrderRespondents { get; set; }
        public bool HasWorkOrders { get; set; }
        /// <summary>
        /// whether survey has Filter Questions
        /// </summary>
        public bool HasFilterQuestions { get; set; }
        public IEnumerable<CommonQuestion> FilterQuestions { get; set; }
        public IEnumerable<CommonWorkOrder> WorkOrders { get; set; }
        public IEnumerable<FilterQuestionResponse> FilterResponses { get; set; }
        public string LogoFile { get; set; }
        public int AmberFlagPromptID { get; set; }
        public string AmberFlagPrompt { get; set; }
    }
}