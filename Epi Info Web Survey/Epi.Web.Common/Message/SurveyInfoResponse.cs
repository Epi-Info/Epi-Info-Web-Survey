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
    public class SurveyInfoResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for SurveyInfoResponse.
        /// </summary>
        public SurveyInfoResponse() { this.SurveyInfoList = new List<SurveyInfoDTO>(); }

        /// <summary>
        /// Overloaded Constructor for SurveyInfoResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public SurveyInfoResponse(string correlationId) : base(correlationId) { this.SurveyInfoList = new List<SurveyInfoDTO>(); }


        /// <summary>
        /// Single SurveyInfo
        /// </summary>
        [DataMember]
        public List<SurveyInfoDTO> SurveyInfoList;
    }
}
