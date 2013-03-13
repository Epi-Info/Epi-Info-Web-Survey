using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System;
using System.Web.Security;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
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
                
               // SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(surveyId);
                SurveyInfoModel surveyInfoModel = GetSurveyInfo(surveyId);
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
                bool IsMobileDevice = false;

                IsMobileDevice = this.Request.Browser.IsMobileDevice;
                if (IsMobileDevice == false)
                {
                    IsMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
                }

                FormsAuthentication.SetAuthCookie("BeginSurvey", false);
                
                Guid responseId = Guid.NewGuid();


                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(surveyId, responseId.ToString());




                //SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyAnswer.SurveyId);
                SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId);

                XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);

                MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, 1, SurveyAnswer, IsMobileDevice);

                var _FieldsTypeIDs = from _FieldTypeID in
                                         xdoc.Descendants("Field")
                                         select _FieldTypeID;


                // Adding Required fileds from MetaData to the list
                foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                    if (bool.Parse(_FieldTypeID.Attribute("IsRequired").Value))
                    {
                        if (!form.RequiredFieldsList.Contains(_FieldTypeID.Attribute("Name").Value))
                        {
                            if (form.RequiredFieldsList != "")
                            {
                                form.RequiredFieldsList = form.RequiredFieldsList + "," + _FieldTypeID.Attribute("Name").Value.ToLower();
                            }
                            else
                            {
                                form.RequiredFieldsList = _FieldTypeID.Attribute("Name").Value.ToLower();
                            }
                        }
                    }

                }
                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, form, SurveyAnswer, false, false, 1);

                return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseId = responseId, PageNumber = 1 });
            }
            catch (Exception ex)
            {
              
                            Epi.Web.Utility.EmailMessage.SendLogMessage(  ex, this.HttpContext);
                 
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }

        }
        public SurveyInfoModel GetSurveyInfo(string SurveyId)
        {

            var CacheObj = HttpRuntime.Cache.Get(SurveyId);
            if (CacheObj == null)
            {

                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
                HttpRuntime.Cache.Insert(SurveyId, surveyInfoModel, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(1));

                return surveyInfoModel;
            }
            else
            {
                return (SurveyInfoModel)CacheObj;

            }


        }
    }
}
