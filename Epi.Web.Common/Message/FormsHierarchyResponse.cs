using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.DTO;
using System;

namespace Epi.Web.Common.Message
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormsHierarchyResponse : Epi.Web.Common.MessageBase.ResponseBase
        {

        public FormsHierarchyResponse() 
            {
            this.FormsHierarchy = new List<FormsHierarchyDTO>();
            
            }

        [DataMember]
        public List<FormsHierarchyDTO> FormsHierarchy;

        }
    }
