using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Models;
using Epi.Web.Repositories.Core;

namespace Epi.Web.Controllers
{
    public class HomeController : Controller
    {
        
        
        // Initialize ISurveyInfoRepository 
        private ISurveyInfoRepository _iSurveyInfoRepository;

        /// <summary>
        /// Injectinting ISurveyInfoRepository through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public HomeController(ISurveyInfoRepository iSurveyInfoRepository)
        {
            _iSurveyInfoRepository = iSurveyInfoRepository;
        }

        /// <summary>
        /// Accept SurveyId as parameter, 
        /// Get the SurveyInfoDTO by GetSurveyInfoById call and convert it to a SurveyInfoModel object
        /// pump the SurveyInfoModel to the "SurveyIntroduction" view
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public ActionResult ListSurvey(string surveyid)
        { 
            var s = _iSurveyInfoRepository.GetSurveyInfoById(surveyid).ToSurveyInfoModel();

            return View("SurveyIntroduction", s);

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public ActionResult StartSurvey(string surveyid)
        {
            var surveyInfoDTO = _iSurveyInfoRepository.GetSurveyInfoById(surveyid);

            var form = MvcDynamicForms.Demo.Models.FormProvider.GetForm(surveyInfoDTO, 1);// Requesting the first page of the survey.
            return View("Survey", form);

        }
       
    }
}
