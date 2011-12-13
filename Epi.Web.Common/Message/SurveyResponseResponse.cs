using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.Common.MessageBase;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
{
    /// <summary>
    /// Represents a SurveyInfo response message to client
    /// </summary>    
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyResponseResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for SurveyInfoResponse.
        /// </summary>
        public SurveyResponseResponse() { }

        /// <summary>
        /// Overloaded Constructor for SurveyInfoResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public SurveyResponseResponse(string correlationId) : base(correlationId) { }

        /// <summary>
        /// List of SurveyInfos. 
        /// </summary>
        [DataMember]
        public IList<SurveyResponseDTO> SurveyResponseDTOList;

        /// <summary>
        /// Single SurveyInfo
        /// </summary>
        [DataMember]
        public SurveyResponseDTO SurveyResponseDTO;
    }
}
