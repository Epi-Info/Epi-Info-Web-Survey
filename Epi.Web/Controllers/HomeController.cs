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
        [HttpGet]
        public ActionResult ListSurvey(string surveyid)
        {
            /*TO DO: Later we will pass an empty SurveyInfoDTO object and validate it here something like
             if surveyInfodto.SurveyName== null then go to the exception page*/
            try
            {
                var s = _iSurveyInfoRepository.GetSurveyInfoById(surveyid).ToSurveyInfoModel();

                return View("SurveyIntroduction", s);
            }
            catch (Exception ex)
            {
                return View("Exception");
            }
        }

        
       /// <summary>
       /// passing the surveyModel, retrieving the SurveyId
       /// get the surveyInfoDTO based on SurveyId
       /// Get the form object based on SurveyInfoDTO and pass to the view
       /// Model Binding
       /// </summary>
       /// <param name="surveyModel"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult ListSurvey(Epi.Web.Models.SurveyInfoModel surveyModel)
        {
            var surveyInfoDTO = _iSurveyInfoRepository.GetSurveyInfoById(surveyModel.SurveyId);

            var form = MvcDynamicForms.Demo.Models.FormProvider.GetForm(surveyInfoDTO, 1);// Requesting the first page of the survey.
            
            return View("Survey", form);

        }
       
    }
}
