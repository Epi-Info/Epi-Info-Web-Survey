using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.BusinessObject
{
    public class SurveyInfoBO
    {
        private string _SurveyId;
        private string _SurveyNumber;
        private int  _SurveyType;
        private string _SurveyName;
        private string _IntroductionText;
        private string _DepartmentName;
        private string _OrganizationName;
        private string _XML;
        private string _ExitText;
        private long _TemplateXMLSize;
        private Guid _UserPublishKey;
        private Guid _OrganizationKey;
        private DateTime _ClosingDate;
        private DateTime _DateCreated;
        private string _StatusText;
        private bool _IsDraftMode;
        private DateTime _StartDate;
        private string _ParentId;
        private int _ViewId;
        private int _OwnerId;
        private bool _IsSqlProject;
        private string _DBConnectionString;
        public string StatusText
        { 
            get { return _StatusText; }
            set { _StatusText = value; } 
        }
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
        public int SurveyType
        {
            get { return _SurveyType; }
            set { _SurveyType = value; }
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


        public DateTime ClosingDate
        {
            get { return _ClosingDate; }
            set { _ClosingDate = value; }
        }

        public Guid UserPublishKey 
        {
            get { return _UserPublishKey; }
            set { _UserPublishKey = value; }
        }

        public Guid OrganizationKey
        {
            get { return _OrganizationKey; }
            set { _OrganizationKey = value; }
        }

        public long TemplateXMLSize
        {
            get { return _TemplateXMLSize; }
            set { _TemplateXMLSize = value; }
        }

        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set{ _DateCreated = value;}
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
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        [DataMember]
        public string ParentId
            {
            get { return _ParentId; }
            set { _ParentId = value; }
            }

        [DataMember]
        public int ViewId
            {
            get { return _ViewId; }
            set { _ViewId = value; }
            }
        [DataMember]
        public int OwnerId
            {
            get { return _OwnerId; }
            set { _OwnerId = value; }
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
    }
}
