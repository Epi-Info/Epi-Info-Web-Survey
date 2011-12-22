using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;

namespace Epi.Web.MVC.Facade
{
    public class SurveyFacade
    {
        // declare ISurveyInfoRepository which inherits IRepository of SurveyInfoResponse object
        private ISurveyInfoRepository _iSurveyInfoRepository;

        // declare ISurveyResponseRepository which inherits IRepository of SurveyResponseResponse object
        private ISurveyAnswerRepository _iSurveyResponseRepository;

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
            _iSurveyResponseRepository = iSurveyResponseRepository;
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
            Epi.Web.Common.DTO.SurveyInfoDTO surveyInfoDTO = GetSurveyInfoDTO(surveyId);
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
            _surveyAnswerRequest.Criteria.ResposneId = responseId.ToString();
            _surveyAnswerRequest.SurveyResponseDTO = _surveyAnswerDTO;
            _surveyAnswerRequest.SurveyResponseDTO.ResponseId = responseId.ToString();
            _surveyAnswerRequest.SurveyResponseDTO.DateCompleted = DateTime.Now;
            _surveyAnswerRequest.SurveyResponseDTO.SurveyId = surveyId;
            _surveyAnswerRequest.SurveyResponseDTO.Status = (int)Constant.Status.InProgress;
            _surveyAnswerRequest.SurveyResponseDTO.XML = _surveyResponseXML.CreateResponseXml(surveyId).InnerXml;
            _surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.CREATE;  //"Create";
            _iSurveyResponseRepository.SaveSurveyAnswer(_surveyAnswerRequest);
        }



        public void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId,MvcDynamicForms.Form form)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response
            // 3 save the current survey response

            // 1 Get the record for the current survey response
            SurveyAnswerResponse surveyResponseResponse = GetSurveyAnswerResponse(responseId);

            // 2 update the current survey response
            _surveyAnswerRequest.SurveyResponseDTO = surveyResponseResponse.SurveyResponseDTO;
            _surveyResponseXML.Add(form);
            _surveyAnswerRequest.SurveyResponseDTO.XML = _surveyResponseXML.CreateResponseXml(surveyInfoModel.SurveyId).InnerXml;

            // 3 save the current survey response

            surveyResponseResponse = SaveSurveyAnswerResponse(_surveyAnswerRequest);
        }
        

        /// <summary>
        /// returns SurveyInfoDTO object based on SurveyId
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        private Epi.Web.Common.DTO.SurveyInfoDTO GetSurveyInfoDTO(string SurveyId)
        {
            _surveyInfoRequest.Criteria.SurveyId = SurveyId;
            return _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest).SurveyInfo;
        }

        public SurveyInfoModel GetSurveyInfoModel(string surveyId)
        {
            _surveyInfoRequest.Criteria.SurveyId = surveyId;
            SurveyInfoResponse surveyInfoResponse = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest);
            SurveyInfoModel s = Mapper.ToSurveyInfoModel(surveyInfoResponse.SurveyInfo);
            return s;
        }



        /// <summary>
        /// Get the record for the current response (Step1: Saving Survey)
        /// </summary>
        /// <param name="ResponseId"></param>
        /// <returns></returns>
        private SurveyAnswerResponse GetSurveyAnswerResponse(string responseId)
        {
            _surveyAnswerRequest.Criteria.ResposneId = responseId;
            SurveyAnswerResponse surveyResponseResponse = _iSurveyResponseRepository.GetSurveyAnswer(_surveyAnswerRequest);
            return surveyResponseResponse;
        }
        /// <summary>
        /// Save the current survey response Step:3
        /// </summary>
        /// <param name="p_surveyAnswerRequest"></param>
        /// <returns></returns>
        private SurveyAnswerResponse SaveSurveyAnswerResponse(SurveyAnswerRequest p_surveyAnswerRequest)
        {
            p_surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.UPDATE;  //"Update";
            return _iSurveyResponseRepository.SaveSurveyAnswer(p_surveyAnswerRequest);
        }
        
    }
}