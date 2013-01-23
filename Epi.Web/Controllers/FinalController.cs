using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System;
using System.Web.Security;
using System.Configuration;
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

                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

                string exitText = regex.Replace(surveyInfoModel.ExitText.Replace("  ", " &nbsp;"), "<br />");

                surveyInfoModel.ExitText = MvcHtmlString.Create(exitText).ToString();

                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, surveyInfoModel);

                //}
                //return null;
            }
            catch (Exception ex)
            {
                
                            Epi.Web.Utility.EmailMessage.SendLogMessage( ex, this.HttpContext);
                   
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }



        [HttpPost]

        public ActionResult Index(string surveyId, SurveyAnswerModel surveyAnswerModel)
        {


            try
            {

                FormsAuthentication.SetAuthCookie("BeginSurvey", false);
                
                Guid responseId = Guid.NewGuid();


                _isurveyFacade.CreateSurveyAnswer(surveyId, responseId.ToString());
                return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseId = responseId, PageNumber = 1 });
            }
            catch (Exception ex)
            {
              
                            Epi.Web.Utility.EmailMessage.SendLogMessage(  ex, this.HttpContext);
                 
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }

        }

    }
}
