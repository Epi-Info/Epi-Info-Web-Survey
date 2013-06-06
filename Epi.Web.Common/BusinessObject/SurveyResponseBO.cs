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
        public long TemplateXMLSize { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsProductionMode { get; set; }
        
       
    }
}
