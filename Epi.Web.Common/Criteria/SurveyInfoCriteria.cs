using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Epi.Web.Common.Criteria
{
    /// <summary>
    /// Holds criteria for SurveyInfo queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyInfoCriteria : Criteria
    {
        public SurveyInfoCriteria()
        {
            this.SurveyIdList = new List<string>();
            this.ClosingDate = DateTime.MinValue;
            this.SurveyType = -1;
        }

        /// <summary>
        /// Unique SurveyInfo identifier.
        /// </summary>
        [DataMember]
        public List<string> SurveyIdList { get; set; }

        /// <summary>
        /// Last Day survey can be completed
        /// </summary>
        [DataMember]
        public DateTime ClosingDate { get; set; }

        /// <summary>
        /// Survey Type
        /// </summary>
        [DataMember]
        public int SurveyType { get; set; }


        /// <summary>
        /// User Publish Key for Manager Service
        /// </summary>
        [DataMember]
        public Guid UserPublishKey { get; set; }


        /// <summary>
        /// Flag as to whether to include order statistics.
        /// </summary>
        [DataMember]
        public bool IncludeOrderStatistics { get; set; }
    }
}
