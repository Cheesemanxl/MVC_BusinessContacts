using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DataLibrary.Models;

namespace DataLibrary.DataAccess
{
    //Helper functions for accessing SQL data
    public static class SqlDataAccess
    {
        //Gets a connnection string defaultss to BusinessContactsDB
        public static string GetConnectionString(string connectionName = "BusinessContactsDB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        //querys data and puts it into a list 
        public static List<T> LoadUserData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString("DefaultConnection")))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        //Grab a user model and alter its role in the AspNetUserRoles database
        public static void SaveUserRole(UserModel data)
        {
            //int used to check if user exists in database yet
            int userCount = 0;

            //connect to the default sql connection database
            using (SqlConnection cnn = new SqlConnection(GetConnectionString("DefaultConnection")))
            {
                //open connection
                cnn.Open();

                //get row of the user we want to alter role of
                using (SqlCommand sql = new SqlCommand($"SELECT COUNT(*) FROM dbo.AspNetUserRoles WHERE UserId LIKE '{data.UserId}'", cnn))
                {
                    //will be 1 if user exists in data and 0 if not
                    userCount = (int)sql.ExecuteScalar();
                }

                //if user exists
                if (userCount == 1)
                {
                    //update the row of the user with the new role id
                    using (SqlCommand sql = new SqlCommand($@"UPDATE dbo.AspNetUserRoles
                                                              SET RoleId = '{data.RoleId}'
                                                              WHERE UserId LIKE '{data.UserId}'", cnn))
                    {
                        sql.ExecuteNonQuery();
                    }
                }
                //if user doesn't exist
                else if (userCount == 0)
                {
                    //add new row to database including user id and role id specified
                    using (SqlCommand sql = new SqlCommand($@"INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
                                                              VALUES ('{data.UserId}','{data.RoleId}')", cnn))
                    {
                        sql.ExecuteNonQuery();
                    }
                }

                //close data connection
                cnn.Close();
            }
        }

        //Loads all data using a query from the BusinessContactsDB
        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        //Executes sql code within the BusinessContactsDB
        public static int SaveData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }
    }
}
