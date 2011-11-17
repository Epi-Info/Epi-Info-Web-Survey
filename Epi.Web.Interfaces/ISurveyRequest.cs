using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Epi.Web.Interfaces
{
    
        public class ISurveyRequest
        {
            public DateTime ClosingDate { get; set; }
            public string SurveyName { get; set; }
            public string OrganizationName { get; set; }
            public string IntroductionText { get; set; }
            public bool IsSingleResponse { get; set;  }
            public string TemplateXML { get; set; }
        }


    
}
