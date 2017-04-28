using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace HaloOnline.Reports.Services
{
    using Common;
    using Common.Domain;
    using Common.Extensions;
    using DataAccess;
    using Domain;
    using Rest.Messages;

    public class ReportsServices : IReportsServices
    {

        #region Private Helpers

        private int GetQuestionByQuestionTypeID(int surveyID, int questionTypeID)
        {
            var questionID = 0;
            var questions = GetQuestionsByQuestionTypeID(surveyID, questionTypeID);
            if (questions.Count() > 0)
            {
                var question = questions.FirstOrDefault();
                if (question != null)
                {
                    questionID = question.QuestionID;
                }
            }
            return questionID;
        }

        private ConfidenceScore GetEmptyConfidenceScore()
        {
            return new ConfidenceScore
            {
                Period = "",
                Respondents = 0,
                VeryConfident = 0,
                FairlyConfident = 0,
                NotVeryConfident = 0,
                NotAtAllConfident = 0
            };
        }

        private IEnumerable<ReportPrompt> GetPromptsByQuestionTypeID(int surveyID, int questionTypeID)
        {
            var reportPrompts = new List<ReportPrompt>();
            SqlParameter[] parameters = {
                                            new SqlParameter("@SurveyID", surveyID),
                                            new SqlParameter("@QuestionTypeID", questionTypeID)
                                        };
            var dtReportPrompts = HaloDatabase.GetDataTable("dbo.GetPromptsByQuestionTypeID", parameters);
            if (dtReportPrompts.Rows.Count > 0)
            {
                reportPrompts = dtReportPrompts
                    .AsEnumerable()
                    .Select(s => new ReportPrompt
                    {
                        PromptID = s.IsNull("PromptID") ? 0 : s.Field<int>("PromptID"),
                        Prompt = s.IsNull("Prompt") ? string.Empty : s.Field<string>("Prompt"),
                        QuestionID = s.IsNull("QuestionID") ? 0 : s.Field<int>("QuestionID"),
                        QuestionType = s.IsNull("QuestionType") ? string.Empty : s.Field<string>("QuestionType"),
                        PositivePrompt = s.IsNull("PositivePrompt") ? string.Empty : s.Field<string>("PositivePrompt"),
                        NegativePrompt = s.IsNull("NegativePrompt") ? string.Empty : s.Field<string>("NegativePrompt"),
                    })
                    .ToList();
            }
            return reportPrompts;
        }

        #endregion

        public SurveyMessage GetSurveyByID(int surveyID)
        {
            var survey = new SurveyMessage();
            SqlParameter[] parameters = {
                                            new SqlParameter("@SurveyID", surveyID)
                                        };
            var dtSurvey = HaloDatabase.GetDataTable("dbo.GetSurveyDetails", parameters);
            if (dtSurvey.Rows.Count > 0)
            {
                survey = dtSurvey
                    .AsEnumerable()
                    .Select(s => new SurveyMessage
                    {
                        SurveyID = s.Field<int>("SurveyID"),
                        SurveyName = s.IsNull("Survey") ? string.Empty : s.Field<string>("Survey"),
                        Description = string.Format("{0} (Started: {1})", s.Field<string>("Survey"), s.IsNull("StartDate") ? string.Empty : s.Field<DateTime>("StartDate").ToShortDateString()),
                        LogoFile = s.IsNull("LogoFile") ? string.Empty : s.Field<string>("LogoFile"),
                        StartDate = s.IsNull("StartDate") ? string.Empty : s.Field<DateTime>("StartDate").ToShortDateString()
                    })
                    .FirstOrDefault();
            }
            return survey;

        }
        public IEnumerable<QuestionMessage> GetQuestionsByQuestionTypeID(int surveyID, int questionTypeID)
        {
            var reportQuestions = new List<QuestionMessage>();
            SqlParameter[] parameters = {
                                            new SqlParameter("@SurveyID", surveyID),
                                            new SqlParameter("@QuestionTypeID", questionTypeID)
                                        };
            var dtReportQuestions = HaloDatabase.GetDataTable("dbo.GetQuestionByQuestionTypeID", parameters);
            if (dtReportQuestions.Rows.Count > 0)
            {
                reportQuestions = dtReportQuestions
                    .AsEnumerable()
                    .Select(s => new QuestionMessage
                    {
                        QuestionID = s.IsNull("QuestionID") ? 0 : s.Field<int>("QuestionID"),
                        QuestionTypeID = s.IsNull("QuestionTypeID") ? 0 : s.Field<int>("QuestionTypeID"),
                        Question = s.IsNull("Question") ? string.Empty : s.Field<string>("Question"),
                        Summary = s.IsNull("Summary") ? string.Empty : s.Field<string>("Summary"),
                        PromptID = s.IsNull("PromptID") ? 0 : s.Field<int>("PromptID"),
                        OptionSetID = s.IsNull("OptionSetID") ? 0 : s.Field<int>("OptionSetID")
                    })
                    .ToList();
            }
            return reportQuestions;
        }

        public IEnumerable<SurveyMessage> GetSurveysByPasscode(string passcode)
        {
            IEnumerable<SurveyMessage> surveyMessages = new List<SurveyMessage>();
            SqlParameter[] parameters = {
                                            new SqlParameter("@Passcode", passcode)
                                        };
            var dtSurvey = HaloDatabase.GetDataTable("dbo.rptGetPasscodeSurveys", parameters);
            if (dtSurvey.Rows.Count > 0)
            {
                surveyMessages = dtSurvey
                    .AsEnumerable()
                    .Select(s => new SurveyMessage
                    {
                        SurveyID = s.Field<int>("SurveyID"),
                        SurveyName = s.Field<string>("Survey"),
                        Description = s.IsNull("StartDate") ? s.Field<string>("Survey") : string.Format("{0} (start: {1})",
                                                    s.Field<string>("Survey"),
                                                    s.Field<DateTime>("StartDate").ToShortDateString()),
                        LogoFile = s.Field<string>("LogoFile"),
                        StartDate = s.IsNull("StartDate") ? string.Empty : s.Field<DateTime>("StartDate").ToShortDateString()
                    })
                    .ToList();
            }

            return surveyMessages;
        }

        public ReportResponseMessage GetResponses(ReportRequest reportRequest)
        {
            var responseMessage = new ReportResponseMessage();

            //TODO: Convert string date to DateTime properly
            DateRange period = new DateRange { From = DateTime.Parse(reportRequest.StartDate), To = DateTime.Parse(reportRequest.EndDate) };
            var parameters = DataHelper.BuildParameters(reportRequest);
            var dtResponses = HaloDatabase.GetDataTable("dbo.rptGetSurveyRespondents", parameters.ToArray());

            if (dtResponses.Rows.Count > 0)
            {
                var periodResponses = dtResponses.AsEnumerable().Where(r => ((r.Field<DateTime>("CompletionDate") >= period.From) &&
                                                                               (r.Field<DateTime>("CompletionDate") <= period.To)));
                var respondents = (periodResponses != null) ? periodResponses.Count() : 0;
                ReportResponse current = new ReportResponse
                {
                    Period = string.Format("{0} - {1}", period.From.ToShortDateString(), period.To.ToShortDateString()),
                    Respondents = respondents
                };

                var previousPeriod = DataHelper.GetPreviousPeriod(period);
                var previousParameters = new List<SqlParameter>();
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.ParameterName == "@StartDate")
                    {
                        previousParameters.Add(new SqlParameter("@StartDate", previousPeriod.From));
                    }
                    else if (parameter.ParameterName == "@EndDate")
                    {
                        previousParameters.Add(new SqlParameter("@EndDate", previousPeriod.To));
                    }
                    else
                    {
                        previousParameters.Add(new SqlParameter(parameter.ParameterName, parameter.Value));
                    }
                }
                var dtPreviousResponses = HaloDatabase.GetDataTable("dbo.rptGetSurveyRespondents", previousParameters.ToArray());
                var previousResponses = dtPreviousResponses.AsEnumerable().Where(r => ((r.Field<DateTime>("CompletionDate") >= previousPeriod.From) &&
                                                                                (r.Field<DateTime>("CompletionDate") <= previousPeriod.To)));
                var previousRespondents = (previousResponses != null) ? previousResponses.Count() : 0;
                ReportResponse previous = new ReportResponse
                {
                    Period = string.Format("{0} - {1}", previousPeriod.From.ToShortDateString(), previousPeriod.To.ToShortDateString()),
                    Respondents = previousRespondents
                };
                responseMessage.Current = current;
                responseMessage.Previous = previous;
            }
            else
            {
                responseMessage.Current = new ReportResponse
                {
                    Period = string.Format("{0} - {1}", period.From.ToShortDateString(), period.To.ToShortDateString()),
                    Respondents = 0
                };
                responseMessage.Previous = new ReportResponse
                {
                    Period = string.Format("{0} - {1}", period.From.ToShortDateString(), period.To.ToShortDateString()),
                    Respondents = 0
                };
            }

            return responseMessage;
        }

        public ConfidenceScoreMessage GetCustomerConfidenceScoreData(ReportRequest reportRequest)
        {
            var ConfidenceScoreMessage = new ConfidenceScoreMessage();
            var parameters = DataHelper.BuildParameters(reportRequest);
            var confidenceQuestionID = GetQuestionByQuestionTypeID(reportRequest.SurveyID, 1);
            parameters.Add(new SqlParameter("@QuestionID", confidenceQuestionID));

            var dt = HaloDatabase.GetDataTable("dbo.rptGetCustomerConfidenceScoreData", parameters.ToArray());
            if (dt.Rows.Count > 0)
            {
                var dtConfidenceScore = dt.AsEnumerable();
                ConfidenceScoreMessage.Current = new ConfidenceScore
                {
                    Period = string.Format("{0} - {1}", dtConfidenceScore.Select(x => x.Field<DateTime>("StartDate")).FirstOrDefault(), dtConfidenceScore.Select(x => x.Field<DateTime>("EndDate")).FirstOrDefault()),
                    Respondents = dtConfidenceScore.Sum(n => n.Field<int>("NumberOfRespondents")),
                    VeryConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Very confident").Select(r => (double)r["Percentage"]).FirstOrDefault(),
                    FairlyConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Fairly confident").Select(r => (double)r["Percentage"]).FirstOrDefault(),
                    NotVeryConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Not very confident").Select(r => (double)r["Percentage"]).FirstOrDefault(),
                    NotAtAllConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Not at all confident").Select(r => (double)r["Percentage"]).FirstOrDefault()
                };
                ConfidenceScoreMessage.AllTime = new ConfidenceScore
                {
                    Period = string.Format("{0} - {1}", dtConfidenceScore.Select(x => x.Field<DateTime>("AllTimeStartDate")).FirstOrDefault(), dtConfidenceScore.Select(x => x.Field<DateTime>("AllTimeEndDate")).FirstOrDefault()),
                    Respondents = dtConfidenceScore.Sum(n => n.Field<int>("NumberOfRespondents")),
                    VeryConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Very confident").Select(r => (double)r["AllTimePercentage"]).FirstOrDefault(),
                    FairlyConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Fairly confident").Select(r => (double)r["AllTimePercentage"]).FirstOrDefault(),
                    NotVeryConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Not very confident").Select(r => (double)r["AllTimePercentage"]).FirstOrDefault(),
                    NotAtAllConfident = (decimal)dtConfidenceScore.Where(x => (string)x["option"] == "Not at all confident").Select(r => (double)r["AllTimePercentage"]).FirstOrDefault()
                };
            }
            else
            {
                ConfidenceScoreMessage.Current = GetEmptyConfidenceScore();
                ConfidenceScoreMessage.AllTime = GetEmptyConfidenceScore();
            }
            ConfidenceScoreMessage.Previous = GetEmptyConfidenceScore();

            return ConfidenceScoreMessage;
        }
        
        public IEnumerable<ConfidenceTrendMessage> GetCustomerConfidenceTrendData(int surveyId)
        {
            var confidenceTrendData = new List<ConfidenceTrendMessage>();

            var parameters = new List<SqlParameter>();
            var yesterday = DateTime.Today.AddDays(-1);
            DateRange period = new DateRange { From = yesterday.AddMonths(-6), To = yesterday };

            parameters = parameters.Update(new SqlParameter("@SurveyId", surveyId));
            parameters = parameters.Update(new SqlParameter("@StartDate", period.From));
            parameters = parameters.Update(new SqlParameter("@EndDate", period.To));

            var dtQuestionResponseTrend = HaloDatabase.GetDataTable("dbo.rptGetConfidenceTrendData", parameters.ToArray());
            if (dtQuestionResponseTrend.Rows.Count > 0)
            {
                var dtSorted = dtQuestionResponseTrend.AsEnumerable().OrderByDescending(r => r.Field<string>("PeriodOfResponse")).ToList();
                foreach (DataRow row in dtSorted)
                {
                    var confidenceTrendDataEntry = new ConfidenceTrendMessage
                    {
                        Period = Convert.ToString(row["PeriodOfResponse"])
                    };
                    if (!row.IsNull("Percentage"))
                    {
                        confidenceTrendDataEntry.Current = Math.Round(Convert.ToDecimal(row["Percentage"]), 2);
                    }
                    confidenceTrendData.Add(confidenceTrendDataEntry);
                }
                confidenceTrendData = confidenceTrendData.OrderBy(e => e.Period).ToList();
            }
            else
            {
                var confidenceTrendDataEntry = new ConfidenceTrendMessage
                {
                    Period = string.Format("{0} - {1}", period.From.ToShortDateString(), period.To.ToShortDateString()),
                    Current = 0,
                    Previous = 0
                };
                confidenceTrendData.Add(confidenceTrendDataEntry);
            }

            return confidenceTrendData;
        }

        public CustomerExperienceMessage GetCustomerExperienceData(ReportRequest reportRequest)
        {
            var confidenceExperienceData = new CustomerExperienceMessage();
            confidenceExperienceData.ChartData = new List<CustomerExperienceChartDataEntry>();
            DateRange period = new DateRange { From = DateTime.Parse(reportRequest.StartDate), To = DateTime.Parse(reportRequest.EndDate) };
            period.To = new DateTime(period.To.Year, period.To.Month, period.To.Day, 23, 59, 59);
            var previousPeriod = DataHelper.GetPreviousPeriod(period);
            confidenceExperienceData.Periods = new Periods
            {
                Current = $"{period.From.ToShortDateString()} - {period.To.ToShortDateString()}",
                Previous = $"{previousPeriod.From.ToShortDateString()} - {previousPeriod.To.ToShortDateString()}"
            };
            // Get confidence QuestionIDs
            var confidencePrompts = GetPromptsByQuestionTypeID(reportRequest.SurveyID, 8); // 8 is slider questiontypeid
                

            foreach (var prompt in confidencePrompts)
            {
                var chartDataEntry = new CustomerExperienceChartDataEntry();
                dynamic dynamicChartDataEntry = new ExpandoObject();

                var parameters = DataHelper.BuildParameters(reportRequest);
                parameters = parameters.Update(new SqlParameter("@PromptID", prompt.PromptID));

                var ds = HaloDatabase.GetDataSet("dbo.rptGetCustomerExperienceData", parameters.ToArray());
                if (ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        chartDataEntry.Percentages = dt.AsEnumerable().Select(r => new CustomerExperiencePercentage
                        {
                            Score = r.IsNull("Option") ? 0 : Convert.ToInt32(r.Field<object>("Option")),
                            Current = r.IsNull("Percentage") ? 0 : Math.Round((decimal)r.Field<double>("Percentage")),
                            Previous = r.IsNull("PreviousPercentage") ? 0 : Math.Round((decimal)r.Field<double>("PreviousPercentage")),
                        });
                        chartDataEntry.Percentages = chartDataEntry.Percentages.OrderBy(p => p.Score);

                        chartDataEntry.Averages = new CustomerExperienceAverage
                        {
                            Current = Math.Round(dt.AsEnumerable().Where(r => !r.IsNull("Mean")).Select(i => (decimal)i.Field<double>("Mean")).FirstOrDefault()),
                            Previous = Math.Round(dt.AsEnumerable().Where(r => !r.IsNull("PreviousMean")).Select(i => (decimal)i.Field<double>("PreviousMean")).FirstOrDefault()),
                        };

                        // Dynamic object
                        var dynamicPercentages = new List<dynamic>();
                        // Get max and min percentages to determine where to position the mean indicators
                        var maxCurrent = dt.AsEnumerable().Where(r => !r.IsNull("Percentage")).Any()
                                            ? dt.AsEnumerable().Where(r => !r.IsNull("Percentage")).Max(r => r.Field<double>("Percentage"))
                                            : 0;
                        var maxPrevious = dt.AsEnumerable().Where(r => !r.IsNull("PreviousPercentage")).Any()
                                            ? dt.AsEnumerable().Where(r => !r.IsNull("PreviousPercentage")).Max(r => r.Field<double>("PreviousPercentage"))
                                            : 0;
                        var maxOverall = maxCurrent > maxPrevious ? maxCurrent : maxPrevious;
                        foreach (var percentage in chartDataEntry.Percentages)
                        {
                            dynamic dynamicPercentageEntry = new ExpandoObject();
                            dynamicPercentageEntry.Score = percentage.Score;
                            dynamicPercentageEntry.Current = percentage.Current;
                            dynamicPercentageEntry.Previous = percentage.Previous;
                            if (chartDataEntry.Averages.Current == percentage.Score)
                            {
                                dynamicPercentageEntry.CurrentMeanValue = (maxOverall + 4) > 88 ? 88 : (maxOverall + 4);
                                dynamicPercentageEntry.CurrentMean = percentage.Score;
                            }
                            if (chartDataEntry.Averages.Previous == percentage.Score)
                            {
                                dynamicPercentageEntry.PreviousMeanValue = (maxOverall + 10) > 98 ? 98 : (maxOverall + 10);
                                dynamicPercentageEntry.PreviousMean = percentage.Score;
                            }
                            dynamicPercentages.Add(dynamicPercentageEntry);
                        }
                        chartDataEntry.DynamicPercentages = dynamicPercentages;

                        var separator = new string[] { "||" };
                        var prompts = dt.Rows[0].IsNull("Prompt") ?
                            new string[] { string.Empty, string.Empty } :
                            dt.Rows[0].Field<string>("Prompt").Split(separator, StringSplitOptions.None);
                        chartDataEntry.SliderPrompts = new SliderPrompts { Min = prompts[0], Max = prompts[1] };

                        confidenceExperienceData.ChartData.Add(chartDataEntry);
                    }
                }

                if (ds.Tables.Count > 1)
                {
                    var dtComments = ds.Tables[1];
                    if (dtComments.AsEnumerable().Any())
                    {
                        chartDataEntry.Comments = dtComments.AsEnumerable().Select(r => new CustomerExperienceComment
                        {
                            Score = r.Field<int>("Score"),
                            Comment = r.Field<string>("Comment"),
                            RedFlag = (r.Field<bool>("RedFlag")) ? "<img src='../images/flag_red.png' alt='red flag'>" : " ",
                            AmberFlag = (r.Field<bool>("AmberFlag")) ? "<img src='../images/flag_yellow.png' alt='amber flag'>" : " ",
                        });
                    }
                }
            }
            return confidenceExperienceData;
        }

        public RecognitionMessage GetRecognitionData(ReportRequest reportRequest)
        {
            var recognitions = new List<Recognition>();
            var parameters = DataHelper.BuildParameters(reportRequest);
            int recognitionQuestionID = GetQuestionByQuestionTypeID(reportRequest.SurveyID, 10);
            parameters = parameters.Update(new SqlParameter("@QuestionID", recognitionQuestionID));

            var dsRecognitionData = HaloDatabase.GetDataTable("dbo.rptGetRecognitionData", parameters.ToArray());
            if (dsRecognitionData.Rows.Count > 0)
            {
                // get promptId for comment and name
                var namePromptID = dsRecognitionData.AsEnumerable()
                    .Where(r => r.IsNull("Prompt") || string.IsNullOrEmpty(r.Field<string>("Prompt")))
                    .Select(s => (int)s.Field<int>("PromptID"))
                    .FirstOrDefault();
                var commentPromptID = dsRecognitionData.AsEnumerable()
                    .Where(r => !r.IsNull("Prompt") && !string.IsNullOrEmpty(r.Field<string>("Prompt")))
                    .Select(s => (int)s.Field<int>("PromptID"))
                    .FirstOrDefault();
                // Get names and respondentID
                var peopleList = dsRecognitionData.AsEnumerable()
                    .Where(r => (r.Field<int>("PromptID") == namePromptID))
                    .Select(s => new { RespondentID = s.Field<int>("RespondentID"), Name = s.Field<string>("TextResponse") })
                    .Distinct()
                    .OrderBy(r => r.Name).ThenBy(r => r.RespondentID)
                    .ToList();
                var commentList = dsRecognitionData.AsEnumerable()
                    .Where(r => (r.Field<int>("PromptID") == commentPromptID))
                    .Select(s => new { RespondentID = s.Field<int>("RespondentID"), Comment = s.Field<string>("TextResponse") })
                    .Distinct()
                    .ToList();

                var duplicates = 0;
                foreach (var person in peopleList)
                {
                    if (recognitions.Where(r => r.Name.ToLower().Trim() == person.Name.ToLower().Trim()).Count() == 0)
                    {
                        var recog = new Recognition()
                        {
                            Name = person.Name,
                            Comments = commentList.Where(c => c.RespondentID == person.RespondentID).Select(c => c.Comment).ToList(),
                            Count = peopleList.Where(p => p.Name.ToLower().Trim() == person.Name.ToLower().Trim()).Count()
                        };
                        recognitions.Add(recog);
                    }
                    else
                    {
                        recognitions.Where(w => w.Name.ToLower().Trim() == person.Name.ToLower().Trim())
                            .ToList()
                            .ForEach(s => s.Comments.Add(commentList.Where(c => c.RespondentID == person.RespondentID).Select(c => c.Comment).FirstOrDefault()));
                        duplicates++;
                    }
                }
            }
            return new RecognitionMessage { Recognitions = recognitions };
        }

        public FlagMessage GetFlagsData(ReportRequest reportRequest)
        {
            var flagData = new FlagMessage();
            var parameters = DataHelper.BuildParameters(reportRequest);

            var dtFlagData = HaloDatabase.GetDataTable("dbo.rptGetFlagData", parameters.ToArray());
            if (dtFlagData.Rows.Count > 0)
            {
                var current = new FlagDataEntry();
                current = dtFlagData.AsEnumerable()
                    .Where(r => r.Field<string>("Period").ToLower() == "current")
                    .Select(e => new FlagDataEntry
                    {
                        Period = string.Format("{0} - {1}", e.Field<DateTime>("StartDate").ToShortDateString(), e.Field<DateTime>("EndDate").ToShortDateString()),
                        AmberFlags = e.IsNull("AmberFlags") ? 0 : e.Field<int>("AmberFlags"),
                        RedFlags = e.IsNull("RedFlags") ? 0 : e.Field<int>("RedFlags")

                    })
                    .FirstOrDefault();
                var previous = new FlagDataEntry();
                previous = dtFlagData.AsEnumerable()
                    .Where(r => r.Field<string>("Period").ToLower() == "previous")
                    .Select(e => new FlagDataEntry
                    {
                        Period = string.Format("{0} - {1}", e.Field<DateTime>("StartDate").ToShortDateString(), e.Field<DateTime>("EndDate").ToShortDateString()),
                        AmberFlags = e.IsNull("AmberFlags") ? 0 : e.Field<int>("AmberFlags"),
                        RedFlags = e.IsNull("RedFlags") ? 0 : e.Field<int>("RedFlags")

                    })
                    .FirstOrDefault();
                var allTime = new FlagDataEntry();
                allTime = dtFlagData.AsEnumerable()
                    .Where(r => r.Field<string>("Period").ToLower() == "alltime")
                    .Select(e => new FlagDataEntry
                    {
                        Period = string.Format("{0} - {1}", e.Field<DateTime>("StartDate").ToShortDateString(), e.Field<DateTime>("EndDate").ToShortDateString()),
                        AmberFlags = e.IsNull("AmberFlags") ? 0 : e.Field<int>("AmberFlags"),
                        RedFlags = e.IsNull("RedFlags") ? 0 : e.Field<int>("RedFlags")

                    })
                    .FirstOrDefault();
                flagData.Current = current;
                flagData.Previous = previous;
            }
            return flagData;
        }

        public IEnumerable<PeriodComment> GetPeriodCommentsData(ReportRequest reportRequest)
        {
            var commentsTable = new List<PeriodComment>();
            var parameters = DataHelper.BuildParameters(reportRequest);

            var dsCommentsData = HaloDatabase.GetDataTable("dbo.rptGetCommentsTableData", parameters.ToArray());
            if (dsCommentsData.Rows.Count > 0)
            {
                // get comments
                commentsTable = dsCommentsData
                                    .AsEnumerable()
                                    .Select(s => new PeriodComment
                                    {
                                        RespondentID = s.IsNull("RespondentID") ? 0 : s.Field<int>("RespondentID"),
                                        Comment = s.IsNull("Comment") ? string.Empty : s.Field<string>("Comment"),
                                        AmberFlag = s.IsNull("Comment") ? false : s.Field<bool>("AmberFlag"),
                                        RedFlag = s.IsNull("RedFlag") ? false : s.Field<bool>("RedFlag")
                                    })
                                    .ToList();
            }
            return commentsTable;
        }
        
        public IEnumerable<string> GetDirectCommentsData(int surveyID, DateRange period)
        {
            var comments = new List<string>();
            var workOrders = new List<CommonWorkOrder>();
            SqlParameter[] parameters = {
                                        new SqlParameter("@SurveyID", surveyID),
                                            new SqlParameter("@StartDate", period.From),
                                            new SqlParameter("@EndDate", period.To)
                                    };

            var commentsData = HaloDatabase.GetDataTable("dbo.rptGetDirectCommentsData", parameters.ToArray());
            if (commentsData.Rows.Count > 0)
            {
                comments = commentsData.AsEnumerable()
                    .Select(s => s.Field<string>("TextResponse"))
                    .Distinct()
                    .ToList();
            }
            return comments;
        }

        [Obsolete]
        public IEnumerable<string> GetDirectCommentsData(ReportRequest reportRequest)
        {
            var comments = new List<string>();
            var parameters = DataHelper.BuildParameters(reportRequest);

            var commentsData = HaloDatabase.GetDataTable("dbo.rptGetDirectCommentsData", parameters.ToArray());
            if (commentsData.Rows.Count > 0)
            {
                comments = commentsData.AsEnumerable()
                    .Select(s => s.Field<string>("TextResponse"))
                    .Distinct()
                    .ToList();
            }
            return comments;
        }

        public int InsertDirectComment(DirectCommentRequest directCommentRequest)
        {
            var parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@SurveyId", directCommentRequest.SurveyId));
            parameterList.Add(new SqlParameter("@InputSource", 3));
            parameterList.Add(new SqlParameter("@CompletionDate", DateTime.Now));
            parameterList.Add(new SqlParameter("@TextResponse", directCommentRequest.Comment));
            var parameters = parameterList.ToArray();
            var respondentId = HaloDatabase.ExecuteNonQuery("dbo.InsertDirectComment", parameters);

            return respondentId;

        }

        public ReportsSurvey GetSurveyStatistics(int surveyID, DateRange period)
        {
            var survey = new ReportsSurvey();
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
                    .Select(s => new ReportsSurvey
                    {
                        SurveyID = s.Field<int>("SurveyID"),
                        SurveyName = s.IsNull("Survey") ? string.Empty : s.Field<string>("Survey"),
                        Description = string.Format("{0} (Started: {1})", s.Field<string>("Survey"), s.IsNull("StartDate") ? string.Empty : s.Field<DateTime>("StartDate").ToShortDateString()),
                        LogoFile = s.IsNull("LogoFile") ? string.Empty : s.Field<string>("LogoFile"),
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
                            .Select(x => new ReportsQuestion { PromptID = x.PromptID1, Question = x.Prompt1 })
                            .FirstOrDefault();
                        if (questionOne != null)
                        {
                            questionOne.Options = survey.FilterResponses
                                .Select(r => new { Option = r.Option1, Value = r.ValueResponse1 })
                                .ToList()
                                .GroupBy(g => new { Option = g.Option, Value = g.Value })
                                .Select(x => new ReportsOption { Option = x.Key.Option, Value = x.Key.Value, Count = x.Count() })
                                .ToList();

                        }

                        var questionTwo = survey.FilterResponses
                            .Where(x => !string.IsNullOrEmpty(x.Prompt2))
                            .Select(x => new ReportsQuestion { PromptID = x.PromptID2, Question = x.Prompt2 })
                            .FirstOrDefault();
                        if (questionTwo != null)
                        {
                            questionTwo.Options = survey.FilterResponses
                                .Select(r => new { Option = r.Option2, Value = r.ValueResponse2 })
                                .ToList()
                                .GroupBy(g => new { Option = g.Option, Value = g.Value })
                                .Select(x => new ReportsOption { Option = x.Key.Option, Value = x.Key.Value, Count = x.Count() })
                                .ToList();

                        }
                        survey.FilterQuestions = new List<ReportsQuestion> { questionOne, questionTwo };
                    }
                }
            }
            return survey;
        }

        public IEnumerable<ReportsFilterQuestionResponse> GetFilterQuestionResponses(int surveyID, DateRange period)
        {
            var options = new List<ReportsFilterQuestionResponse>();
            SqlParameter[] parameters = {
                                            new SqlParameter("@SurveyID", surveyID),
                                             new SqlParameter("@StartDate", period.From),
                                             new SqlParameter("@EndDate", period.To)
                                        };
            var dtOptions = HaloDatabase.GetDataTable("dbo.rptGetFilterQuestionResponses", parameters);
            if (dtOptions.Rows.Count > 0)
            {
                options = dtOptions.AsEnumerable()
                            .Select(r => new ReportsFilterQuestionResponse
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

        public IEnumerable<ReportsRespondent> GetSurveyRespondents(int surveyID, DateRange period)
        {
            var respondents = new List<ReportsRespondent>();
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
                    .Select(s => new ReportsRespondent
                    {
                        SurveyID = surveyID,
                        CompletionDate = s.IsNull("CompletionDate") ? DateTime.MinValue : s.Field<DateTime>("CompletionDate"),
                        ResponseDate = s.IsNull("CompletionDate") ? string.Empty : s.Field<DateTime>("CompletionDate").ToShortDateString(),
                        RespondentID = s.IsNull("RespondentID") ? 0 : s.Field<int>("RespondentID"),
                        WorkOrder = s.IsNull("WorkOrderID") ? new ReportsWorkOrder() :
                                            new ReportsWorkOrder
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
