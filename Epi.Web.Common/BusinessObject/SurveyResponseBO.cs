using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.BusinessObject
{
    public class SurveyResponseBO
    {

        public SurveyResponseBO()
        {
            this.DateUpdated = DateTime.Now;
            this.Status = 1;
        }

        public string ResponseId{ get; set; }
        public Guid UserPublishKey { get; set; }
        public string SurveyId { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int Status { get; set; }
        public string XML { get; set; }
        public string Json { get; set; }
        public long TemplateXMLSize { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDraftMode { get; set; }
        public int ViewId { get;   set; }
        public string ParentRecordId { get;   set; }
        public string RelateParentId { get;   set; }
        public List<SurveyResponseBO> ResponseHierarchyIds { get;   set; }

        public int RecrodSourceId { get; set; }
        public string PassCode { get; set; }
    }
}
