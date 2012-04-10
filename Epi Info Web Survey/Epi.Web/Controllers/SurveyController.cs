using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Epi.Core.EnterInterpreter;
using System.Web.Security;
namespace Epi.Web.MVC.Controllers
{
        [Authorize]
    public class SurveyController : Controller
    {


        

        //declare SurveyTransactionObject object
        private ISurveyFacade _isurveyFacade;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        
        public SurveyController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        
        
                
       
       /// <summary>
       /// create the new resposeid and put it in temp data. create the form object. create the first survey response
       /// </summary>
       /// <param name="surveyId"></param>
       /// <returns></returns>
 
        [HttpGet]

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] 
        public ActionResult Index(string responseId, int PageNumber = 0)
        {


            try
            {
                 
               
                Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(responseId);
                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(surveyAnswerDTO.SurveyId);
                PreValidationResultEnum ValidationTest = PreValidateResponse(Mapper.ToSurveyAnswerModel(surveyAnswerDTO), surveyInfoModel);
                if (PageNumber == 0)
                {
                    PageNumber = GetSurveyPageNumber(surveyAnswerDTO.XML.ToString());

                }
                else { 
                
                
                }
                

                switch(ValidationTest)
                {
                    case PreValidationResultEnum.SurveyIsPastClosingDate:
                        return View("SurveyClosedError");
                    case PreValidationResultEnum.SurveyIsAlreadyCompleted:
                        return View("IsSubmitedError");
                    case PreValidationResultEnum.Success:
                    default:
                        var form = _isurveyFacade.GetSurveyFormData(surveyAnswerDTO.SurveyId, PageNumber, surveyAnswerDTO);
                        // if redirect then perform server validation before displaying
                        if (TempData.ContainsKey("isredirect") && !string.IsNullOrWhiteSpace(TempData["isredirect"].ToString()))
                        {
                            form.Validate();
                        }
                        this.SetCurrentPage(surveyAnswerDTO, PageNumber);
                        return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                }
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
            //}
            //return null;
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [ValidateAntiForgeryToken]
        //public ActionResult Index(SurveyInfoModel surveyInfoModel, string Submitbutton, string Savebutton, string ContinueButton, string PreviousButton, int PageNumber = 1)
        public ActionResult Index(SurveyAnswerModel surveyAnswerModel, string Submitbutton, string Savebutton, string ContinueButton, string PreviousButton, int PageNumber = 0)
        {
            
            string responseId = surveyAnswerModel.ResponseId;
            try
            {

                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyAnswer.SurveyId);

                PreValidationResultEnum ValidationTest = PreValidateResponse(Mapper.ToSurveyAnswerModel(SurveyAnswer), surveyInfoModel);
             
                switch (ValidationTest)
                {
                    case PreValidationResultEnum.SurveyIsPastClosingDate:
                        return View("SurveyClosedError");
                    case PreValidationResultEnum.SurveyIsAlreadyCompleted:
                        return View("IsSubmitedError");
                    case PreValidationResultEnum.Success:
                    default:
                        MvcDynamicForms.Form form;
                        int CurrentPageNum = GetSurveyPageNumber(SurveyAnswer.XML.ToString());
                        int ReffererPageNum;
                        string url = this.Request.UrlReferrer.ToString();
                        int LastIndex = url.LastIndexOf("/");
                        string StringNumber = null;
                        if (url.Length - LastIndex + 1 <= url.Length)
                        {
                            StringNumber = url.Substring(LastIndex, url.Length - LastIndex);
                            StringNumber = StringNumber.Trim('/');
                        }

                        if (int.TryParse(StringNumber, out ReffererPageNum))
                        {
                            if (ReffererPageNum != CurrentPageNum)
                            {
                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, ReffererPageNum, SurveyAnswer);
                            }
                            else
                            {
                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer);
                            }
                            
                            UpdateModel(form);
                        }
                        else
                        {
                            //get the survey form
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer);
                            if (ReffererPageNum == CurrentPageNum)
                            {
                                UpdateModel(form);
                            }
                        }
                       
                        bool IsSubmited = false;
                        bool IsSaved = false;

                            form.HiddenFieldsList = this.Request.Form["HiddenFieldsList"].ToString();
                         
                            form.HighlightedFieldsList = this.Request.Form["HighlightedFieldsList"].ToString();
                         
                            form.DisabledFieldsList = this.Request.Form["DisabledFieldsList"].ToString();

                            form.AssignList = this.Request.Form["AssignList"].ToString();
                         
                        _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);


                        if (!string.IsNullOrEmpty(this.Request.Form["is_save_action"]) && this.Request.Form["is_save_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {

                            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()) == 0 ? 1 : GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer);
                            //Update the model
                            UpdateModel(form);

                            form.HiddenFieldsList = this.Request.Form["HiddenFieldsList"].ToString();
                            
                            form.HighlightedFieldsList = this.Request.Form["HighlightedFieldsList"].ToString();
                            
                            form.DisabledFieldsList = this.Request.Form["DisabledFieldsList"].ToString();

                            form.AssignList = this.Request.Form["AssignList"].ToString();

                            
                            IsSaved = form.IsSaved = true;
                            form.StatusId = SurveyAnswer.Status;

                            // Pass Code Logic  start 
                            Epi.Web.Common.Message.UserAuthenticationResponse AuthenticationResponse = _isurveyFacade.GetAuthenticationResponse(responseId);

                            string strPassCode = Epi.Web.MVC.Utility.SurveyHelper.GetPassCode();
                            if (string.IsNullOrEmpty(AuthenticationResponse.PassCode))
                            {
                                _isurveyFacade.UpdatePassCode(responseId, strPassCode);
                            }
                            if (AuthenticationResponse.PassCode == null)
                            {
                                form.PassCode = strPassCode;

                            }
                            else
                            {
                                form.PassCode = AuthenticationResponse.PassCode;
                            }
                            // Pass Code Logic  end 
                             _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);
                                
                                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);

                        }
                        else if (form.Validate())
                        {
                            if (!string.IsNullOrEmpty(Submitbutton))
                            {



                                // execute after event
                                EnterRule FunctionObject_A = (EnterRule)form.FormCheckCodeObj.GetCommand("level=record&event=after&identifier=");
                                if (FunctionObject_A != null && !FunctionObject_A.IsNull())
                                {
                                    try
                                    {
                                        FunctionObject_A.Execute();
                                    }
                                    catch (Exception ex)
                                    {
                                        // do nothing so that processing can 
                                        // continue
                                    }
                                }
                                Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();
                                ContextDetailList = Epi.Web.MVC.Utility.SurveyHelper.GetContextDetailList(FunctionObject_A);

                                SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                                // ReValidate All Pages
                                for (int i = 1; i < form.NumberOfPages+1; i++)
                                {
                                    form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, i, SurveyAnswer);
                                    if (!form.Validate())
                                    {
                                        TempData["isredirect"] = "true";
                                        //  return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                                        _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, i);
                                        return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = responseId, PageNumber = i.ToString() });
                                    }

                                    ///////////////////////////// Execute - Record After - start//////////////////////
                                    else
                                    {
                                        form = Epi.Web.MVC.Utility.SurveyHelper.UpdateControlsValuesFromContext(form, ContextDetailList);
                                        _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, false, false, i);
                                    }
                                    ///////////////////////////// Execute - Record After - End//////////////////////
                                }

                               

                                
                                IsSubmited = true;//survey has been submited this will change the survey status to 3 - Completed
                                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);
                                FormsAuthentication.SignOut();
                                return RedirectToAction("Index", "Final", new { surveyId = surveyInfoModel.SurveyId });
                            }
                            else
                            {
                                //This is a Navigation to a url
                                 form.HiddenFieldsList = this.Request.Form["HiddenFieldsList"].ToString();
                               
                                 form.HighlightedFieldsList = this.Request.Form["HighlightedFieldsList"].ToString();
                                
                                 form.DisabledFieldsList = this.Request.Form["DisabledFieldsList"].ToString();
                                 form.AssignList = this.Request.Form["AssignList"].ToString();
                                 

                                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);

                                SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber, SurveyAnswer);
                                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                            }

                        }
                        else
                        {
                            //Invalid Data - stay on same page
                            CurrentPageNum = GetSurveyPageNumber(SurveyAnswer.XML.ToString()) ;
                           


                            if (CurrentPageNum != PageNumber) // failed validation and navigating to different page// must keep url the same 
                            {
                                TempData["isredirect"] = "true";
                                return RedirectToAction("Index", "Survey", new { RequestId = form.ResponseId, PageNumber = CurrentPageNum });
                                 
                            }
                            else
                            {
                                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                            }
                        }

                }

               
            }

            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
            
        }

 
        private int GetCurrentPage()
        {
            int CurrentPage = 1;
            
            string PageNum = this.Request.UrlReferrer.ToString().Substring(this.Request.UrlReferrer.ToString().LastIndexOf('/')+1);

            int.TryParse(PageNum, out CurrentPage);
            return CurrentPage;
        }




        private void SetCurrentPage(Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, int PageNumber)
        {

            XDocument Xdoc = XDocument.Parse(surveyAnswerDTO.XML);
            if (PageNumber != 0)
            {
                Xdoc.Root.Attribute("LastPageVisited").Value = PageNumber.ToString();
            }

            surveyAnswerDTO.XML = Xdoc.ToString();
            
            Epi.Web.MVC.Repositories.SurveyAnswerRepository iSurveyAnswerRepository = new Repositories.SurveyAnswerRepository(new DataServiceClient.DataServiceClient());
            Epi.Web.Common.Message.SurveyAnswerRequest  sar = new Common.Message.SurveyAnswerRequest();
            sar.Action = "Update";
            sar.SurveyAnswerList.Add(surveyAnswerDTO);

            iSurveyAnswerRepository.SaveSurveyAnswer(sar);

        }



        private Epi.Web.Common.DTO.SurveyAnswerDTO GetSurveyAnswer(string responseId)
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            //responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
            result =  _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

            return result;

        }



        private enum PreValidationResultEnum
        {
            Success,
            SurveyIsPastClosingDate,
            SurveyIsAlreadyCompleted
        }


        private PreValidationResultEnum PreValidateResponse(SurveyAnswerModel SurveyAnswer, SurveyInfoModel SurveyInfo)
        {
            PreValidationResultEnum result = PreValidationResultEnum.Success;

            if (DateTime.Now > SurveyInfo.ClosingDate)
            {
                return PreValidationResultEnum.SurveyIsPastClosingDate;
            }


            if (SurveyAnswer.Status == 3)
            {
                return PreValidationResultEnum.SurveyIsAlreadyCompleted;
            }

            return result;
        }

        private int GetSurveyPageNumber(string ResponseXml)
        {

            XDocument xdoc = XDocument.Parse(ResponseXml);
            
            int PageNumber = 0;

            if (  (string)xdoc.Root.Attribute("LastPageVisited") != null)
            {
                PageNumber= int.Parse(xdoc.Root.Attribute("LastPageVisited").Value);
            }
            else
            {
                PageNumber =1;
            }

            return PageNumber;

        }

        public static string GetResponseFormState(string Xml, string ListName)
        {

            string List = "";

            if (!string.IsNullOrEmpty(Xml))
            {
                XDocument xdoc = XDocument.Parse(Xml);

                if (!string.IsNullOrEmpty(xdoc.Root.Attribute(ListName).Value.ToString()))
                {
                     List = xdoc.Root.Attribute(ListName).Value;
                   
                    
                }

            }

            return List;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateResponseXml(string Name, string Value, string responseId)
        {
            try
            {
                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyAnswer.SurveyId);

                int NumberOfPages = Epi.Web.MVC.Utility.SurveyHelper.GetNumberOfPags(SurveyAnswer.XML);
                    
                     
                for (int i = NumberOfPages; i > 0; i--)
                {
                    SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswer.ResponseId).SurveyResponseList[0];

                    MvcDynamicForms.Form formRs = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, i, SurveyAnswer);

                     formRs = Epi.Web.MVC.Utility.SurveyHelper.UpdateControlsValues(formRs, Name,Value);

                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, formRs, SurveyAnswer, false, false, i);

                }
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        } 


    }
}
