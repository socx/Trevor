
using HaloOnline.Reports.Rest.Messages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace HaloOnline.Common
{
    using Extensions;

    public class DataHelper
    {
        public static List<SqlParameter> BuildParameters(DataRequest dataRequest)
        {
            var sqlParameters = new List<SqlParameter>();
            var properties = dataRequest.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                var propertyValue = prop.GetValue(dataRequest, null);
                // TODO: REFACTOR; IN THE CURRENT STATE WE MAY END UP WITH INFINITELY LONG SWITCH CASE
                if ((propertyValue != null) && !string.IsNullOrEmpty(propertyValue.ToString().ToLower()) && (propertyValue.ToString().ToLower() != "all"))
                {
                    switch (prop.Name)
                    {
                        case "AllWithOrWithoutFilters":
                            if (propertyValue.ToString().ToLower() == "without")
                                sqlParameters.Add(new SqlParameter("@HasWorkOrder", false));
                            else if (propertyValue.ToString().ToLower() == "with")
                                sqlParameters.Add(new SqlParameter("@HasWorkOrder", true));
                            break;
                        case "StartDate":
                        case "EndDate":
                            var dateValue = new DateTime();
                            if( DateTime.TryParse(propertyValue.ToString(), out dateValue))
                            {
                                sqlParameters = sqlParameters.Update(new SqlParameter($"@{prop.Name}", dateValue));
                            }
                            break;
                        case "ImagePath":
                            //DO NOTHING
                            break;
                        default:
                            sqlParameters.Add(new SqlParameter($"@{prop.Name}", propertyValue));
                            break;

                    }
                }
            }

            return sqlParameters;
        }

        /// <summary>
        /// Adds or updates a parameter from the given parameter list
        /// If a parameter with the same name as the new parameter exists, it is deleted and the new parameter is added.
        /// Otherwise the new parameter is ismply added
        /// </summary>
        /// <param name="parameters">List of parameters</param>
        /// <param name="parameter">Parameter to add or replace an existing one with</param>
        /// <returns></returns>
        public static List<SqlParameter> UpdateParameters(List<SqlParameter> parameters, SqlParameter parameter )
        {
            var existingParameter = parameters.Where(p => p.ParameterName == parameter.ParameterName).FirstOrDefault();
            if(existingParameter != null)
            {
                parameters.Remove(existingParameter);
            }
            parameters.Add(parameter);

            return parameters;
        }

        //TODO: Write this properly and put in the most suitable place/class.
        public static DateRange GetPreviousPeriod(DateRange period)
        {
            DateRange previousPeriod = new DateRange();
            if (period.From > period.To)
            {
                throw new Exception("Invalid Date Range, End Date must be greater than Start Date");
            }
            // Normalise dates
            period.From = new DateTime(period.From.Year, period.From.Month, period.From.Day, 0, 0, 0);
            period.To = new DateTime(period.To.Year, period.To.Month, period.To.Day, 23, 59, 59);
            int daysInterval = (period.From - period.To).Days;

            previousPeriod.To = period.From.AddSeconds(-1);
            previousPeriod.From = previousPeriod.To.AddDays(daysInterval);
            previousPeriod.From = new DateTime(previousPeriod.From.Year, previousPeriod.From.Month, previousPeriod.From.Day, 0, 0, 0);

            return previousPeriod;
        }

        /// <summary>
        /// Builds a DateRange object out of start and end date strings.
        /// </summary>
        /// <param name="startDateString"></param>
        /// <param name="endDateString"></param>
        /// <returns></returns>
        public static DateRange BuildDateRange(string startDateString, string endDateString)
        {
            var startDate = BuildDateTimeFromYAFormat(startDateString);
            var endDate = BuildDateTimeFromYAFormat(endDateString);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            DateRange period = new DateRange { From = startDate, To = endDate };
            return period;
        }

        public static DateTime BuildDateTimeFromYAFormat(string dateString)
        {
            var dt = new DateTime();
            Regex r = new Regex(@"^\d{4}\d{2}\d{2}$");
            if (!r.IsMatch(dateString))
            {
                throw new FormatException(
                    string.Format("{0} is not the correct format. Should be yyyyMMddThhmmZ", dateString));
            }
                
            dt = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
            return dt;
        }
    }
}