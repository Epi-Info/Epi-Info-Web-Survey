using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.BusinessObject
    {
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormsHierarchyBO
        {
        private string _FormId;
        private List<SurveyResponseBO> _ResponseIds;
        private bool _IsRoot;
        private int _ViewId;
        private bool _IsSqlProject;
        private SurveyInfoBO _SurveyInfo;
        [DataMember]
        public string FormId
            {
            get { return _FormId; }
            set { _FormId = value; }
            }
        [DataMember]
        public List<SurveyResponseBO> ResponseIds
            {
            get { return _ResponseIds; }
            set { _ResponseIds = value; }
            }
        [DataMember]
        public bool IsRoot
            {
            get { return _IsRoot; }
            set { _IsRoot = value; }
            }
        [DataMember]
        public int ViewId
            {
            get { return _ViewId; }
            set { _ViewId = value; }
            }
        [DataMember]
        public bool IsSqlProject
        {
            get { return _IsSqlProject; }
            set { _IsSqlProject = value; }
        }

        [DataMember]
        public SurveyInfoBO SurveyInfo
        {
            get { return _SurveyInfo; }
            set { _SurveyInfo = value; }
        
        }
        }
    }
