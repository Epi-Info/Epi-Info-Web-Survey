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
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Utility;
using System.Text;

namespace Epi.Web.MVC.Controllers
{
    public class FinalController : Controller
    {
        private ISurveyFacade _isurveyFacade;

        /// <summary>
        /// Injecting SurveyTransactionObject through constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        private List<string> EmailAddressList = new  List<string>();
        public FinalController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }

        [HttpGet]
        public ActionResult Index(string surveyId, string final, string responseId)
        {

            try
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                ViewBag.Version = version;

                string SurveyMode = "";
                SurveyInfoModel surveyInfoModel = GetSurveyInfo(surveyId);
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");
                if (surveyInfoModel.ExitText != null)
                {
                    string exitText = regex.Replace(surveyInfoModel.ExitText.Replace("  ", " &nbsp;"), "<br />");
                    surveyInfoModel.ExitText = MvcHtmlString.Create(exitText).ToString();
                }
                string strPassCode = Epi.Web.MVC.Utility.SurveyHelper.GetPassCode();

                surveyInfoModel.PassCode = strPassCode;
                TempData["PassCode"] = strPassCode;

                if (surveyInfoModel.IsDraftMode)
                {
                    surveyInfoModel.IsDraftModeStyleClass = "draft";
                }
                else
                {
                    surveyInfoModel.IsDraftModeStyleClass = "final";
                }
                bool IsMobileDevice = false;
                IsMobileDevice = this.Request.Browser.IsMobileDevice;
                Omniture OmnitureObj = Epi.Web.MVC.Utility.OmnitureHelper.GetSettings(SurveyMode, IsMobileDevice);
                ViewBag.ResponseId = responseId;
                ViewBag.Omniture = OmnitureObj;
                FormsAuthentication.SignOut();
                //  FormsAuthentication.SetAuthCookie("BeginSurvey", false);
                if (surveyInfoModel.XML.Contains("EIWS_MetaData_Email_Notification_hidden")) {
                    SendNotification(surveyId, responseId);
                }
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, surveyInfoModel);
            }
            catch (Exception ex)
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage( ex, this.HttpContext);
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }

        
        [HttpPost]
        public ActionResult Index(string surveyId, SurveyAnswerModel surveyAnswerModel)
        {
            try
            {
                bool isMobileDevice = this.Request.Browser.IsMobileDevice;

                if (isMobileDevice == false)
                {
                    isMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
                }
                if (!string.IsNullOrEmpty(this.Request.Form["is_print_action"]) && this.Request.Form["is_print_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                    string ResponseId = this.Request.Form["ResponseId"];
                    ActionResult actionResult = RedirectToAction("Index", "Print", new { responseId = ResponseId , FromFinal = true});
                    return actionResult;
                    }
                //FormsAuthentication.SignOut();
                FormsAuthentication.SetAuthCookie("BeginSurvey", false);
                Guid responseId = Guid.NewGuid();
                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(surveyId, responseId.ToString());

                // Pass Code Logic  start 
                Epi.Web.Common.Message.UserAuthenticationResponse AuthenticationResponse = _isurveyFacade.GetAuthenticationResponse(responseId.ToString());

                string strPassCode = Epi.Web.MVC.Utility.SurveyHelper.GetPassCode();
                if (string.IsNullOrEmpty(AuthenticationResponse.PassCode))
                    {
                    _isurveyFacade.UpdatePassCode(responseId.ToString(), TempData["PassCode"].ToString());
                    }


                SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId);
                XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);
                MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, 1, SurveyAnswer, isMobileDevice);

                var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")
                                     select _FieldTypeID;

                foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                    bool isRequired;
                    string attributeValue = _FieldTypeID.Attribute("IsRequired").Value;

                    if (bool.TryParse(attributeValue, out isRequired))
                    {
                        if (isRequired)
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
                }
                
                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, form, SurveyAnswer, false, false, 1);

                return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseId = responseId, PageNumber = 1 });
            }
            catch (Exception ex)
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage(  ex, this.HttpContext);
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }

        }
        public SurveyInfoModel GetSurveyInfo(string SurveyId)
        {
            SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
            return surveyInfoModel;
        }
        private List<FormsHierarchyDTO> GetFormsHierarchy(string responseId)
        {
            FormsHierarchyResponse FormsHierarchyResponse = new FormsHierarchyResponse();
            FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            SurveyAnswerRequest ResponseIDsHierarchyRequest = new SurveyAnswerRequest();
            SurveyAnswerResponse ResponseIDsHierarchyResponse = new SurveyAnswerResponse();
            // FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            if (Session["RootFormId"] != null && responseId != null)
            {
                FormsHierarchyRequest.SurveyInfo.SurveyId = Session["RootFormId"].ToString();
                FormsHierarchyRequest.SurveyResponseInfo.ResponseId = responseId.ToString();
                FormsHierarchyResponse = _isurveyFacade.GetFormsHierarchy(FormsHierarchyRequest);

                SurveyAnswerDTO SurveyAnswerDTO = new SurveyAnswerDTO();
                SurveyAnswerDTO.ResponseId = responseId.ToString();
                ResponseIDsHierarchyRequest.SurveyAnswerList.Add(SurveyAnswerDTO);
                ResponseIDsHierarchyResponse = _isurveyFacade.GetSurveyAnswerHierarchy(ResponseIDsHierarchyRequest);
                FormsHierarchyResponse.FormsHierarchy = CombineLists(FormsHierarchyResponse.FormsHierarchy, ResponseIDsHierarchyResponse.SurveyResponseList);
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
        private List<PrintResponseModel> GetResponseList(string responseId, bool FromFinal)
        {
            List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy(responseId);
            SurveyModel SurveyModel = new SurveyModel();
            // SurveyModel.Form = form;
            SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, Session["RootFormId"].ToString());

            //   Common.Message.SurveyAnswerResponse answerResponse = _isurveyFacade.GetSurveyAnswerResponse(responseId);
            //
            List<PrintResponseModel> PrintList = new List<PrintResponseModel>();

            foreach (var form in SurveyModel.RelateModel)
            {



                foreach (var answerResponse in form.ResponseIds)
                {
                    PrintResponseModel PrintResponseModel = new PrintResponseModel();
                    SurveyInfoModel surveyInfoModel = GetSurveyInfo(answerResponse.SurveyId);
                    Common.Message.SurveyControlsRequest Request = new Common.Message.SurveyControlsRequest();
                    Request.SurveyId = answerResponse.SurveyId;
                    Common.Message.SurveyControlsResponse List = _isurveyFacade.GetSurveyControlList(Request);

                    var QuestionAnswerList = SurveyHelper.GetQuestionAnswerList(answerResponse.XML, List);
                    var SourceTables = _isurveyFacade.GetSourceTables(Session["RootFormId"].ToString());
                    PrintResponseModel.ResponseList = SurveyHelper.SetCommentLegalValues(QuestionAnswerList, List, surveyInfoModel, SourceTables);
                    PrintResponseModel.NumberOfPages = SurveyHelper.GetNumberOfPags(answerResponse.XML);
                    PrintResponseModel.SurveyName = surveyInfoModel.SurveyName;
                    PrintResponseModel.CurrentDate = DateTime.Now.ToString();
                    PrintResponseModel.ResponseId = responseId;
                    PrintResponseModel.SurveyId = form.FormId;
                    PrintResponseModel.IsFromFinal = FromFinal;
                    PrintList.Add(PrintResponseModel);
                }
            }

            return PrintList;
        }
        private void SendNotification(string surveyId, string responseId)
        {
            StringBuilder Body = new StringBuilder();
            string Sender = ConfigurationManager.AppSettings["EMAIL_FROM"].ToString();
            string Subject = "Epi Info Response Submission Notification";
            
           List <PrintResponseModel> list = GetResponseList(responseId, true);
            int NumberOfPages = list[0].NumberOfPages;
            Body.Append("Thank you for using Epi info web survey to submit your response to: " + list[0].SurveyName + "<br/>");
            //    Body.Append(" <style>.table, th, td { border: 1px solid black;}</style>");
            Body.Append("<style>.table { font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;border-collapse: collapse;width:90%;}.table td, #customers th {border: 1px solid #ddd; padding: 8px;} .table tr:nth-child(even){background-color: #f2f2f2;} .table tr:hover {background-color: #ddd;} .table th { padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #2f6fa3;color: white;}</style>");
            for (int i = 1; NumberOfPages + 1 > i; i++)
            {
                Body.Append(GetEmailInfo(list[0].ResponseList.Where(m=>m.PageNumber == i).ToList(), i));
                Body.Append("<br/>  ");
             
            }
            try
            {
                
                Epi.Web.Common.Email.Email EmailObj = new Common.Email.Email();
                EmailObj.Body = Body.ToString();

                EmailObj.From = Sender;
                EmailObj.Subject = Uri.UnescapeDataString(Subject);
                  
                EmailObj.To = EmailAddressList;
                bool EmailSent = Epi.Web.Common.Email.EmailHandler.SendMessage(EmailObj);
                
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }
        private string GetEmailInfo(List<PrintModel> Response, int PageNumber)
        {

            string EmailInfo = "<p>Page " + PageNumber + "</p> <br/>";
            EmailInfo += "<table class='table' width ='auto'>";
           
            EmailInfo += "<tr>";
            EmailInfo += "<th>Question</th>";
            EmailInfo += "<th>Answer</th>";
            
            EmailInfo += "</tr>";

           
            foreach (var row in Response) 
            {
                if (row.ControlName.Contains("EIWS_MetaData_Email_Notification_hidden"))
                {

                    this.EmailAddressList.Add(row.Value);
                }
                else
                {
                    EmailInfo += "<tr>";

                    EmailInfo += "<td>" + row.Question + "</td>";
                    EmailInfo += "<td>" + row.Value + "</td>";
                    EmailInfo += "</tr>";
                }
                     
                
            }
          

            EmailInfo += "</table> ";


            return EmailInfo;


        }
    }
}
