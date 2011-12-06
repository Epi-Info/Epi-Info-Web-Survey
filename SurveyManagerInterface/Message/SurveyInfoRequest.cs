using System.Runtime.Serialization;
using Epi.Web.WCF.SurveyService.MessageBase;
using Epi.Web.WCF.SurveyService.Criteria;
using Epi.Web.Common.DTO;

namespace Epi.Web.WCF.SurveyService.Messages
{
    /// <summary>
    /// Represents a customer request message from client.
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
        /// Customer object.
        /// </summary>
        [DataMember]
        public SurveyInfoDTO SurveyInfo;
    }
}
