using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Enter.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using Epi.Web.Enter.Common.DTO;
namespace Epi.Web.MVC.Facade
{
    public interface ISurveyFacade
    {

        MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, Epi.Web.Enter.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsMobileDevice, List<SurveyAnswerDTO> _SurveyAnswerDTOList = null);
        Epi.Web.Enter.Common.DTO.SurveyAnswerDTO CreateSurveyAnswer(string surveyId, string responseId, int UserId, bool IsChild = false, string RelateResponseId = "", bool IsEditMode = false);
        void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form, Epi.Web.Enter.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsSubmited, bool IsSaved, int PageNumber, int UserId);

        SurveyInfoModel GetSurveyInfoModel(string surveyId);
        List<FormInfoModel> GetFormsInfoModelList(FormsInfoRequest formReq);

        SurveyAnswerResponse GetSurveyAnswerResponse(string responseId, string FormId = "", int UserId = 0);
        UserAuthenticationResponse ValidateUser(string userName, string password);
        void UpdatePassCode(string responseId, string passcode);
        UserAuthenticationResponse GetAuthenticationResponse(string responseId);
        ISurveyAnswerRepository GetSurveyAnswerRepository();

        SurveyAnswerResponse GetFormResponseList(SurveyAnswerRequest FormResponseReq);
        FormSettingResponse GetFormSettings(FormSettingRequest FormSettingRequest);

        SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest SARequest);

        SurveyAnswerResponse SetChildRecord(SurveyAnswerRequest SurveyAnswerRequest);
        UserAuthenticationResponse GetUserInfo(int UserId);
        bool UpdateUser(UserDTO User);

        FormSettingResponse SaveSettings(FormSettingRequest FormSettingReq);
        SurveyInfoResponse GetChildFormInfo(SurveyInfoRequest SurveyInfoRequest);
        FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest FormsHierarchyRequest);
        SurveyAnswerResponse GetSurveyAnswerHierarchy(SurveyAnswerRequest pRequest);

        SurveyAnswerResponse GetAncestorResponses(SurveyAnswerRequest pRequest);

        SurveyAnswerResponse GetResponsesByRelatedFormId(SurveyAnswerRequest FormResponseReq);


        void DeleteResponseXml(SurveyAnswerRequest FormResponseReq);

        OrganizationResponse GetOrganizationsByUserId(OrganizationRequest Request);
        OrganizationResponse GetUserOrganizations(OrganizationRequest Request);

        OrganizationResponse GetAdminOrganizations(OrganizationRequest Request);

        OrganizationResponse GetOrganizationInfo(OrganizationRequest Request);

        OrganizationResponse SetOrganization(OrganizationRequest Request);
        OrganizationResponse GetOrganizationUsers(OrganizationRequest OrgRequest);

        UserResponse GetUserInfo(UserRequest Request);
        UserResponse SetUserInfo(UserRequest Request);

        void UpdateResponseStatus(SurveyAnswerRequest Request);

        bool HasResponse(string SurveyId, string ResponseId);
    }
}
