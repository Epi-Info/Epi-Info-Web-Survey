using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.DTO
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyInfoDTO 
    {
        private string _SurveyId;
        private string _SurveyNumber;
        private string _SurveyName;
        private int _SurveyType;
        private string _IntroductionText;
        private string _ExitText;
        private string _DepartmentName;
        private string _OrganizationName;
        private string _XML;
        private bool _IsSuccess;
        private DateTime _ClosingDate;
        private Guid _UserPublishKey;
        private Guid _OrganizationKey;
        private bool _IsDraftMode;
        private DateTime _StartDate;
        private bool _IsSqlProject;
        private string _DBConnectionString;
        private int _ViewId;
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
        public int SurveyType
        {
            get { return _SurveyType; }
            set { _SurveyType = value; }
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
        public string ExitText
        {
            get { return _ExitText; }
            set { _ExitText = value; }
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

        [DataMember]
        public DateTime ClosingDate
        {
            get { return _ClosingDate; }
            set { _ClosingDate = value; }
        }
       
        [DataMember]
        public Guid UserPublishKey
        {
            get { return _UserPublishKey; }
            set { _UserPublishKey = value; }
        }

        [DataMember]
        public Guid OrganizationKey
        {
            get { return _OrganizationKey; }
            set { _OrganizationKey = value; }
        }

        [DataMember]
        public bool IsDraftMode
        {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
        }


        [DataMember]
        public DateTime StartDate
        {
            get {return _StartDate;}
            set { _StartDate = value; }
        }

        [DataMember]
        public bool IsSqlProject
        {
            get { return _IsSqlProject; }
            set { _IsSqlProject = value; }
        }
        [DataMember]
        public string DBConnectionString
        {
            get { return _DBConnectionString; }
            set { _DBConnectionString = value; }
        }

        
        [DataMember]
        public int ViewId
        {
            get { return _ViewId; }
            set { _ViewId = value; }
        }

        private string _ParentId;
        [DataMember]
        public string ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; }
        }

        private string _PublishedOrgName;
        [DataMember]
        public string PublishedOrgName
        {
            get { return _PublishedOrgName; }
            set { _PublishedOrgName = value; }
        }
    }
}
