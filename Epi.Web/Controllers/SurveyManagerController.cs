using Epi.Web.Common;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Xml;
using Epi.Web.Common.Message ;
using Epi.Web.MVC.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.MVC.Facade;
using System.Configuration;
using Epi.Web.Common.Criteria;
using Epi.Web.MVC.Utility;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using Newtonsoft.Json.Linq;
using System.Collections;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Security;
using OfficeOpenXml.Drawing.Chart;
using Epi.Web.Common.Helper;

namespace Epi.Web.MVC.Controllers
{
    public class SurveyManagerController : Controller
    {
        private ISurveyFacade _isurveyFacade;
        public string FullPath = "";
        public SurveyManagerController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        [HttpGet]
        public ActionResult Index(bool ResetOrg = false)
        {

            string content = string.Empty;
            PublishModel Model = new PublishModel();

            ViewBag.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var IsAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;


            Session["IsAuthenticated"] = false;
            Session["IsNewOrg"] = false;
            string filepath = Server.MapPath("~\\Content\\Text\\TermOfUse.txt");
            try
            {
                using (var stream = new StreamReader(filepath))
                {
                    content = stream.ReadToEnd();
                }
            }
            catch (Exception exc)
            {

            }
            ViewData["TermOfUse1"] = content;
            var UserName = System.Web.HttpContext.Current.User.Identity.Name;

            if (IsAuthenticated && !ResetOrg)
            {
                Session["IsAuthenticated"] = Model.IsAuthenticated = IsAuthenticated;
                //check if the user exists 

                OrganizationAccountRequest Request = new OrganizationAccountRequest();
                Request.Admin = new AdminDTO();
                Request.Admin.AdminEmail = UserName;
                OrganizationAccountResponse Response = this._isurveyFacade.GetUserOrgId(Request);
                var OrgId = "";
                if (!string.IsNullOrEmpty(Response.OrganizationDTO.OrganizationKey))
                {

                    OrgId = Epi.Web.Common.Security.Cryptography.Decrypt(Response.OrganizationDTO.OrganizationKey);
                }
                if (Epi.Web.MVC.Utility.SurveyHelper.IsGuid(OrgId))
                {
                    Model.OrganizationKey = OrgId;
                    Model.IsValidOrg = ValidateOrganizationId(Model.OrganizationKey);

                    if (!Model.IsValidOrg)
                    {
                        ModelState.AddModelError("OrganizationKey", "Organization Key does not exist.");
                    }
                    else
                    {
                        Session["OrgId"] = Model.OrganizationKey;

                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); ;
                        OrganizationAccountRequest OrgRequest = new OrganizationAccountRequest();
                        OrgRequest.Organization.OrganizationKey = Model.OrganizationKey;
                        OrganizationAccountResponse OrganizationAccountResponse = this._isurveyFacade.GetOrg(OrgRequest);
                        Model.OrgName = OrganizationAccountResponse.OrganizationDTO.Organization;
                    }


                }
                else
                {
                    // create a new account 
                    Request.AccountType = "ORGANIZATION";
                    Request.Admin.IsActive = true;
                    Request.Admin.Notify = false;
                    Request.Organization = new OrganizationDTO();
                    Model.OrgName = Request.Organization.Organization = UserName.Replace("\\", " ") + "_Organization";
                    Request.Organization.IsEnabled = true;

                    Guid OrgKey = Guid.NewGuid();
                    Request.Organization.OrganizationKey = OrgKey.ToString();

                    Response = this._isurveyFacade.CreateAccount(Request);
                    if (Response.Message == "Success")
                    {
                        Model.OrganizationKey = OrgKey.ToString();
                        Model.IsValidOrg = ValidateOrganizationId(Model.OrganizationKey);

                        if (!Model.IsValidOrg)
                        {
                            ModelState.AddModelError("OrganizationKey", "Organization Key does not exist.");
                        }
                        else
                        {
                            Session["OrgId"] = Model.OrganizationKey;
                            ViewBag.IsNewOrg = Session["IsNewOrg"] = true;
                            Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); ;
                        }
                    }
                }
                Model.PublishDivState = true;
                string Token = "";
                if (!string.IsNullOrEmpty(UserName))
                {
                    UserName = UserName.Substring(4);
                    Token = Common.Security.Cryptography.GetToken(UserName);
                }
                Model.ViewRecordsURL = ConfigurationManager.AppSettings["ViewRecordsURL"] + Uri.EscapeDataString(Token);
                return View("Index", Model);
            }
            else
            {
                Model.PublishDivState = true;
                string Token = "";
                if (!string.IsNullOrEmpty(UserName)) {
                    UserName = UserName.Substring(4);
                    Token = Common.Security.Cryptography.GetToken(UserName);
                }
                Model.ViewRecordsURL = ConfigurationManager.AppSettings["ViewRecordsURL"] + Uri.EscapeDataString(Token);
                return View("Index", Model);
            }


        }
        [HttpPost]
        public ActionResult Index(PublishModel Model, string PublishSurvey, string DownLoadResponse, string ValidateOrganization, HttpPostedFileBase Newfile, HttpPostedFileBase Newfile1, HttpPostedFileBase Newfile2, string SurveyName, string RePublishSurvey, string RePublishSurveyName, string SendEmail, string SetJson, HttpPostedFileBase FileUpload1)
        {
            Model.ViewRecordsURL = ConfigurationManager.AppSettings["ViewRecordsURL"];
            string filepath = Server.MapPath("~\\Content\\Text\\TermOfUse.txt");
            try
            {
                using (var stream = new StreamReader(filepath))
                {
                    string content = stream.ReadToEnd();
                    ViewData["TermOfUse1"] = content;
                }
            }
            catch (Exception exc)
            {

            }


            try
            {
                ViewBag.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                bool IsAuthenticated = Model.IsAuthenticated = bool.Parse(Session["IsAuthenticated"].ToString());
                ViewBag.IsNewOrg = Session["IsNewOrg"]; ViewBag.Org = "OrganizationKey:";
                OrganizationAccountRequest OrgRequest = new OrganizationAccountRequest();
                if (Model.OrganizationKey != null)
                {
                    OrgRequest.Organization.OrganizationKey = Model.OrganizationKey;
                }
                else
                {
                    OrgRequest.Organization.OrganizationKey = Model.OrganizationKey = Session["OrgId"].ToString();
                }
                OrganizationAccountResponse Response = this._isurveyFacade.GetOrg(OrgRequest);
                Model.OrgName = Response.OrganizationDTO.Organization;

                if (!string.IsNullOrEmpty(ValidateOrganization))
                {
                    ModelState["FileName"].Errors.Clear();
                    ModelState["SurveyName"].Errors.Clear();
                    ModelState["RepublishUserPublishKey"].Errors.Clear();
                    ModelState["UserPublishKey"].Errors.Clear();
                    ModelState["Path"].Errors.Clear();
                    ModelState["RepublishPath"].Errors.Clear();
                    ModelState["ValueSetFilePath"].Errors.Clear();
                    ModelState["EndDate"].Errors.Clear();
                    ModelState["RePublishDivState"].Errors.Clear();
                    ModelState["PublishDivState"].Errors.Clear();
                    ModelState["DownLoadDivState"].Errors.Clear();
                    ModelState["ValueSetUserPublishKey"].Errors.Clear();
                    if (ModelState.IsValid)
                    {
                        //  OrganizationAccountRequest Request = new OrganizationAccountRequest();

                        //  Request.Organization.OrganizationKey = Model.OrganizationKey;
                        //OrganizationAccountResponse Response = this._isurveyFacade.GetOrg(Request);
                        if (Response.OrganizationDTO != null)
                        {
                            Model.IsValidOrg = true;//ValidateOrganizationId(Model.OrganizationKey);
                        }
                        else
                        {
                            Model.IsValidOrg = false;
                        }
                        // Model.OrgName = Response.OrganizationDTO.Organization;



                        if (!Model.IsValidOrg)
                        {
                            ModelState.AddModelError("OrganizationKey", "Organization Key does not exist.");
                        }
                        else
                        {
                            Session["OrgId"] = Model.OrganizationKey;
                            ViewBag.SurveyNameList1 = Model.SurveyNameList = GetAllSurveysByOrgId(Model.OrganizationKey);



                        }
                    }
                    return View("Index", Model);
                }
                if (!string.IsNullOrEmpty(PublishSurvey))
                {
                    Model.IsValidOrg = true;
                    Model.OrganizationKey = Session["OrgId"].ToString();
                    Model.SurveyNameList = GetAllSurveysByOrgId(Model.OrganizationKey);

                    if (string.IsNullOrEmpty(Request.Form["ExistingSurvey"]))
                    {
                        Model.UpdateExisting = false;
                    }
                    else
                    {
                        Model.UpdateExisting = true;
                        if (string.IsNullOrEmpty(Model.SurveyKey))
                        {
                            ModelState.AddModelError("SurveyKey", "Survey Id is required.");
                        }

                    }
                    ModelState["FileName"].Errors.Clear();
                    ModelState["OrganizationKey"].Errors.Clear();
                    ModelState["UserPublishKey"].Errors.Clear();
                    ModelState["RepublishUserPublishKey"].Errors.Clear();
                    ModelState["RepublishPath"].Errors.Clear();
                    ModelState["ValueSetFilePath"].Errors.Clear();
                    ModelState["ValueSetUserPublishKey"].Errors.Clear();
                    if (ModelState.IsValid)
                    {
                        var response = DoPublish(Model, Newfile);
                        Model.SuccessfulPublish = true;
                        if (response.SurveyInfoList.Count() > 0)
                        {
                            Model.SurveyKey = response.SurveyInfoList[0].SurveyId.ToString();
                            Model.UserPublishKey = response.SurveyInfoList[0].UserPublishKey.ToString();
                            Model.SurveyURL = ConfigurationManager.AppSettings["URL"] + response.SurveyInfoList[0].SurveyId.ToString();
                        }

                    }
                    else
                    {

                        // ModelState.AddModelError("Error", "Please validate the information provided and try publishing again.");
                        //  Model.Path = "";
                        Model.SuccessfulPublish = false;
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); ;
                        return View("Index", Model);


                    }


                    return View(Model);
                }
                // RePublish start
                if (!string.IsNullOrEmpty(RePublishSurvey))
                {
                    Model.IsValidOrg = true;
                    Model.OrganizationKey = Session["OrgId"].ToString();
                    //  Model.SurveyNameList = GetAllSurveysByOrgId(Model.OrganizationKey);
                    if (Model.RepublishSurveyMode == "0")
                    {
                        Model.IsDraft = true;
                    }
                    else
                    {
                        Model.IsDraft = false;
                    }

                    Model.UpdateExisting = true;
                    if (string.IsNullOrEmpty(Model.RepublishSurveyKey))
                    {
                        ModelState.AddModelError("RepublishSurveyKey", "Survey Id is required.");
                    }


                    ModelState["SurveyName"].Errors.Clear();
                    ModelState["FileName"].Errors.Clear();
                    ModelState["OrganizationKey"].Errors.Clear();
                    ModelState["UserPublishKey"].Errors.Clear();
                    ModelState["RepublishPath"].Errors.Clear();
                    ModelState["ValueSetFilePath"].Errors.Clear();
                    ModelState["ValueSetUserPublishKey"].Errors.Clear();
                    if (IsAuthenticated)
                    {
                        ModelState["RePublishUserPublishKey"].Errors.Clear();
                    }
                    ModelState["Path"].Errors.Clear();
                    ModelState["EndDate"].Errors.Clear();
                    if (ModelState.IsValid)
                    {

                        Model.SurveyName = RePublishSurveyName;
                        if (IsAuthenticated)
                        {
                            Model.RepublishUserPublishKey = GetUserPublishKey(Model.RepublishSurveyKey.ToString());
                        }
                        var response = DoPublish(Model, Newfile1);
                        Model.SuccessfulPublish = true;
                        Model.SurveyKey = response.SurveyInfoList[0].SurveyId.ToString();
                        if (!IsAuthenticated)
                        {
                            Model.UserPublishKey = response.SurveyInfoList[0].UserPublishKey.ToString();
                        }
                        Model.SurveyURL = ConfigurationManager.AppSettings["URL"] + response.SurveyInfoList[0].SurveyId.ToString();
                    }
                    else
                    {

                        // ModelState.AddModelError("Error", "Please validate the information provided and try publishing again.");
                        //  Model.Path = "";
                        Model.SuccessfulPublish = false;
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey);
                        return View("Index", Model);


                    }
                    Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); ;

                    return View(Model);
                }
                // RePublish End

                if (!string.IsNullOrEmpty(DownLoadResponse))
                {
                    Model.IsValidOrg = true;
                    Model.OrganizationKey = Session["OrgId"].ToString();
                    // Model.SurveyNameList = GetAllSurveysByOrgId(Model.OrganizationKey);
                    if (string.IsNullOrEmpty(Model.SurveyKey))
                    {
                        ModelState.AddModelError("SurveyKey", "Survey Id is required.");
                    }
                    //ModelState["FileName"].Errors.Clear();
                    // ModelState["SurveyKey"].Errors.Clear();
                    ModelState["OrganizationKey"].Errors.Clear();
                    ModelState["SurveyName"].Errors.Clear();
                    ModelState["Path"].Errors.Clear();
                    ModelState["EndDate"].Errors.Clear();
                    ModelState["RepublishPath"].Errors.Clear();
                    ModelState["RepublishUserPublishKey"].Errors.Clear();
                    ModelState["ValueSetFilePath"].Errors.Clear();
                    ModelState["ValueSetUserPublishKey"].Errors.Clear();
                    if (IsAuthenticated)
                    {
                        ModelState["UserPublishKey"].Errors.Clear();
                    }
                    if (ModelState.IsValid)
                    {
                        DoDownLoad(Model);


                    }
                    else
                    {
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey);
                        return View("Index", Model);
                    }
                }
                //send email notification
                if (!string.IsNullOrEmpty(SendEmail))
                {
                    Model.IsValidOrg = true;
                    var orgkey = Model.OrganizationKey = Session["OrgId"].ToString();
                    // Model.SurveyNameList = GetAllSurveysByOrgId(Model.OrganizationKey);
                    if (string.IsNullOrEmpty(Model.EmailSurveyKey))
                    {
                        ModelState.AddModelError("EmailSurveyKey", "Survey Id is required.");
                    }
                    ModelState["FileName"].Errors.Clear();
                    ModelState["SurveyName"].Errors.Clear();
                    ModelState["RepublishUserPublishKey"].Errors.Clear();
                    ModelState["UserPublishKey"].Errors.Clear();
                    ModelState["Path"].Errors.Clear();
                    ModelState["RepublishPath"].Errors.Clear();
                    ModelState["EndDate"].Errors.Clear();
                    ModelState["RePublishDivState"].Errors.Clear();
                    ModelState["PublishDivState"].Errors.Clear();
                    ModelState["DownLoadDivState"].Errors.Clear();
                    ModelState["OrganizationKey"].Errors.Clear();
                    ModelState["ValueSetFilePath"].Errors.Clear();
                    ModelState["ValueSetUserPublishKey"].Errors.Clear();



                    if (IsAuthenticated && ModelState["EmailUserPublishKey"] != null)
                    {
                        ModelState["EmailUserPublishKey"].Errors.Clear();
                    }

                    if (ModelState.IsValid)
                    {
                        var SurveyInfo = this._isurveyFacade.GetSurveyInfoModel(Model.EmailSurveyKey);
                        if (IsAuthenticated || SurveyInfo.UserPublishKey.ToString() == Model.EmailUserPublishKey)
                        {
                            SendEmailNotification(Model, Newfile2, SurveyInfo);

                            Model.SuccessfulPublish = true;
                            Model.SuccessfullySentEmail = true;
                        }
                        else
                        {
                            ModelState.AddModelError("EmailUserPublishKey", "Please provide the right Security Token.");
                            Model.SendEmaiDivState = true;
                        }
                        // Model = new PublishModel();
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(orgkey);
                        return View(Model);
                    }
                    else
                    {
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(orgkey);
                        return View("Index", Model);
                    }
                }

                if (!string.IsNullOrEmpty(SetJson))
                {
                    try
                    {
                        var SurveyIds = GetAllSurveysByOrgId(Session["OrgId"].ToString());

                        foreach (var SurveyInfo in SurveyIds)
                        {
                            SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyInfo.Value);
                            SurveyAnswerRequest SurveyAnswerRequest = SetMessageObject(true, SurveyInfo.Value, new Guid(surveyInfoModel.UserPublishKey.ToString()), Session["OrgId"].ToString(), false, -2);
                            SurveyAnswerResponse SurveyAnswerResponse = new Common.Message.SurveyAnswerResponse();
                            SurveyAnswerResponse = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswerRequest);
                            int PageSize = SurveyAnswerResponse.PageSize;
                            if (PageSize > 0)
                            {

                                for (int i = 1; SurveyAnswerResponse.NumberOfPages > i - 1; i++)
                                {
                                    int TotalCount = 0;
                                    SurveyControlsRequest Request = new SurveyControlsRequest();
                                    Request.SurveyId = SurveyInfo.Value;
                                    SurveyControlsResponse List = _isurveyFacade.GetSurveyControlList(Request);


                                    SurveyAnswerRequest _Request = SetMessageObject(false, SurveyInfo.Value, new Guid(surveyInfoModel.UserPublishKey.ToString()), Session["OrgId"].ToString(), false, -2, i, PageSize);
                                    SurveyAnswerResponse Responses = _isurveyFacade.GetSurveyAnswerResponse(_Request);

                                    //var Responses = _isurveyFacade.GetFormResponseList(SurveyAnswerRequest);
                                    foreach (var ResponseDetail in Responses.SurveyResponseList)
                                    {
                                        // if (string.IsNullOrEmpty(ResponseDetail.Json)) {
                                        List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy(SurveyInfo.Value, ResponseDetail.ResponseId);
                                        var json = _isurveyFacade.GetSurveyResponseJson(ResponseDetail, FormsHierarchy, List);
                                        if (!string.IsNullOrEmpty(json))
                                        {
                                            bool temp = this._isurveyFacade.SetJsonColumn(json, ResponseDetail.ResponseId);
                                        }
                                        //  }
                                    }
                                }
                            }
                        }

                        Model.SetJsonDivState = true;
                        Model.JsonIsSet = true;
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString());
                        return View("Index", Model);
                    }
                    catch (Exception ex)
                    {
                        Model.SetJsonDivState = true;

                        return View("Index", Model);
                        throw ex;
                    }

                }


                if (FileUpload1 != null)
                {
                    Model.IsValidOrg = true;
                    var orgkey = Model.OrganizationKey = Session["OrgId"].ToString();
                    ModelState["FileName"].Errors.Clear();
                    ModelState["SurveyName"].Errors.Clear();
                    ModelState["RepublishUserPublishKey"].Errors.Clear();
                    ModelState["UserPublishKey"].Errors.Clear();
                    ModelState["Path"].Errors.Clear();
                    ModelState["RepublishPath"].Errors.Clear();
                    ModelState["OrganizationKey"].Errors.Clear();
                    ModelState["EndDate"].Errors.Clear();
                    ModelState["ValueSetFilePath"].Errors.Clear();
                    ModelState["ValueSetUserPublishKey"].Errors.Clear();


                    if (ModelState.IsValid)
                    {
                        string ValueSetXml =  GetValueSetXml(Model.SelectedValueSet, FileUpload1);
                        SourceTablesRequest SourceTablesRequest = new SourceTablesRequest();
                        SourceTablesRequest.List = new List<SourceTableDTO>();
                        SourceTableDTO SourceTableDTO = new SourceTableDTO();
                        SourceTableDTO.TableXml = ValueSetXml;
                        SourceTableDTO.TableName = Model.SelectedValueSet;
                        SourceTablesRequest.List.Add(SourceTableDTO);
                        SourceTablesRequest.SurveyId = Model.ValueSetSurveyKey;
                        _isurveyFacade.UpdateSourceTable(SourceTablesRequest);
                    }
                   
                    Model.UpdateDataSetDivState = true;
                    Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString());
                    return View("Index", Model);
                }
                    }
            catch (Exception ex)
            {
                ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey);
                Model.SuccessfulPublish = false;
                ModelState.AddModelError("Error", "Please validate the information provided.");
                return View("Index", Model);
            }
            return View("Index", Model);
        }
        private List<FormsHierarchyDTO> GetFormsHierarchy(string RootFormId, string RootResponseId)
        {
            FormsHierarchyResponse FormsHierarchyResponse = new FormsHierarchyResponse();
            FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            SurveyAnswerRequest ResponseIDsHierarchyRequest = new SurveyAnswerRequest();
            SurveyAnswerResponse ResponseIDsHierarchyResponse = new SurveyAnswerResponse();
            // FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            try
            {
                if (RootFormId != null && RootResponseId != null)
                {
                    FormsHierarchyRequest.SurveyInfo.SurveyId = RootFormId.ToString();
                    FormsHierarchyRequest.SurveyResponseInfo.ResponseId = RootResponseId.ToString();
                    FormsHierarchyResponse = _isurveyFacade.GetFormsHierarchy(FormsHierarchyRequest);

                    SurveyAnswerDTO SurveyAnswerDTO = new SurveyAnswerDTO();
                    SurveyAnswerDTO.ResponseId = RootResponseId.ToString();
                    ResponseIDsHierarchyRequest.SurveyAnswerList.Add(SurveyAnswerDTO);
                    ResponseIDsHierarchyResponse = _isurveyFacade.GetSurveyAnswerHierarchy(ResponseIDsHierarchyRequest);
                    FormsHierarchyResponse.FormsHierarchy = CombineLists(FormsHierarchyResponse.FormsHierarchy, ResponseIDsHierarchyResponse.SurveyResponseList);
                }
            }
            catch (Exception ex)
            {
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

        private string GetValueSetXml(string ValueSetName, HttpPostedFileBase newfile2)
        {

            StringBuilder Xml = new StringBuilder();
            try
            {



                StreamReader reader = new StreamReader(newfile2.InputStream);

                string[] headers = reader.ReadLine().Split(',');

                string attributesFormat = string.Join(" ", headers.Select((colStr, colIdx) => string.Format("{0}=\"{{{1}}}\"", colStr, colIdx)));


                string rowFormat = string.Format("{0}", attributesFormat);

                string line;

                Xml.Append("<SourceTable TableName=\"" + ValueSetName + "\" >");
                while ((line = reader.ReadLine()) != null)
                {
                    string Element = "<Item " + string.Format(rowFormat, line.Split(',')) + " />";

                    Xml.Append(Element);


                }
                Xml.Append("</SourceTable>");

            }
            catch (Exception ex)
            {

                throw ex;
            }
            XDocument doc = XDocument.Parse(Xml.ToString());

            return doc.ToString();
        }
        private  string GetValueSetXml(string ValueSetName, string JsonString)
            {

                StringBuilder Xml = new StringBuilder();
                try
                {

                  
                var objects = JsonConvert.DeserializeObject<List<object>>(JsonString);
                var valueSetObject = objects.Select(obj => JsonConvert.SerializeObject(obj)).ToArray();


                Xml.Append("<SourceTable TableName=\"" + ValueSetName + "\" >");
                    foreach (var item in valueSetObject)
                    {
                    StringBuilder Element = new StringBuilder();
                    Element.Append("<Item ");
                    var Dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(item);
                    //   string Element = "<Item " + string.Format(rowFormat, line.Split(',')) + " />";
                    foreach (var kv in Dictionary)
                    {

                        Element.Append(kv.Key +"=\""+kv.Value+"\" " );

                    }
                    Element.Append("/>");
                    Xml.Append(Element);
                }
                Xml.Append("</SourceTable>");

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                XDocument doc = XDocument.Parse(Xml.ToString());

                return doc.ToString();

            }


            private void SendEmailNotification(PublishModel model, HttpPostedFileBase newfile2, SurveyInfoModel SurveyInfo)
        {


            using (var package = new OfficeOpenXml.ExcelPackage(newfile2.InputStream))
            {
                OfficeOpenXml.ExcelWorkbook workbook = package.Workbook;
                string Subject = workbook.Worksheets[2].Cells[2, 1].Text;
                StringBuilder Body = new StringBuilder( workbook.Worksheets[2].Cells[2, 2].Text);
                string Sender = workbook.Worksheets[2].Cells[2, 3].Text;

                string Subject2 = workbook.Worksheets[2].Cells[3, 1].Text;
                StringBuilder Body2 = new StringBuilder(workbook.Worksheets[2].Cells[3, 2].Text);
                string Sender2 = workbook.Worksheets[2].Cells[3, 3].Text;

                ExcelWorksheet xlWorksheet = workbook.Worksheets[1];

                var start = xlWorksheet.Dimension.Start;
                var end = xlWorksheet.Dimension.End;


                if (!model.IsBulk)
                {
                    for (int row = 2; row <= end.Row; row++)
                    { // Row by row...
                        Body = new StringBuilder(workbook.Worksheets[2].Cells[2, 2].Text);
                        Body2 = new StringBuilder(workbook.Worksheets[2].Cells[3, 2].Text);
                        int DoSend = 0;
                        int.TryParse(xlWorksheet.Cells[row, 1].Text, out DoSend);
                        // validate email Address
                        bool IsValid = ValidateEmail(xlWorksheet.Cells[row, 2].Text);
                        Dictionary<string, string> SurveyQuestionAnswerList = new Dictionary<string, string>();
                        if (IsValid)
                        {

                            string UserEamil = xlWorksheet.Cells[row, 2].Text;
                            string _ResponseId = xlWorksheet.Cells[row, 3].Text;
                            string _Status = xlWorksheet.Cells[row, 4].Text;
                            string SurveyUrl = "";
                            int ResponseStatuse = 1;
                            string DateCompleted = "";
                            // Add ResponseId , status and Date sent to EXCEL 

                            if (string.IsNullOrEmpty(_Status))
                            {
                                xlWorksheet.SetValue(row, 4, "InProgress");

                            }
                            xlWorksheet.SetValue(row, 5, DateTime.Now.ToShortDateString());




                            // Create a response

                            if (string.IsNullOrEmpty(_ResponseId))
                            {
                                string strPassCode;
                                Guid ResponseID;
                                CreateResponse(SurveyInfo, out SurveyUrl, out strPassCode, out ResponseID);    // CreateResponse
                                                                                                               // Update response if  key value pair availeble 

                                bool IsPreFiledValues = false;
                                if (!string.IsNullOrEmpty(xlWorksheet.Cells[row, 10].Text))
                                {

                                    IsPreFiledValues = true;

                                }
                                if (IsPreFiledValues)
                                {

                                    ResponseID = UpdateResponse(SurveyInfo, xlWorksheet, end, row, ResponseID, out SurveyQuestionAnswerList);   // UpdateResponse

                                }
                                xlWorksheet.SetValue(row, 3, ResponseID.ToString());
                                xlWorksheet.SetValue(row, 6, SurveyUrl);
                                xlWorksheet.SetValue(row, 7, strPassCode);
                            }
                            else
                            {
                                // Get Response status
                                GetResponseStatus(model, _ResponseId, out SurveyUrl, out ResponseStatuse, out DateCompleted); //GetResponseStatus
                            }
                            // send email
                            if (DoSend == 1)
                            {
                                Epi.Web.Common.Email.Email EmailObj = new Common.Email.Email();
                                if (ResponseStatuse < 3 && !string.IsNullOrEmpty(SurveyUrl))
                                {
                                    Body.AppendLine();
                                    Body.Append( GetEmailInfo(SurveyQuestionAnswerList, SurveyUrl));
                                    SendEmail(Subject, Body.ToString(), Sender, xlWorksheet, row, UserEamil); //Initial  email 

                                }
                                else
                                {
                                    if (ResponseStatuse != 3)
                                    {

                                        SendEmail(Subject2, Body2.ToString(), Sender2, xlWorksheet, row, UserEamil);  //Reminder email 

                                    }
                                    var _ResponseStatus = GetStatus(ResponseStatuse);
                                    xlWorksheet.SetValue(row, 4, _ResponseStatus);
                                    xlWorksheet.SetValue(row, 8, DateCompleted);



                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(xlWorksheet.Cells[row, 2].Text))
                            {
                                xlWorksheet.SetValue(row, 9, "Email address is not valid. Please check email address and try again.");
                            }
                        }

                    }

                }
                else
                {
                    //////////////////////Bulk logic/////////////////////////////////////////////////
                    int DoSend = 0;
                    int.TryParse(xlWorksheet.Cells[2, 1].Text, out DoSend);
                    // validate email Address
                    bool IsValid = ValidateEmail(xlWorksheet.Cells[2, 2].Text);
                    string UserEamil = xlWorksheet.Cells[2, 2].Text;
                   
                    string Initial = xlWorksheet.Cells[2, 3].Text;

                    List<BulkResponsesModel> BulkResponsesModelList = new List<BulkResponsesModel>();
                    int ResponseStatuse = 1;
                    for (int row = 2; row <= end.Row; row++)
                    {
                        Dictionary<string, string> SurveyQuestionAnswerList = new Dictionary<string, string>();
                        if (IsValid)
                        {


                            string _ResponseId = xlWorksheet.Cells[row, 3].Text;
                            string _Status = xlWorksheet.Cells[row, 4].Text;
                            string SurveyUrl = "";

                            string DateCompleted = "";
                            // Add ResponseId , status and Date sent to EXCEL 

                            if (string.IsNullOrEmpty(_Status))
                            {
                                xlWorksheet.SetValue(row, 4, "InProgress");

                            }
                            xlWorksheet.SetValue(row, 5, DateTime.Now.ToShortDateString());




                            // Create a response

                            if (string.IsNullOrEmpty(_ResponseId))
                            {
                                string strPassCode;
                                Guid ResponseID;
                                CreateResponse(SurveyInfo, out SurveyUrl, out strPassCode, out ResponseID);    // CreateResponse
                                                                                                               // Update response if  key value pair availeble 

                                bool IsPreFiledValues = false;
                                if (!string.IsNullOrEmpty(xlWorksheet.Cells[row, 10].Text))
                                {

                                    IsPreFiledValues = true;

                                }
                                if (IsPreFiledValues)
                                {

                                    ResponseID = UpdateResponse(SurveyInfo, xlWorksheet, end, row, ResponseID, out SurveyQuestionAnswerList);   // UpdateResponse

                                }
                                xlWorksheet.SetValue(row, 3, ResponseID.ToString());
                                xlWorksheet.SetValue(row, 6, SurveyUrl);
                                xlWorksheet.SetValue(row, 7, strPassCode);
                            }
                            else
                            {
                                // Get Response status
                                GetResponseStatus(model, _ResponseId, out SurveyUrl, out ResponseStatuse, out DateCompleted); //GetResponseStatus
                                
                                
                            }

                            var _ResponseStatus = GetStatus(ResponseStatuse);
                            xlWorksheet.SetValue(row, 4, _ResponseStatus);
                            xlWorksheet.SetValue(row, 8, DateCompleted);


                            BulkResponsesModel BulkResponsesModel = new BulkResponsesModel();

                            BulkResponsesModel.Url = SurveyUrl;
                            BulkResponsesModel.SurveyQuestionAnswerList = SurveyQuestionAnswerList;
                            BulkResponsesModel.DateCompleted = DateCompleted;
                            BulkResponsesModel.Status = ResponseStatuse;
                           BulkResponsesModelList.Add(BulkResponsesModel);
                          
                       

                        }
                        else
                        {
                            xlWorksheet.SetValue(2, 9, "Email address is not valid. Please check email address and try again.");
                        }
                    }

                      if (BulkResponsesModelList.Count > 0 && DoSend == 1)
                    {
                        int InitialEmailCounter = 0;
                        int ReminderEmailCounter = 0;

                        foreach (var item in BulkResponsesModelList)
                        {
                            if (item.Status < 3 && string.IsNullOrEmpty(Initial)) //Initial  email 
                            {
                                Body.AppendLine();
                                Body.Append(GetEmailInfo(item.SurveyQuestionAnswerList, item.Url));
                                InitialEmailCounter++;
                            }
                            else
                            { 
                                if (item.Status != 3) //Reminder email 
                                {
                                    Body2.AppendLine();
                                    Body2.Append(GetEmailInfo(item.SurveyQuestionAnswerList, item.Url));
                                    ReminderEmailCounter++;
                                }
                        }
                        }
                      //  SendEmail(Subject, Body.ToString(), Sender, xlWorksheet, 2, UserEamil); //Initial  email 
                        if (InitialEmailCounter > 0)
                        {

                            SendEmail(Subject, Body.ToString(), Sender, xlWorksheet, 2, UserEamil); //Initial  email 

                        }
                      
                        if (ReminderEmailCounter >0)
                        {

                            SendEmail(Subject2, Body2.ToString(), Sender2, xlWorksheet, 2, UserEamil);  //Reminder email 

                        }




                      

                    }

                }


                // adding a sumery 


                // GetPieChart(InProgress, Saved, Submited, workbook);

                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + SurveyInfo.SurveyName + ".xlsx");
                    package.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }



            }
        }

        private string GetUrlstring(List<string> uRL_List)
        {
            StringBuilder UrlString = new StringBuilder();
            foreach (var url in uRL_List) {
                UrlString.Append(url);
                UrlString.AppendLine();

            }


            return UrlString.ToString();
        }

        private void GetResponseStatus(PublishModel model, string _ResponseId, out string SurveyUrl, out int ResponseStatuse, out string DateCompleted)
        {
            var SurveyResponse = _isurveyFacade.GetSurveyAnswerResponse(_ResponseId, model.EmailSurveyKey).SurveyResponseList[0];
            ResponseStatuse = SurveyResponse.Status;
            DateCompleted = SurveyResponse.DateCompleted.ToString();
            SurveyUrl = ConfigurationManager.AppSettings["ResponseURL"] + SurveyResponse.ResponseId.ToString() + "/" + SurveyResponse.PassCode;
        }

        private static void SendEmail(string Subject, string Body, string Sender, ExcelWorksheet xlWorksheet, int row, string UserEamil)
        {
            try
            {
                Epi.Web.Common.Email.Email EmailObj = new Common.Email.Email();
                EmailObj.Body = Body;// + "\n\n Survey URL: \n" + SurveyUrl;

                EmailObj.From = Sender;// ConfigurationManager.AppSettings["EMAIL_FROM"].ToString();
                EmailObj.Subject = Uri.UnescapeDataString(Subject);


                List<string> tempList = new List<string>();
                tempList.Add(UserEamil);
                EmailObj.To = tempList;
                bool EmailSent = Epi.Web.Common.Email.EmailHandler.SendMessage(EmailObj);
                if (EmailSent)
                {
                    xlWorksheet.SetValue(row, 9, "Email sent successfully!");
                }
                else
                {
                    xlWorksheet.SetValue(row, 9, "Error occurred while sending email.");
                }
            }
            catch (Exception ex)
            {
                xlWorksheet.SetValue(row, 9, "Error occurred while sending email.");
                //throw ex;
            }
        }

        private Guid UpdateResponse(SurveyInfoModel SurveyInfo, ExcelWorksheet xlWorksheet, ExcelCellAddress end, int row, Guid ResponseID, out Dictionary<string, string> SurveyQuestionAnswerList)
        {
            XDocument SurveyXml = XDocument.Parse(SurveyInfo.XML);
            SurveyQuestionAnswerList = new Dictionary<string, string>();
            Epi.Web.Common.Message.PreFilledAnswerRequest request = new PreFilledAnswerRequest();
            request.AnswerInfo.SurveyQuestionAnswerList = new Dictionary<string, string>();
            for (int cell = 10; cell <= end.Column; cell++)
            {
                if (xlWorksheet.Cells[1, cell].Value != null) {
                    string ColumnName = xlWorksheet.Cells[1, cell].Value.ToString();
                    string Value = xlWorksheet.Cells[row, cell].Value.ToString();
                    request.AnswerInfo.SurveyQuestionAnswerList.Add(ColumnName, Value);
                    SurveyQuestionAnswerList.Add(ColumnName, Value);
                }
            }




            Epi.Web.Common.Xml.SurveyResponseXML Implementation = new Epi.Web.Common.Xml.SurveyResponseXML(request, SurveyXml);
            string ResponseXml = Implementation.CreateResponseDocument(SurveyXml).ToString();
            SurveyAnswerRequest Request = new SurveyAnswerRequest();
            SurveyAnswerDTO DTO = new SurveyAnswerDTO();
            DTO.SurveyId = SurveyInfo.SurveyId;
            DTO.ResponseId = ResponseID.ToString();
            DTO.Status = 1;
            DTO.XML = ResponseXml;
            Request.Action = "Update";
            Request.SurveyAnswerList.Add(DTO);
            _isurveyFacade.SaveSurveyAnswer(Request);
            return ResponseID;
        }

        private void CreateResponse(SurveyInfoModel SurveyInfo, out string SurveyUrl, out string strPassCode, out Guid ResponseID)
        {
            strPassCode = Epi.Web.MVC.Utility.SurveyHelper.GetPassCode();
            ResponseID = Guid.NewGuid();
            SurveyUrl = ConfigurationManager.AppSettings["ResponseURL"] + ResponseID.ToString() + "/" + strPassCode;

            Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(SurveyInfo.SurveyId, ResponseID.ToString());
            _isurveyFacade.UpdatePassCode(ResponseID.ToString(), strPassCode);
        }

        private bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }

        }

        private static void GetPieChart(int InProgress, int Saved, int Submited, ExcelWorkbook workbook)
        {


            //workbook.Properties.Title = "Charts With Excel";
            try
            {
                workbook.Worksheets.Delete("Summary Report");
            }
            catch (Exception ex)
            {


            }

            var ws = workbook.Worksheets.Add("Summary Report");
            var data = new List<KeyValuePair<string, int>>();
            data.Add(new KeyValuePair<string, int>("InProgress", InProgress));
            data.Add(new KeyValuePair<string, int>("Saved", Saved));
            data.Add(new KeyValuePair<string, int>("Submited", Submited));

            //Fill the table
            var startCell = ws.Cells[1, 1];
            startCell.Offset(0, 0).Value = "Response Status";
            startCell.Offset(0, 1).Value = "Number Of Responses";
            startCell.Offset(1, 0).LoadFromCollection(data);

            //Add the chart to the sheet
            var pieChart = ws.Drawings.AddChart("Chart1", eChartType.Pie);
            pieChart.SetPosition(data.Count + 1, 0, 0, 0);
            pieChart.SetSize(500, 400);
            pieChart.Title.Text = "Response Status";

            //Set the data range
            var series = pieChart.Series.Add(ws.Cells[2, 2, data.Count + 1, 2], ws.Cells[2, 1, data.Count + 1, 1]);
            var pieSeries = (ExcelPieChartSerie)series;
            pieSeries.Explosion = 5;

            //Format the labels
            pieSeries.DataLabel.ShowValue = true;
            pieSeries.DataLabel.ShowPercent = true;
            pieSeries.DataLabel.ShowLeaderLines = true;
            pieSeries.DataLabel.Separator = ";  ";
            pieSeries.DataLabel.Position = eLabelPosition.BestFit;

            var xdoc = pieChart.ChartXml;
            var nsuri = xdoc.DocumentElement.NamespaceURI;
            var nsm = new XmlNamespaceManager(xdoc.NameTable);
            nsm.AddNamespace("c", nsuri);

            //Added the number format node via XML
            var numFmtNode = xdoc.CreateElement("c:numFmt", nsuri);

            var formatCodeAtt = xdoc.CreateAttribute("formatCode", nsuri);
            formatCodeAtt.Value = "0.00%";
            numFmtNode.Attributes.Append(formatCodeAtt);

            var sourceLinkedAtt = xdoc.CreateAttribute("sourceLinked", nsuri);
            sourceLinkedAtt.Value = "0";
            numFmtNode.Attributes.Append(sourceLinkedAtt);

            var dLblsNode = xdoc.SelectSingleNode("c:chartSpace/c:chart/c:plotArea/c:pieChart/c:ser/c:dLbls", nsm);
            dLblsNode.AppendChild(numFmtNode);


            //Format the legend
            pieChart.Legend.Add();
            pieChart.Legend.Position = eLegendPosition.Right;

            //pck.Save();

        }

        private string GetStatus(int responseStatuse)
        {
            switch (responseStatuse)
            {
                case 1:
                    return "InProgress";
                    break;
                case 2:
                    return "Saved";
                    break;
                case 3:
                    return "Submited";
                    break;
                default:
                    return "";
                    break;
            }
        }

        private string GetUserPublishKey(string surveyid)
        {
            var SurveyInfo = this._isurveyFacade.GetSurveyInfoModel(surveyid);
            return SurveyInfo.UserPublishKey.ToString();
        }

        private SurveyInfoResponse DoPublish(PublishModel Model, HttpPostedFileBase Newfile)
        {

            Guid NewGuid = new Guid();
            bool IsGuid = Guid.TryParse(Model.SurveyKey, out NewGuid);

            SurveyInfoRequest SurveyRequest = new SurveyInfoRequest();
            SurveyInfoDTO SurveyInfoDTO = new Common.DTO.SurveyInfoDTO();
            SurveyRequest.Criteria.FileInputStream = Newfile;
            SurveyInfoDTO.OrganizationKey = SurveyRequest.Criteria.OrganizationKey = new Guid(Model.OrganizationKey);
            SurveyInfoDTO.SurveyName = Model.SurveyName;
            SurveyInfoDTO.StartDate = DateTime.Now;
            SurveyInfoDTO.SurveyType = SurveyRequest.Criteria.SurveyType = 1;

            if (Model.UpdateExisting)
            {
                SurveyInfoDTO.SurveyId = Model.RepublishSurveyKey;
                SurveyInfoDTO.IsDraftMode = Model.IsDraft;
                SurveyInfoDTO.ClosingDate = DateTime.Parse(Model.EndDateUpdate); Model.EndDate = Model.EndDateUpdate;
                SurveyInfoDTO.SurveyName = Model.SurveyName;
                SurveyRequest.SurveyInfoList.Add(SurveyInfoDTO);
                SurveyInfoDTO.UserPublishKey = new Guid(Model.RepublishUserPublishKey);
                SurveyRequest.Action = "Update";
            }
            else
            {
                SurveyInfoDTO.SurveyId = Guid.NewGuid().ToString();
                SurveyInfoDTO.ClosingDate = DateTime.Parse(Model.EndDate);
                SurveyRequest.SurveyInfoList.Add(SurveyInfoDTO);
                SurveyInfoDTO.UserPublishKey = SurveyRequest.Criteria.UserPublishKey = Guid.NewGuid();
                SurveyRequest.Action = "Create";

            }
            var response = this._isurveyFacade.PublishExcelSurvey(SurveyRequest);
            return response;
        }



        private bool ValidateOrganizationId(string orgkey)
        {
            bool IsValidOrg = false;

            OrganizationRequest OrganizationRequest = new Common.Message.OrganizationRequest();
            OrganizationRequest.Organization.OrganizationKey = orgkey;
            IsValidOrg = this._isurveyFacade.ValidateOrganization(OrganizationRequest);
            return IsValidOrg;
        }

        private SelectList GetAllSurveysByOrgId(string orgkey,string surveyid ="")
        {
            var SurveyListObject = this._isurveyFacade.GetAllSurveysByOrgKey(orgkey);
            string SurveyName = "";
            string FirstFourSurveyId = "";

            int numberOfSurveys = SurveyListObject.SurveyInfoList.Count();
            List<SelectListItem> SurveyList = new List<SelectListItem>();
            List<SurveyInfoDTO> DTOList = SurveyListObject.SurveyInfoList.OrderBy(x => x.SurveyName).ToList();
            foreach (var survey in DTOList)
            {
                FirstFourSurveyId = "****" + survey.SurveyId.ToString().Substring(32, 4);
                SurveyName = survey.SurveyName + "-" + FirstFourSurveyId;
                SurveyList.Add(new SelectListItem { Value = survey.SurveyId.ToString(), Text = SurveyName, Selected = false });

            }
            SelectList SelectList = new SelectList(SurveyList, "Value", "Text");
            return SelectList;
        }


        private ActionResult DoDownLoad(PublishModel Model)
        {
            string Newfile = Model.FileName + ".CSV";
            bool IsAuthenticated = Model.IsAuthenticated = bool.Parse(Session["IsAuthenticated"].ToString());
            if (string.IsNullOrEmpty(Model.SurveyKey))
            {
                ModelState.AddModelError("SurveyKey", "Survey Id is required."); ;
            }
            //if (Model.UserPublishKey == Guid.Empty)
            //{
            //    ModelState.AddModelError("UserPublishKey", "Security Token is required."); ;
            //}
            ModelState["EndDate"].Errors.Clear();

            ModelState["Path"].Errors.Clear();
            ModelState["SurveyName"].Errors.Clear();
            if (IsAuthenticated)
            {
                ModelState["UserPublishKey"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    stopwatch.Start();
                    int NumberOfPages = 0;
                    int InitialSetupCounter = 0;
                    bool IsDraftMode = false;
                    /////////////////////////////////////////////////
                    // get a list of all controls in the survey
                    PrintResponseModel PrintResponseModel = new PrintResponseModel();
                    Common.Message.SurveyControlsRequest Request = new Common.Message.SurveyControlsRequest();
                    Request.SurveyId = Model.SurveyKey;
                    Common.Message.SurveyControlsResponse List = _isurveyFacade.GetSurveyControlList(Request);


                    ///////////////////////////////////////////
                    //   string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                    if (Model.SurveyMode.ToString() == "1")
                    {
                        IsDraftMode = false;
                    }
                    else
                    {
                        IsDraftMode = true;

                    }
                    // if (Environment.OSVersion.Version.Major >= 6)
                    // {
                    // path = Directory.GetParent(path).ToString();
                    //}



                    // FullPath = path + @"\Downloads\" + FileName;
                    FullPath = @Model.Path;
                    ///get  Response size
                    if (IsAuthenticated)
                    {
                        Model.UserPublishKey = GetUserPublishKey(Model.SurveyKey.ToString());
                    }
                    SurveyAnswerRequest SurveyAnswerRequest = SetMessageObject(true, Model.SurveyKey, new Guid(Model.UserPublishKey), Model.OrganizationKey, IsDraftMode, 0);
                    SurveyAnswerResponse SurveyAnswerResponse = new Common.Message.SurveyAnswerResponse();
                    SurveyAnswerResponse = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswerRequest);
                    int PageSize = SurveyAnswerResponse.PageSize;

                    int TotalCount = 0;


                    FileInfo newFile = new FileInfo(Newfile);

                    int Row = 1;
                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        if (PageSize > 0)
                        {
                            var Sheet = package.Workbook.Worksheets.Add("Sheet1");
                            for (int i = 1; SurveyAnswerResponse.NumberOfPages > i - 1; i++)
                            {
                                SurveyAnswerRequest = SetMessageObject(false, Model.SurveyKey, new Guid(Model.UserPublishKey), Model.OrganizationKey, IsDraftMode, 0, i, PageSize);
                                SurveyAnswerResponse Response = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswerRequest);


                                foreach (var item in Response.SurveyResponseList)
                                {
                                    //  var item = Response.SurveyResponseList[0];
                                    var ResponseValueList = SurveyHelper.GetQuestionAnswerList(item.XML, List);
                                    if (InitialSetupCounter == 0)
                                    {
                                        NumberOfPages = SurveyHelper.GetNumberOfPags(item.XML);
                                        InitialSetupCounter++;
                                        int col = 1;
                                        for (int j = 1; NumberOfPages > j - 1; j++)
                                        {


                                            // var ws = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Page" + j);
                                            // if (ws == null)
                                            //{
                                            //var sheet = package.Workbook.Worksheets.Add("Page" + j);
                                            // }
                                            var List1 = ResponseValueList.Where(x => x.PageNumber == j).ToList();

                                            // var Sheet = package.Workbook.Worksheets["Page" + j];

                                            foreach (var item1 in List1)
                                            {
                                                Row = 1;
                                                Sheet.Cells[Row, col].Value = item1.ControlName;
                                                switch (item1.ControlType)
                                                {
                                                    case "NumericTextBox":
                                                        Sheet.Column(col).Style.Numberformat.Format = "0.00";
                                                        break;
                                                    case "Date":
                                                        Sheet.Column(col).Style.Numberformat.Format = "mm-dd-yyyy";
                                                        break;
                                                    default:

                                                        break;
                                                }
                                                Sheet.Cells[Row + 1, col].Value = item1.Value;
                                                col++;
                                            }


                                        }
                                        Row++;

                                    }//if
                                    else
                                    {
                                        int col = 1;
                                        for (int j = 1; NumberOfPages > j - 1; j++)
                                        {


                                            var List2 = ResponseValueList.Where(x => x.PageNumber == j).ToList();

                                            //var Sheet = package.Workbook.Worksheets["Page" + j];
                                            // int col = 1;
                                            foreach (var ListItem in List2)
                                            {

                                                Sheet.Cells[Row, col].Value = ListItem.Value;
                                                col++;
                                            }


                                        }

                                    }// else

                                    Row++;
                                }// Outer Foreach
                            }
                            using (var memoryStream = new MemoryStream())
                            {
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;  filename=" + Newfile);
                                package.SaveAs(memoryStream);
                                memoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                                ModelState.AddModelError("Error", "Download was Successful.");
                                Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString()); ;
                                return View("Index", Model);
                            }
                        }
                        else
                        {
                            stopwatch.Stop();
                            Model.SuccessfulPublish = false;
                            ModelState.AddModelError("Error", "No records found for the download criteria entered.");


                            Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString()); ;
                            return View("Index", Model);
                        }
                    }//using
                    stopwatch.Stop();
                    Model.Path = @FullPath;
                    Model.SuccessfulPublish = true;
                    Model.TimeElapsed = stopwatch.Elapsed.ToString();
                    Model.RecordCount = TotalCount - 1;
                    return View("Index", Model);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    Model.SuccessfulPublish = false;
                    ModelState.AddModelError("Error", "Please validate the information provided and try downloading again.");
                    ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString()); ;
                    return View("Index", Model);
                }
            }
            else
            {
                Model.SuccessfulPublish = false;
                ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString()); ;
                return View("Index", Model);
            }
        }

        private SurveyAnswerRequest SetMessageObject(bool ReturnSizeInfoOnly, string SurveyId, Guid PublishKey, string OrganizationKey, bool IsDraftMode, int SurveyStatus, int PageNumber = -1, int PageSize = -1)
        {
            SurveyAnswerRequest Request = new SurveyAnswerRequest();
            SurveyAnswerDTO SurveyResponseDTO = new SurveyAnswerDTO();

            Request.Criteria = new SurveyAnswerCriteria();
            Request.Criteria.SurveyId = SurveyId;
            Request.Criteria.UserPublishKey = PublishKey;
            Request.Criteria.OrganizationKey = new Guid(OrganizationKey);
            Request.Criteria.ReturnSizeInfoOnly = ReturnSizeInfoOnly;
            Request.Criteria.StatusId = SurveyStatus;
            Request.Criteria.IsDraftMode = IsDraftMode;
            Request.Criteria.SurveyAnswerIdList = new List<string>();
            Request.Criteria.IsDownLoadFromApp = true;
            Request.Criteria.PageNumber = PageNumber;
            Request.Criteria.PageSize = PageSize;
            List<SurveyAnswerDTO> DTOList = new List<SurveyAnswerDTO>();

            Request.SurveyAnswerList = DTOList;


            return Request;
        }
        [HttpPost]
        public JsonResult GetSurveyInfo(string surveyid, bool getsourcetable = false )
        {
             PublishModel Model = new PublishModel();
            if (getsourcetable)
            {
                Model.SourceTables = new List<Web.Models.SourceTableModel>();
                var SourceTables = this._isurveyFacade.GetSourceTables(surveyid).List;
                foreach (var item in SourceTables)
                {
                    Web.Models.SourceTableModel SourceTable = new Web.Models.SourceTableModel();
                    SourceTable.TableName = item.TableName;
                    SourceTable.TableXml = Common.Json.SurveyResponseJson.XmlToJson(item.TableXml);
                    Model.SourceTables.Add(SourceTable);

                }
            }
            var SurveyInfo = this._isurveyFacade.GetSurveyInfoModel(surveyid);
           
            Model.EndDate = AddLeadingZero(SurveyInfo.ClosingDate.Month.ToString()) + "/" + AddLeadingZero(SurveyInfo.ClosingDate.Day.ToString()) + "/" + SurveyInfo.ClosingDate.Year;
            Model.IsDraft = SurveyInfo.IsDraftMode;
            Model.SurveyName = SurveyInfo.SurveyName;
            return Json(Model);


        }
        [HttpPost]
        public JsonResult SaveValueSet(string JsonData ,string surveyid, string valueSetName )
        {
            try
            {
                string ValueSetXml = GetValueSetXml(valueSetName, JsonData);
                SourceTablesRequest SourceTablesRequest = new SourceTablesRequest();
                SourceTablesRequest.List = new List<SourceTableDTO>();
                SourceTableDTO SourceTableDTO = new SourceTableDTO();
                SourceTableDTO.TableXml = ValueSetXml;
                SourceTableDTO.TableName = valueSetName;
                SourceTablesRequest.List.Add(SourceTableDTO);
                SourceTablesRequest.SurveyId = surveyid;
                _isurveyFacade.UpdateSourceTable(SourceTablesRequest);

                return Json(true);
            }
            catch (Exception ex) {

                return Json(false);
            }

        }
        [HttpPost]
        public JsonResult GetJsonResponse(string surveyid,string PublisherKey,bool IsDraft)
        {

            var _PublisherKey = GetUserPublishKey(surveyid);

            if (_PublisherKey.ToLower() == PublisherKey.ToLower())
            {

                var jsonContent = SqlHelper.GetSurveyJsonData(surveyid, IsDraft);
            return Json(jsonContent);
            }
            else
            {

                return Json(false);
            }

        }
         
        [HttpPost]
        public JsonResult GetJsonHeader(string surveyid, string PublisherKey, bool IsDraft)
        {
             
                var jsonContent = SqlHelper.GetHeadData(surveyid);
                return Json(jsonContent);
            

        }
        public JsonResult GetDashboardInfo(string surveyid)
        {

            DashboardResponse DashboardResponse = new DashboardResponse();
            DashboardResponse = _isurveyFacade.GetSurveyDashboardInfo(surveyid);
            DashboardResponse.DateList = new List<string>();
            foreach (var date in DashboardResponse.RecordCountPerDate) {

                DashboardResponse.DateList.Add(date.Key);


            }


            var info = JsonConvert.SerializeObject(DashboardResponse);
            return Json(info);

        }
        private string AddLeadingZero(string value)
        {
            string NewValue = "";
            if (value.Count() < 2)
            {
                NewValue = "0" + value;
            }
            else
            {

                NewValue = value;
            }
            return NewValue;
        }
        private string GetEmailInfo(Dictionary<string,string> Response, string URL)
        {
            

            string EmailInfo = "<table width ='100%' style ='border:Solid 1px Black;'>";
            EmailInfo += "<tr>";
            EmailInfo += "<td stlye='color:blue;'>Survey URL</td>";
            foreach (var row in Response) //Loop through DataGridView to get rows
            {
                
              
                    EmailInfo += "<td stlye='color:blue;'>" + row.Key + "</td>";
              
               
            }
            EmailInfo += "</tr>";
            EmailInfo += "<tr>";
            EmailInfo += "<td stlye='color:blue;'>" + URL + "</td>";
            foreach (var row in Response) //Loop through DataGridView to get rows
            {


                EmailInfo += "<td stlye='color:blue;'>" + row.Value + "</td>";


            }
           EmailInfo += "</tr>";
            EmailInfo += "</table>";


            return EmailInfo;


        }

        [HttpPost]
        public JsonResult GetSurveyReportList(string surveyid) {

            try
            {

                PublishReportRequest PublishReportRequest = new PublishReportRequest();
                PublishReportRequest.ReportInfo.SurveyId = surveyid;
                PublishReportRequest.IncludHTML = false;

                PublishReportResponse result = _isurveyFacade.GetSurveyReportList(PublishReportRequest);
                List<ReportModel> ModelList = new List<ReportModel>();
                foreach (var item in result.Reports)
                {
                    ReportModel Model = new ReportModel();
                    Model.DateCreated = item.CreatedDate.ToString();
                    Model.DataSource = item.DataSource;
                    Model.ReportURL = ConfigurationManager.AppSettings["ReportURL"] + item.ReportId;
                    Model.RecordCount = item.RecordCount;
                    Model.ReportName = item.ReportName;
                    ModelList.Add(Model);
                }

                return Json(ModelList); // convert to report model
            }
            catch (Exception ex)
            {

                return Json(false);
            }


        }
    }
}
