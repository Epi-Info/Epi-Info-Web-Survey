using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System;
namespace Epi.Web.MVC.Controllers
{
    public class FinalController : Controller
    {


        //declare SurveyTransactionObject object
        private ISurveyFacade _isurveyFacade;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public FinalController(ISurveyFacade isurveyFacade)
        {

            _isurveyFacade = isurveyFacade;
        }
        [HttpGet]
        public ActionResult Index(string surveyId, string final)
        {

            //if (!string.IsNullOrEmpty(final))
            //{
            try
            {
                
                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(surveyId);
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, surveyInfoModel);

                //}
                //return null;
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string surveyId, SurveyAnswerModel surveyAnswerModel )
        {


            try
            {
               Guid responseId = Guid.NewGuid(); 

              
               _isurveyFacade.CreateSurveyAnswer(surveyId, responseId.ToString());
               return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseId = responseId, PageNumber = 1 });
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }

        }

    }
}
