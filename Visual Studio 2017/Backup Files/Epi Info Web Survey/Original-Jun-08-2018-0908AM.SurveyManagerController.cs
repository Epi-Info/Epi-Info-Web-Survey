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


using System.Web.Security;
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
        public ActionResult Index()
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
            if (IsAuthenticated)
            {
                Session["IsAuthenticated"] = Model.IsAuthenticated = IsAuthenticated;
                //check if the user exists 
                var UserName = System.Web.HttpContext.Current.User.Identity.Name;
                OrganizationAccountRequest Request = new OrganizationAccountRequest();
                Request.Admin = new AdminDTO();
                Request.Admin.AdminEmail = UserName;
                OrganizationAccountResponse Response = this._isurveyFacade.GetUserOrgId(Request);
                var OrgId = "";
                if (!string.IsNullOrEmpty(Response.OrganizationDTO.OrganizationKey))
                {
            
                  OrgId=  Epi.Web.Common.Security.Cryptography.Decrypt(Response.OrganizationDTO.OrganizationKey);
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
                    }

                    
                }
                else { 
                // create a new account 
                    Request.AccountType = "ORGANIZATION";
                      Request.Admin.IsActive = true;
                      Request.Admin.Notify = false;
                      Request.Organization = new OrganizationDTO();
                      Request.Organization.Organization = UserName.Replace("\\"," ") + "_Organization";
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
                              ViewBag.IsNewOrg =  Session["IsNewOrg"] = true;
                            Model.SurveyNameList=ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); ;
                          }
                      }
                   }
                Model.PublishDivState = true; 
                return View("Index", Model);
            }
            else {
                Model.PublishDivState = true;
                return View("Index", Model);
            }
           
            
        }
        [HttpPost]
         public ActionResult Index(PublishModel Model, string PublishSurvey, string DownLoadResponse, string ValidateOrganization, HttpPostedFileBase Newfile, HttpPostedFileBase Newfile1, HttpPostedFileBase Newfile2, string SurveyName, string RePublishSurvey, string RePublishSurveyName,string SendEmail)
        {

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
                if (!string.IsNullOrEmpty(ValidateOrganization))
                {
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
                    if (ModelState.IsValid)
                    {
                        Model.IsValidOrg = ValidateOrganizationId(Model.OrganizationKey);

                        if (!Model.IsValidOrg)
                        {
                            ModelState.AddModelError("OrganizationKey", "Organization Key does not exist.");
                        }
                        else {
                           Session["OrgId"] =  Model.OrganizationKey;
                          Model.SurveyNameList = GetAllSurveysByOrgId(Model.OrganizationKey);
                          ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); ;
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
                    if (ModelState.IsValid)
                    {
                        var response = DoPublish(Model, Newfile);
                        Model.SuccessfulPublish = true;
                        if (response.SurveyInfoList.Count()>0)
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
                    else {
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
                    if (IsAuthenticated)
                    {
                        ModelState["UserPublishKey"].Errors.Clear();
                    }
                    if (ModelState.IsValid)
                    {
                        DoDownLoad(Model);
                    }
                    else {
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); 
                        return View("Index", Model);
                    }
                }
                //send email notification
                if (!string.IsNullOrEmpty(SendEmail))
                {
                    Model.IsValidOrg = true;
                    Model.OrganizationKey = Session["OrgId"].ToString();
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




                    if (IsAuthenticated)
                    {
                        ModelState["EmailUserPublishKey"].Errors.Clear();
                    }
                  
                    if (ModelState.IsValid)
                    {
                        SendEmailNotification(Model, Newfile2);
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey);
                    }
                    else
                    {
                        Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey);
                        return View("Index", Model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Model.OrganizationKey); 
                Model.SuccessfulPublish = false;
                ModelState.AddModelError("Error", "Please validate the information provided and try publishing again.");
                return View("Index", Model);
            }
            return View("Index", Model);
        }

        private void SendEmailNotification(PublishModel model, HttpPostedFileBase newfile2)
        {
            string strPassCode = Epi.Web.MVC.Utility.SurveyHelper.GetPassCode();
            var SurveyInfo = this._isurveyFacade.GetSurveyInfoModel(model.EmailSurveyKey);
            Guid ResponseID =   Guid.NewGuid(); 
            Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(SurveyInfo.SurveyId, ResponseID.ToString());

           // Epi.Web.Common.Message.UserAuthenticationResponse AuthenticationResponse = _isurveyFacade.GetAuthenticationResponse(ResponseID.ToString());

          
          //  if (string.IsNullOrEmpty(AuthenticationResponse.PassCode))
           // {
                 
                _isurveyFacade.UpdatePassCode(ResponseID.ToString(), strPassCode);
           // }

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
                SurveyInfoDTO.SurveyId =  Model.RepublishSurveyKey;
                SurveyInfoDTO.IsDraftMode = Model.IsDraft;
                SurveyInfoDTO.ClosingDate = DateTime.Parse(Model.EndDateUpdate); Model.EndDate = Model.EndDateUpdate;
                SurveyInfoDTO.SurveyName = Model.SurveyName;
                SurveyRequest.SurveyInfoList.Add(SurveyInfoDTO);
                SurveyInfoDTO.UserPublishKey = new Guid(Model.RepublishUserPublishKey);
                SurveyRequest.Action = "Update";
            }
            else
            {
                SurveyInfoDTO.SurveyId = Guid.NewGuid().ToString() ;
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

        private SelectList GetAllSurveysByOrgId(string orgkey)
        {
              var SurveyListObject = this._isurveyFacade.GetAllSurveysByOrgKey(orgkey);
            string SurveyName = "";
            string FirstFourSurveyId = "";
             
            int numberOfSurveys = SurveyListObject.SurveyInfoList.Count();
            List<SelectListItem> SurveyList = new List<SelectListItem>();
            List<SurveyInfoDTO> DTOList = SurveyListObject.SurveyInfoList.OrderBy(x=>x.SurveyName).ToList();
            foreach (var survey in DTOList)
            {
                FirstFourSurveyId = "****" + survey.SurveyId.ToString().Substring(32,4); 
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
                                                Sheet.Cells[Row+1, col].Value = item1.Value;
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
                            }
                        }
                        else {

                            Model.SuccessfulPublish = false;
                            ModelState.AddModelError("Error", "No records found for the download criteria entered.");


                            ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString()); ;
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
        public JsonResult GetSurveyInfo(string surveyid) 
        {

            var SurveyInfo = this._isurveyFacade.GetSurveyInfoModel(surveyid);
            PublishModel Model = new PublishModel();
            Model.EndDate = AddLeadingZero(SurveyInfo.ClosingDate.Month.ToString()) + "/" + AddLeadingZero(SurveyInfo.ClosingDate.Day.ToString()) + "/" + SurveyInfo.ClosingDate.Year;
            Model.IsDraft = SurveyInfo.IsDraftMode;
            Model.SurveyName = SurveyInfo.SurveyName;
            return Json(Model);
        
        }
        private string AddLeadingZero(string value) 
        {
            string NewValue = "";
            if (value.Count()<2)
            {
                NewValue = "0" + value;
            }
            else
            {

                NewValue = value;
            }
            return NewValue;
        }
    }
}
