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
            //string surveyid = Request.QueryString["param"];
            //SurveyManager sm= new SurveyManager();
            var s = _iSurveyManager.GetSurveyInfoById(param);
            return View(s);
            
        }

    }
}
