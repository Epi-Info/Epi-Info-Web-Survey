using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDynamicForms;
using System.Globalization;
using System.Threading;
namespace Epi.Web.MVC.Models
    {
    public class SurveyModel
        { 
        private Form _Form;
        private List<RelateModel> _RelateModel;
        private FormResponseInfoModel _FormResponseInfoModel;
        private int _RequestedViewId;
        private string _RelatedButtonWasClicked;
        private string _CurrentCultureDateFormat;
        public SurveyModel(){

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
            DateFormat = DateFormat.Remove(DateFormat.IndexOf("y"), 2);
            _CurrentCultureDateFormat = DateFormat;
        }
        public string CurrentCultureDateFormat
        {
            get { return _CurrentCultureDateFormat; }
            set { _CurrentCultureDateFormat = value; }
        }
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