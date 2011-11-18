using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Epi.Web.Interfaces
{
    
        public interface ISurveyRequest
        {
            DateTime ClosingDate { get; set; }
            string SurveyName { get; set; }
            string SurveyNumber { get; set; }
            string OrganizationName { get; set; }
            string DepartmentName { get; set; }
            string IntroductionText { get; set; }
            bool IsSingleResponse { get; set;  }
            string TemplateXML { get; set; }
        }


    
}
