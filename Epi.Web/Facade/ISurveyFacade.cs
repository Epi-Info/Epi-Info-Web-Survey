using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;

namespace Epi.Web.MVC.Facade
{
    public interface ISurveyFacade
    {
        MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO);
        void CreateSurveyAnswer(string surveyId, string responseId);
        void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO);
        
        SurveyInfoModel GetSurveyInfoModel(string surveyId);
        SurveyAnswerResponse GetSurveyAnswerResponse(string responseId);
       
    }
}
