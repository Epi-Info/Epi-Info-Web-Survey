using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.Message
{
    [DataContract]
    public class SurveyRequest  
    {
        DateTime closingDate;
        string surveyName;
        string surveyNumber;
        string organizationName;
        string departmentName;
        string introductionText;
        string templateXML;
        bool isSingleResponse;


        [DataMember]
        public DateTime ClosingDate { get { return this.closingDate; } set { this.closingDate = value; } }

        [DataMember]
        public bool IsSingleResponse { get { return this.isSingleResponse; } set { this.isSingleResponse = value; } }

        [DataMember]
        public string SurveyName { get { return this.surveyName; } set { this.surveyName = value; } }

        [DataMember]
        public string SurveyNumber { get { return this.surveyNumber; } set { this.surveyNumber = value; } }

        [DataMember]
        public string OrganizationName { get { return this.organizationName; } set { this.organizationName = value; } }

        [DataMember]
        public string DepartmentName { get { return this.departmentName; } set { this.departmentName = value; } }

        [DataMember]
        public string IntroductionText { get { return this.introductionText; } set { this.introductionText = value; } }

        [DataMember]
        public string TemplateXML { get { return this.templateXML; } set { this.templateXML = value; } }

    }
}
