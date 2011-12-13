using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.BusinessObject
{
    public class SurveyResponseBO
    {
        public string ResponseId{ get; set; }
        public string SurveyId { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public DateTime DateCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public string XML { get; set; }
    }
}
