using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using System.Collections.Generic;

namespace Epi.Web.MVC.Facade
{
    public interface ISurveyFacade
    {

        MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool isDevice = false, string calledThereby = "", bool IsAndroid = false, bool GetSourceTables = true);
        Epi.Web.Common.DTO.SurveyAnswerDTO  CreateSurveyAnswer(string surveyId, string responseId);
        void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsSubmited, bool IsSaved, int PageNumber);
        
        SurveyInfoModel GetSurveyInfoModel(string surveyId);
        SurveyAnswerResponse GetSurveyAnswerResponse(string responseId);
        UserAuthenticationResponse ValidateUser(string responseId, string passcode);
        void UpdatePassCode(string responseId, string passcode);
        UserAuthenticationResponse GetAuthenticationResponse(string responseId);
        ISurveyAnswerRepository GetSurveyAnswerRepository();
        OrganizationAccountResponse CreateAccount(OrganizationAccountRequest AccountRequest);

        OrganizationAccountResponse GetStateList(OrganizationAccountRequest Request);

        SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage);
        SurveyInfoResponse PublishExcelSurvey(SurveyInfoRequest Request);
        SurveyAnswerResponse GetSurveyAnswerResponse(SurveyAnswerRequest responseId);
        bool ValidateOrganization(OrganizationRequest Request);
        SurveyInfoResponse GetAllSurveysByOrgKey(string OrgKey);
        OrganizationAccountResponse GetUserOrgId(OrganizationAccountRequest Request);
        SourceTablesResponse GetSourceTables(string surveyId);
    }
}
