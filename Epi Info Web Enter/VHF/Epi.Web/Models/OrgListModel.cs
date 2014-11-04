using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class OrgListModel
        {
        public List<OrganizationModel> OrganizationList { get; set; }
        public string Message;
        }
    }