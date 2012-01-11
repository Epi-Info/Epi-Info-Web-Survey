using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using Epi.Web.MVC.Facade;
namespace Epi.Web.MVC.Facade
{
    public class SurveyFacade:ISurveyFacade
    {
        // declare ISurveyInfoRepository which inherits IRepository of SurveyInfoResponse object
        private ISurveyInfoRepository _iSurveyInfoRepository;

        // declare ISurveyResponseRepository which inherits IRepository of SurveyResponseResponse object
        private ISurveyAnswerRepository _iSurveyAnswerRepository;

        //declare SurveyInfoRequest
        private Epi.Web.Common.Message.SurveyInfoRequest _surveyInfoRequest;

        //declare SurveyResponseRequest
        private Epi.Web.Common.Message.SurveyAnswerRequest _surveyAnswerRequest;

        //declare SurveyAnswerDTO
        private Common.DTO.SurveyAnswerDTO _surveyAnswerDTO;

        //declare SurveyResponseXML object
        private SurveyResponseXML _surveyResponseXML;

        /// <summary>
        /// Injectinting ISurveyInfoRepository through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public SurveyFacade(ISurveyInfoRepository iSurveyInfoRepository, ISurveyAnswerRepository iSurveyResponseRepository,
                                  Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest, Epi.Web.Common.Message.SurveyAnswerRequest surveyResponseRequest,
                                  Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
                                   SurveyResponseXML surveyResponseXML)
        {
            _iSurveyInfoRepository = iSurveyInfoRepository;
            _iSurveyAnswerRepository = iSurveyResponseRepository;
            _surveyInfoRequest = surveyInfoRequest;
            _surveyAnswerRequest = surveyResponseRequest;
            _surveyAnswerDTO = surveyAnswerDTO;
            _surveyResponseXML = surveyResponseXML;
        }

        /// <summary>
        /// get the survey form data
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="responseId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="surveyAnswerDTO"></param>
        /// <returns></returns>
        public MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO)
        {
            //Get the SurveyInfoDTO
            Epi.Web.Common.DTO.SurveyInfoDTO surveyInfoDTO = SurveyHelper.GetSurveyInfoDTO(_surveyInfoRequest,_iSurveyInfoRepository,surveyId);
            MvcDynamicForms.Form form = Epi.Web.MVC.Utility.FormProvider.GetForm(surveyInfoDTO, pageNumber, surveyAnswerDTO);
            return form;
        }
        /// <summary>
        /// This method accepts a surveyId and responseId and creates the first survey response entry
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public void CreateSurveyAnswer(string surveyId,string responseId)
        {
           
            SurveyHelper.CreateSurveyResponse(surveyId, responseId, _surveyAnswerRequest, _surveyAnswerDTO, _surveyResponseXML, _iSurveyAnswerRepository);
        }



        public void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId,MvcDynamicForms.Form form)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response and save the response
            
            //// 1 Get the record for the current survey response
            SurveyAnswerResponse surveyAnswerResponse = GetSurveyAnswerResponse(responseId);
            
            ///2 Update the current survey response and save it
           
            SurveyHelper.UpdateSurveyResponse(surveyInfoModel, form, _surveyAnswerRequest, _surveyResponseXML, _iSurveyAnswerRepository,surveyAnswerResponse, responseId);
        }
        

       

        public SurveyInfoModel GetSurveyInfoModel(string surveyId)
        {
            _surveyInfoRequest.Criteria.SurveyIdList.Add(surveyId);
            SurveyInfoResponse surveyInfoResponse = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest);
            SurveyInfoModel s = Mapper.ToSurveyInfoModel(surveyInfoResponse.SurveyInfoList[0]);
            return s;
        }



        /// <summary>
        /// Get the record for the current response (Step1: Saving Survey)
        /// </summary>
        /// <param name="ResponseId"></param>
        /// <returns></returns>
        public SurveyAnswerResponse GetSurveyAnswerResponse(string responseId)
        {
            _surveyAnswerRequest.Criteria.SurveyAnswerIdList.Add(responseId);
            SurveyAnswerResponse surveyAnswerResponse = _iSurveyAnswerRepository.GetSurveyAnswer(_surveyAnswerRequest);
            return surveyAnswerResponse;
        }
        
        
    }
}