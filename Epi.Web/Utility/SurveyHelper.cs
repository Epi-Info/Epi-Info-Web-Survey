using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using Epi.Web.MVC.Facade;

namespace Epi.Web.MVC.Utility
{
    public class SurveyHelper
    {
        /// <summary>
        /// Creates the first survey response in the response table
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="responseId"></param>
        /// <param name="surveyAnswerRequest"></param>
        /// <param name="surveyAnswerDTO"></param>
        /// <param name="surveyResponseXML"></param>
        /// <param name="iSurveyAnswerRepository"></param>
        public static void CreateSurveyResponse(string surveyId, string responseId,SurveyAnswerRequest surveyAnswerRequest,
                                          Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
                                          SurveyResponseXML surveyResponseXML, ISurveyAnswerRepository iSurveyAnswerRepository)
        {
            surveyAnswerRequest.Criteria.ResposneId = responseId.ToString();
            surveyAnswerRequest.SurveyResponseDTO = surveyAnswerDTO;
            surveyAnswerRequest.SurveyResponseDTO.ResponseId = responseId.ToString();
            surveyAnswerRequest.SurveyResponseDTO.DateCompleted = DateTime.Now;
            surveyAnswerRequest.SurveyResponseDTO.SurveyId = surveyId;
            surveyAnswerRequest.SurveyResponseDTO.Status = (int)Constant.Status.InProgress;
            surveyAnswerRequest.SurveyResponseDTO.XML = surveyResponseXML.CreateResponseXml(surveyId).InnerXml;
            surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.CREATE;  //"Create";
            iSurveyAnswerRepository.SaveSurveyAnswer(surveyAnswerRequest);
        }

        public static void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel,MvcDynamicForms.Form form, SurveyAnswerRequest surveyAnswerRequest,
                                                             SurveyResponseXML surveyResponseXML,
                                                            ISurveyAnswerRepository iSurveyAnswerRepository,
                                                             SurveyAnswerResponse surveyAnswerResponse,string responseId)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response
            // 3 save the current survey response

           
            // 2 a. update the current survey answer request
            surveyAnswerRequest.SurveyResponseDTO = surveyAnswerResponse.SurveyResponseDTO;
            surveyResponseXML.Add(form);
            surveyAnswerRequest.SurveyResponseDTO.XML = surveyResponseXML.CreateResponseXml(surveyInfoModel.SurveyId).InnerXml;
            // 2 b. save the current survey response
            surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.UPDATE;  //"Update";
            iSurveyAnswerRepository.SaveSurveyAnswer(surveyAnswerRequest);
        }



      

        /// <summary>
        /// Returns a SurveyInfoDTO object
        /// </summary>
        /// <param name="surveyInfoRequest"></param>
        /// <param name="iSurveyInfoRepository"></param>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public static Epi.Web.Common.DTO.SurveyInfoDTO GetSurveyInfoDTO(SurveyInfoRequest surveyInfoRequest,
                                                  ISurveyInfoRepository iSurveyInfoRepository,                 
                                                  string SurveyId)
        {
            surveyInfoRequest.Criteria.SurveyId = SurveyId;
            return iSurveyInfoRepository.GetSurveyInfo(surveyInfoRequest).SurveyInfo;
        }
    }
}