using Epi.Web.Common;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Xml;
using Epi.Web.Common.Message;
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
using OfficeOpenXml.Drawing.Chart;

namespace Epi.Web.MVC.Controllers
    {
    [Authorize]
    public class PrintController : Controller
    {
        //
        // GET: /Print/

        private ISurveyFacade _isurveyFacade;
        public string FullPath = "";
        public PrintController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }

        [HttpGet]
        public ActionResult Index(string responseId, bool FromFinal, SurveyModel FormsHierarchyModel)
        {
            List<PrintResponseModel> PrintList = GetResponseList(responseId, FromFinal);
            Session["RID"] = responseId;
            return View("Index", PrintList);
        }
        [HttpPost]
        public ActionResult Index(SurveyModel FormsHierarchyModel)
        {
            List<PrintResponseModel> PrintList = GetResponseList(Session["RID"].ToString(), false);
            GetCSV(Session["RID"].ToString());
            return View("Index", PrintList);
        }
        [HttpPost]
        public ActionResult GetCSV(string responseId)
        {

            List<PrintResponseModel> PrintList = GetResponseList(responseId, true);
            string Newfile = "Response_" + PrintList[0].ResponseId + ".CSV";

            FileInfo newFile = new FileInfo(Newfile);
           // Stopwatch stopwatch = new Stopwatch();
            int Row = 1;
           // stopwatch.Start();
            using (ExcelPackage package = new ExcelPackage(newFile))
            {

                var Sheet = package.Workbook.Worksheets.Add("Sheet1");


                int col = 1;



                foreach (var item1 in PrintList)
                {
                    foreach (var item in item1.ResponseList)
                    {
                        Row = 1;
                        Sheet.Cells[Row, col].Value = item.ControlName;
                        switch (item.ControlType)
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
                        Sheet.Cells[Row + 1, col].Value = item.Value;
                        col++;
                    }
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

                    // Model.SurveyNameList = ViewBag.SurveyNameList1 = GetAllSurveysByOrgId(Session["OrgId"].ToString()); ;
                    return View("Index", PrintList);
                }
               // stopwatch.Stop();
            }

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
        private string GetUserPublishKey(string surveyid)
        {
            var SurveyInfo = this._isurveyFacade.GetSurveyInfoModel(surveyid);
            return SurveyInfo.UserPublishKey.ToString();
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
        private SelectList GetAllSurveysByOrgId(string orgkey)
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
    }

}
