using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    
    /// <summary>
    /// The Survey Model that will be pumped to view
    /// </summary>
    public class SurveyInfoModel
    {
        private string _SurveyId;
        private string _SurveyNumber;
        private string _SurveyName;
        private string _IntroductionText;
        private string _ExitText;
        private string _DepartmentName;
        private string _OrganizationName;
        private string _XML;
        private bool _IsSuccess;
        private DateTime _ClosingDate;
        private DateTime _StartDate;
        private  string _LogoURL;
        private string _LogoLocation;
        private bool _TestMode;
        private int _SurveyType;

        private Guid _UserPublishKey;

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

        
        public string OrganizationName
        {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
        }

        
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set { _DepartmentName = value; }
        }


        
        public string IntroductionText
        {
            get { return _IntroductionText; }
            set { _IntroductionText = value; }
        }

        public string ExitText
        {
            get { return _ExitText; }
            set { _ExitText = value; }
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

        public DateTime ClosingDate
        {
            get { return _ClosingDate; }
            set { _ClosingDate = value; }
        }

        public int SurveyType
        {
            get { return _SurveyType; }
            set { _SurveyType = value; }
        }

        public Guid UserPublishKey
        { 
            get { return _UserPublishKey; } 
            set { this._UserPublishKey = value; } 
        }
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }
        
        public string LogoURL
        {
            get { return _LogoURL; }
            set { _LogoURL = value; }
        }
        public string LogoLocation
        {
            get { return _LogoLocation; }
            set { _LogoLocation = value; }
        }
        public bool TestMode
        {
            get { return _TestMode; }
            set { _TestMode = value; }
        }
    }
}