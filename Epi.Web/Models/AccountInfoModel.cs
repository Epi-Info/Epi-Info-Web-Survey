using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Mvc;
namespace Epi.Web.MVC.Models
    {
    public class AccountInfoModel
        {
        private string _Email;
        private string _ConfirmEmail;
        private string _OrgName;
        private string _Status;
        private string _FirstName;
        private string _LastName;
        private string _PhoneNumber;
        private string _AdressLine1;
        private string _AdressLine2;
        private string _City;
        private int _SelectedState;
        //private readonly List<StateModel> _States;
        private string _Zip;


        public AccountInfoModel()
        {
           States = new List<SelectListItem>();
        }

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
          [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "The email and confirmation do not match.")]
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
        [Required(ErrorMessage = "First Name is required.")]
          public string FirstName
              {
              get { return _FirstName; }
              set { _FirstName = value; }
              }
          [Required(ErrorMessage = "Last Name is required.")]
          public string LastName
              {
              get { return _LastName; }
              set { _LastName = value; }
              }
          [Required(ErrorMessage = "Phone Number is required.")]
          [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Invalid Phone Number.")]
          public string PhoneNumber
              {
              get { return _PhoneNumber; }
              set { _PhoneNumber = value; }
              }
          [Required(ErrorMessage = "Adress is required.")]
         
          public string AdressLine1
              {
              get { return _AdressLine1; }
              set { _AdressLine1 = value; }
              }
          public string AdressLine2
              {
              get { return _AdressLine2; }
              set { _AdressLine2 = value; }
              }
         [Required(ErrorMessage = "City is required.")]
          public string City
              {
              get { return _City; }
              set { _City = value; }
              }
          [Required(ErrorMessage = "State is required.")]
         public int SelectedState
              {
              get { return _SelectedState; }
              set { _SelectedState = value; }
              }
         [Required(ErrorMessage = "Zip Code is required.")]
         [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Invalid Zip Code Number.")]
          public string Zip
              {
              get { return _Zip; }
              set { _Zip = value; }
              }

         public List<SelectListItem> States
             {
             get;
             set;

            }
         //public IEnumerable<SelectListItem> States
         //    {
         //    get
         //        {
         //        var allStates = _States.Select(f => new SelectListItem
         //        {
         //            Value = f.StateId.ToString(),
         //            Text = f.StateName
         //        });
         //        return DefaultState.Concat(allStates);
         //        }
         //    }
         //public IEnumerable<SelectListItem> DefaultState
         //    {
         //    get
         //        {
         //        return Enumerable.Repeat(new SelectListItem
         //        {
         //            Value = "-1",
         //            Text = "Select a State"
         //        }, count: 1);
         //        }
         //    }
        }
    }