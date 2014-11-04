using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.DTO;

namespace Epi.Web.Enter.Common.Message
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
        public SurveyInfoResponse() 
        { 
            this.SurveyInfoList = new List<SurveyInfoDTO>(); 
        }

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
