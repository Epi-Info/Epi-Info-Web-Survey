using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Epi.Web.Enter.Common.Criteria
    {
    public class FormInfoCriteria : Criteria
        {
        
        [DataMember]
        public Guid OrganizationKey { get; set; }

        [DataMember]
        public int UserId { get; set; } 

        [DataMember]
        public DateTime DateCreatedMin { get; set; }
        [DataMember]
        public DateTime DateCreatedMax { get; set; }

        [DataMember]
        public string FormName { get; set; }

        }
    }
