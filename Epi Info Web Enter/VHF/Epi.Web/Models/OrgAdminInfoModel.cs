using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Mvc;
namespace Epi.Web.MVC.Models
    {
    public class OrgAdminInfoModel
        {
        private string _AdminEmail;
        private string _ConfirmAdminEmail;
        private string _OrgName;
        private string _Status;
        private string _AdminFirstName;
        private string _AdminLastName;
        private string _PhoneNumber;
        private string _AdressLine1;
        private string _AdressLine2;
        private string _City;
        private int _SelectedState;
        private bool _IsEditMode;
        private bool _IsOrgEnabled;
        //private readonly List<StateModel> _States;
        private string _Zip;


        public OrgAdminInfoModel()
        {
           //States = new List<SelectListItem>();
        }

         [Required(ErrorMessage = "Email is required.")]
         [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        public string AdminEmail
            {
            get { return _AdminEmail; }
            set { _AdminEmail = value; }
            }
          [Required(ErrorMessage = "The organization name is required.")]
        public string OrgName
            {
            get { return _OrgName; }
            set { _OrgName = value; }
            }
          [Required(ErrorMessage = "Confirm email is required.")]
          [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
          [Compare("AdminEmail", ErrorMessage = "The email and confirmation do not match.")]
          public string ConfirmAdminEmail
              {
              get { return _ConfirmAdminEmail; }
              set { _ConfirmAdminEmail = value; }
              }
          public string Status
              {
              get { return _Status; }
              set { _Status = value; }
              }
        [Required(ErrorMessage = "First Name is required.")]
          public string AdminFirstName
              {
              get { return _AdminFirstName; }
              set { _AdminFirstName = value; }
              }
          [Required(ErrorMessage = "Last Name is required.")]
        public string AdminLastName
              {
              get { return _AdminLastName; }
              set { _AdminLastName = value; }
              }
         // [Required(ErrorMessage = "Phone Number is required.")]
         // [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Invalid Phone Number.")]
         // public string PhoneNumber
         //     {
         //     get { return _PhoneNumber; }
         //     set { _PhoneNumber = value; }
         //     }
         // [Required(ErrorMessage = "Adress is required.")]
         
         // public string AdressLine1
         //     {
         //     get { return _AdressLine1; }
         //     set { _AdressLine1 = value; }
         //     }
         // public string AdressLine2
         //     {
         //     get { return _AdressLine2; }
         //     set { _AdressLine2 = value; }
         //     }
         //[Required(ErrorMessage = "City is required.")]
         // public string City
         //     {
         //     get { return _City; }
         //     set { _City = value; }
         //     }
         // [Required(ErrorMessage = "State is required.")]
         //public int SelectedState
         //     {
         //     get { return _SelectedState; }
         //     set { _SelectedState = value; }
         //     }
         //[Required(ErrorMessage = "Zip Code is required.")]
         //[RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Invalid Zip Code Number.")]
         // public string Zip
         //     {
         //     get { return _Zip; }
         //     set { _Zip = value; }
         //     }

         //public List<SelectListItem> States
         //    {
         //    get;
         //    set;

         //   }
         public bool IsEditMode
             {
             get { return _IsEditMode; }
             set { _IsEditMode = value; }

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
         
         public  bool IsOrgEnabled
             {
             get { return _IsOrgEnabled; }
             set { _IsOrgEnabled = value; }

             }
        }
         
    }