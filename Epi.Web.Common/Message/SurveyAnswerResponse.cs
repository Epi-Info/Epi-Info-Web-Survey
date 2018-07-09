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
    public class SurveyAnswerResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for SurveyInfoResponse.
        /// </summary>
        public SurveyAnswerResponse() { this.SurveyResponseList = new List<SurveyAnswerDTO>(); }

        /// <summary>
        /// Overloaded Constructor for SurveyInfoResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public SurveyAnswerResponse(string correlationId) : base(correlationId) { this.SurveyResponseList = new List<SurveyAnswerDTO>(); }

           /// <summary>
        /// Single SurveyInfo
        /// </summary>
        [DataMember]
        public List<SurveyAnswerDTO> SurveyResponseList;

        /// <summary>
        /// Total number of pages for query
        /// </summary>
        [DataMember]
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Number of Records per page
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int NumberOfResponses { get; set; }
        [DataMember]
        public SurveyInfoDTO SurveyInfo;

    }
}
