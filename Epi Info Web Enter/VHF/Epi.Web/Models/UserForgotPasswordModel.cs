using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Epi.Web.MVC.Models
{
    public class UserForgotPasswordModel
    {
        [Required(ErrorMessage = "The email address is required")]
        [DataType (DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }
    }
}