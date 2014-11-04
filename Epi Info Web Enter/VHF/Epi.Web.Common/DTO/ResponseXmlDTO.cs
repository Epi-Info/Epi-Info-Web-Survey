using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.DTO
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
   public  class ResponseXmlDTO
        {
        [DataMember]
        public string ResponseId { get; set; }
        [DataMember]
        public string Xml { get; set; }
        [DataMember]
        public int UserId { get; set; }

        }
    }
