using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.DTO;

namespace Epi.Web.WCF.SurveyService.Messages
{
    /// <summary>
    /// Represents a SurveyInfo request message from client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyInfoRequest : RequestBase
    {
        /// <summary>
        /// Selection criteria and sort order
        /// </summary>
        [DataMember]
        public SurveyInfoCriteria Criteria;

        /// <summary>
        /// SurveyInfo object.
        /// </summary>
        [DataMember]
        public SurveyInfoDTO SurveyInfo;
    }
}
