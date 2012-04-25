using System;
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
        private Guid _OrganizationKey;
        private bool _IsEnabled;
        private Guid _AdminId;
        [DataMember]
        public string Organization
        {
            get { return _Organization; }
            set { _Organization = value; }
        }

        [DataMember]
        public Guid OrganizationKey 
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
        [DataMember]
        public Guid AdminId
        {
            get { return _AdminId; }
            set { _AdminId = value; }
        }

    }
}
