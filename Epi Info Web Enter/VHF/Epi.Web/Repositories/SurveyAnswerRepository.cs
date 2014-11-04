using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.MVC.DataServiceClient;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.Exception;
using System.ServiceModel;


namespace Epi.Web.MVC.Repositories
{
    public class SurveyAnswerRepository : RepositoryBase, ISurveyAnswerRepository
    {



        private Epi.Web.MVC.DataServiceClient.IEWEDataService _iDataService;

        public SurveyAnswerRepository(Epi.Web.MVC.DataServiceClient.IEWEDataService iDataService)
        {
            _iDataService = iDataService;
        }

        /// <summary>
        /// Calling the proxy client to fetch a SurveyResponseResponse object
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public SurveyAnswerResponse GetSurveyAnswer(SurveyAnswerRequest pRequest)
        {
            try
            {
                //SurveyResponseResponse result = Client.GetSurveyResponse(pRequest);
                SurveyAnswerResponse result = _iDataService.GetSurveyAnswer(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SurveyAnswerResponse GetFormResponseList(SurveyAnswerRequest pRequest)
        {
            try
            {
                //SurveyResponseResponse result = Client.GetSurveyResponse(pRequest);
                SurveyAnswerResponse result = _iDataService.GetFormResponseList(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FormSettingResponse GetFormSettings(FormSettingRequest pRequest)
        {
            try
            {

                FormSettingResponse result = _iDataService.GetFormSettings(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserAuthenticationResponse UpdatePassCode(UserAuthenticationRequest AuthenticationRequest)
        {
            try
            {

                UserAuthenticationResponse result = _iDataService.SetPassCode(AuthenticationRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public UserAuthenticationResponse ValidateUser(UserAuthenticationRequest pRequest)
        {
            try
            {

                //UserAuthenticationResponse result = _iDataService.PassCodeLogin(pRequest);
                UserAuthenticationResponse result = _iDataService.UserLogin(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUser(UserAuthenticationRequest pRequest)
        {
            try
            {

                //UserAuthenticationResponse result = _iDataService.PassCodeLogin(pRequest);
                return _iDataService.UpdateUser(pRequest);
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public UserAuthenticationResponse GetUserInfo(UserAuthenticationRequest pRequest)
        {

            try
            {

                UserAuthenticationResponse result = _iDataService.GetUser(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public UserAuthenticationResponse GetAuthenticationResponse(UserAuthenticationRequest pRequest)
        {
            try
            {

                UserAuthenticationResponse result = _iDataService.GetAuthenticationResponse(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SurveyAnswerResponse SaveSurveyAnswer(SurveyAnswerRequest pRequest)
        {
            try
            {
                SurveyAnswerResponse result = _iDataService.SetSurveyAnswer(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SurveyAnswerResponse DeleteResponseXml(SurveyAnswerRequest pRequest)
        {
            try
            {
                SurveyAnswerResponse result = _iDataService.SetSurveyAnswer(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #region stubcode
        public List<Enter.Common.DTO.SurveyAnswerDTO> GetList(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public Enter.Common.DTO.SurveyAnswerDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(Enter.Common.DTO.SurveyAnswerDTO t)
        {
            throw new NotImplementedException();
        }

        public void Update(Enter.Common.DTO.SurveyAnswerDTO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion


        List<SurveyAnswerResponse> IRepository<SurveyAnswerResponse>.GetList(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        SurveyAnswerResponse IRepository<SurveyAnswerResponse>.Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(SurveyAnswerResponse t)
        {
            throw new NotImplementedException();
        }

        public void Update(SurveyAnswerResponse t)
        {
            throw new NotImplementedException();
        }

        public SurveyAnswerResponse SetChildRecord(SurveyAnswerRequest SurveyAnswerRequest)
        {

            try
            {
                SurveyAnswerResponse result = _iDataService.SetSurveyAnswer(SurveyAnswerRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FormSettingResponse SaveSettings(FormSettingRequest FormSettingReq)
        {

            try
            {

                FormSettingResponse result = _iDataService.SaveSettings(FormSettingReq);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public SurveyAnswerResponse GetSurveyAnswerHierarchy(SurveyAnswerRequest pRequest)
        {

            try
            {

                SurveyAnswerResponse result = _iDataService.GetSurveyAnswerHierarchy(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public SurveyAnswerResponse GetSurveyAnswerAncestor(SurveyAnswerRequest pRequest)
        {

            try
            {

                SurveyAnswerResponse result = _iDataService.GetAncestorResponseIdsByChildId(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public SurveyAnswerResponse GetResponsesByRelatedFormId(SurveyAnswerRequest FormResponseReq)
        {

            try
            {

                SurveyAnswerResponse result = _iDataService.GetResponsesByRelatedFormId(FormResponseReq);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public OrganizationResponse GetOrganizationsByUserId(OrganizationRequest OrgReq)
        {

            try
            {

                OrganizationResponse result = _iDataService.GetOrganizationsByUserId(OrgReq);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public OrganizationResponse GetUserOrganizations(OrganizationRequest OrgReq)
        {

            try
            {

                OrganizationResponse result = _iDataService.GetUserOrganizations(OrgReq);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public OrganizationResponse GetAdminOrganizations(OrganizationRequest OrgReq)
        {

            try
            {

                OrganizationResponse result = _iDataService.GetUserOrganizations(OrgReq);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public OrganizationResponse GetOrganizationInfo(OrganizationRequest OrgRequest)
        {


            try
            {

                OrganizationResponse result = _iDataService.GetOrganizationInfo(OrgRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }



        }
        public OrganizationResponse SetOrganization(OrganizationRequest Request)
        {


            try
            {

                OrganizationResponse result = _iDataService.SetOrganization(Request);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }



        }
        public OrganizationResponse GetOrganizationUsers(OrganizationRequest OrgReq)
        {

            try
            {

                OrganizationResponse result = _iDataService.GetOrganizationUsers(OrgReq);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public UserResponse GetUserInfo(UserRequest Request)
        {

            try
            {

                UserResponse result = _iDataService.GetUserInfo(Request);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public UserResponse SetUserInfo(UserRequest Request)
        {
            try
            {

                UserResponse result = _iDataService.SetUserInfo(Request);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }

        public void UpdateResponseStatus(SurveyAnswerRequest Request)
        {
            try
            {

                _iDataService.UpdateResponseStatus(Request);

            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }

        public bool HasResponse(string SurveyId, string ResponseId)
        {
            try
            {

                return _iDataService.HasResponse(SurveyId, ResponseId);

            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
    }
}