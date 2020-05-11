using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.DTO
{
    public class OrganizationDTO
    {
        private string _Organization;
        private string _OrganizationKey;
        private bool _IsEnabled;
        
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

        public int OrganizationId { get;  set; }
    }
}
