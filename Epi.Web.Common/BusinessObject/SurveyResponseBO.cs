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
            this.DateLastUpdated = DateTime.Now;
            this.Status = 1;
        }

        public string ResponseId{ get; set; }
        public string SurveyId { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public DateTime DateCompleted { get; set; }
        public int Status { get; set; }
        public string XML { get; set; }
    }
}
