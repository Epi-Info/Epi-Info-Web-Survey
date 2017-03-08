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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.Routing;
using System.Web.WebPages;
using System.Web.Caching;
 
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Epi.Web.Common.Message;
using Epi.Web.Common.DTO;
namespace Epi.Web.MVC.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private ISurveyFacade _isurveyFacade;
        private string RequiredList;
        private IEnumerable<XElement> PageFields;
        public SurveyController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
 
        [HttpGet]
        public ActionResult Index(string responseId, int pageNumber = 0,bool issaved = false)
        {
            try
            {
                // Check if URL has passcode
                string PassCode = this.Request.FilePath.ToString().Substring(this.Request.FilePath.ToString().LastIndexOf('/') + 1);
                if (PassCode.Length == 4)
                {
                     string RedirectURL = this.Request.FilePath.ToString().Replace(PassCode, "1");
                    return Redirect(RedirectURL);
                }
                //
                string calledThereby = "SurveyController.Index[HttpGet]";
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                bool IsAndroid = false;
                ViewBag.Version = version;
                if (this.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    IsAndroid = true;
                }
                bool isMobileDevice = false;
                isMobileDevice = this.Request.Browser.IsMobileDevice;
                
                if (isMobileDevice == false)
                {
                    isMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
                }

                Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(responseId);
                   
                SurveyInfoModel surveyInfoModel = GetSurveyInfo(surveyAnswerDTO.SurveyId);
                PreValidationResultEnum ValidationTest = PreValidateResponse(Mapper.ToSurveyAnswerModel(surveyAnswerDTO), surveyInfoModel);
                
                if(pageNumber == 0)
                {
                    pageNumber = GetSurveyPageNumber(surveyAnswerDTO.XML.ToString());
                }

                switch (ValidationTest)
                {
                    case PreValidationResultEnum.SurveyIsPastClosingDate:
                        return View("SurveyClosedError");

                    case PreValidationResultEnum.SurveyIsAlreadyCompleted:
                        return View("IsSubmitedError");

                    case PreValidationResultEnum.Success:
                    default:
                        var form = _isurveyFacade.GetSurveyFormData(surveyAnswerDTO.SurveyId, pageNumber, surveyAnswerDTO, isMobileDevice,"",IsAndroid);
                        TempData["Width"] = form.Width + 5;

                        if (TempData.ContainsKey("isredirect") && !string.IsNullOrWhiteSpace(TempData["isredirect"].ToString()))
                        {
                            form.Validate(form.RequiredFieldsList);
                        }
                        
                        surveyAnswerDTO.IsDraftMode = surveyInfoModel.IsDraftMode;
                        this.SetCurrentPage(surveyAnswerDTO, pageNumber);

                        if (isMobileDevice)
                        {
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
                        }
                        bool RecordBeforeFlug = GetRecordBeforeFlag(surveyAnswerDTO.XML.ToString());








                        ///////////////////////////// Execute - Record Before - start//////////////////////
                        XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);
                        if (RecordBeforeFlug == false)
                            {
                            Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();
                            EnterRule FunctionObject_B = (EnterRule)form.FormCheckCodeObj.GetCommand("level=record&event=before&identifier=");
                            if (FunctionObject_B != null && !FunctionObject_B.IsNull())
                                {
                                try
                                    {
                                    surveyAnswerDTO.XML = CreateResponseDocument(xdoc, surveyAnswerDTO.XML.ToString());

                                    // form.RequiredFieldsList = this.RequiredList;
                                    FunctionObject_B.Context.HiddenFieldList = form.HiddenFieldsList;
                                    FunctionObject_B.Context.HighlightedFieldList = form.HighlightedFieldsList;
                                    FunctionObject_B.Context.DisabledFieldList = form.DisabledFieldsList;
                                    FunctionObject_B.Context.RequiredFieldList = form.RequiredFieldsList;

                                    FunctionObject_B.Execute();

                                    // field list
                                    form.HiddenFieldsList = FunctionObject_B.Context.HiddenFieldList;
                                    form.HighlightedFieldsList = FunctionObject_B.Context.HighlightedFieldList;
                                    form.DisabledFieldsList = FunctionObject_B.Context.DisabledFieldList;
                                    form.RequiredFieldsList = FunctionObject_B.Context.RequiredFieldList;


                                    ContextDetailList = Epi.Web.MVC.Utility.SurveyHelper.GetContextDetailList(FunctionObject_B);
                                    form = Epi.Web.MVC.Utility.SurveyHelper.UpdateControlsValuesFromContext(form, ContextDetailList);
                                    surveyAnswerDTO.RecordBeforeFlag = true;

                                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId.ToString(), form, surveyAnswerDTO, false, false, 0);
                                    //Getting the updated Xml and load the form from the new Response XML
                                    surveyAnswerDTO = GetSurveyAnswer(responseId);
                                    form = _isurveyFacade.GetSurveyFormData(surveyAnswerDTO.SurveyId, pageNumber, surveyAnswerDTO, isMobileDevice,"",IsAndroid);
                                    }
                                catch (Exception ex)
                                    {
                                    // do nothing so that processing
                                    // can continue
                                    }
                                }
                            }
                        if (this.Request.Url.ToString().Contains('?'))
                        {
                      
                        Uri uri = new Uri(this.Request.Url.ToString());
                        form.RedirectURL =  uri.GetLeftPart(UriPartial.Path);
                        }
                        form.IsSaved = issaved;
                        if (Session["PassCode"]!=null)
                        form.PassCode = Session["PassCode"].ToString();
                       
                        if (issaved)
                        {

                            form.StatusId = 1;
                           
                        }
                        return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                }
            }
            catch (Exception ex)
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage( ex, this.HttpContext);
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }

        private bool GetRecordBeforeFlag(string Xml)
            {
            bool Flag = false;
            XDocument xdoc = XDocument.Parse( Xml);


            if (!string.IsNullOrEmpty(xdoc.Root.Attribute("RecordBeforeFlag").Value))
                {
                Flag =  true;
                }

            return Flag;
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SurveyAnswerModel surveyAnswerModel, string Submitbutton, string Savebutton, string ContinueButton, string PreviousButton, int PageNumber = 0)
        {
            ActionResult actionResult = null;
            bool IsAndroid = false;
            
           
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ViewBag.Version = version;
            string responseId = surveyAnswerModel.ResponseId;
            bool isMobileDevice = false;
            isMobileDevice = this.Request.Browser.IsMobileDevice;

            if (isMobileDevice == false)
            {
                isMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
            }
            
            if (this.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsAndroid = true;
            }
            try
            {
                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId);
                SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
                PreValidationResultEnum ValidationTest = PreValidateResponse(Mapper.ToSurveyAnswerModel(SurveyAnswer), surveyInfoModel);
             
                switch (ValidationTest)
                {
                    case PreValidationResultEnum.SurveyIsPastClosingDate:
                        actionResult = View("SurveyClosedError");
                        break;

                    case PreValidationResultEnum.SurveyIsAlreadyCompleted:
                        actionResult = View("IsSubmitedError");
                        break;
                        
                    default:
                        MvcDynamicForms.Form form;    
                        int CurrentPageNum = GetSurveyPageNumber(SurveyAnswer.XML.ToString());
                        int NewPageNumber;

                        string url = "";
                        if (this.Request.UrlReferrer == null)
                        {
                            url = this.Request.Url.ToString();
                        }
                        else
                        {
                            url = this.Request.UrlReferrer.ToString();
                        }

                        int LastIndex = url.LastIndexOf("/");
                        string StringNumber = null;

                        if (url.Length - LastIndex + 1 <= url.Length)
                        {
                            StringNumber = url.Substring(LastIndex, url.Length - LastIndex);
                            StringNumber = StringNumber.Trim('/');
                        }
                        if (StringNumber.Contains("?RequestId="))
                            {
                             
                            StringNumber = StringNumber.Substring(0, StringNumber.IndexOf("?"));
                              
                            }
                        if (int.TryParse(StringNumber, out NewPageNumber))
                        {
                            
                        if (NewPageNumber != CurrentPageNum)
                            {
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, NewPageNumber, SurveyAnswer, isMobileDevice,"",IsAndroid );
                            Epi.Web.MVC.Utility.FormProvider.UpdateHiddenFields(NewPageNumber, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                            }
                            else
                            {
                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer, isMobileDevice,"",IsAndroid);
                                Epi.Web.MVC.Utility.FormProvider.UpdateHiddenFields(CurrentPageNum, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                            }
                            
                            UpdateModel(form);
                        }
                        else
                        {


                        form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer, isMobileDevice,"",IsAndroid );
                        form.ClearAllErrors();
                        Epi.Web.MVC.Utility.FormProvider.UpdateHiddenFields(CurrentPageNum, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);

                        UpdateModel(form);

                        }

                        if (isMobileDevice)
                        {
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
                            form.StatusId = SurveyAnswer.Status;
                        }

                        bool IsSubmited = false;
                        bool IsSaved = false;
                         
                        form = SetLists(form);
                         
                        _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);
                        //is_print_action

                        if (!string.IsNullOrEmpty(this.Request.Form["is_save_action"]) && this.Request.Form["is_save_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                            SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()) == 0 ? 1 : GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer, isMobileDevice,"",IsAndroid);
                            
                            form = SetLists(form);
                            
                            IsSaved = form.IsSaved = true;
                            form.StatusId = SurveyAnswer.Status;
                            
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
                            Session["PassCode"] = form.PassCode;
                            form = SetLists(form);
                            UpdateModel(form);
                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);

                            TempData["Width"] = form.Width + 5;
                          //  actionResult = View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                          //  return actionResult;
                            return RedirectToAction("Index", "Survey", new { RequestId = form.ResponseId, PageNumber = CurrentPageNum,IsSaved = true });
                        }
                        else if (!string.IsNullOrEmpty(this.Request.Form["is_print_action"]) && this.Request.Form["is_print_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                            SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()) == 0 ? 1 : GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer, isMobileDevice,"",IsAndroid );
                            UpdateModel(form);
                            form = SetLists(form);

                            IsSaved = form.IsSaved = true;
                            form.StatusId = SurveyAnswer.Status;

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

                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);

                            TempData["Width"] = form.Width + 5;
                            actionResult = RedirectToAction("Index", "Print", new { responseId = responseId, FromFinal = false}); 
                            return actionResult;
                            }
                        else if (!string.IsNullOrEmpty(this.Request.Form["is_goto_action"]) && this.Request.Form["is_goto_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            form = SetLists(form);

                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);

                            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber, SurveyAnswer, isMobileDevice,"",IsAndroid );
                            TempData["Width"] = form.Width + 5;
                            actionResult = View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                            return actionResult;
                        }
                        else if (form.Validate(form.RequiredFieldsList))
                        {
                            if (!string.IsNullOrEmpty(Submitbutton))
                            {
                                EnterRule FunctionObject_A = (EnterRule)form.FormCheckCodeObj.GetCommand("level=record&event=after&identifier=");
                                if (FunctionObject_A != null && !FunctionObject_A.IsNull())
                                {
                                    try
                                    {
                                        FunctionObject_A.Execute();
                                    }
                                    catch
                                    {
                                        // continue
                                    }
                                }

                                Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();
                                ContextDetailList = Epi.Web.MVC.Utility.SurveyHelper.GetContextDetailList(FunctionObject_A);

                                SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                                SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
                                UpdateModel(form);
                                for (int i = 1; i < form.NumberOfPages+1; i++)
                                {
                                    form = Epi.Web.MVC.Utility.FormProvider.GetForm(form.SurveyInfo, i, SurveyAnswer, isMobileDevice);

                                    if (!form.Validate(form.RequiredFieldsList))
                                    {
                                        TempData["isredirect"] = "true";
                                        TempData["Width"] = form.Width + 5;
                                        _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, i);
                                        actionResult = RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = responseId, PageNumber = i.ToString() });
                                        return actionResult;
                                    }
                                }

                                SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
                                IsSubmited = true;//survey has been submited this will change the survey status to 3 - Completed
                                UpdateModel(form);
                                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);
                                //FormsAuthentication.SignOut();

                                actionResult = RedirectToAction("Index", "Final", new { surveyId = surveyInfoModel.SurveyId ,responseId = responseId });
                                return actionResult;
                            }
                            else
                            {
                                SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;

                                

                                Common.Message.SurveyAnswerResponse answerResponse = _isurveyFacade.GetSurveyAnswerResponse(responseId);
                                SurveyAnswer = answerResponse.SurveyResponseList[0];

                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer, isMobileDevice, "", IsAndroid);
                                
                                form = SetLists(form);
                                UpdateModel(form);
                                 _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber); // To update Current Page
                               
                            
                                TempData["Width"] = form.Width + 5;


                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber, SurveyAnswer, isMobileDevice, "", IsAndroid); // To load Next Page
                                if (isMobileDevice)
                                {
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
                                    form.StatusId = SurveyAnswer.Status;
                                }

                                actionResult = View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                                return actionResult;
                            }
                        }
                        else
                        {
                            CurrentPageNum = GetSurveyPageNumber(SurveyAnswer.XML.ToString()) ;

                            if (CurrentPageNum != PageNumber) // failed validation and navigating to different page// must keep url the same 
                            {
                                TempData["isredirect"] = "true";
                                TempData["Width"] = form.Width + 5;
                                actionResult = RedirectToAction("Index", "Survey", new { RequestId = form.ResponseId, PageNumber = CurrentPageNum });
                            }
                            else
                            {
                                TempData["Width"] = form.Width + 5;
                                actionResult = View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage(ex, this.HttpContext);
                actionResult = View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
            
            return actionResult;
        }

 
        private int GetCurrentPage()
        {
            int currentPage = 1;
            string pageNum = this.Request.UrlReferrer.ToString().Substring(this.Request.UrlReferrer.ToString().LastIndexOf('/')+1);
            int.TryParse(pageNum, out currentPage);

            return currentPage;
        }

        private void SetCurrentPage(Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, int pageNumber)
        {
            XDocument Xdoc = XDocument.Parse(surveyAnswerDTO.XML);

            if (pageNumber != 0)
            {
                Xdoc.Root.Attribute("LastPageVisited").Value = pageNumber.ToString();
            }

            surveyAnswerDTO.XML = Xdoc.ToString();

            Epi.Web.Common.Message.SurveyAnswerRequest  sar = new Common.Message.SurveyAnswerRequest();
            sar.Action = "Update";
            sar.SurveyAnswerList.Add(surveyAnswerDTO);

            this._isurveyFacade.GetSurveyAnswerRepository().SaveSurveyAnswer(sar);
        }

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetSurveyAnswer(string responseId)
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;
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

        public static string GetResponseFormState(string xml, string listName)
        {
            string list = "";

            if (!string.IsNullOrEmpty(xml))
            {
                XDocument xdoc = XDocument.Parse(xml);

                if (!string.IsNullOrEmpty(xdoc.Root.Attribute(listName).Value.ToString()))
                {
                    list = xdoc.Root.Attribute(listName).Value;
                }
            }

            return list;
        }

        public static string GetRequiredList(string xml) 
        {
            XDocument xdoc = XDocument.Parse(xml);
            string list = xdoc.Root.Attribute("RequiredFieldsList").Value;
            return list;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateResponseXml(string nameList, string value, string responseId)
        {
            try
            {
                if (!string.IsNullOrEmpty(nameList))
                {
                    string[] names = null;

                    names = nameList.Split(',');

                    bool IsMobileDevice = false;

                    IsMobileDevice = this.Request.Browser.IsMobileDevice;
                    Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

                    SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId); 
                    int NumberOfPages = Epi.Web.MVC.Utility.SurveyHelper.GetNumberOfPags(SurveyAnswer.XML);

                    foreach (string name in names)
                    {
                        for (int i = NumberOfPages; i > 0; i--)
                        {
                            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswer.ResponseId).SurveyResponseList[0];

                            MvcDynamicForms.Form formRs = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, i, SurveyAnswer, IsMobileDevice);

                            formRs = Epi.Web.MVC.Utility.SurveyHelper.UpdateControlsValues(formRs, name, value);

                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, formRs, SurveyAnswer, false, false, i);

                        }
                    }
                    return Json(true);
                }
                return Json(true);
            }
            catch 
            {
                return Json(false);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveSurvey(string Key, int Value, string responseId)
        {
            try
            {
                bool IsMobileDevice = false;
                int PageNumber =  Value;
                IsMobileDevice = this.Request.Browser.IsMobileDevice;

                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

                SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId); 
                PreValidationResultEnum ValidationTest = PreValidateResponse(Mapper.ToSurveyAnswerModel(SurveyAnswer), surveyInfoModel);
                var form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, PageNumber, SurveyAnswer, IsMobileDevice);
              
                form.StatusId = SurveyAnswer.Status;
                var IsSaved = form.IsSaved = true;
                SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()) == 0 ? 1 : GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer, IsMobileDevice);

                UpdateModel(form);

                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, false, IsSaved, PageNumber);
                
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public SurveyInfoModel GetSurveyInfo(string SurveyId)
        {
            SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
            return surveyInfoModel;  
        }

        public MvcDynamicForms.Form SetLists(MvcDynamicForms.Form form) 
        {
            form.HiddenFieldsList = this.Request.Form["HiddenFieldsList"].ToString();
            form.HighlightedFieldsList = this.Request.Form["HighlightedFieldsList"].ToString();
            form.DisabledFieldsList = this.Request.Form["DisabledFieldsList"].ToString();
            form.RequiredFieldsList = this.Request.Form["RequiredFieldsList"].ToString();
            form.AssignList = this.Request.Form["AssignList"].ToString();

            return form;
        }

    //GetPrintView
        [HttpGet]

        public ActionResult GetPrintView(string ResponseId) 
            {
            
            Common.Message.SurveyAnswerResponse answerResponse = _isurveyFacade.GetSurveyAnswerResponse(ResponseId);
            SurveyInfoModel surveyInfoModel = GetSurveyInfo(answerResponse.SurveyResponseList[0].SurveyId); 

            PrintResponseModel PrintResponseModel = new PrintResponseModel();
            Common.Message.SurveyControlsRequest Request = new Common.Message.SurveyControlsRequest();
            Request.SurveyId = answerResponse.SurveyResponseList[0].SurveyId;
            Common.Message.SurveyControlsResponse List = _isurveyFacade.GetSurveyControlList(Request);
            PrintResponseModel.ResponseList = Epi.Web.MVC.Utility.SurveyHelper.GetQuestionAnswerList(answerResponse.SurveyResponseList[0].XML, List);
            PrintResponseModel.NumberOfPages = Epi.Web.MVC.Utility.SurveyHelper.GetNumberOfPags(answerResponse.SurveyResponseList[0].XML);
            PrintResponseModel.SurveyName = surveyInfoModel.SurveyName;
            PrintResponseModel.CurrentDate = DateTime.Now.ToString();
            return PartialView("Print", PrintResponseModel);
            }

        private static int GetNumberOfPages(XDocument Xml)
            {
            var _FieldsTypeIDs = from _FieldTypeID in
                                     Xml.Descendants("View")
                                 select _FieldTypeID;

            return _FieldsTypeIDs.Elements().Count();
            }

        private string CreateResponseDocument(XDocument pMetaData, string pXML)
            {
            XDocument XmlResponse = new XDocument();
            int NumberOfPages = GetNumberOfPages(pMetaData);
            for (int i = 0; NumberOfPages > i - 1; i++)
                {
                var _FieldsTypeIDs = from _FieldTypeID in
                                         pMetaData.Descendants("Field")
                                     where _FieldTypeID.Attribute("Position").Value == (i - 1).ToString()
                                     select _FieldTypeID;

                PageFields = _FieldsTypeIDs;

                XDocument CurrentPageXml = ToXDocument(CreateResponseXml("", false, i, ""));

                if (i == 0)
                    {
                    XmlResponse = ToXDocument(CreateResponseXml("", true, i, ""));
                    }
                else
                    {
                    XmlResponse = MergeXml(XmlResponse, CurrentPageXml, i);
                    }
                }

            return XmlResponse.ToString();
            }

        public XmlDocument CreateResponseXml(string SurveyId, bool AddRoot, int CurrentPage, string Pageid)
            {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("SurveyResponse");

            if (CurrentPage == 0)
                {
                root.SetAttribute("SurveyId", SurveyId);
                root.SetAttribute("LastPageVisited", "1");
                root.SetAttribute("HiddenFieldsList", "");
                root.SetAttribute("HighlightedFieldsList", "");
                root.SetAttribute("DisabledFieldsList", "");
                root.SetAttribute("RequiredFieldsList", "");
                root.SetAttribute("RecordBeforeFlag", "");
                xml.AppendChild(root);
                }

            XmlElement PageRoot = xml.CreateElement("Page");
            if (CurrentPage != 0)
                {
                PageRoot.SetAttribute("PageNumber", CurrentPage.ToString());               
                PageRoot.SetAttribute("PageId", Pageid);//Added PageId Attribute to the page node
                PageRoot.SetAttribute("MetaDataPageId", Pageid.ToString());
                xml.AppendChild(PageRoot);
                }

            foreach (var Field in this.PageFields)
                {
                XmlElement child = xml.CreateElement(Epi.Web.MVC.Constants.Constant.RESPONSE_DETAILS);
                child.SetAttribute("QuestionName", Field.Attribute("Name").Value);
                child.InnerText = Field.Value;
                PageRoot.AppendChild(child);
                //Start Adding required controls to the list
                //   SetRequiredList(Field);
                }

            return xml;
            }

        public static XDocument ToXDocument(XmlDocument xmlDocument)
            {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
                {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
                }
            }

        public static XDocument MergeXml(XDocument SavedXml, XDocument CurrentPageResponseXml, int Pagenumber)
            {
            XDocument xdoc = XDocument.Parse(SavedXml.ToString());
            XElement oldXElement = xdoc.XPathSelectElement("SurveyResponse/Page[@PageNumber = '" + Pagenumber.ToString() + "']");

            if (oldXElement == null)
                {
                SavedXml.Root.Add(CurrentPageResponseXml.Elements());
                return SavedXml;
                }

            else
                {
                oldXElement.Remove();
                xdoc.Root.Add(CurrentPageResponseXml.Elements());
                return xdoc;
                }
            }

        
        [HttpGet]
        public JsonResult GetCodesValue(string SourceTableName= "", string SelectedValue="",string SurveyId="") 
        {
            string CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"]; ;
            string IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();
            int CacheDuration = 0;
            int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);

            var TableCode = Regex.Replace(SourceTableName.ToString(), @"[^0-9a-zA-Z]+", "");
            TableCode = Regex.Replace(TableCode, @"\s+", "");
            var CacheId = SurveyId + "_SourceTables";
            SourceTablesResponse CacheObj = (SourceTablesResponse)HttpRuntime.Cache.Get(CacheId);
           
           

            if (CacheIsOn.ToUpper() == "TRUE")
            {
                if (CacheObj == null)
                {
                    var SourceTables = _isurveyFacade.GetSourceTables(Session["RootFormId"].ToString());
                    CacheObj = (SourceTablesResponse)SourceTables;
                    if (IsCacheSlidingExpiration.ToUpper() == "TRUE")
                    {

                        HttpRuntime.Cache.Insert(CacheId, SourceTables, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(CacheDuration));
                    }
                    else
                    {
                        HttpRuntime.Cache.Insert(CacheId, SourceTables, null, DateTime.Now.AddMinutes(CacheDuration), Cache.NoSlidingExpiration);

                    }
                }
               
            }
            else {
                var SourceTables = _isurveyFacade.GetSourceTables(Session["RootFormId"].ToString());
                CacheObj = (SourceTablesResponse)SourceTables;
            
            }
            
            try
            {
              
                SourceTableDTO Table = CacheObj.List.Where(x => x.TableName.Contains(SourceTableName.ToString())).Single();
                XDocument Xdoc = XDocument.Parse(Table.TableXml);
                var _ControlValues = from _ControlValue in Xdoc.Descendants("Item")
                                     where _ControlValue.Attributes().SingleOrDefault(xa => string.Equals(xa.Name.LocalName, SourceTableName,
                                     StringComparison.InvariantCultureIgnoreCase)).Value == SelectedValue.ToString()
                                     select _ControlValue;

                 var Attributes = _ControlValues.Attributes().ToList();
                Dictionary<string,string> List = new  Dictionary<string,string>();
                 foreach (var Attribute in Attributes)
                {
                    List.Add(Attribute.Name.LocalName.ToLower(), Attribute.Value);
                }



                 return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false);
                //throw ex;
            }
           
        
        }
    }
}
