using MVCApp.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLibrary;
using DataLibrary.Logic;

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        //Go to the home page
        public ActionResult Index()
        {
            return View();
        }

        #region user controls

        [Authorize(Roles = "Admin")]
        //Go to the view users page
        public ActionResult ViewUsers()
        {
            //alter title
            ViewBag.Message = "User List";

            //load users from database into a var using sql
            var data = UserProcessor.LoadUsers();

            //instantiate list to hold user data
            List<UserModel> users = new List<UserModel>();

            //add user models to the list for each user in the data
            foreach (var row in data)
            {
                users.Add(new UserModel
                {
                    Id = row.Id,
                    UserName = row.UserName,
                    Email = row.Email,
                    UserId = row.Id,
                    RoleId = row.RoleId
                });
            }

            //show the ViewUsers view populated with the list
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        //go to the edit role page
        public ActionResult EditRole(string userId, int? role, string username, string email)
        {
            //create a user model from the list item that was clicked on the view contacts page
            UserModel user = new UserModel
            {
                UserId = userId,
                RoleId = role,
                UserName = username,
                Email = email
            };

            //return the edit role view populated with the user
            return View(user);
        }

        //Edit the role from the edit role page using user input
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditRole(UserModel model)
        {
            //check model state
            if (ModelState.IsValid)
            {
                //alter the sql database using the user input
                UserProcessor.ChangeUserRole(model.UserId, model.RoleId, model.UserName, model.Email);

                //Go to the view users page
                return RedirectToAction("ViewUsers");
            }

            //reload page if any errors occur
            return View(model);
        }

        #endregion

        #region contact controls

        [Authorize]
        //Go to the view contacts page
        public ActionResult ViewContacts()
        {
            //alter page title
            ViewBag.Message = "Contacts List";

            //load contacts from the sql database
            var data = ContactProcessor.LoadContacts();

            //instantiate list for the contacts
            List<ContactModel> contacts = new List<ContactModel>();

            //populate list with each row from the contacts database
            foreach (var row in data)
            {
                contacts.Add(new ContactModel
                {
                    Id = row.Id,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Company = row.Company,
                    Phone = row.Phone,
                    Email = row.Email
                });
            }

            //return ViewContacts view populated with the list
            return View(contacts);
        }

        [Authorize(Roles = "Admin, Manager")]
        //Go to the create contacts page
        public ActionResult CreateContact()
        {
            //alter page title
            ViewBag.Message = "Page to create a new contact.";

            //return the view
            return View();
        }

        //Create a contact from the view contacts page
        [HttpPost] 
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult CreateContact(ContactModel model)
        {
            //check model state
            if (ModelState.IsValid)
            {
                //add contact to the database from user input
                ContactProcessor.CreateContact(model.FirstName, model.LastName, model.Company, model.Phone, model.Email);

                //load the view contacts page
                return RedirectToAction("ViewContacts");
            }

            //reload page
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        //Go to edit contact view
        public ActionResult EditContact(int id, string firstname, string lastname, string company, string phone, string email)
        {
            //alter page title
            ViewBag.Message = "Page to edit a contact.";

            ContactModel contact = new ContactModel
            {
                Id = id,
                FirstName = firstname,
                LastName = lastname,
                Company = company,
                Phone = phone,
                Email = email
            };

            //return the view
            return View(contact);
        }

        //edit a contact from the edit contact page
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult EditContact(ContactModel model)
        {
            //check model state
            if (ModelState.IsValid)
            {
                ContactProcessor.AlterContact(model.Id, model.FirstName, model.LastName, model.Company, model.Phone, model.Email);
                //load the view contacts page
                return RedirectToAction("ViewContacts");
            }

            //reload page
            return View();
        }

        //Run command to delete the contact with the param id == Id from the dbo.ContactTable Database 
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult DeleteContact(int id)
        {
            ContactProcessor.DeleteContact(id);

            return RedirectToAction("ViewContacts");
        }
        #endregion
    }
}