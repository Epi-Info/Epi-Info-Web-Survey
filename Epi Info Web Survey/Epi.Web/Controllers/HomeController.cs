using System;
using System.Web.Mvc;
using Epi.Web.MVC.Models;

namespace Epi.Web.MVC.Controllers
{
    public class HomeController : Controller
    {

        //declare  SurveyFacade
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
        
       
        /// <summary>
        /// injecting surveyFacade to the constructor 
        /// </summary>
        /// <param name="surveyFacade"></param>
        public HomeController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
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
                //create the responseid
                Guid ResponseID = Guid.NewGuid();

                //put the ResponseId in Temp data for later use
                TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();

                // create the first survey response
                _isurveyFacade.CreateSurveyAnswer(surveyid, ResponseID.ToString());
              //  TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = "";
                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(surveyid);
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, surveyInfoModel);
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }



        public ActionResult Response(string responseid, int StatusId)
        {
            TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = responseid;
             
            try
            {
                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(GetCurrentSurveyAnswer().SurveyId.ToString());
                if (surveyInfoModel.ClosingDate > DateTime.Now && StatusId == GetCurrentSurveyAnswer().Status)
                {
                return RedirectToRoute(new { Controller = "Survey", Action = "Index", surveyId = GetCurrentSurveyAnswer().SurveyId.ToString(), PageNumber = 1 });
                }
                else{
                 return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
                }
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
        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Index(Epi.Web.MVC.Models.SurveyInfoModel surveyModel)
        {
            try
            {
                //string page;
                // return RedirectToAction(Epi.Web.Models.Constants.Constant.INDEX, Epi.Web.Models.Constants.Constant.SURVEY_CONTROLLER, new {id="page" });
                return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER);
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
         }

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetCurrentSurveyAnswer()
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            if (TempData.ContainsKey(Epi.Web.MVC.Constants.Constant.RESPONSE_ID) && TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] != null
                                                                               && !string.IsNullOrEmpty(TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString()))
            {

                string responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();


                //TODO: Now repopulating the TempData (by reassigning to responseId) so it persisits, later we will need to find a better 
                //way to replace it. 
                TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = responseId;
                return _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];



            }

            return result;
        }
    }
}
