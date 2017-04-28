using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnline.Reports.Services
{
    using Common;
    using Domain;
    using Rest.Messages;

    public interface IReportsServices
    {
        SurveyMessage GetSurveyByID(int surveyID);
        IEnumerable<SurveyMessage> GetSurveysByPasscode(string passcode);
        ReportsSurvey GetSurveyStatistics(int surveyID, DateRange period);
        ReportResponseMessage GetResponses(ReportRequest reportRequest);
        ConfidenceScoreMessage GetCustomerConfidenceScoreData(ReportRequest reportRequest);
        IEnumerable<ConfidenceTrendMessage> GetCustomerConfidenceTrendData(int SurveyId);
        CustomerExperienceMessage GetCustomerExperienceData(ReportRequest reportRequest);
        RecognitionMessage GetRecognitionData(ReportRequest reportRequest);
        FlagMessage GetFlagsData(ReportRequest reportRequest);

        IEnumerable<PeriodComment> GetPeriodCommentsData(ReportRequest reportRequest);
        IEnumerable<string> GetDirectCommentsData(int surveyID, DateRange period);
        
        int InsertDirectComment(DirectCommentRequest directCommentRequest);
    }

    
}
