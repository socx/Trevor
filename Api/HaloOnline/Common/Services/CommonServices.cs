using HaloOnline.Common.Domain;
using HaloOnline.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Services
{
    public class CommonServices : ICommonServices
    {
        public CommonSurvey GetSurveyStatistics(int surveyID, DateRange period)
        {
            var survey = new CommonSurvey();
            var workOrders = new List<CommonWorkOrder>();
            SqlParameter[] parameters = {
                                        new SqlParameter("@SurveyID", surveyID),
                                            new SqlParameter("@StartDate", period.From),
                                            new SqlParameter("@EndDate", period.To)
                                    };
            var dtSurvey = HaloDatabase.GetDataTable("dbo.rptGetSurveyStatistics", parameters);
            if (dtSurvey.Rows.Count > 0)
            {
                survey = dtSurvey
                    .AsEnumerable()
                    .Select(s => new CommonSurvey
                    {
                        SurveyID = s.Field<int>("SurveyID"),
                        SurveyName = s.Field<string>("Survey"),
                        Description = string.Format("{0} (Started: {1})", s.Field<string>("Survey"), s.Field<DateTime>("StartDate").ToShortDateString()),
                        LogoFile = s.Field<string>("LogoFile"),
                        StartDate = s.IsNull("StartDate") ? string.Empty : s.Field<DateTime>("StartDate").ToShortDateString(),
                        EndDate = s.IsNull("EndDate") ? string.Empty : s.Field<DateTime>("EndDate").ToShortDateString(),
                        TotalNumberOfRespondents = s.IsNull("TotalNumberOfRespondents") ? 0 : s.Field<int>("TotalNumberOfRespondents"),
                        WorkOrderRespondents = s.IsNull("WorkOrderRespondents") ? 0 : s.Field<int>("WorkOrderRespondents"),
                        HasWorkOrders = s.IsNull("HasWorkOrders") ? false : s.Field<bool>("HasWorkOrders"),
                        HasFilterQuestions = s.IsNull("HasFilterQuestions") ? false : s.Field<bool>("HasFilterQuestions"),
                        AmberFlagPromptID = s.IsNull("AmberFlagPromptID") ? 0 : s.Field<int>("AmberFlagPromptID"),
                        AmberFlagPrompt = s.IsNull("AmberFlagPrompt") ? string.Empty : s.Field<string>("AmberFlagPrompt")
                    })
                    .FirstOrDefault();
                if (survey.HasWorkOrders)
                {
                    var respondents = GetSurveyRespondents(surveyID, period);
                    survey.WorkOrders = respondents.ToList()
                                                .Where(e => e.WorkOrder.WorkOrderID > 0)
                                                .Select(r => r.WorkOrder)
                                                .ToList();
                }
                else if (survey.HasFilterQuestions)
                {
                    survey.FilterResponses = GetFilterQuestionResponses(surveyID, period);

                    // Get Filter Questions
                    if (survey.FilterResponses != null && survey.FilterResponses.Any())
                    {
                        var questionOne = survey.FilterResponses
                            .Where(x => !string.IsNullOrEmpty(x.Prompt1))
                            .Select(x => new CommonQuestion { PromptID = x.PromptID1, Question = x.Prompt1 })
                            .FirstOrDefault();
                        if (questionOne != null)
                        {
                            questionOne.Options = survey.FilterResponses
                                .Select(r => new { Option = r.Option1, Value = r.ValueResponse1 })
                                .ToList()
                                .GroupBy(g => new { Option = g.Option, Value = g.Value })
                                .Select(x => new CommonOption { Option = x.Key.Option, Value = x.Key.Value, Count = x.Count() })
                                .ToList();

                        }

                        var questionTwo = survey.FilterResponses
                            .Where(x => !string.IsNullOrEmpty(x.Prompt2))
                            .Select(x => new CommonQuestion { PromptID = x.PromptID2, Question = x.Prompt2 })
                            .FirstOrDefault();
                        if (questionTwo != null)
                        {
                            questionTwo.Options = survey.FilterResponses
                                .Select(r => new { Option = r.Option2, Value = r.ValueResponse2 })
                                .ToList()
                                .GroupBy(g => new { Option = g.Option, Value = g.Value })
                                .Select(x => new CommonOption { Option = x.Key.Option, Value = x.Key.Value, Count = x.Count() })
                                .ToList();

                        }
                        survey.FilterQuestions = new List<CommonQuestion> { questionOne, questionTwo };
                    }
                }
            }
            return survey;
        }

        public IEnumerable<FilterQuestionResponse> GetFilterQuestionResponses(int surveyID, DateRange period)
        {
            var options = new List<FilterQuestionResponse>();
            SqlParameter[] parameters = {
                                            new SqlParameter("@SurveyID", surveyID),
                                             new SqlParameter("@StartDate", period.From),
                                             new SqlParameter("@EndDate", period.To)
                                        };
            var dtOptions = HaloDatabase.GetDataTable("dbo.rptGetFilterQuestionResponses", parameters);
            if (dtOptions.Rows.Count > 0)
            {
                options = dtOptions.AsEnumerable()
                            .Select(r => new FilterQuestionResponse
                            {
                                RespondentID = r.Field<int>("RespondentID"),
                                CompletionDate = !r.IsNull("CompletionDate") ? r.Field<DateTime>("CompletionDate") : DateTime.MinValue,
                                ResponseID1 = !r.IsNull("ResponseID1") ? r.Field<int>("ResponseID1") : 0,
                                ValueResponse1 = !r.IsNull("ValueResponse1") ? r.Field<int>("ValueResponse1") : 0,
                                PromptID1 = !r.IsNull("PromptID1") ? r.Field<int>("PromptID1") : 0,
                                Prompt1 = !r.IsNull("Prompt1") ? r.Field<string>("Prompt1") : string.Empty,
                                Option1 = !r.IsNull("Option1") ? r.Field<string>("Option1") : string.Empty,
                                ResponseID2 = !r.IsNull("ResponseID2") ? r.Field<int>("ResponseID2") : 0,
                                ValueResponse2 = !r.IsNull("ValueResponse2") ? r.Field<int>("ValueResponse2") : 0,
                                PromptID2 = !r.IsNull("PromptID2") ? r.Field<int>("PromptID2") : 0,
                                Prompt2 = !r.IsNull("Prompt2") ? r.Field<string>("Prompt2") : string.Empty,
                                Option2 = !r.IsNull("Option2") ? r.Field<string>("Option2") : string.Empty,
                            })
                            .ToList();
            }

            return options;
        }

        public IEnumerable<Respondent> GetSurveyRespondents(int surveyID, DateRange period)
        {
            var respondents = new List<Respondent>();
            SqlParameter[] parameters = {
                                            new SqlParameter("@SurveyID", surveyID),
                                             new SqlParameter("@StartDate", period.From),
                                             new SqlParameter("@EndDate", period.To)
                                        };
            var dtRespondent = HaloDatabase.GetDataTable("dbo.rptGetSurveyRespondents", parameters);
            if (dtRespondent.Rows.Count > 0)
            {
                var respondentList = dtRespondent
                    .AsEnumerable()
                    .Select(s => new Respondent
                    {
                        SurveyID = surveyID,
                        CompletionDate = s.IsNull("CompletionDate") ? DateTime.MinValue : s.Field<DateTime>("CompletionDate"),
                        ResponseDate = s.IsNull("CompletionDate") ? string.Empty : s.Field<DateTime>("CompletionDate").ToShortDateString(),
                        RespondentID = s.IsNull("RespondentID") ? 0 : s.Field<int>("RespondentID"),
                        WorkOrder = s.IsNull("WorkOrderID") ? new CommonWorkOrder() :
                                            new CommonWorkOrder
                                            {
                                                WorkOrderID = s.IsNull("WorkOrderID") ? 0 : s.Field<int>("WorkOrderID"),
                                                WorkOrderText = s.IsNull("WorkOrder") ? string.Empty : s.Field<string>("WorkOrder"),
                                                LocationLevel1 = s.IsNull("LocationLevel1") ? string.Empty : s.Field<string>("LocationLevel1"),
                                                LocationLevel2 = s.IsNull("LocationLevel2") ? string.Empty : s.Field<string>("LocationLevel2"),
                                                LocationLevel3 = s.IsNull("LocationLevel3") ? string.Empty : s.Field<string>("LocationLevel3"),
                                                LocationLevel4 = s.IsNull("LocationLevel4") ? string.Empty : s.Field<string>("LocationLevel4"),
                                                SLA = s.IsNull("SLA") ? string.Empty : s.Field<string>("SLA"),
                                                SLABreach = s.IsNull("SLABreach") ? string.Empty : s.Field<string>("SLABreach"),
                                                SLAContract = s.IsNull("SLAContract") ? string.Empty : s.Field<string>("SLAContract"),
                                                SLAService = s.IsNull("SLAService") ? string.Empty : s.Field<string>("SLAService"),
                                                SLAServiceType = s.IsNull("SLAServiceType") ? string.Empty : s.Field<string>("SLAServiceType"),
                                                Failure = s.IsNull("Failure") ? string.Empty : s.Field<string>("Failure"),
                                                Remedy = s.IsNull("Remedy") ? string.Empty : s.Field<string>("Remedy"),
                                                Problem = s.IsNull("Problem") ? string.Empty : s.Field<string>("Problem"),
                                                Cause = s.IsNull("Cause") ? string.Empty : s.Field<string>("Cause"),
                                            },
                        Extra = s.IsNull("Extra") ? string.Empty : s.Field<string>("Extra"),
                    })
                    .ToList();
                respondents.AddRange(respondentList);
            }
            return respondents;
        }
    }
}