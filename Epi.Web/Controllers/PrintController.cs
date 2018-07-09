using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using System.Linq;
using Epi.Web.MVC.Utility;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
namespace Epi.Web.MVC.Controllers
    {
    [Authorize]
    public class PrintController : Controller
    {
        //
        // GET: /Print/

          private ISurveyFacade _isurveyFacade;

          public PrintController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
 
        [HttpGet]
        public ActionResult Index(string responseId ,bool FromFinal, SurveyModel FormsHierarchyModel)
        {
            List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy();
            SurveyModel SurveyModel = new SurveyModel();
           // SurveyModel.Form = form;
            SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, Session["RootFormId"].ToString());

            //   Common.Message.SurveyAnswerResponse answerResponse = _isurveyFacade.GetSurveyAnswerResponse(responseId);
            //
            List<PrintResponseModel> PrintList = new List<PrintResponseModel>();
            
            foreach (var form in SurveyModel.RelateModel) {



                foreach (var answerResponse in form.ResponseIds) {
                    PrintResponseModel PrintResponseModel = new PrintResponseModel();
                    SurveyInfoModel surveyInfoModel = GetSurveyInfo(answerResponse.SurveyId);
                    Common.Message.SurveyControlsRequest Request = new Common.Message.SurveyControlsRequest();
                    Request.SurveyId = answerResponse.SurveyId;
                    Common.Message.SurveyControlsResponse List = _isurveyFacade.GetSurveyControlList(Request);

                    var QuestionAnswerList = SurveyHelper.GetQuestionAnswerList(answerResponse.XML, List);
                    var SourceTables = _isurveyFacade.GetSourceTables(Session["RootFormId"].ToString());
                    PrintResponseModel.ResponseList = SurveyHelper.SetCommentLegalValues(QuestionAnswerList, List, surveyInfoModel, SourceTables);
                    PrintResponseModel.NumberOfPages = SurveyHelper.GetNumberOfPags(answerResponse.XML);
                    PrintResponseModel.SurveyName =surveyInfoModel.SurveyName;
                    PrintResponseModel.CurrentDate = DateTime.Now.ToString();
                    PrintResponseModel.ResponseId = responseId;
                    PrintResponseModel.SurveyId = form.FormId;
                    PrintResponseModel.IsFromFinal = FromFinal;
                    PrintList.Add(PrintResponseModel);
                }
            }
            return View("Index", PrintList);
        }

        public SurveyInfoModel GetSurveyInfo(string SurveyId)
            {
            SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
            return surveyInfoModel;
            }

        private List<FormsHierarchyDTO> GetFormsHierarchy()
        {
            FormsHierarchyResponse FormsHierarchyResponse = new FormsHierarchyResponse();
            FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            SurveyAnswerRequest ResponseIDsHierarchyRequest = new SurveyAnswerRequest();
            SurveyAnswerResponse ResponseIDsHierarchyResponse = new SurveyAnswerResponse();
            // FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
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



    }

}
