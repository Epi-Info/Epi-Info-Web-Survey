using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using Epi.Web.MVC.Facade;
using Epi.Web.Common.DTO;
using System.Collections.Generic;
using System.Linq;
using Epi.Web.Common.Json;

namespace Epi.Web.MVC.Facade
{
    public class SurveyFacade : ISurveyFacade
    {

        // declare ISurveyInfoRepository which inherits IRepository of SurveyInfoResponse object
        private ISurveyInfoRepository _iSurveyInfoRepository;

        // declare ISurveyResponseRepository which inherits IRepository of SurveyResponseResponse object
        private ISurveyAnswerRepository _iSurveyAnswerRepository;
        public ISurveyAnswerRepository GetSurveyAnswerRepository() { return this._iSurveyAnswerRepository; }

        //declare SurveyInfoRequest
        private Epi.Web.Common.Message.SurveyInfoRequest _surveyInfoRequest;

        //declare SurveyResponseRequest
        private Epi.Web.Common.Message.SurveyAnswerRequest _surveyAnswerRequest;

        //declare UserAuthenticationRequest
        private Epi.Web.Common.Message.UserAuthenticationRequest _surveyAuthenticationRequest;
        //declare PassCodeDTO
        private Epi.Web.Common.DTO.PassCodeDTO _PassCodeDTO;
        //declare SurveyAnswerDTO
        private Common.DTO.SurveyAnswerDTO _surveyAnswerDTO;

        //declare SurveyResponseXML object
        private SurveyResponseXML _surveyResponseXML;

        private OrganizationAccountResponse _OrganizationAccountResponse;
        private IOrganizationAccountRepository _iOrgAccountRepository;
        /// <summary>
        /// Injectinting ISurveyInfoRepository through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public SurveyFacade(ISurveyInfoRepository iSurveyInfoRepository, ISurveyAnswerRepository iSurveyResponseRepository,
                                  Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest, Epi.Web.Common.Message.SurveyAnswerRequest surveyResponseRequest,
                                  Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
                                   SurveyResponseXML surveyResponseXML, UserAuthenticationRequest surveyAuthenticationRequest, Epi.Web.Common.DTO.PassCodeDTO PassCodeDTO, IOrganizationAccountRepository iOrgAccountRepository)
        {
            _iSurveyInfoRepository = iSurveyInfoRepository;
            _iSurveyAnswerRepository = iSurveyResponseRepository;
            _surveyInfoRequest = surveyInfoRequest;
            _surveyAnswerRequest = surveyResponseRequest;
            _surveyAnswerDTO = surveyAnswerDTO;
            _surveyResponseXML = surveyResponseXML;
            _surveyAuthenticationRequest = surveyAuthenticationRequest;
            _PassCodeDTO = PassCodeDTO;
            _iOrgAccountRepository = iOrgAccountRepository;
        }

        /// <summary>
        /// get the survey form data
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="responseId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="surveyAnswerDTO"></param>
        /// <returns></returns>
        public MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool isMobileDevice = false, string callerThereby = "", bool IsAndroid = false, bool GetSourceTables = true)
        {
            SourceTablesResponse Response = new SourceTablesResponse();
            SourceTablesRequest Request = new SourceTablesRequest();
            if (GetSourceTables)
            {

                Request.SurveyId = surveyId;
                Response = _iSurveyInfoRepository.GetSourceTables(Request);//Pain Point 


            }
            Epi.Web.Common.DTO.SurveyInfoDTO surveyInfoDTO = SurveyHelper.GetSurveyInfoDTO(_surveyInfoRequest, _iSurveyInfoRepository, surveyId);
            MvcDynamicForms.Form form = null;
            form = Epi.Web.MVC.Utility.FormProvider.GetForm(surveyInfoDTO, pageNumber, surveyAnswerDTO, isMobileDevice, IsAndroid, Response.List);

            return form;
        }
        /// <summary>
        /// This method accepts a surveyId and responseId and creates the first survey response entry
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>

        public Epi.Web.Common.DTO.SurveyAnswerDTO CreateSurveyAnswer(string surveyId, string responseId, int UserId, bool IsChild = false, string RelateResponseId = "", bool IsEditMode = false, int CurrentOrgId = -1)
        {

            return SurveyHelper.CreateSurveyResponse(surveyId, responseId, _surveyAnswerRequest, _surveyAnswerDTO, _surveyResponseXML, _iSurveyAnswerRepository, UserId, IsChild, RelateResponseId, IsEditMode, CurrentOrgId);
        }


        public void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsSubmited, bool IsSaved, int PageNumber)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response and save the response

            //// 1 Get the record for the current survey response
            SurveyAnswerResponse surveyAnswerResponse = GetSurveyAnswerResponse(responseId);

            ///2 Update the current survey response and save it

            SurveyHelper.UpdateSurveyResponse(surveyInfoModel, form, _surveyAnswerRequest, _surveyResponseXML, _iSurveyAnswerRepository, surveyAnswerResponse, responseId, surveyAnswerDTO, IsSubmited, IsSaved, PageNumber);
        }




        public SurveyInfoModel GetSurveyInfoModel(string surveyId)
        {
            _surveyInfoRequest.Criteria.SurveyIdList = new List<string>();
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

        public UserAuthenticationResponse ValidateUser(string responseId, string passcode)
        {
            _surveyAuthenticationRequest.PassCode = passcode;
            _surveyAuthenticationRequest.SurveyResponseId = responseId;
            UserAuthenticationResponse AuthenticationResponse = _iSurveyAnswerRepository.ValidateUser(_surveyAuthenticationRequest);
            return AuthenticationResponse;
        }
        public void UpdatePassCode(string ResponseId, string Passcode) {

            // convert DTO to  UserAuthenticationRquest
            _PassCodeDTO.ResponseId = ResponseId;
            _PassCodeDTO.PassCode = Passcode;
            UserAuthenticationRequest AuthenticationRequestObj = Mapper.ToUserAuthenticationObj(_PassCodeDTO);
            SurveyHelper.UpdatePassCode(AuthenticationRequestObj, _iSurveyAnswerRepository);

        }
        public UserAuthenticationResponse GetAuthenticationResponse(string responseId) {

            _surveyAuthenticationRequest.SurveyResponseId = responseId;
            UserAuthenticationResponse AuthenticationResponse = _iSurveyAnswerRepository.GetAuthenticationResponse(_surveyAuthenticationRequest);
            return AuthenticationResponse;
        }

        public OrganizationAccountResponse CreateAccount(OrganizationAccountRequest AccountRequest)

        {
            OrganizationAccountResponse OrganizationAccountResponse = _iOrgAccountRepository.CreateAccount(AccountRequest);
            return OrganizationAccountResponse;

        }
        public OrganizationAccountResponse GetStateList(OrganizationAccountRequest Request)
        {

            OrganizationAccountResponse OrganizationAccountResponse = _iOrgAccountRepository.GetStateList(Request);
            return OrganizationAccountResponse;

        }
        public SurveyAnswerResponse GetFormResponseList(SurveyAnswerRequest FormResponseReq)
        {

            SurveyAnswerResponse FormResponseList = _iSurveyAnswerRepository.GetFormResponseList(FormResponseReq);

            return FormResponseList;
        }
        public SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage)
        {
            SurveyControlsResponse SurveyControlsResponse = _iSurveyInfoRepository.GetSurveyControlList(pRequestMessage);

            return SurveyControlsResponse;
        }
        public SurveyInfoResponse PublishExcelSurvey(SurveyInfoRequest Request) {

            SurveyInfoResponse SurveyInfoResponse = _iSurveyInfoRepository.PublishExcelSurvey(Request);
            return SurveyInfoResponse;
        }

        public SurveyAnswerResponse GetSurveyAnswerResponse(SurveyAnswerRequest Request)
        {

            SurveyAnswerResponse surveyAnswerResponse = _iSurveyAnswerRepository.GetSurveyAnswer(Request);
            return surveyAnswerResponse;
        }
        public bool ValidateOrganization(OrganizationRequest Request)
        {
            bool IsValid = _iSurveyInfoRepository.ValidateOrganization(Request);
            return IsValid;
        }
        public SurveyInfoResponse GetAllSurveysByOrgKey(string OrgKey)
        {

            SurveyInfoResponse SurveyInfoResponse = _iSurveyInfoRepository.GetAllSurveysByOrgKey(OrgKey);
            return SurveyInfoResponse;
        }

        public OrganizationAccountResponse GetUserOrgId(OrganizationAccountRequest Request) {
            OrganizationAccountResponse Response = new OrganizationAccountResponse();

            Response = _iOrgAccountRepository.GetUserOrgId(Request);

            return Response;
        }
        public SourceTablesResponse GetSourceTables(string surveyId)
        {
            SourceTablesResponse Response = new SourceTablesResponse();
            SourceTablesRequest Request = new SourceTablesRequest();

            Request.SurveyId = surveyId;
            Response = _iSurveyInfoRepository.GetSourceTables(Request);//Pain Point 
            return Response;
        }
        public SurveyInfoResponse GetChildFormInfo(SurveyInfoRequest SurveyInfoRequest)
        {

            SurveyInfoResponse SurveyInfoResponse = _iSurveyInfoRepository.GetFormChildInfo(SurveyInfoRequest);
            return SurveyInfoResponse;
        }
        public FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest FormsHierarchyRequest)
        {


            FormsHierarchyResponse FormsHierarchyResponse = _iSurveyInfoRepository.GetFormsHierarchy(FormsHierarchyRequest);
            return FormsHierarchyResponse;

        }
        public SurveyAnswerResponse GetSurveyAnswerHierarchy(SurveyAnswerRequest pRequest)
        {

            SurveyAnswerResponse SurveyAnswerResponse = _iSurveyAnswerRepository.GetSurveyAnswerHierarchy(pRequest);

            return SurveyAnswerResponse;
        }
        public void UpdateResponseStatus(SurveyAnswerRequest Request)
        {
            _iSurveyAnswerRepository.UpdateResponseStatus(Request);

        }
        public bool HasResponse(string SurveyId, string ResponseId)
        {

            return _iSurveyAnswerRepository.HasResponse(SurveyId, ResponseId);
        }
        public MvcDynamicForms.Form GetSurveyFormData(
          string surveyId,
          int pageNumber,
          Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
          bool IsMobileDevice,
          List<SurveyAnswerDTO> _SurveyAnswerDTOList = null,
          List<Epi.Web.Common.DTO.FormsHierarchyDTO> FormsHierarchyDTOList = null, bool IsAndroid = false, bool GetSourceTables = true)
        {
            List<SurveyInfoDTO> List = new List<SurveyInfoDTO>();


            //Get the SurveyInfoDTO
            Epi.Web.Common.DTO.SurveyInfoDTO surveyInfoDTO;
            if (FormsHierarchyDTOList == null)
            {
                surveyInfoDTO = SurveyHelper.GetSurveyInfoDTO(_surveyInfoRequest, _iSurveyInfoRepository, surveyId);//Pain Point 5s
                if (_SurveyAnswerDTOList != null)
                {
                    foreach (var item in _SurveyAnswerDTOList)
                    {
                        Epi.Web.Common.Message.SurveyInfoRequest request = new SurveyInfoRequest();
                        request.Criteria.SurveyIdList.Add(item.SurveyId);
                        Epi.Web.Common.DTO.SurveyInfoDTO _SurveyInfoDTO = SurveyHelper.GetSurveyInfoDTO(request, _iSurveyInfoRepository, item.SurveyId);
                        List.Add(_SurveyInfoDTO);
                    }
                }
            }

            else
            {
                //  var SurveyInfoDTO = FormsHierarchyDTOList.First(x => x.FormId == surveyAnswerDTO.SurveyId);

                var SurveyInfoDTO = FormsHierarchyDTOList.First(x => x.FormId == surveyId);
                surveyInfoDTO = SurveyInfoDTO.SurveyInfo;



                _SurveyAnswerDTOList = new List<SurveyAnswerDTO>();
                _SurveyAnswerDTOList.Add(surveyAnswerDTO);
                var DTOList = FormsHierarchyDTOList.Where(x => x.ResponseIds.Count() > 0);
                foreach (var item in DTOList)
                {
                    //if (item.ResponseIds.Count() > 0)
                    //{
                    var DTO = item.ResponseIds.FirstOrDefault(z => z.ResponseId == surveyAnswerDTO.RelateParentId);
                    if (DTO != null && !_SurveyAnswerDTOList.Contains(DTO))

                        _SurveyAnswerDTOList.Add(DTO);

                    //}
                }

                foreach (var item in _SurveyAnswerDTOList)
                {
                    var _SurveyInfoDTO = FormsHierarchyDTOList.FirstOrDefault(x => x.FormId == item.SurveyId);
                    List.Add(_SurveyInfoDTO.SurveyInfo);
                }


            }
            MvcDynamicForms.Form form = null;
            SourceTablesResponse Response = new SourceTablesResponse();
            SourceTablesRequest Request = new Common.Message.SourceTablesRequest();
            if (GetSourceTables)
            {
                //if (IsMobileDevice)
                //{
                if (FormsHierarchyDTOList != null && FormsHierarchyDTOList.Count() > 0)
                {
                    Request.SurveyId = FormsHierarchyDTOList[0].FormId.ToString();
                }
                else
                {
                    if (string.IsNullOrEmpty(surveyAnswerDTO.ParentRecordId)) {
                        Request.SurveyId = surveyId;
                    }
                    else {
                        Request.SurveyId = surveyAnswerDTO.ParentRecordId;

                    }
                }
                Response = _iSurveyInfoRepository.GetSourceTables(Request);//Pain Point 
                //}
                //else
                //{
                //    Request.SurveyId = FormsHierarchyDTOList[0].FormId.ToString();
                //    Response = _iSurveyInfoRepository.GetSourceTables(Request);//Pain Point 

                //}
            }
            //if (IsMobileDevice)
            //{
            //    Epi.Web.MVC.Utility.MobileFormProvider.SurveyInfoList = List;
            //    Epi.Web.MVC.Utility.MobileFormProvider.SurveyAnswerList = _SurveyAnswerDTOList;
            //    form = Epi.Web.MVC.Utility.MobileFormProvider.GetForm(surveyInfoDTO, pageNumber, surveyAnswerDTO, IsAndroid, Response.List);
            //}
            //else
            //{
            Epi.Web.MVC.Utility.FormProvider.SurveyInfoList = List;
            Epi.Web.MVC.Utility.FormProvider.SurveyAnswerList = _SurveyAnswerDTOList;
            form = Epi.Web.MVC.Utility.FormProvider.GetForm(surveyInfoDTO, pageNumber, surveyAnswerDTO, IsMobileDevice, IsAndroid, Response.List);
            //}
            return form;
        }
        //public SurveyAnswerDTO CreateSurveyAnswer(string surveyId, string responseId, int UserId, bool IsChild = false, string RelateResponseId = "", bool IsEditMode = false , int CurrentOrgId = -1)
        //{

        //    return SurveyHelper.CreateSurveyResponse(surveyId, responseId, _surveyAnswerRequest, _surveyAnswerDTO, _surveyResponseXML, _iSurveyAnswerRepository, UserId, IsChild, RelateResponseId, IsEditMode, CurrentOrgId);
        //}
        public void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsSubmited, bool IsSaved, int PageNumber, int UserId)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response and save the response

            //// 1 Get the record for the current survey response
            SurveyAnswerResponse surveyAnswerResponse = new SurveyAnswerResponse();//GetSurveyAnswerResponse(responseId, surveyInfoModel.SurveyId.ToString());
            surveyAnswerResponse.SurveyResponseList.Add(surveyAnswerDTO);
            ///2 Update the current survey response and save it

            SurveyHelper.UpdateSurveyResponse(surveyInfoModel, form, _surveyAnswerRequest, _surveyResponseXML, _iSurveyAnswerRepository, surveyAnswerResponse, responseId, surveyAnswerDTO, IsSubmited, IsSaved, PageNumber, UserId);
        }
        /// <summary>
        /// Get the record for the current response (Step1: Saving Survey)
        /// </summary>
        /// <param name="ResponseId"></param>
        /// <returns></returns>
        public SurveyAnswerResponse GetSurveyAnswerResponse(string responseId, string FormId = "", int UserId = 0)
        {
            _surveyAnswerRequest.Criteria.SurveyAnswerIdList.Clear();
            _surveyAnswerRequest.Criteria.SurveyAnswerIdList.Add(responseId);
            _surveyAnswerRequest.Criteria.SurveyId = FormId;
            _surveyAnswerRequest.Criteria.UserId = UserId;
            SurveyAnswerResponse surveyAnswerResponse = _iSurveyAnswerRepository.GetSurveyAnswer(_surveyAnswerRequest);
            return surveyAnswerResponse;
        }
        public SurveyAnswerResponse SetChildRecord(SurveyAnswerRequest surveyAnswerRequest)
        {
            SurveyAnswerResponse Response = new SurveyAnswerResponse();
            return Response;
        }
        public SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest sARequest)
        {
            SurveyAnswerResponse Response = new SurveyAnswerResponse();
            return Response;
        }
        public SurveyAnswerResponse GetResponsesByRelatedFormId(SurveyAnswerRequest formResponseReq)
        {
            SurveyAnswerResponse Response = new SurveyAnswerResponse();
            return Response;
        }

        public void  SaveSurveyAnswer(SurveyAnswerRequest pRequest){

            _iSurveyAnswerRepository.SaveSurveyAnswer(  pRequest);
}

        public string GetSurveyResponseJson(Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, List<FormsHierarchyDTO> FormsHierarchyDTOList, SurveyControlsResponse List)
        {
            SurveyResponseJson Implementation = new SurveyResponseJson();
            return Implementation.GetSurveyResponseJson(surveyAnswerDTO, FormsHierarchyDTOList,List);
        }
        public OrganizationAccountResponse GetOrg(OrganizationAccountRequest Request) {
            OrganizationAccountResponse Response = new OrganizationAccountResponse();

            Response = _iOrgAccountRepository.GetOrg(Request);

            return Response ;

        }
    }
}