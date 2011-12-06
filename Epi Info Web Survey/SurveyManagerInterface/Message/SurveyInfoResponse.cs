using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.WCF.SurveyService.MessageBase;
using Epi.Web.Common.DTO;

namespace Epi.Web.WCF.SurveyService.Messages
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
        public SurveyInfoResponse() { }

        /// <summary>
        /// Overloaded Constructor for SurveyInfoResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public SurveyInfoResponse(string correlationId) : base(correlationId) { }

        /// <summary>
        /// List of SurveyInfos. 
        /// </summary>
        [DataMember]
        public IList<SurveyInfoDTO> SurveyInfoList;

        /// <summary>
        /// Single SurveyInfo
        /// </summary>
        [DataMember]
        public SurveyInfoDTO SurveyInfo;
    }
}
