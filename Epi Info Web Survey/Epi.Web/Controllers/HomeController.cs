using System;
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


        private ISurveyResponseRepository _iSurveyResponseRepository;
        private Epi.Web.Common.Message.SurveyResponseRequest _surveyResponseRequest;


        /// <summary>
        /// Injectinting ISurveyInfoRepository through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public HomeController(ISurveyInfoRepository iSurveyInfoRepository, Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest, ISurveyResponseRepository iSurveyResponseRepository)
        {
            _iSurveyInfoRepository = iSurveyInfoRepository;
            _surveyInfoRequest = surveyInfoRequest;
        }

        [HttpGet]
        public ActionResult Index()
        {
          
            return View("Index");
        }


        
        public ActionResult Index1()
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
        public ActionResult ListSurvey(Epi.Web.Models.SurveyInfoModel surveyModel, string submit, string surveyid)
        {
            //SurveyInfoRequest surveyInfoRequest = new SurveyInfoRequest();
            _surveyInfoRequest.Criteria.SurveyId = surveyModel.SurveyId;
            var surveyInfoDTO = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest).SurveyInfo;

            var form = MvcDynamicForms.Demo.Models.FormProvider.GetForm(surveyInfoDTO, 1);// Requesting the first page of the survey.

            if (!string.IsNullOrEmpty(submit))
            {
                return Submit(surveyid, surveyModel);
            }
            else
            {
              
                return View("Survey", form);
            }
        }

        public ActionResult Submit(string surveyid, Epi.Web.Models.SurveyInfoModel surveyModel)
        {
            _surveyInfoRequest.Criteria.SurveyId = surveyModel.SurveyId;
            var surveyInfoDTO = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest).SurveyInfo;

            var form = MvcDynamicForms.Demo.Models.FormProvider.GetForm(surveyInfoDTO, 1);// Requesting the first page of the survey.
            
            _surveyInfoRequest.Criteria.SurveyId = surveyid;
            SurveyInfoResponse surveyInfoResponse = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest);
            var s = Mapper.ToSurveyInfoModel(surveyInfoResponse.SurveyInfo);

            UpdateModel(form);

            if (form.Validate())
            {
                // 1 Get the record for the current survey response
                // 2 update the current survey response
                // 3 save the current survey response

                // 1 Get the record for the current survey response
                SurveyResponseRequest surveyResponseRequest = new SurveyResponseRequest();
                surveyResponseRequest.Criteria.ResposneId = "current surveyresponseId goes here";
                SurveyResponseResponse surveyResponseResponse = _iSurveyResponseRepository.GetSurveyResponse(surveyResponseRequest);

                // 2 update the current survey response
                surveyResponseRequest.SurveyResponseDTO = surveyResponseResponse.SurveyResponseDTO;
                SurveyResponseXML surveyXML = new SurveyResponseXML();
                surveyXML.Add(form);
                surveyResponseRequest.SurveyResponseDTO.XML = surveyXML.CreateResponseXml().ToString();

                // 3 save the current survey response
                surveyResponseResponse = _iSurveyResponseRepository.SaveSurveyResponse(surveyResponseRequest);

                return View("PostSubmit", s);
            }
            else
            {
                return View("Survey", form);
            }
           
        }
    }
}
