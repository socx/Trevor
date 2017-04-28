using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HaloOnline.Reports.Store
{
    using Common;
    using Common.Domain;
    using ViewData.Rest.Messages;
    using ViewData.Services;

    [RoutePrefix("api/viewdata")]
    public class ViewDataController : ApiController
    {
        private readonly IViewDataServices _viewDataServices;
        private readonly ILogger _fileLogger;

        public ViewDataController(IViewDataServices viewDataServices, ILogger logger)
        {
            _viewDataServices = viewDataServices;
            _fileLogger = logger;
            //_fileLogger = new FileLogger() { ErrorLogFilePath = System.Web.HttpContext.Current.Server.MapPath("\\logs\\") };
        }

        /// <summary>
        /// Get list of Surveys for the given passcode
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{surveyid:int}")]
        public IHttpActionResult GetViewData([FromUri] ViewDataRequest request)
        {
            try
            {
                var responsesBulk = _viewDataServices.GetViewData(request);
                if (responsesBulk == null)
                {
                    return Content(HttpStatusCode.OK, $"Could not find any information for survey : {request.SurveyID}");
                }
                else
                {
                    if ((responsesBulk.Data == null) || !responsesBulk.Data.Any())
                    {
                        return Content(HttpStatusCode.OK, $"Could not find any responses for survey id : {request.SurveyID}");
                    }
                    var amberflagImage = $"<img src='{request.ImagePath}/flag_yellow.png' />";
                    var redflagImage = $"<img src='{request.ImagePath}/flag_red.png' />";
                    foreach (var row in responsesBulk.Data)
                    {
                        var amberFlagColumnIndex = 4;
                        var redFlagColumnIndex = 3;
                        var amberFlagImageColumnIndex = 6;
                        var redFlagImageColumnIndex = 5;
                        if ((amberFlagColumnIndex != -1) && (amberFlagImageColumnIndex != -1))
                        {
                            row[amberFlagImageColumnIndex] = row[amberFlagColumnIndex].ToLower() == "1" ? amberflagImage : string.Empty;
                        }
                        if ((redFlagColumnIndex != -1) && (redFlagImageColumnIndex != -1))
                        {
                            row[redFlagImageColumnIndex] = row[redFlagColumnIndex].ToLower() == "1" ? redflagImage : string.Empty;
                        }
                    }
                    var questionColumns = responsesBulk.Columns.Select(x => new TableColumn { Title = (x.Title == "RedFlagImage") ? "Red Flag" : (x.Title == "AmberFlagImage") ? "Amber Flag" : x.Title, ToolTip = x.ToolTip, Width = "10 %" }).ToArray();

                    // Set column width
                    questionColumns[0].Width = "3%";    // ID
                    questionColumns[1].Width = "5%";    // Date
                    questionColumns[3].Width = "2%";    // RedFlag
                    questionColumns[4].Width = "2%";    // AmberFlag
                    questionColumns[5].Width = "1%";    // RedFlagImage
                    questionColumns[6].Width = "1%";    // AmberFlagImage

                    var evenWidth = 80 / (questionColumns.Count() - 7);
                    // SystemData Column
                    questionColumns[2].Width = string.Format("{0}%", 100 - (80 + 14));

                    for (int i = 7; i < questionColumns.Count(); i++)
                    {
                        questionColumns[i].Width = string.Format("{0}%", evenWidth);
                    }
                    var firstSixColumns = questionColumns.Where(x => !questionColumns.Take(6).Contains(x));
                    var hiddenColumns = new List<string>() { "RedFlag", "AmberFlag" };

                    return Ok(
                        new
                        {
                            Columns = questionColumns
                                        .Select(x => new { title = x.Title, width = x.Width, className = !firstSixColumns.Contains(x) ? "dt-center" : "", visible = !hiddenColumns.Contains(x.Title), tooltip = x.ToolTip })
                                        .ToArray(),
                            Data = responsesBulk.Data
                        });
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");            
        }

    }
}
