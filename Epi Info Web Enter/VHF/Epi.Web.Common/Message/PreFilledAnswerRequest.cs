using System.Runtime.Serialization;
using System.Collections.Generic;

using Epi.Web.Enter.Common.DTO;

namespace Epi.Web.Enter.Common.Message
    {
    /// <summary>
    /// Represents a Survey Response  request message from client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class PreFilledAnswerRequest
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
