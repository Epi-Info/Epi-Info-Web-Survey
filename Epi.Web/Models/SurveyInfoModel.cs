using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Threading;

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
        private bool _IsDraftMode;
        private int _SurveyType;
        private string _IsDraftModeStyleClass;
        private Guid _UserPublishKey;
        private string _PassCode;
        private string _ResponseId;
        private string _CurrentCultureDateFormat;

        //public SurveyInfoModel()
        //{
        //    //CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            //string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
            //DateFormat = DateFormat.Remove(DateFormat.IndexOf("y"), 2);
            //_CurrentCultureDateFormat = DateFormat;
        //}
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
        
        
        public bool IsDraftMode
        {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
        }
        
        public string IsDraftModeStyleClass
        {
            get { return _IsDraftModeStyleClass; }
            set { _IsDraftModeStyleClass = value; }
        }

  
        public string PassCode
            {
            get { return _PassCode; }
            set { _PassCode = value; }
            }
      
        public string ResponseId
            {
            get { return _ResponseId; }
            set { _ResponseId = value; }
            }
    }
}