using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Criteria
{
    /// <summary>
    /// Holds criteria for SurveyResponse queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyAnswerCriteria : Criteria
    {


        public SurveyAnswerCriteria()
        {
            this.SurveyAnswerIdList = new List<string>();
        }

        /// <summary>
        /// Unique SurveyResponse identifier.
        /// </summary>
        [DataMember]
        public List<string> SurveyAnswerIdList { get; set; }

        /// <summary>
        /// SurveyInfo identifier.
        /// </summary>
        [DataMember]
        public string SurveyId { get; set; }


        /// <summary>
        /// IsCompleted predicate.
        /// </summary>
        [DataMember]
        public bool IsCompleted { get; set; }


        /// <summary>
        /// IsCompleted date.
        /// </summary>
        [DataMember]
        public DateTime DateCompleted { get; set; }

        /// <summary>
        /// Flag as to whether to include order statistics.
        /// </summary>
        [DataMember]
        public bool IncludeOrderStatistics { get; set; }
    }
}
