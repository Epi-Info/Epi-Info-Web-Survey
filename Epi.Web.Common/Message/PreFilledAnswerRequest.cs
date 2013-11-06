using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
    {
    /// <summary>
    /// Represents a Survey Response  request message from client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class PreFilledAnswerRequest: RequestBase
        {

        public PreFilledAnswerRequest()
        {
        this.AnswerInfo = new PreFilledAnswerDTO();
        }

        /// <summary>
        /// AnswerInfo object.
        /// </summary>
        [DataMember]
        public PreFilledAnswerDTO AnswerInfo;
        }
    }
