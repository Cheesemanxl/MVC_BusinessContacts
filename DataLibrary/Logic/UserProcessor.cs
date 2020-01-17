using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Logic
{
    public class UserProcessor
    {
        //Grabs all rows from the AspNetUsers database
        public static List<UserModel> LoadUsers()
        {
            //create sql statement to get all info from the user database
            string sql = @"SELECT dbo.AspNetUsers.Id, dbo.AspNetUsers.Email, dbo.AspNetUsers.UserName, dbo.AspNetUserRoles.UserId, dbo.AspNetUserRoles.RoleId
                           FROM dbo.AspNetUsers
                           LEFT JOIN dbo.AspNetUserRoles ON dbo.AspNetUsers.Id = dbo.AspNetUserRoles.UserId
                           ORDER BY dbo.AspNetUserRoles.RoleId";

            //executes the sql statement and returns the number of rows affected
            return SqlDataAccess.LoadUserData<UserModel>(sql);
        }

        //map data from the selected item in the ViewUsers view
        public static void ChangeUserRole(string userId, int? roleId, string username, string email)
        {
            UserModel data = new UserModel
            {
                UserId = userId,
                RoleId = roleId,
                UserName = username,
                Email = email
            };

            //alter role using sql
            SqlDataAccess.SaveUserRole(data);
        }

    }
}
