using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Models;
using Epi.Web.SurveyManager;

namespace Epi.Web.Controllers
{
    public class HomeController : Controller
    {
        
         //private ISurveyManager _iSurveyManager;
        private ISurveyManager _iSurveyManager;


         public HomeController(ISurveyManager iSurveyManager)
        {
            _iSurveyManager = iSurveyManager;
        }
        public ActionResult ListSurvey(string param)
        {
            string surveyid = Request.Path.Substring(Request.Path.LastIndexOf('/') + 1,Request.Path.Length - Request.Path.LastIndexOf('/') - 1);


            var s = _iSurveyManager.GetSurveyInfoById(surveyid);

            return View("SurveyIntroduction", s);
            
        }


        public ActionResult StartSurvey(string param)
        {
            string surveyid = Request.Path.Substring(Request.Path.LastIndexOf('/') + 1, Request.Path.Length - Request.Path.LastIndexOf('/') - 1);


            var SurveyMetaData = _iSurveyManager.GetSurveyInfoById(surveyid);

            var form = MvcDynamicForms.Demo.Models.FormProvider.GetForm(SurveyMetaData ,1);// Requesting the first page of the survey.
            return View("Survey", form);

        }
    }
}
