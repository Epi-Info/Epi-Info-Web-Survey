using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Enter.Common.BusinessObject
{
    public class SurveyResponseBO :ICloneable
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
        public long TemplateXMLSize { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDraftMode { get; set; }
        public bool IsLocked { get; set; }
        public string ParentRecordId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string ParentId { get; set; }
        public string RelateParentId { get; set; }
        public bool IsNewRecord { get; set; }
        public List<SurveyResponseBO> ResponseHierarchyIds { get; set; }
        public int ViewId { get; set; }

        public Dictionary<string, string> SqlData { get; set; }
        public int RecrodSourceId { get; set; }
        public object Clone() 
            {

              return this.MemberwiseClone();
            
            }
    }
}
