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
    public class FormsInfoResponse
    {
           /// <summary>
        /// Default Constructor for SurveyInfoResponse.
        /// </summary>
        public FormsInfoResponse() 
        {
            this.FormInfoList = new List<FormInfoDTO>(); 
        }
        /// <summary>
        /// Single SurveyInfo
        /// </summary>
        [DataMember]
        public List<FormInfoDTO> FormInfoList;
    }
}
