using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HaloOnline.DataAccess
{
    public class HaloDatabase
    {
        private static ConnectionStringSettings HaloDB_ConnectionStringSetting = ConfigurationManager.ConnectionStrings["HaloDBConnection"];

        public static DataSet GetDataSet(string procedureName, SqlParameter[] parameters = null)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (IDbConnection cnn = new SqlConnection(HaloDB_ConnectionStringSetting.ConnectionString))
                {
                    string commandString = procedureName;
                    IDbCommand cmd = new SqlCommand(commandString, (SqlConnection)cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 240;
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    using (DbDataAdapter dataAdapter = new SqlDataAdapter((SqlCommand)cmd))
                    {
                        dataAdapter.Fill(dataSet);
                    }
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dataSet;
        }

        public static DataTable GetDataTable(string procedureName, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable(procedureName);
            try
            {
                using (IDbConnection cnn = new SqlConnection(HaloDB_ConnectionStringSetting.ConnectionString))
                {
                    string commandString = procedureName;
                    IDbCommand cmd = new SqlCommand(commandString, (SqlConnection)cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 240;
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    DbDataAdapter dataAdapter = new SqlDataAdapter((SqlCommand)cmd);

                    dataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dataTable;
        }

        public static int ExecuteNonQuery(string procedureName, SqlParameter[] parameters = null)
        {
            int rowAffected = 0;
            using (IDbConnection cnn = new SqlConnection(HaloDB_ConnectionStringSetting.ConnectionString))
            {
                string commandString = procedureName;
                IDbCommand cmd = new SqlCommand(commandString, (SqlConnection)cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 240;
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                cnn.Open();
                rowAffected = cmd.ExecuteNonQuery();
            }
            return rowAffected;
        }
    }
}