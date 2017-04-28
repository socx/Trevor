using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Extensions
{
    public static class ExtensionMethods
    { 
        public static List<SqlParameter> Update(this List<SqlParameter> sqlParameters, SqlParameter newSqlParameter)
        {
            var existingParameter = sqlParameters.Where(p => p.ParameterName == newSqlParameter.ParameterName).FirstOrDefault();
            if (existingParameter != null)
            {
                sqlParameters.Remove(existingParameter);
            }
            sqlParameters.Add(newSqlParameter);
            return sqlParameters;
        }

        public static List<T> TrimFirst<T>(this List<T> list, int maxCount)
        {
            if (list.Count <= maxCount)
                return list;

            do
            {
                T first = list.First<T>();
                list.Remove(first);
            }
            while (list.Count > maxCount);

            return list;
        }

    }
}