using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrganizationRequest : Epi.Web.Common.MessageBase.RequestBase
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
    }
}
