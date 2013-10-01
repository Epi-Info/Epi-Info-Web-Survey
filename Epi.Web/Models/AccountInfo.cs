using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
 
using System.Web.Mvc;
namespace Epi.Web.MVC.Models
    {
    public class AccountInfo
        {
        private string _Email;
        private string _ConfirmEmail;
        private string _OrgName;
        private string _Status;
         [Required(ErrorMessage = "Email is required.")]
         [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        public string Email
            {
            get { return _Email; }
            set { _Email = value; }
            }
          [Required(ErrorMessage = "The organization name is required.")]
        public string OrgName
            {
            get { return _OrgName; }
            set { _OrgName = value; }
            }
          [Required(ErrorMessage = "Confirm email is required.")]
          [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
          [Compare("Email", ErrorMessage = "The email and confirmation do not match.")]
          public string ConfirmEmail
              {
              get { return _ConfirmEmail; }
              set { _ConfirmEmail = value; }
              }
          public string Status
              {
              get { return _Status; }
              set { _Status = value; }
              }
        }
    }