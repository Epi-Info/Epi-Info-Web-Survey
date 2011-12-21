using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Models;
using Epi.Web.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.Common.Criteria;
using Epi.Web.Models.Constants;
using MvcDynamicForms.Demo.Models;
using Epi.Web.Models.Utility;

namespace Epi.Web.Controllers
{
    public class HomeController : Controller
    {

        // declare ISurveyInfoRepository which inherits IRepository of SurveyInfoResponse object
        private ISurveyInfoRepository _iSurveyInfoRepository;

        //declare SurveyInfoRequest
        private Epi.Web.Common.Message.SurveyInfoRequest _surveyInfoRequest;

        
        /// <summary>
        /// Injectinting ISurveyInfoRepository and SurveyInfoRequest through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public HomeController(ISurveyInfoRepository iSurveyInfoRepository,
                                  Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest)
        {
            _iSurveyInfoRepository = iSurveyInfoRepository;
            _surveyInfoRequest = surveyInfoRequest;
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

                _surveyInfoRequest.Criteria.SurveyId = surveyid;
                SurveyInfoResponse surveyInfoResponse = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest);
                var s = Mapper.ToSurveyInfoModel(surveyInfoResponse.SurveyInfo);

                return View(Epi.Web.Models.Constants.Constant.INDEX_PAGE, s);
            }
            catch (Exception ex)
            {
                return View(Epi.Web.Models.Constants.Constant.EXCEPTION_PAGE);
            }
        }

        /// <summary>
        /// redirecting to Survey controller to action method Index
        /// </summary>
        /// <param name="surveyModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Epi.Web.Models.SurveyInfoModel surveyModel)
        {
            
            return RedirectToAction(Epi.Web.Models.Constants.Constant.INDEX, Epi.Web.Models.Constants.Constant.SURVEY_CONTROLLER);
            
        }


    }
}
