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
        }

        /// <summary>
        /// Unique SurveyInfo identifier.
        /// </summary>
        [DataMember]
        public List<string> SurveyIdList { get; set; }

        /// <summary>
        /// Flag as to whether to include order statistics.
        /// </summary>
        [DataMember]
        public bool IncludeOrderStatistics { get; set; }
    }
}
