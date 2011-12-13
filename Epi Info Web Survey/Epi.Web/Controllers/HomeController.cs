﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Models;
using Epi.Web.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.Common.Criteria;

namespace Epi.Web.Controllers
{
    public class HomeController : Controller
    {
        
        
        // Initialize ISurveyInfoRepository 
        private ISurveyInfoRepository _iSurveyInfoRepository;
        //Initialize SurveyInfoRequest
        private Epi.Web.Common.Message.SurveyInfoRequest _surveyInfoRequest;
        /// <summary>
        /// Injectinting ISurveyInfoRepository through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public HomeController(ISurveyInfoRepository iSurveyInfoRepository, Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest)
        {
            _iSurveyInfoRepository = iSurveyInfoRepository;
            _surveyInfoRequest = surveyInfoRequest;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
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
        public ActionResult ListSurvey(string surveyid)
        {
            /*TO DO: Later we will pass an empty SurveyInfoDTO object and validate it here something like
             if surveyInfodto.SurveyName== null then go to the exception page*/
            try
            {
               
                _surveyInfoRequest.Criteria.SurveyId = surveyid;
                SurveyInfoResponse surveyInfoResponse = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest);
                var s = Mapper.ToSurveyInfoModel(surveyInfoResponse.SurveyInfo);

                return View("SurveyIntroduction", s);
            }
            catch (Exception ex)
            {
                return View("Exception");
            }
        }

        
       /// <summary>
       /// Assign the SurveyId to the surveyInforequest object
        /// get the surveyInfoDTO based on surveyInforequest object by method GetSurveyInfo(SurveyInfoRequest)
       /// Get the form object based on SurveyInfoDTO and pass to the view
       /// Model Binding
       /// </summary>
       /// <param name="surveyModel"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult ListSurvey(Epi.Web.Models.SurveyInfoModel surveyModel)
        {
            //SurveyInfoRequest surveyInfoRequest = new SurveyInfoRequest();
            _surveyInfoRequest.Criteria.SurveyId = surveyModel.SurveyId;
            var surveyInfoDTO = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest).SurveyInfo;

            var form = MvcDynamicForms.Demo.Models.FormProvider.GetForm(surveyInfoDTO, 1);// Requesting the first page of the survey.
            
            return View("Survey", form);

        }
       
    }
}
