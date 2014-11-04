using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDynamicForms;
namespace Epi.Web.MVC.Models
    {
    public class SurveyModel
        { 
        private Form _Form;
        private List<RelateModel> _RelateModel;
        private FormResponseInfoModel _FormResponseInfoModel;
        private int _RequestedViewId;
        private string _RelatedButtonWasClicked;
        public Form Form
            {
            get { return _Form; }
            set { _Form = value; }
            }
        public List<RelateModel> RelateModel
            {
            get { return _RelateModel; }
            set { _RelateModel = value; }
            }
        public FormResponseInfoModel FormResponseInfoModel
            {
            get { return _FormResponseInfoModel; }
            set { _FormResponseInfoModel = value; }
            }
        public int RequestedViewId
            {
            get { return _RequestedViewId; }
            set { _RequestedViewId = value; }
            }
        public string RelatedButtonWasClicked
            {
            get { return _RelatedButtonWasClicked; }
            set { _RelatedButtonWasClicked = value; }
            }
        }
    }