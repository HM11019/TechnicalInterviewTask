using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationNewJob.Models;
using WebApplicationNewJob.Services.Http;

namespace WebApplicationNewJob.Classes
{
    public class DataLibrary
    {

        private readonly IConfiguration _configuration;
        private readonly IWebhostingEnvironment env;
        private readonly string logPath;
        private readonly IHttpService _httpService;

        public static SqlConnection ConnectionSQL(string DBSettings)
        {
            SqlConnection con = new SqlConnection(DBSettings);

            try
            {
                con.Open();
            }
            catch (Exception)
            {
                con.Dispose();
                return null;
            }

            return con;
        }


        public static async Task<bool> InsertOrUpdate(string sqlQuery, SqlParameter[] parameters, string DBSettings)
        {
            bool isSuccess = false;
            SqlConnection con = ConnectionSQL(DBSettings);

            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    int affectedRows = await cmd.ExecuteNonQueryAsync();
                    isSuccess = affectedRows > 0;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return isSuccess;
        }

        public static async Task<List<Employee>> GetEmployeesAsyncADONET(string sqlQuery, string DBSettings)
        {
            var employees = new List<Employee>();

            SqlConnection con = ConnectionSQL(DBSettings);
            SqlCommand cmd = new SqlCommand(sqlQuery, con);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var emp = new Employee
                        {
                            EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Title = reader["Title"].ToString(),
                            Address = reader["Address"].ToString(),
                            Country = reader["Country"].ToString()
                        };

                        employees.Add(emp);
                    }
                }
            

            return employees;
        }


        // Generic method to get any table from database. This method save the values in Dictionary
        public static async Task<List<Dictionary<string, object>>> GetTableDataAsync(string sqlQuery, string DBSettings)
        {
            var resultList = new List<Dictionary<string, object>>();

            try
            {
                SqlConnection con = ConnectionSQL(DBSettings);
                SqlCommand cmd = new SqlCommand(sqlQuery, con);                
                cmd.CommandTimeout = 0;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                                row[columnName] = value;
                            }

                            resultList.Add(row);
                        }
                    }
                }
            
            catch (Exception ex)
            {
                // Manejo de errores personalizado (log, retorno, throw, etc.)
                throw new Exception("Error al ejecutar la consulta: " + ex.Message, ex);
            }

            return resultList;
        }

    }
}
