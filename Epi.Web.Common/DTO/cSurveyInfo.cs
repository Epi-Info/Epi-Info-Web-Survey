using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.DTO
{
    class cSurveyInfo
    {
        private string _SurveyId;
        private string _SurveyNumber;
        private string _SurveyName;
        private string _SurveyDescription;
        private string _XML;
        private bool _IsSuccess;


        public string SurveyId
        {
            get { return _SurveyId; }
            set { _SurveyId = value; }
        }
        public string SurveyNumber
        {
            get { return _SurveyNumber; }
            set { _SurveyNumber = value; }
        }

        public string SurveyName
        {
            get { return _SurveyName; }
            set { _SurveyName = value; }
        }

        public string SurveyDescription
        {
            get { return _SurveyDescription; }
            set { _SurveyDescription = value; }
        }
        public string XML
        {
            get { return _XML; }
            set { _XML = value; }
        }

        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

    }
}
