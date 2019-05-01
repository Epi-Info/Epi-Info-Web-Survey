using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using Epi.Web.Common.DTO;

namespace Epi.Web.MVC.Facade
{
    public interface ISurveyFacade
    {

        MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsMobileDevice, List<SurveyAnswerDTO> _SurveyAnswerDTOList = null, List<FormsHierarchyDTO> FormsHierarchyDTOList = null,bool IsAndroid = false ,bool SetAllDropDownValues=true);
        Epi.Web.Common.DTO.SurveyAnswerDTO CreateSurveyAnswer(string surveyId, string responseId, int UserId=0, bool IsChild = false, string RelateResponseId = "", bool IsEditMode = false, int CurrentOrgId = -1);
        void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsSubmited, bool IsSaved, int PageNumber, int UserId=-1);

        SurveyInfoModel GetSurveyInfoModel(string surveyId);
        SurveyAnswerResponse GetSurveyAnswerResponse(string responseId,string SurveyId= "", int UserId = 0);
        UserAuthenticationResponse ValidateUser(string responseId, string passcode);
        void UpdatePassCode(string responseId, string passcode);
        UserAuthenticationResponse GetAuthenticationResponse(string responseId);
        ISurveyAnswerRepository GetSurveyAnswerRepository();
        OrganizationAccountResponse CreateAccount(OrganizationAccountRequest AccountRequest);
        SurveyAnswerResponse GetFormResponseList(SurveyAnswerRequest FormResponseReq);
        OrganizationAccountResponse GetStateList(OrganizationAccountRequest Request);

        SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage);
        SurveyInfoResponse PublishExcelSurvey(SurveyInfoRequest Request);
        SurveyAnswerResponse GetSurveyAnswerResponse(SurveyAnswerRequest responseId);
        bool ValidateOrganization(OrganizationRequest Request);
        SurveyInfoResponse GetAllSurveysByOrgKey(string OrgKey);
        OrganizationAccountResponse GetUserOrgId(OrganizationAccountRequest Request);
        SourceTablesResponse GetSourceTables(string surveyId);
        SurveyInfoResponse GetChildFormInfo(SurveyInfoRequest SurveyInfoRequest);
        FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest FormsHierarchyRequest);
        SurveyAnswerResponse GetSurveyAnswerHierarchy(SurveyAnswerRequest pRequest);
        void UpdateResponseStatus(SurveyAnswerRequest Request);
        bool HasResponse(string SurveyId, string ResponseId);
        SurveyAnswerResponse SetChildRecord(SurveyAnswerRequest surveyAnswerRequest);
        SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest sARequest);
        SurveyAnswerResponse GetResponsesByRelatedFormId(SurveyAnswerRequest formResponseReq);
        void SaveSurveyAnswer(SurveyAnswerRequest pRequest);
        string GetSurveyResponseJson(Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, List<FormsHierarchyDTO> FormsHierarchyDTOList, Dictionary<string, SurveyControlsResponse> List);
          OrganizationAccountResponse GetOrg(OrganizationAccountRequest Request);
        bool SetJsonColumnAll(string orgid);
    }

}
