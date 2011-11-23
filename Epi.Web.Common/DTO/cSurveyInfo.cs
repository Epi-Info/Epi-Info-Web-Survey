using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.DTO
{
    [DataContract]
    public class cSurveyInfo
    {
        private string _SurveyId;
        private string _SurveyNumber;
        private string _SurveyName;
        private string _IntroductionText;
        private string _DepartmentName;
        private string _OrganizationName;
        private string _XML;
        private bool _IsSuccess;


        [DataMember]
        public string SurveyId
        {
            get { return _SurveyId; }
            set { _SurveyId = value; }
        }
        [DataMember]
        public string SurveyNumber
        {
            get { return _SurveyNumber; }
            set { _SurveyNumber = value; }
        }

        [DataMember]
        public string SurveyName
        {
            get { return _SurveyName; }
            set { _SurveyName = value; }
        }

        [DataMember]
        public string OrganizationName
        {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
        }

        [DataMember]
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set { _DepartmentName = value; }
        }


        [DataMember]
        public string IntroductionText
        {
            get { return _IntroductionText; }
            set { _IntroductionText = value; }
        }
        [DataMember]
        public string XML
        {
            get { return _XML; }
            set { _XML = value; }
        }
        [DataMember]
        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

    }
}
