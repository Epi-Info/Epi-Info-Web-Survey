using System;
using System.Runtime.Serialization;

namespace Epi.Web.Common.Criteria
{
    /// <summary>
    /// Holds criteria for SurveyResponse queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyResponseCriteria : Criteria
    {
        /// <summary>
        /// Unique SurveyResponse identifier.
        /// </summary>
        [DataMember]
        public string ResposneId { get; set; }

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
