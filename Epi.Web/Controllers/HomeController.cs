using System;
using System.Web.Mvc;
using Epi.Web.MVC.Models;

namespace Epi.Web.MVC.Controllers
{
    public class HomeController : Controller
    {

        //declare  SurveyFacade
        private Epi.Web.MVC.Facade.SurveyFacade _surveyFacade;
        
       
        /// <summary>
        /// injecting surveyFacade to the constructor 
        /// </summary>
        /// <param name="surveyFacade"></param>
        public HomeController(Epi.Web.MVC.Facade.SurveyFacade surveyFacade)
        {
            _surveyFacade = surveyFacade;
        }


        public ActionResult Splash()
        {
            return View("Splash");
        }
        /// <summary>
        /// Accept SurveyId as parameter, 
        /// 
        /// Get the SurveyInfoResponse by GetSurveyInfo call and convert it to a SurveyInfoModel object
        /// pump the SurveyInfoModel to the "SurveyIntroduction" view
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(string surveyid)
        {
            /*TO DO: Later we will pass an empty SurveyInfoDTO object and validate it here something like
             if surveyInfodto.SurveyName== null then go to the exception page*/
            try
            {

                //_surveyInfoRequest.Criteria.SurveyId = surveyid;
                //SurveyInfoResponse surveyInfoResponse = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest);
                //SurveyInfoModel surveyModel = Mapper.ToSurveyInfoModel(surveyInfoResponse.SurveyInfo);
                SurveyInfoModel surveyInfoModel = _surveyFacade.GetSurveyInfoModel(surveyid);
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, surveyInfoModel);
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }

        /// <summary>
        /// redirecting to Survey controller to action method Index
        /// </summary>
        /// <param name="surveyModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Epi.Web.MVC.Models.SurveyInfoModel surveyModel)
        {
            //string page;
           // return RedirectToAction(Epi.Web.Models.Constants.Constant.INDEX, Epi.Web.Models.Constants.Constant.SURVEY_CONTROLLER, new {id="page" });
            return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER);
        }


    }
}
