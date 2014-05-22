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

        MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsMobileDevice);
        Epi.Web.Common.DTO.SurveyAnswerDTO  CreateSurveyAnswer(string surveyId, string responseId);
        void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsSubmited, bool IsSaved, int PageNumber);
        
        SurveyInfoModel GetSurveyInfoModel(string surveyId);
        SurveyAnswerResponse GetSurveyAnswerResponse(string responseId);
        UserAuthenticationResponse ValidateUser(string responseId, string passcode);
        void UpdatePassCode(string responseId, string passcode);
        UserAuthenticationResponse GetAuthenticationResponse(string responseId);
        ISurveyAnswerRepository GetSurveyAnswerRepository();

    }
}
