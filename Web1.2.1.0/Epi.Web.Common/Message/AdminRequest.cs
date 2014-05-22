using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Message
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class AdminRequest : Epi.Web.Common.MessageBase.RequestBase
        {

        public AdminRequest()
        {
          //  this.AdminEmail = new AdminDTO();
        }
        [DataMember]
        public string AdminEmail;
            

        [DataMember]
        public string OrganizationId;
        [DataMember]
        public string OrganizationKey; 

        [DataMember]
        public bool IsActive;
            


        }
    }
