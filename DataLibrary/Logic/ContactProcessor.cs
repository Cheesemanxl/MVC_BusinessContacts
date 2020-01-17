using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Logic
{
    public static class ContactProcessor
    {
        public static int CreateContact(string firstName, string lastName, string company, string phone, string email)
        {
            //Map data to a contact model
            ContactModel data = new ContactModel
            {
                FirstName = firstName,
                LastName = lastName,
                Company = company,
                Phone = phone,
                Email = email
            };

            //create sql statement to insert contact into table using parameterized sql
            string sql = @"insert into dbo.ContactTable (FirstName, LastName, Company, Phone, Email)
                           values (@FirstName, @LastName, @Company, @Phone, @Email)";

            //executes the statement and returns number of rows affected
            return SqlDataAccess.SaveData(sql, data);
        }

        public static void AlterContact(int id, string firstName, string lastName, string company, string phone, string email)
        {
            //Map data to a contact model
            ContactModel data = new ContactModel
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Company = company,
                Phone = phone,
                Email = email
            };

            //create sql statement to edit contact in table using parameterized sql
            string sql = @"UPDATE dbo.ContactTable
                            SET FirstName = @FirstName, LastName = @LastName, Company = @Company, Phone = @Phone, Email = @Email
                            WHERE Id = @Id";

            //executes the statement
            SqlDataAccess.SaveData(sql, data);
        }

        //Grabs all rows from the contacts database
        public static List<ContactModel> LoadContacts()
        {
            //create sql statement to get all info from the contacts database
            string sql = @"select Id, FirstName, LastName, Company, Phone, Email
                           from dbo.ContactTable";

            //executes the sql statement and returns the number of rows affected
            return SqlDataAccess.LoadData<ContactModel>(sql);
        }

        public static void DeleteContact(int id)
        {
            ContactModel contact = new ContactModel { Id = id };

            string sql = @"DELETE FROM dbo.ContactTable WHERE Id = @Id";

            SqlDataAccess.SaveData(sql, contact);
        }
    }
}
