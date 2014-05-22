﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.BusinessObject
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrganizationBO
    {
        private string _Organization;
        private string _OrganizationKey;
        private bool _IsEnabled;
       
        [DataMember]
        public string Organization
        {
            get { return _Organization; }
            set { _Organization = value; }
        }

        [DataMember]
        public string OrganizationKey 
        {
            get { return _OrganizationKey; }
            set { _OrganizationKey = value; }
        }

        [DataMember]
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
        }
       

    }
}
