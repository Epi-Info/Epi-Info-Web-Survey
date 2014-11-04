using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.Criteria
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
            this.StatusId = -1;
            this.DateCompleted = DateTime.MinValue;
        }


        /// <summary>
        /// Which page to retrieve
        /// </summary>
        [DataMember]
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of Records per page
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// Number of Records per page
        /// </summary>
        [DataMember]
        public bool ReturnSizeInfoOnly { get; set; }


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
        /// Complete / Inprogress indicator
        /// </summary>
        [DataMember]
        public int StatusId { get; set; }


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

        
       [DataMember]
        public Guid UserPublishKey { get; set; }

       [DataMember]
       public Guid OrganizationKey { get; set; }
         
       [DataMember]
       public bool IsMobile { get; set; }
       [DataMember]
       public int UserId { get; set; }
       [DataMember]
       public bool IsEditMode { get; set; }

       [DataMember]
       public string SortOrder { get; set; }

       [DataMember]
       public string Sortfield { get; set; }

       [DataMember]
       public bool GetAllColumns { get; set; }

       [DataMember]
       public string SearchCriteria { get; set; }

        [DataMember]
       public bool IsSqlProject { get; set; }
        [DataMember]
        public bool IsDeleteMode { get; set; }
    }
}
