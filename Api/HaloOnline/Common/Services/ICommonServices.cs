using HaloOnline.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnline.Common.Services
{
    interface ICommonServices
    {
        CommonSurvey GetSurveyStatistics(int surveyID, DateRange period);
        IEnumerable<FilterQuestionResponse> GetFilterQuestionResponses(int surveyID, DateRange period);
        IEnumerable<Respondent> GetSurveyRespondents(int surveyID, DateRange period);
    }
}
