using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HaloOnline.Reports.Store
{
    using Common;
    using Common.Services;
    using Rest.Messages;
    using Services;
    using System.Dynamic;
    using System.IO;
    using System.Text.RegularExpressions;

    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        private readonly IReportsServices _reportsServices;
        private readonly ILogger _fileLogger;

        public ReportsController(IReportsServices reportsServices, ILogger fileLogger)
        {
            _reportsServices = reportsServices;
            _fileLogger = fileLogger;
            _fileLogger = new FileLogger() { ErrorLogFilePath = System.Web.HttpContext.Current.Server.MapPath("\\logs\\") };
        }

        [HttpGet]
        [Route("surveys/{surveyid:int}")]
        public IHttpActionResult GetSurveyDetails(int surveyid)
        {
            try
            {
                var data = _reportsServices.GetSurveyByID(surveyid);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        [HttpGet]
        [Route("surveylist/{passcode}")]
        public IHttpActionResult GetSurveys(string passcode)
        {
            try
            {
                var surveyMessages = _reportsServices.GetSurveysByPasscode(passcode);
                if (surveyMessages != null)
                {
                    return Ok(surveyMessages);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "Invalid Passcode");
        }

        [HttpGet]
        [Route("surveystats/{surveyid:int}/{startDateString}/{endDateString}")]
        public IHttpActionResult GetSurveyStatistics([FromUri]int surveyid, [FromUri]string startDateString, [FromUri]string endDateString)
        {
            try
            {
                DateRange period = DataHelper.BuildDateRange(startDateString, endDateString);
                var statistics = _reportsServices.GetSurveyStatistics(surveyid, period);
                if (statistics != null)
                {
                    return Ok(new { Statistics = statistics });
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        [HttpGet]
        [Route("responsedata")]
        public IHttpActionResult GetResponses([FromUri] ReportRequest reportRequest)
        {
            try
            {
                var responseMessages = _reportsServices.GetResponses(reportRequest);
                if (responseMessages != null)
                {
                    return Ok(responseMessages);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.NoContent, "Invalid Passcode");
        }

        [HttpGet]
        [Route("confidencescoredata")]
        public IHttpActionResult GetConfidenceScoreData([FromUri] ReportRequest reportRequest)
        {
            try
            {
                var data = _reportsServices.GetCustomerConfidenceScoreData(reportRequest);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        [HttpGet]
        [Route("confidencetrenddata/{surveyid}")]
        public IHttpActionResult GetConfidenceTrendData(int SurveyId)
        {
            var dataPoints = new List<dynamic>();
            try
            {
                var data = _reportsServices.GetCustomerConfidenceTrendData(SurveyId);
                var dataPointsWithValue = 0;
                if (data != null)
                {
                    // Fix data and return 
                    var numberOfDataPoints = 0;
                    foreach (var entry in data)
                    {
                        dynamic dataPoint = new ExpandoObject();
                        dataPoint.Period = BuildDateStringFromYM(entry.Period);
                        if (entry.Current.HasValue)
                        {
                            dataPoint.Current = entry.Current;
                            dataPointsWithValue++;
                        }
                        dataPoints.Add(dataPoint);
                        numberOfDataPoints++;
                        if (numberOfDataPoints >= 6)
                            break;
                    }
                    
                    return Ok(new { HasEnoughDataPoints = dataPointsWithValue > 1, DataPoints = dataPoints.ToList() });
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        [HttpGet]
        [Route("customerexperiencedata")]
        public IHttpActionResult GetCustomerExperienceData([FromUri] ReportRequest reportRequest)
        {
            try
            {
                var data = _reportsServices.GetCustomerExperienceData(reportRequest);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        /// <summary>
        /// Get recognition data for the given survey id within the given date range
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recognitiondata")]
        public IHttpActionResult GetRecognitionData([FromUri] ReportRequest reportRequest)
        {
            try
            {
                var data = _reportsServices.GetRecognitionData(reportRequest);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        /// <summary>
        /// Return Flag data for given survey with the given criteria
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("flagdata")]
        public IHttpActionResult GetFlagData([FromUri] ReportRequest reportRequest)
        {
            try
            {
                var flagData = _reportsServices.GetFlagsData(reportRequest);
                if (flagData != null)
                {
                    return Ok(flagData);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        /// <summary>
        /// Get period comments data for the given survey id within the given criteria
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("periodcommentsdata")]
        public IHttpActionResult GetPeriodCommentsData([FromUri] ReportRequest reportRequest)
        {
            try
            {
                var data = _reportsServices.GetPeriodCommentsData(reportRequest);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }
        
        [HttpPost]
        [Route("pdfdownload")]
        public IHttpActionResult PostPDFReport(PdfReportRequest pdfReportRequest)
        {
            var returnUrl = string.Empty;
            try
            {
                // Generate the filename
                Uri baseUri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, String.Empty));
                string fileName = $"Survey_Report_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
                var localFilePath = System.Web.HttpContext.Current.Server.MapPath("\\pdfreports\\" + fileName);

                //Phantom
                var htmlContent = pdfReportRequest.PrintHtml;
                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);

                //TODO: Remove this is to temporarily log
                htmlToPdf.LogReceived += (sender, a) => {
                    var logLine = a.Data;
                    _fileLogger.LogInfo(logLine);
                };

                File.WriteAllBytes(localFilePath, pdfBytes);

                returnUrl = $"{baseUri}pdfreports/{fileName}";

            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }

            return Content(HttpStatusCode.OK, returnUrl);
        }
        
        /// <summary>
        /// Get direct comments data for the given survey id within the given criteria
        /// </summary>
        /// <param name="reportRequest">The criteria with which to retrieve the direct comments</param>
        /// <returns></returns>
        [HttpGet]
        [Route("directcommentsdata/{surveyid:int}/{startDateString}/{endDateString}")]
        public IHttpActionResult GetDirectCommentsData([FromUri]int surveyid, [FromUri]string startDateString, [FromUri]string endDateString)
        {
            try
            {
                DateRange period = DataHelper.BuildDateRange(startDateString, endDateString);
                var data = _reportsServices.GetDirectCommentsData(surveyid, period);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "No data found");
        }

        /// <summary>
        /// Get direct comments data for the given survey id within the given date range
        /// </summary>
        /// <param name="directCommentRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("directcomment")]
        public IHttpActionResult PostInsertDirectComment(DirectCommentRequest directCommentRequest)
        {
            dynamic responseJson = new ExpandoObject();
            try
            {
                var respondentId = _reportsServices.InsertDirectComment(directCommentRequest);
                if (respondentId > 0)
                {
                    responseJson.Message = "Comment recorded successfully";
                    return Content(HttpStatusCode.OK, responseJson);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            responseJson.Message = "Comment recorded successfully";
            return Content(HttpStatusCode.OK, responseJson);
        }
        
        private string BuildDateStringFromYM(string dateInYMFormat)
        {
            var dateString = string.Empty;
            var months = new Dictionary<string, string>
            {
                {"01", "Jan" },{"02", "Feb" },{"03", "Mar" },{"04", "Apr" },{"05", "May" },{"06", "Jun" },
                { "07", "Jul" },{"08", "Aug" },{"09", "Sep" }, {"10", "Oct" }, {"11", "Nov" }, {"12", "Dec" }
            };
            try
            {
                //Regex r = new Regex(@"^\d{4}\d{2}\d{2}T\d{2}\d{2}Z$");
                Regex r = new Regex(@"^\d{4}-\d{2}$");
                if (!r.IsMatch(dateInYMFormat))
                {
                    throw new FormatException(
                        string.Format("{0} is not the correct format. Should be YYYY-MM", dateString));
                }
                var splitDate = dateInYMFormat.Split('-');
                //dateString = string.Format("{0}{1}", months[splitDate[1]], splitDate[0].Substring(2,2));
                dateString = $"{ months[splitDate[1]]}  '{splitDate[0].Substring(2, 2)}";
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return dateString;
        }
        
    }
}
