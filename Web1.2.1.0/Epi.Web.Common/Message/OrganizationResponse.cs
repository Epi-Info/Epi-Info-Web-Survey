using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrganizationResponse : Epi.Web.Common.MessageBase.ResponseBase 
    {

        /// <summary>
        /// Default Constructor for OrganizationResponse.
        /// </summary>
        public OrganizationResponse() {}

        /// <summary>
        /// Overloaded Constructor for OrganizationResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public OrganizationResponse(string correlationId) : base(correlationId) { }

        [DataMember]
        public string Message;

        /// <summary>
        /// OrganizationInfo object.
        /// </summary>
        [DataMember]
        public List<OrganizationDTO> OrganizationList;
    }
}
