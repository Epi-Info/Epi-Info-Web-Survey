using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Message
    {

    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrganizationAccountRequest : Epi.Web.Common.MessageBase.ResponseBase
        {


        public OrganizationAccountRequest()
        {
            this.Organization = new OrganizationDTO();
        }


        [DataMember]
        public OrganizationDTO Organization;
        [DataMember]
        public AdminDTO Admin;

        }
    }
