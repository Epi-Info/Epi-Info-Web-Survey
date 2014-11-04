using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;
 
namespace Epi.Web.MVC.Models
    {
    public class UserOrgModel
        {
        public List<UserModel>  UserList { get; set; }

        public List<OrganizationModel> OrgList { get; set; }

        public string Message { get; set; }
        public int UserHighestRole { get;set; }
         
        }
    }