using System;
using System.Collections.Generic;
using System.Linq;

namespace HaloOnline.ViewData.Services
{
    using Common;
    using DataAccess;
    using Domain;
    using System.Data;
    using System.Data.SqlClient;
    using Rest.Messages;

    public class ViewDataServices : IViewDataServices
    {        
        private IEnumerable<ViewDataPrompt> GetPromptsByPromptID(int promptID)
        {
            var viewDataPrompts = new List<ViewDataPrompt>();
            SqlParameter[] parameters = { new SqlParameter("@PromptID", promptID) };
            var dtViewDataPrompts = HaloDatabase.GetDataTable("dbo.rptGetPrompt", parameters);
            if (dtViewDataPrompts.Rows.Count > 0)
            {
                viewDataPrompts = dtViewDataPrompts
                    .AsEnumerable()
                    .Select(s => new ViewDataPrompt
                    {
                        PromptID = s.IsNull("PromptID") ? 0 : s.Field<int>("PromptID"),
                        Prompt = s.IsNull("Prompt") ? string.Empty : s.Field<string>("Prompt"),
                        QuestionID = s.IsNull("QuestionID") ? 0 : s.Field<int>("QuestionID"),
                    })
                    .ToList();
            }
            return viewDataPrompts;
        }

        public ViewDataPrompt GetAmberPrompt(int surveyID)
        {
            var amberPrompt = new ViewDataPrompt();
            SqlParameter[] parameters = { new SqlParameter("@SurveyID", surveyID) };
            var dtAmberPrompt = HaloDatabase.GetDataTable("dbo.rptGetSurveyAmberPrompt", parameters);
            if (dtAmberPrompt.Rows.Count > 0)
            {
                var drAmberPrompt = dtAmberPrompt.Rows[0];
                amberPrompt.Prompt = !drAmberPrompt.IsNull("Prompt") ? drAmberPrompt.Field<string>("Prompt") : string.Empty;
                amberPrompt.PromptID = !drAmberPrompt.IsNull("PromptID") ? drAmberPrompt.Field<int>("PromptID") : 0;
                amberPrompt.QuestionID = !drAmberPrompt.IsNull("QuestionID") ? drAmberPrompt.Field<int>("QuestionID") : 0;
            }
            return amberPrompt;
        }

        public ViewDataMessage GetViewData(ViewDataRequest request)
        {
            var columns = new List<SurveyResponseColumn>();
            var data = new List<string[]>();
            var parameters = DataHelper.BuildParameters(request);
            var dtResponseData = HaloDatabase.GetDataTable("dbo.GetViewData", parameters.ToArray());
            var amberFlagPrompt = GetAmberPrompt(request.SurveyID);

            if (dtResponseData.Rows.Count > 0)
            {
                foreach (DataRow dRow in dtResponseData.Rows)
                {
                    var rowArray = new string[dtResponseData.Columns.Count];
                    int index = 0;
                    foreach (DataColumn dcol in dtResponseData.Columns)
                    {
                        rowArray[index] = dRow[dcol.ColumnName].ToString();
                        index++;
                    }
                    data.Add(rowArray);
                }

                foreach (DataColumn dcol in dtResponseData.Columns)
                {
                    var colName = (dcol.ColumnName.IndexOf("?") != -1) ? dcol.ColumnName.Substring(0, dcol.ColumnName.IndexOf("?")) : dcol.ColumnName;
                    colName = (colName.IndexOf(":") != -1) ? colName.Substring(0, colName.IndexOf(":")) : colName;
                    var strPrompt = string.Empty;
                    strPrompt = (dcol.ColumnName.IndexOf("?") != -1) ? dcol.ColumnName.Substring(dcol.ColumnName.IndexOf("?") + 1, dcol.ColumnName.Length - (dcol.ColumnName.IndexOf("?") + 1)).Trim() : dcol.ColumnName;
                    //TODO: REFACTOR UGLY CODE YUCK!
                    var promptID = 0;
                    if (!Int32.TryParse(strPrompt, out promptID))
                    {
                        strPrompt = (dcol.ColumnName.IndexOf(":") != -1) ? dcol.ColumnName.Substring(dcol.ColumnName.IndexOf(":") + 1, dcol.ColumnName.Length - (dcol.ColumnName.IndexOf(":") + 1)).Trim() : dcol.ColumnName;
                        if (!Int32.TryParse(strPrompt, out promptID))
                        {
                            promptID = 0;
                        }
                    }
                    var prompt = string.Empty;
                    if (promptID > 0)
                    {
                        var prompts = GetPromptsByPromptID(promptID);
                        if (prompts.Count() > 0)
                        {
                            prompt = prompts.Where(p => p.PromptID == promptID).FirstOrDefault().Prompt;
                            var promptArray = prompt.Split(new string[1] { "||" }, StringSplitOptions.None);
                            if (promptArray.Length == 2)
                            {
                                prompt = promptArray[1];
                            }
                        }
                    }
                    if ((colName.ToLower() == "amberflagimage") && !string.IsNullOrEmpty(amberFlagPrompt.Prompt))
                    {
                        var amberPromptArray = amberFlagPrompt.Prompt.Split(new string[1] { "||" }, StringSplitOptions.None);
                        prompt = amberPromptArray.Count() < 2 ? prompt : amberPromptArray[1];
                    }
                    columns.Add(new SurveyResponseColumn { Title = colName, ToolTip = string.IsNullOrEmpty(prompt) ? colName : prompt });
                }
            }
            return new ViewDataMessage { Columns = columns, Data = data };
        }
    }
}