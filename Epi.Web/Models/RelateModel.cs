using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class RelateModel
        {
        private string _FormId;
        private List<SurveyAnswerModel> _ResponseIds;
        private bool _IsRoot;
        private int _ViewId;
        private bool _IsSqlProject;
        public string FormId
            {
            get { return _FormId; }
            set { _FormId = value; }
            }

        public List<SurveyAnswerModel> ResponseIds
            {
            get { return _ResponseIds; }
            set { _ResponseIds = value; }
            }
       
        public bool IsRoot
            {
            get { return _IsRoot; }
            set { _IsRoot = value; }
            }
       
        public int ViewId
            {
            get { return _ViewId; }
            set { _ViewId = value; }
            }
        public bool IsSqlProject
        {
            get { return _IsSqlProject; }
            set { _IsSqlProject = value; }
        }
        }
    }