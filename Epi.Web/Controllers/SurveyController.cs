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
 
using System.Reflection;
using System.Diagnostics;
using System.Reflection;
using System.Diagnostics;
using Epi.Web.MVC.Utility;
using System.Linq;

using Epi.Web.Common.Message;
using Epi.Web.Common.DTO;
using System.Globalization;
using System.Threading;
using System.Web.Configuration;

namespace Epi.Web.MVC.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private ISurveyFacade _isurveyFacade;
        private string RequiredList;
        private IEnumerable<XElement> PageFields;
        private string RootFormId = "";
        private string RootResponseId = "";
        private bool IsEditMode;
       
        private int ReffererPageNum;
        List<KeyValuePair<int, string>> Columns = new List<KeyValuePair<int, string>>();
        public SurveyController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
 
        [HttpGet]
        public ActionResult Index(string responseId, int pageNumber = 0,bool issaved = false,string Edit = "", string FormValuesHasChanged = "",string surveyid ="")
        {
            try
            {
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

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
                if (Session["RootResponseId"] != null && Session["RootResponseId"].ToString() == responseId)
                {
                    Session["RelateButtonPageId"] = null;
                }
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
                Session["IsSqlProject"] = surveyInfoModel.IsSqlProject;
                if (Session["RootFormId"] == null) {

                    Session["RootFormId"] = surveyInfoModel.SurveyId;
                }
                if (pageNumber == 0)
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
                        List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy();// Pain Point
                        var form = _isurveyFacade.GetSurveyFormData(surveyAnswerDTO.SurveyId, pageNumber, surveyAnswerDTO, isMobileDevice,null, FormsHierarchy, IsAndroid);
                       
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
                                    form = _isurveyFacade.GetSurveyFormData(surveyAnswerDTO.SurveyId, pageNumber, surveyAnswerDTO, isMobileDevice,null,null,IsAndroid);
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

                        string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
                        DateFormat = DateFormat.Remove(DateFormat.IndexOf("y"), 2);                      
                        form.CurrentCultureDateFormat = DateFormat;
                        SurveyModel Model = new SurveyModel();
                        Model.Form = new MvcDynamicForms.Form();
                        Model.Form = form;
                        return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, Model);
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
        //  [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [ValidateAntiForgeryToken]
        //public ActionResult Index(SurveyInfoModel surveyInfoModel, string Submitbutton, string Savebutton, string ContinueButton, string PreviousButton, int PageNumber = 1)
        public ActionResult Index(SurveyAnswerModel surveyAnswerModel,
            string Submitbutton,
            string Savebutton,
            string ContinueButton,
            string PreviousButton,
            string Close,
            string CloseButton,
            int PageNumber = 1,
            string Form_Has_Changed = "",
            string Requested_View_Id = "",
            bool Log_Out = false


            )
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
            DateFormat = DateFormat.Remove(DateFormat.IndexOf("y"), 2);
            Session["FormValuesHasChanged"] = Form_Has_Changed;
            Session["IsEditMode"] = false;
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

            // Get Hierarchy
            List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy();


            try
            {
                string FormValuesHasChanged = Form_Has_Changed;
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
                        MvcDynamicForms.Form form= new MvcDynamicForms.Form();
                        form.CurrentCultureDateFormat = DateFormat;

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
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, NewPageNumber, SurveyAnswer, isMobileDevice,null, FormsHierarchy, IsAndroid);
                            Epi.Web.MVC.Utility.FormProvider.UpdateHiddenFields(NewPageNumber, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                            }
                            else
                            {
                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer, isMobileDevice,null, FormsHierarchy, IsAndroid);
                                Epi.Web.MVC.Utility.FormProvider.UpdateHiddenFields(CurrentPageNum, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                            }                         

                            UpdateModel(form);
                        }
                        else
                        {


                        form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer, isMobileDevice,null, FormsHierarchy, IsAndroid );
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
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()) == 0 ? 1 : GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer, isMobileDevice,null,null,IsAndroid);
                            
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
                           // return RedirectToAction("Index", "Survey", new { RequestId = form.ResponseId, PageNumber = CurrentPageNum,IsSaved = true });

                            return RedirectToAction("Index", "Survey", new { RequestId = form.ResponseId, PageNumber = 1, IsSaved = true });
                        }
                        else if (!string.IsNullOrEmpty(this.Request.Form["is_print_action"]) && this.Request.Form["is_print_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                            SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()) == 0 ? 1 : GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer, isMobileDevice,null,null,IsAndroid );
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


                            SurveyModel SurveyModel = new SurveyModel();
                            SurveyModel.Form = form;
                            SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, form.SurveyInfo.SurveyId);

                            TempData["Width"] = form.Width + 5;
                            actionResult = RedirectToAction("Index", "Print", new { responseId = responseId, FromFinal = false }); 
                            return actionResult;
                            }
                        else if (!string.IsNullOrEmpty(this.Request.Form["Go_Home_action"]) && this.Request.Form["Go_Home_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {

                            IsSaved = true;
                            form = SaveCurrentForm(form, surveyInfoModel, SurveyAnswer, responseId, 0, IsSubmited, IsSaved, isMobileDevice, FormValuesHasChanged, PageNumber, FormsHierarchy);
                            form = SetLists(form);
                            //TempData["Width"] = form.Width + 5;
                            //SurveyModel SurveyModel = new SurveyModel();
                            //SurveyModel.Form = form;
                            //SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, Session["RootFormId"].ToString());


                            return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = Session["RootResponseId"].ToString(), PageNumber = 1 });

                        }
                        else if (!string.IsNullOrEmpty(this.Request.Form["Go_One_Level_Up_action"]) && this.Request.Form["Go_One_Level_Up_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            IsSaved = true;

                            string RelateParentId = "";
                            form = SaveCurrentForm(form, surveyInfoModel, SurveyAnswer, responseId, 0, IsSubmited, IsSaved, isMobileDevice, FormValuesHasChanged, PageNumber, FormsHierarchy);
                            form = SetLists(form);
                            TempData["Width"] = form.Width + 5;
                            SurveyModel SurveyModel = new SurveyModel();
                            SurveyModel.Form = form;
                            SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, form.SurveyInfo.SurveyId);

                            var CurentRecordParent = FormsHierarchy.Single(x => x.FormId == surveyInfoModel.SurveyId);
                            foreach (var item in CurentRecordParent.ResponseIds)
                            {
                                if (item.ResponseId == responseId && !string.IsNullOrEmpty(item.RelateParentId))
                                {

                                    RelateParentId = item.RelateParentId;
                                    break;
                                }


                            }
                            Dictionary<string, int> SurveyPagesList = (Dictionary<string, int>)Session["RelateButtonPageId"];
                            if (SurveyPagesList != null && !string.IsNullOrEmpty(RelateParentId))
                            {
                                PageNumber = SurveyPagesList[RelateParentId];
                            }
                            if (!string.IsNullOrEmpty(RelateParentId))
                            {

                                return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = RelateParentId, PageNumber = PageNumber });

                            }
                            else
                            {
                                return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = RootResponseId, PageNumber = PageNumber });
                            }


                        }
                        else if (!string.IsNullOrEmpty(this.Request.Form["Get_Child_action"]) && this.Request.Form["Get_Child_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            int RequestedViewId;

                            SetRelateSession(responseId, PageNumber);
                            RequestedViewId = int.Parse(this.Request.Form["Requested_View_Id"]);
                            form = SaveCurrentForm(form, surveyInfoModel, SurveyAnswer, responseId, 0, IsSubmited, IsSaved, isMobileDevice, FormValuesHasChanged, PageNumber, FormsHierarchy);
                            form = SetLists(form);
                            TempData["Width"] = form.Width + 5;
                            Session["RequestedViewId"] = RequestedViewId;
                            SurveyModel SurveyModel = new SurveyModel();
                            SurveyModel.Form = form;
                            SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, form.SurveyInfo.SurveyId);
                            SurveyModel.RequestedViewId = RequestedViewId;
                            int.TryParse(this.Request.Form["Requested_View_Id"].ToString(), out RequestedViewId);
                            var RelateSurveyId = FormsHierarchy.Single(x => x.ViewId == RequestedViewId);

                            int ViewId = int.Parse(Requested_View_Id);

                            string ChildResponseId = AddNewChild(RelateSurveyId.FormId, ViewId, responseId, FormValuesHasChanged, "1");
                            return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = ChildResponseId, PageNumber = 1 });

                        }
                        //Read_Response_action
                        else if (!string.IsNullOrEmpty(this.Request.Form["Read_Response_action"]) && this.Request.Form["Read_Response_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {

                            SetRelateSession(responseId, PageNumber);

                         //   this.UpdateStatus(surveyAnswerModel.ResponseId, surveyAnswerModel.SurveyId, 2);

                            int RequestedViewId = int.Parse(this.Request.Form["Requested_View_Id"]);
                            // return RedirectToRoute(new { Controller = "RelatedResponse", Action = "Index", SurveyId = form.SurveyInfo.SurveyId, ViewId = RequestedViewId, ResponseId = responseId, CurrentPage = 1 });

                            return RedirectToRoute(new { Controller = "FormResponse", Action = "Index", formid = form.SurveyInfo.SurveyId, ViewId = RequestedViewId, responseid = responseId, Pagenumber = 1 });

                        }

                        //else if (!string.IsNullOrEmpty(this.Request.Form["Do_Not_Save_action"]) && this.Request.Form["Do_Not_Save_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        //{


                        //    bool.TryParse(Session["IsEditMode"].ToString(), out this.IsEditMode);

                        //    SurveyAnswerRequest SARequest = new SurveyAnswerRequest();
                        //    SARequest.SurveyAnswerList.Add(new SurveyAnswerDTO() { ResponseId = Session["RootResponseId"].ToString() });
                        //    SARequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
                        //    SARequest.Criteria.IsEditMode = this.IsEditMode;
                        //    SARequest.Criteria.IsSqlProject = (bool)Session["IsSqlProject"];
                        //    SurveyAnswerResponse SAResponse = _isurveyFacade.DeleteResponse(SARequest);
                        //    return RedirectToRoute(new { Controller = "FormResponse", Action = "Index", formid = Session["RootFormId"].ToString(), ViewId = 0, PageNumber = Convert.ToInt32(Session["PageNumber"].ToString()) });

                        //}
                        else if (!string.IsNullOrEmpty(this.Request.Form["is_goto_action"]) && this.Request.Form["is_goto_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            form = SetLists(form);

                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber);

                            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber, SurveyAnswer, isMobileDevice,null,null,IsAndroid );
                            TempData["Width"] = form.Width + 5;
                            SurveyModel Model = new SurveyModel();
                            Model.Form = new MvcDynamicForms.Form();
                            Model.Form = form;
                            actionResult = View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, Model);
                            return actionResult;
                        }
                        else if (form.Validate(form.RequiredFieldsList))
                        {


                            if (!string.IsNullOrEmpty(Submitbutton) || !string.IsNullOrEmpty(CloseButton) || (!string.IsNullOrEmpty(this.Request.Form["is_save_action_Mobile"]) && this.Request.Form["is_save_action_Mobile"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase)))
                            {

                                KeyValuePair<string, int> ValidateValues = ValidateAll(form, 0, IsSubmited, IsSaved, isMobileDevice, FormValuesHasChanged);
                                if (!string.IsNullOrEmpty(ValidateValues.Key) && !string.IsNullOrEmpty(ValidateValues.Value.ToString()))
                                {
                                    return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = ValidateValues.Key, PageNumber = ValidateValues.Value.ToString() });
                                }
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


                                SourceTablesResponse SourceTables = _isurveyFacade.GetSourceTables(form.SurveyInfo.SurveyId);
                                
                                
                                for (int i = 1; i < form.NumberOfPages+1; i++)
                                {
                                    form = Epi.Web.MVC.Utility.FormProvider.GetForm(form.SurveyInfo, i, SurveyAnswer, isMobileDevice,IsAndroid,SourceTables.List);

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

                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer, isMobileDevice,null,null, IsAndroid);
                                
                                form = SetLists(form);
                                UpdateModel(form);
                                 _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber); // To update Current Page
                               
                            
                                TempData["Width"] = form.Width + 5;


                                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber, SurveyAnswer, isMobileDevice, null,null, IsAndroid); // To load Next Page
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
                                SurveyModel Model = new SurveyModel();
                                Model.Form = new MvcDynamicForms.Form();
                                Model.Form = form;
                                actionResult = View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, Model);
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
                                SurveyModel Model = new SurveyModel();
                                Model.Form = new MvcDynamicForms.Form();
                                Model.Form = form;
                                actionResult = View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, Model);
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

                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, formRs, SurveyAnswer, false, true, i);

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
                //SurveyResponseXML SurveyResponseXML = new SurveyResponseXML();
                //SurveyResponseXML.SetRequiredList(Field);
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
            SurveyId = Session["RootFormId"].ToString();
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


        private List<FormsHierarchyDTO> GetFormsHierarchy()
        {
            FormsHierarchyResponse FormsHierarchyResponse = new FormsHierarchyResponse();
            FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            SurveyAnswerRequest ResponseIDsHierarchyRequest = new SurveyAnswerRequest();
            SurveyAnswerResponse ResponseIDsHierarchyResponse = new SurveyAnswerResponse();
            // FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            try
            {
                if (Session["RootFormId"] != null && Session["RootResponseId"] != null)
                {
                    FormsHierarchyRequest.SurveyInfo.SurveyId = Session["RootFormId"].ToString();
                    FormsHierarchyRequest.SurveyResponseInfo.ResponseId = Session["RootResponseId"].ToString();
                    FormsHierarchyResponse = _isurveyFacade.GetFormsHierarchy(FormsHierarchyRequest);

                    SurveyAnswerDTO SurveyAnswerDTO = new SurveyAnswerDTO();
                    SurveyAnswerDTO.ResponseId = Session["RootResponseId"].ToString();
                    ResponseIDsHierarchyRequest.SurveyAnswerList.Add(SurveyAnswerDTO);
                    ResponseIDsHierarchyResponse = _isurveyFacade.GetSurveyAnswerHierarchy(ResponseIDsHierarchyRequest);
                    FormsHierarchyResponse.FormsHierarchy = CombineLists(FormsHierarchyResponse.FormsHierarchy, ResponseIDsHierarchyResponse.SurveyResponseList);
                }
            }
            catch (Exception ex) {
                ex.Source = "GetFormsHierarchy";
                throw ex;
            }
            return FormsHierarchyResponse.FormsHierarchy;
        }
        private List<FormsHierarchyDTO> CombineLists(List<FormsHierarchyDTO> RelatedFormIDsList, List<SurveyAnswerDTO> AllResponsesIDsList)
        {

            List<FormsHierarchyDTO> List = new List<FormsHierarchyDTO>();

            foreach (var Item in RelatedFormIDsList)
            {
                FormsHierarchyDTO FormsHierarchyDTO = new FormsHierarchyDTO();
                FormsHierarchyDTO.FormId = Item.FormId;
                FormsHierarchyDTO.ViewId = Item.ViewId;
                FormsHierarchyDTO.IsSqlProject = Item.IsSqlProject;
                FormsHierarchyDTO.IsRoot = Item.IsRoot;
                FormsHierarchyDTO.SurveyInfo = Item.SurveyInfo;
                if (AllResponsesIDsList != null)
                {
                    FormsHierarchyDTO.ResponseIds = AllResponsesIDsList.Where(x => x.SurveyId == Item.FormId).ToList();
                }
                List.Add(FormsHierarchyDTO);
            }
            return List;

        }

        [HttpPost]
        public JsonResult HasResponse(string SurveyId, int ViewId, string ResponseId)
        {

            bool IsSqlProject = (bool)Session["IsSqlProject"];

            bool HasResponse = false;
            List<FormsHierarchyDTO> FormsHierarchy = new List<FormsHierarchyDTO>();
            FormsHierarchy = GetFormsHierarchy();
            var RelateSurveyId = FormsHierarchy.Single(x => x.ViewId == ViewId);
            if (!IsSqlProject)
            {

                bool IsMobileDevice = this.Request.Browser.IsMobileDevice;
                if (IsMobileDevice == false)
                {
                    IsMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
                }
               // int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());

                int ResponseCount = GetResponseCount(FormsHierarchy, ViewId, ResponseId);

                if (ResponseCount > 0)
                {


                    HasResponse = true;
                }
            }
            else
            {
                // Get child count from Sql
                //1-Get the child Id
                //SurveyInfoResponse GetChildFormInfo(SurveyInfoRequest SurveyInfoRequest)

                HasResponse = _isurveyFacade.HasResponse(RelateSurveyId.FormId.ToString(), ResponseId);
            }


            return Json(HasResponse);

        }
        [HttpPost]
        public JsonResult AddChild(string SurveyId, int ViewId, string ResponseId, string FormValuesHasChanged, string CurrentPage)
        {
            Session["RequestedViewId"] = ViewId;
            //1-Get the child Id

            SurveyInfoRequest SurveyInfoRequest = new Common.Message.SurveyInfoRequest();
            SurveyInfoResponse SurveyInfoResponse = new Common.Message.SurveyInfoResponse();
            SurveyInfoDTO SurveyInfoDTO = new Common.DTO.SurveyInfoDTO();
            SurveyInfoDTO.SurveyId = SurveyId;
            SurveyInfoDTO.ViewId = ViewId;
            SurveyInfoRequest.SurveyInfoList.Add(SurveyInfoDTO);
            SurveyInfoResponse = _isurveyFacade.GetChildFormInfo(SurveyInfoRequest);



            //3-Create a new response for the child 
            //string ChildResponseId = CreateResponse(SurveyInfoResponse.SurveyInfoList[0].SurveyId, ResponseId);
            string ChildResponseId = AddNewChild(SurveyInfoResponse.SurveyInfoList[0].SurveyId, ViewId, ResponseId, FormValuesHasChanged, CurrentPage);

            return Json(ChildResponseId);

        }
        private string AddNewChild(string SurveyId, int ViewId, string ResponseId, string FormValuesHasChanged, string CurrentPage)
        {
            Session["RequestedViewId"] = ViewId;
            bool IsMobileDevice = this.Request.Browser.IsMobileDevice;
            if (IsMobileDevice == false)
            {
                IsMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
            }
            //int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());

            string ChildResponseId = CreateResponse(SurveyId, ResponseId);
          //  this.UpdateStatus(ResponseId, SurveyId, 2);

            return ChildResponseId;
        }
        private void UpdateStatus(string ResponseId, string SurveyId, int StatusId)
        {
            SurveyAnswerRequest SurveyAnswerRequest = new SurveyAnswerRequest();
            SurveyAnswerRequest.Criteria.SurveyAnswerIdList.Add(ResponseId);

            SurveyAnswerRequest.SurveyAnswerList.Add(new SurveyAnswerDTO() { ResponseId = ResponseId });
            SurveyAnswerRequest.Criteria.StatusId = StatusId;
          //  SurveyAnswerRequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            if (!string.IsNullOrEmpty(SurveyId))
            {
                SurveyAnswerRequest.Criteria.SurveyId = SurveyId;
            }
            _isurveyFacade.UpdateResponseStatus(SurveyAnswerRequest);
        }
        public string CreateResponse(string SurveyId, string RelateResponseId)
        {
            int UserId = 0;// SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            bool.TryParse(Session["IsEditMode"].ToString(), out this.IsEditMode);
            List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy();
            //if (!string.IsNullOrEmpty(EditForm))
            //    {
            //    Epi.Web.Enter.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(EditForm);
            //    string ChildRecordId = GetChildRecordId(surveyAnswerDTO);

            //    }
            bool IsAndroid = false;

            if (this.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsAndroid = true;
            }
            bool IsMobileDevice = this.Request.Browser.IsMobileDevice;


            if (IsMobileDevice == false)
            {
                IsMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
            }
            //create the responseid
            Guid ResponseID = Guid.NewGuid();
            TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();

            // create the first survey response
            // Epi.Web.Enter.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(surveyModel.SurveyId, ResponseID.ToString());
            int CuurentOrgId = 0;// int.Parse(Session["SelectedOrgId"].ToString());
            Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(SurveyId, ResponseID.ToString(), UserId, true, RelateResponseId, this.IsEditMode, CuurentOrgId);
            SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId);

            // set the survey answer to be production or test 
            SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
            XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);

            MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, 1, SurveyAnswer, IsMobileDevice, null, FormsHierarchy, IsAndroid);

            var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")
                                 select _FieldTypeID;

            TempData["Width"] = form.Width + 100;

            XDocument xdocResponse = XDocument.Parse(SurveyAnswer.XML);

            XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
            string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();

            form.FormCheckCodeObj = form.GetCheckCodeObj(xdoc, xdocResponse, checkcode);

            ///////////////////////////// Execute - Record Before - start//////////////////////
            Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();
            EnterRule FunctionObject_B = (EnterRule)form.FormCheckCodeObj.GetCommand("level=record&event=before&identifier=");
            SurveyResponseXML SurveyResponseXML = new SurveyResponseXML(PageFields, RequiredList);
            if (FunctionObject_B != null && !FunctionObject_B.IsNull())
            {
                try
                {

                    SurveyAnswer.XML = SurveyResponseXML.CreateResponseDocument(xdoc, SurveyAnswer.XML);
                    //SurveyAnswer.XML = Epi.Web.MVC.Utility.SurveyHelper.CreateResponseDocument(xdoc, SurveyAnswer.XML, RequiredList);
                    Session["RequiredList"] = SurveyResponseXML._RequiredList;
                    this.RequiredList = SurveyResponseXML._RequiredList;
                    form.RequiredFieldsList = this.RequiredList;
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

                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, ResponseID.ToString(), form, SurveyAnswer, false, false, 0, 0);
                }
                catch (Exception ex)
                {
                    // do nothing so that processing
                    // can continue
                }
            }
            else
            {
                SurveyAnswer.XML = SurveyResponseXML.CreateResponseDocument(xdoc, SurveyAnswer.XML);//, RequiredList);
                this.RequiredList = SurveyResponseXML._RequiredList;
                Session["RequiredList"] = SurveyResponseXML._RequiredList;
                form.RequiredFieldsList = RequiredList;
                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, form, SurveyAnswer, false, false, 0, 0);
            }

            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswer.ResponseId, SurveyAnswer.SurveyId).SurveyResponseList[0];




            return ResponseID.ToString();


        }
        private int GetResponseCount(List<FormsHierarchyDTO> FormsHierarchy, int RequestedViewId, string responseId)
        {
            int ResponseCount = 0;
            var ViewResponses = FormsHierarchy.Where(x => x.ViewId == RequestedViewId);

            foreach (var item in ViewResponses)
            {
                if (item.ResponseIds.Count > 0)
                {
                    var list = item.ResponseIds.Any(x => x.RelateParentId == responseId);
                    if (list == true)
                    {

                        ResponseCount++;
                        break;
                    }
                }
            }

            return ResponseCount;
        }

        private MvcDynamicForms.Form SaveCurrentForm(MvcDynamicForms.Form form, SurveyInfoModel surveyInfoModel, SurveyAnswerDTO SurveyAnswer, string responseId, int UserId, bool IsSubmited, bool IsSaved,
           bool IsMobileDevice, string FormValuesHasChanged, int PageNumber, List<Epi.Web.Common.DTO.FormsHierarchyDTO> FormsHierarchyDTOList = null
           )
        {
            bool IsAndroid = false;

            if (this.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsAndroid = true;
            }
            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId, surveyInfoModel.SurveyId).SurveyResponseList[0];
            //SurveyAnswer = FormsHierarchyDTOList.SelectMany(x => x.ResponseIds).FirstOrDefault(z => z.ResponseId == responseId);

            SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;

            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()) == 0 ? 1 : GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer, IsMobileDevice, null, FormsHierarchyDTOList, IsAndroid);
            form.FormValuesHasChanged = FormValuesHasChanged;

            UpdateModel(form);

            form.IsSaved = true;
            form.StatusId = SurveyAnswer.Status;

            // Pass Code Logic  start 
            form = SetFormPassCode(form, responseId);
            // Pass Code Logic  end 
            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved, PageNumber, UserId);

            return form;

        }
        private MvcDynamicForms.Form SetFormPassCode(MvcDynamicForms.Form form, string responseId)
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


            return form;
        }
        public void SetRelateSession(string ResponseId, int CurrentPage)
        {
            // Session["RelateButtonPageId"] 
            var Obj = Session["RelateButtonPageId"];
            Dictionary<string, int> List = new Dictionary<string, int>();
            if (Obj == null)
            {

                List.Add(ResponseId, CurrentPage);
                Session["RelateButtonPageId"] = List;
            }
            else
            {

                List = (Dictionary<string, int>)Session["RelateButtonPageId"];
                if (!List.ContainsKey(ResponseId))
                {
                    List.Add(ResponseId, CurrentPage);
                    Session["RelateButtonPageId"] = List;
                }
            }
        }

        [HttpPost]
       
        public ActionResult ReadResponseInfo(string SurveyId, int ViewId, string ResponseId, string CurrentPage)//List<FormInfoModel> ModelList, string formid)
        // public ActionResult ReadResponseInfo( string ResponseId)//List<FormInfoModel> ModelList, string formid)
        {
            
            int PageNumber = int.Parse(CurrentPage);
            bool IsAndroid = false;

            try
            {
                //var temp = SurveyModel;
               

                if (this.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    IsAndroid = true;
                }
                 Session["CurrentFormId"] = SurveyId;
                SetRelateSession(ResponseId, PageNumber);

            }
            catch (Exception ex)
            {
                ex.Source = "Zone1" ;
                throw ex;
            }
            //try
            //{
                bool IsMobileDevice = this.Request.Browser.IsMobileDevice;
                if (IsMobileDevice == false)
                {
                List<FormsHierarchyDTO> FormsHierarchy = new List<FormsHierarchyDTO>();
                FormsHierarchyDTO RelateSurveyId = new FormsHierarchyDTO();
                SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
                SurveyAnswerDTO surveyAnswerDTO = new SurveyAnswerDTO();
                SurveyModel SurveyModel = new SurveyModel();
               

                        FormsHierarchy = GetFormsHierarchy();
               
                try
                {
                    int RequestedViewId;
                        RequestedViewId = ViewId;
                        Session["RequestedViewId"] = RequestedViewId;

                        
                        SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, SurveyId);
                        SurveyModel.RequestedViewId = RequestedViewId;
                        SurveyModel.FormResponseInfoModel = new FormResponseInfoModel();

                        RelateSurveyId = FormsHierarchy.Single(x => x.ViewId == ViewId);

                       

                        SurveyModel.FormResponseInfoModel = GetFormResponseInfoModel(RelateSurveyId.SurveyInfo.SurveyId, ResponseId, FormsHierarchy, ViewId);
                        SurveyModel.FormResponseInfoModel.NumberOfResponses = SurveyModel.FormResponseInfoModel.ResponsesList.Count();
                }
                catch (Exception ex)
                {
                    ex.Source = "NumberOfResponses:" + SurveyModel.FormResponseInfoModel.NumberOfResponses;
                    throw ex;
                }
                



                        if (RelateSurveyId.ResponseIds.Count > 0)
                    {


                        surveyAnswerDTO = FormsHierarchy.SelectMany(x => x.ResponseIds).FirstOrDefault(z => z.ResponseId == RelateSurveyId.ResponseIds[0].ResponseId);
                        SurveyModel.Form = _isurveyFacade.GetSurveyFormData(RelateSurveyId.ResponseIds[0].SurveyId, 1, surveyAnswerDTO, IsMobileDevice, null, FormsHierarchy, IsAndroid);
                    }
                    else
                    {

                        surveyAnswerDTO = GetSurveyAnswer(ResponseId, RelateSurveyId.FormId);
                        SurveyModel.Form = _isurveyFacade.GetSurveyFormData(RelateSurveyId.FormId, 1, surveyAnswerDTO, IsMobileDevice, null, FormsHierarchy, IsAndroid);
                    }



                        return PartialView("ListResponses", SurveyModel);
                   


                    
                }
                else
                {
                    try
                    {
                        return RedirectToAction("Index", "RelatedResponse", new { SurveyId = SurveyId, ViewId = ViewId, ResponseId = ResponseId, CurrentPage = CurrentPage });
                    }
                    catch (Exception ex)
                    {
                        ex.Source = "Zone3";
                        throw ex;
                    }
                }

            //}
            //catch (Exception ex)
            //{
            //    ex.Source = "Zone4";
            //    throw ex;
            //}


        }
        private Epi.Web.Common.DTO.SurveyAnswerDTO GetSurveyAnswer(string responseId, string CurrentFormId = "")
        {

            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            //responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
            result = _isurveyFacade.GetSurveyAnswerResponse(responseId, CurrentFormId, 0).SurveyResponseList[0];

            return result;

        }
        private FormResponseInfoModel GetFormResponseInfoModel(string SurveyId, string ResponseId, List<Epi.Web.Common.DTO.FormsHierarchyDTO> FormsHierarchyDTOList = null, int ViewId = 1)
        {
            int UserId = 0;// SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            FormResponseInfoModel FormResponseInfoModel = new FormResponseInfoModel();

            SurveyResponseXML SurveyResponseXML = new SurveyResponseXML();
            if (!string.IsNullOrEmpty(SurveyId))
            {
                SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
                //  FormSettingRequest FormSettingReq = new Common.Message.FormSettingRequest();

                //Populating the request

                //FormSettingReq.FormInfo.FormId = SurveyId;
                //FormSettingReq.FormInfo.UserId = UserId;
                ////Getting Column Name  List
                //FormSettingResponse FormSettingResponse = _isurveyFacade.GetFormSettings(FormSettingReq);
                //Columns = FormSettingResponse.FormSetting.ColumnNameList.ToList();
                //Columns.Sort(Compare);

                // Setting  Column Name  List
                 
 
                FormResponseInfoModel.Columns = Columns  = GetGridColumns(FormsHierarchyDTOList, SurveyId,ViewId);

                //Getting Resposes
                List<SurveyAnswerDTO> ResponseListDTO = new List<SurveyAnswerDTO>();
                if (!bool.Parse(Session["IsSqlProject"].ToString()))
                {
                    ResponseListDTO = FormsHierarchyDTOList.FirstOrDefault(x => x.FormId == SurveyId).ResponseIds;
                }
                else
                {
                    //FormSettingReq.FormInfo.FormId = SurveyId;
                    //FormSettingReq.FormInfo.UserId = UserId;


                    FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
                    FormResponseReq.Criteria.PageNumber = 1;
                    FormResponseReq.Criteria.UserId = UserId;
                    FormResponseReq.Criteria.IsSqlProject = bool.Parse(Session["IsSqlProject"].ToString());
                    FormResponseReq.Criteria.IsChild = true;
                    FormResponseReq.Criteria.ParentResponseId = ResponseId;
                    // FormResponseReq.Criteria.IsShareable = bool.Parse(Session["IsShareable"].ToString());
                    //FormResponseReq.Criteria.UserOrganizationId = orgid;

                    ResponseListDTO = _isurveyFacade.GetFormResponseList(FormResponseReq).SurveyResponseList;//Pain Point
                }
                //Setting Resposes List
                List<ResponseModel> ResponseList = new List<ResponseModel>();
                foreach (var item in ResponseListDTO)
                {

                    if (item.SqlData != null)
                    {
                        ResponseList.Add(ConvertRowToModel(item, Columns));
                    }
                    else
                    {
                        ResponseList.Add(SurveyResponseXML.ConvertXMLToModel(item, Columns));
                    }
                }

                FormResponseInfoModel.ResponsesList = ResponseList;

                FormResponseInfoModel.PageSize = ReadPageSize();

                FormResponseInfoModel.CurrentPage = 1;
            }
            return FormResponseInfoModel;
        }

        private List<KeyValuePair<int, string>> GetGridColumns(List<FormsHierarchyDTO> FormsHierarchyDTOList, string SurveyId, int viewId)
        {

            //List<KeyValuePair<int, string>>  Columns = new List<KeyValuePair<int, string>>()
            //        {
            //            new KeyValuePair<int, string>(1, "_DateCreated"),
            //            new KeyValuePair<int, string>(2, "_DateUpdated"),
            //            //new KeyValuePair<int, string>(3, "Status"),
            //            new KeyValuePair<int, string>(4, "_Mode"),
            //           // new KeyValuePair<int, string>(5, "_RecordSourceId"),
            //        };
            List<KeyValuePair<int, string>> Columns = new List<KeyValuePair<int, string>>();
            var FormXml = FormsHierarchyDTOList.FirstOrDefault(x => x.FormId == SurveyId).SurveyInfo.XML;
            XDocument xdoc1 = XDocument.Parse(FormXml);
            // Get 5 first control names 
            int counter = 2;
            Columns.Add(new KeyValuePair<int, string>(1, "_DateCreated"));
            foreach (XElement Xelement in xdoc1.Descendants("Page").Elements("Field"))
            {
                if (counter<=5) {
                    string ColumnName = Xelement.Attribute("Name").Value;
                    Columns.Add(new KeyValuePair<int, string>(counter, ColumnName));


                }


                counter++;
            }

                return Columns;
        }

        private int ReadPageSize()
        {
            return Convert.ToInt16(WebConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"].ToString());
        }
        private ResponseModel ConvertRowToModel(SurveyAnswerDTO item, List<KeyValuePair<int, string>> Columns)
        {
            ResponseModel Response = new ResponseModel();

            Response.Column0 = item.SqlData["GlobalRecordId"];
            if (Columns.Count > 0)
            {
                Response.Column1 = item.SqlData[Columns[0].Value];
            }

            if (Columns.Count > 1)
            {
                Response.Column2 = item.SqlData[Columns[1].Value];
            }

            if (Columns.Count > 2)
            {
                Response.Column3 = item.SqlData[Columns[2].Value];
            }
            if (Columns.Count > 3)
            {
                Response.Column4 = item.SqlData[Columns[3].Value];
            }
            if (Columns.Count > 4)
            {
                Response.Column5 = item.SqlData[Columns[4].Value];
            }



            return Response;
        }
        private KeyValuePair<string, int> ValidateAll(MvcDynamicForms.Form form, int UserId, bool IsSubmited, bool IsSaved, bool IsMobileDevice, string FormValuesHasChanged)
        {
            List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy();
            KeyValuePair<string, int> result = new KeyValuePair<string, int>();
            // foreach (var FormObj in FormsHierarchy)
            for (int j = FormsHierarchy.Count() - 1; j >= 0; --j)
            {
                foreach (var Obj in FormsHierarchy[j].ResponseIds)
                {
                    SurveyAnswerDTO SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(Obj.ResponseId, Obj.SurveyId).SurveyResponseList[0];

                    SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId, FormsHierarchy);
                    SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
                    form = UpDateSurveyModel(surveyInfoModel, IsMobileDevice, FormValuesHasChanged, SurveyAnswer, true, FormsHierarchy);

                    for (int i = 1; i < form.NumberOfPages + 1; i++)
                    {
                        SourceTablesResponse SourceTables = null;
                        if (string.IsNullOrEmpty(form.SurveyInfo.ParentId))
                        {
                             SourceTables = _isurveyFacade.GetSourceTables(form.SurveyInfo.SurveyId);
                        }
                        else
                        {
                           SourceTables = _isurveyFacade.GetSourceTables(form.SurveyInfo.ParentId);

                        }
                        bool IsAndroid = false;
                        if (this.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            IsAndroid = true;
                        }
                        form = Epi.Web.MVC.Utility.FormProvider.GetForm(form.SurveyInfo, i, SurveyAnswer, IsMobileDevice, IsAndroid,SourceTables.List);
                        if (!form.Validate(form.RequiredFieldsList))
                        {
                            TempData["isredirect"] = "true";
                            TempData["Width"] = form.Width + 5;
                            //  return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, Obj.ResponseId, form, SurveyAnswer, IsSubmited, IsSaved, i, UserId);

                            result = new KeyValuePair<string, int>(Obj.ResponseId, i);
                            goto Exit;
                        }

                        // create my list of objects 

                    }

                }
            }

            Exit:
            return result;

        }
        public SurveyInfoModel GetSurveyInfo(string SurveyId, List<Epi.Web.Common.DTO.FormsHierarchyDTO> FormsHierarchyDTOList = null)
        {

           

            SurveyInfoModel surveyInfoModel = new SurveyInfoModel();
            if (FormsHierarchyDTOList != null)
            {
                surveyInfoModel = Mapper.ToSurveyInfoModel(FormsHierarchyDTOList.FirstOrDefault(x => x.FormId == SurveyId).SurveyInfo);
            }
            else
            {
                surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
            }
            return surveyInfoModel;

        }
        private MvcDynamicForms.Form UpDateSurveyModel(SurveyInfoModel surveyInfoModel, bool IsMobileDevice, string FormValuesHasChanged, SurveyAnswerDTO SurveyAnswer, bool IsSaveAndClose = false, List<FormsHierarchyDTO> FormsHierarchy = null)
        {
            MvcDynamicForms.Form form = new MvcDynamicForms.Form();
            int CurrentPageNum = GetSurveyPageNumber(SurveyAnswer.XML.ToString());

            bool IsAndroid = false;

            if (this.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsAndroid = true;
            }
            string url = "";
            if (this.Request.UrlReferrer == null)
            {
                url = this.Request.Url.ToString();
            }
            else
            {
                url = this.Request.UrlReferrer.ToString();
            }
            //  url = this.Request.Url.ToString();
            int LastIndex = url.LastIndexOf("/");
            string StringNumber = null;
            if (url.Length - LastIndex + 1 <= url.Length)
            {
                StringNumber = url.Substring(LastIndex, url.Length - LastIndex);
                StringNumber = StringNumber.Trim('/');
                if (StringNumber.Contains('?'))
                {
                    int Index = StringNumber.IndexOf('?');
                    StringNumber = StringNumber.Remove(Index);
                }
            }
            if (IsSaveAndClose)
            {
                StringNumber = "1";
            }
            if (int.TryParse(StringNumber, out ReffererPageNum))
            {
                if (ReffererPageNum != CurrentPageNum)
                {
                    form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, ReffererPageNum, SurveyAnswer, IsMobileDevice, null, FormsHierarchy, IsAndroid);
                    form.FormValuesHasChanged = FormValuesHasChanged;
                    if (IsMobileDevice)
                    {
                        //Epi.Web.MVC.Utility.MobileFormProvider.UpdateHiddenFields(ReffererPageNum, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                    }
                    else
                    {
                        Epi.Web.MVC.Utility.FormProvider.UpdateHiddenFields(ReffererPageNum, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                    }
                }
                else
                {
                    form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, CurrentPageNum, SurveyAnswer, IsMobileDevice, null, FormsHierarchy, IsAndroid, true);
                    form.FormValuesHasChanged = FormValuesHasChanged;
                    if (IsMobileDevice)
                    {
                       // Epi.Web.MVC.Utility.MobileFormProvider.UpdateHiddenFields(CurrentPageNum, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                    }
                    else
                    {
                        Epi.Web.MVC.Utility.FormProvider.UpdateHiddenFields(CurrentPageNum, form, XDocument.Parse(surveyInfoModel.XML), XDocument.Parse(SurveyAnswer.XML), this.ControllerContext.RequestContext.HttpContext.Request.Form);
                    }
                }


                if (!IsSaveAndClose)
                {
                    UpdateModel(form);
                }
            }
            else
            {
                //get the survey form
                form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, GetSurveyPageNumber(SurveyAnswer.XML.ToString()), SurveyAnswer, IsMobileDevice, null, FormsHierarchy, IsAndroid);
                form.FormValuesHasChanged = FormValuesHasChanged;
                form.ClearAllErrors();
                if (ReffererPageNum == 0)
                {
                    int index = 1;
                    if (StringNumber.Contains("?RequestId="))
                    {
                        index = StringNumber.IndexOf("?");
                    }

                    ReffererPageNum = int.Parse(StringNumber.Substring(0, index));

                }
                if (ReffererPageNum == CurrentPageNum)
                {
                    UpdateModel(form);
                }
                UpdateModel(form);
            }
            return form;
        }

    }

}
