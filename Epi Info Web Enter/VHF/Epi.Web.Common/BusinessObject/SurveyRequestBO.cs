using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.BusinessObject
{

    public class SurveyRequestBO  
    {
        DateTime closingDate;
        string surveyName;
        int surveyType;
        string surveyNumber;
        string organizationName;
        string departmentName;
        string introductionText;
        string templateXML;
        bool isSingleResponse;


        
        public DateTime ClosingDate { get { return this.closingDate; } set { this.closingDate = value; } }

        
        public bool IsSingleResponse { get { return this.isSingleResponse; } set { this.isSingleResponse = value; } }

        
        public string SurveyName { get { return this.surveyName; } set { this.surveyName = value; } }

        
        public string SurveyNumber { get { return this.surveyNumber; } set { this.surveyNumber = value; } }

        
        public string OrganizationName { get { return this.organizationName; } set { this.organizationName = value; } }

        
        public string DepartmentName { get { return this.departmentName; } set { this.departmentName = value; } }

        
        public string IntroductionText { get { return this.introductionText; } set { this.introductionText = value; } }

        
        public string TemplateXML { get { return this.templateXML; } set { this.templateXML = value; } }

        public int SurveyType { get { return this.surveyType; } set { this.surveyType = value; } }

    }
}
