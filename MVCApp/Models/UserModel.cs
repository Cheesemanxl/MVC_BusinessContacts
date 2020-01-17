using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCApp.Models
{
    //front end user model
    public class UserModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        [Required]
        [Range(0, 2)]
        public int? RoleId { get; set; }

    }
}