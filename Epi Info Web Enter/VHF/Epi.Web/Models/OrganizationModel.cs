using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Epi.Web.MVC.Models
    {
    public class OrganizationModel
        {
        
            private string _Organization;
            private string _OrganizationKey;
            private bool _IsEnabled;
            private int _OrganizationId;
            [Required(ErrorMessage = "Organization Name is required")]
            public string Organization
                {
                get { return _Organization; }
                set { _Organization = value; }
                }

            public string OrganizationKey
                {
                get { return _OrganizationKey; }
                set { _OrganizationKey = value; }
                }

             
            public bool IsEnabled
                {
                get { return _IsEnabled; }
                set { _IsEnabled = value; }
                }
            public int OrganizationId
                {
                get { return _OrganizationId; }
                set { _OrganizationId = value; }
                }

           
        }
    }