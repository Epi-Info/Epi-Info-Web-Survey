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
        public ActionResult Index(string responseId ,bool FromFinal)
        {
        Common.Message.SurveyAnswerResponse answerResponse = _isurveyFacade.GetSurveyAnswerResponse(responseId);
        SurveyInfoModel surveyInfoModel = GetSurveyInfo(answerResponse.SurveyResponseList[0].SurveyId);

        PrintResponseModel PrintResponseModel = new PrintResponseModel();
        Common.Message.SurveyControlsRequest Request = new Common.Message.SurveyControlsRequest();
        Request.SurveyId = answerResponse.SurveyResponseList[0].SurveyId;
        Common.Message.SurveyControlsResponse List = _isurveyFacade.GetSurveyControlList(Request);

        var QuestionAnswerList = SurveyHelper.GetQuestionAnswerList(answerResponse.SurveyResponseList[0].XML, List);
        var SourceTables = _isurveyFacade.GetSourceTables(Session["RootFormId"].ToString());
        PrintResponseModel.ResponseList = SurveyHelper.SetCommentLegalValues(QuestionAnswerList, List, surveyInfoModel, SourceTables);
        PrintResponseModel.NumberOfPages = SurveyHelper.GetNumberOfPags(answerResponse.SurveyResponseList[0].XML);
        PrintResponseModel.SurveyName = surveyInfoModel.SurveyName;
        PrintResponseModel.CurrentDate = DateTime.Now.ToString();
        PrintResponseModel.ResponseId = responseId;
        PrintResponseModel.SurveyId = surveyInfoModel.SurveyId;
        PrintResponseModel.IsFromFinal = FromFinal;
        return View("Index", PrintResponseModel);
        }

        public SurveyInfoModel GetSurveyInfo(string SurveyId)
            {
            SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
            return surveyInfoModel;
            }

    }
}
