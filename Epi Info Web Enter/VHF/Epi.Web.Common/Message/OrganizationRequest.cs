using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrganizationRequest : Epi.Web.Enter.Common.MessageBase.RequestBase
    {

        public OrganizationRequest()
        {
            this.Organization = new OrganizationDTO();
        }

        /// <summary>
        /// Organization object.
        /// </summary>
        [DataMember]
        public OrganizationDTO Organization;
        [DataMember]
        public Guid AdminSecurityKey;
        [DataMember]
        public int UserId;
        [DataMember]
        public int UserRole;
        [DataMember]
        public string Action;
        [DataMember]
        public UserDTO OrganizationAdminInfo;
         
    }
}
