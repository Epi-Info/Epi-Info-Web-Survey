using System;
using System.Web.Mvc;
using Epi.Web.MVC.Models;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using Epi.Core.EnterInterpreter;
using System.Collections.Generic;
using System.Web.Security;
 

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
               // FormsAuthentication.SignOut();
               

              //  TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = "";
                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(surveyid);
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
            try
            {
                FormsAuthentication.SetAuthCookie("BeginSurvey", false);
                //put the ResponseId in Temp data for later use
                //TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();

                //create the responseid
                Guid ResponseID = Guid.NewGuid();
                TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();
                // create the first survey response
                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(surveyModel.SurveyId, ResponseID.ToString());

                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyAnswer.SurveyId);
 
                MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, 1, SurveyAnswer);

                XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);
                XDocument xdocResponse = XDocument.Parse(SurveyAnswer.XML);

                XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
                string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();

                form.FormCheckCodeObj = form.GetCheckCodeObj(xdoc, xdocResponse, checkcode);

                EnterRule FunctionObject_B = (EnterRule)form.FormCheckCodeObj.GetCommand("level=record&event=before&identifier=");
               



                if (FunctionObject_B != null && !FunctionObject_B.IsNull())
                {
                    try
                    {
                        FunctionObject_B.Execute();
                        
                        // field list
                        form.HiddenFieldsList = FunctionObject_B.Context.HiddenFieldList;
                        form.HighlightedFieldsList = FunctionObject_B.Context.HighlightedFieldList;
                        form.DisabledFieldsList = FunctionObject_B.Context.DisabledFieldList;
                        _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, ResponseID.ToString(), form, SurveyAnswer, false, false, 1);
                    }
                    catch (Exception ex)
                    {
                        // do nothing so that processing
                        // can continue
                    }
                }
                ///////////////////////////// Execute - Record Before - start//////////////////////

              
                Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();
                ContextDetailList = Epi.Web.MVC.Utility.SurveyHelper.GetContextDetailList(FunctionObject_B);

              int NumberOfPages = GetNumberOfPages(XDocument.Parse(surveyInfoModel.XML)); 
                for (int i = NumberOfPages; i > 0; i--)
                {
                    SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswer.ResponseId).SurveyResponseList[0];

                    MvcDynamicForms.Form formRs = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, i, SurveyAnswer);

                    formRs = Epi.Web.MVC.Utility.SurveyHelper.UpdateControlsValuesFromContext(formRs, ContextDetailList);
                     
                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, formRs, SurveyAnswer, false, false, i);

                }
                SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswer.ResponseId).SurveyResponseList[0];

                ///////////////////////////// Execute - Record Before - End//////////////////////
                 

                //string page;
                // return RedirectToAction(Epi.Web.Models.Constants.Constant.INDEX, Epi.Web.Models.Constants.Constant.SURVEY_CONTROLLER, new {id="page" });
                return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ResponseID, PageNumber = 1 });
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
        private static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in
                                     Xml.Descendants("View")

                                 select _FieldTypeID;

            return _FieldsTypeIDs.Elements().Count();
        }

        //public  string UpdateResponseFromContext(XDocument RequestXml, Dictionary<string, string> ContextDetailList, string ResponseXml, Epi.Web.MVC.Models.SurveyInfoModel surveyModel, string ResponseID)
       
      

    }
}
