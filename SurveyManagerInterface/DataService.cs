using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.ObjectMapping;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Exception;
namespace Epi.Web.WCF.SurveyService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DataService : IDataService
    {

        // Session state variables
        private string _accessToken;
        //private ShoppingCart _shoppingCart;
        private string _userName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public CacheDependencyResponse GetCacheDependencyInfo(CacheDependencyRequest cacheDependencyRequest)
        {
            try
            {
                List<string> surveyKeys = cacheDependencyRequest.Criteria.SurveyIdList; 
                
                CacheDependencyResponse response = new CacheDependencyResponse(surveyKeys);
                Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Interfaces.DataInterfaces.ICacheDependencyInfoDao cacheDependencyInfoDao = entityDaoFactory.CacheDependencyInfoDao;
                Epi.Web.BLL.CacheDependencyInfo cacheDependencyInfo = new Epi.Web.BLL.CacheDependencyInfo(cacheDependencyInfoDao);

                List<CacheDependencyBO> bo = cacheDependencyInfo.GetCacheDependencyInfo(surveyKeys);
                List<CacheDependencyDTO> dto = Mapper.ToDataTransferObject(bo);

                Dictionary<string, DateTime> dictionary = new Dictionary<string, DateTime>();

                foreach (CacheDependencyDTO item in dto)
                {
                    dictionary.Add(item.SurveyId, item.LastUpdate);
                }

                response.SurveyDependency = dictionary;

                return response;
            }
            catch (Exception ex)
            {
                CustomFaultException customFaultException = new CustomFaultException();
                customFaultException.CustomMessage = ex.Message;
                customFaultException.Source = ex.Source;
                customFaultException.StackTrace = ex.StackTrace;
                customFaultException.HelpLink = ex.HelpLink;
                throw new FaultException<CustomFaultException>(customFaultException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest surveyInfoRequest)
        {
            try
            {
                SurveyInfoResponse response = new SurveyInfoResponse(surveyInfoRequest.RequestId);

                Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
                Epi.Web.BLL.SurveyInfo surveyInfo = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);

                if (!ValidRequest(surveyInfoRequest, response, Validate.All))
                {
                    return response;
                }

                var criteria = surveyInfoRequest.Criteria as SurveyInfoCriteria;
                string sort = criteria.SortExpression;
                List<string> SurveyIdList = new List<string>();
                foreach (string id in criteria.SurveyIdList)
                {
                    SurveyIdList.Add(id.ToUpper());
                }

                List<SurveyInfoBO> bo = surveyInfo.GetSurveyInfoById(SurveyIdList);

                response.SurveyInfoList = Mapper.ToDataTransferObject(bo);

                return response;
            }
            catch (Exception ex)
            {
                CustomFaultException customFaultException = new CustomFaultException();
                customFaultException.CustomMessage = ex.Message;
                customFaultException.Source = ex.Source;
                customFaultException.StackTrace = ex.StackTrace;
                customFaultException.HelpLink = ex.HelpLink;
                throw new FaultException<CustomFaultException>(customFaultException);
            }
        }


        /// <summary>
        /// Set (add, update, delete) SurveyInfo value.
        /// </summary>
        /// <param name="request">SurveyInfoRequest request message.</param>
        /// <returns>SurveyInfoRequest response message.</returns>
        public SurveyInfoResponse SetSurveyInfo(SurveyInfoRequest request)
        {
            try
            {
                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);


                var response = new SurveyInfoResponse(request.RequestId);

                // Validate client tag, access token, and user credentials
                if (!ValidRequest(request, response, Validate.All))
                    return response;

                // Transform SurveyInfo data transfer object to SurveyInfo business object
                var SurveyInfo = Mapper.ToBusinessObject(request.SurveyInfoList[0]);

                // Validate SurveyInfo business rules

                if (request.Action != "Delete")
                {
                    //if (!SurveyInfo.Validate())
                    //{
                    //    response.Acknowledge = AcknowledgeType.Failure;

                    //    foreach (string error in SurveyInfo.ValidationErrors)
                    //        response.Message += error + Environment.NewLine;

                    //    return response;
                    //}
                }

                // Run within the context of a database transaction. Currently commented out.
                // The Decorator Design Pattern. 
                //using (TransactionDecorator transaction = new TransactionDecorator())
                {
                    if (request.Action == "Create")
                    {
                        if (request.Criteria.FileInputStream == null)
                        {
                            Implementation.InsertSurveyInfo(SurveyInfo);
                            response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
                        }
                        else { 
                        // Excel 
                            string SurveyFile = Implementation.GetSurveyXml(request.Criteria.FileInputStream);
                            SurveyInfo.XML = SurveyFile;
                            Implementation.InsertSurveyInfo(SurveyInfo);
                            response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
                        }
                    }
                    else if (request.Action == "Update")
                    {
                         if (request.Criteria.FileInputStream == null)
                        {
                        Implementation.UpdateSurveyInfo(SurveyInfo);
                        response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
                        }
                         else
                         {
                        // Excel 
                             string SurveyFile = Implementation.GetSurveyXml(request.Criteria.FileInputStream);
                             SurveyInfo.XML = SurveyFile;
                             Implementation.UpdateSurveyInfo(SurveyInfo);
                             response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
                         }
                    }
                    else if (request.Action == "Delete")
                    {
                        var criteria = request.Criteria as SurveyInfoCriteria;
                        var survey = Implementation.GetSurveyInfoById(SurveyInfo.SurveyId);

                        try
                        {
                            if (Implementation.DeleteSurveyInfo(survey))
                            {
                                response.RowsAffected = 1;
                            }
                            else
                            {
                                response.RowsAffected = 0;
                            }
                        }
                        catch
                        {
                            response.RowsAffected = 0;
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                CustomFaultException customFaultException = new CustomFaultException();
                customFaultException.CustomMessage = ex.Message;
                customFaultException.Source = ex.Source;
                customFaultException.StackTrace = ex.StackTrace;
                customFaultException.HelpLink = ex.HelpLink;
                throw new FaultException<CustomFaultException>(customFaultException);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public SurveyAnswerResponse GetSurveyAnswer(SurveyAnswerRequest pRequest)
        {
            try
            {
                SurveyAnswerResponse result = new SurveyAnswerResponse(pRequest.RequestId);
                //Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao surveyInfoDao = new EF.EntitySurveyResponseDao();
                //Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(surveyInfoDao);

                Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);


                // Validate client tag, access token, and user credentials
                if (!ValidRequest(pRequest, result, Validate.All))
                {
                    return result;
                }

                var criteria = pRequest.Criteria as SurveyAnswerCriteria;
                string sort = criteria.SortExpression;

                //if (request.LoadOptions.Contains("SurveyInfos"))
                //{
                //    IEnumerable<SurveyInfoDTO> SurveyInfos;
                //    if (!criteria.IncludeOrderStatistics)
                //    {
                //        SurveyInfos = Implementation.GetSurveyInfos(sort);
                //    }
                //    else
                //    {
                //        SurveyInfos = Implementation.GetSurveyInfosWithOrderStatistics(sort);
                //    }

                //    response.SurveyInfos = SurveyInfos.Select(c => Mapper.ToDataTransferObject(c)).ToList();
                //}

                //if (pRequest.LoadOptions.Contains("SurveyInfo"))
                //{
                result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseById(pRequest.Criteria.SurveyAnswerIdList, pRequest.Criteria.UserPublishKey));
                //}

                return result;
            }
            catch (Exception ex)
            {
                CustomFaultException customFaultException = new CustomFaultException();
                customFaultException.CustomMessage = ex.Message;
                customFaultException.Source = ex.Source;
                customFaultException.StackTrace = ex.StackTrace;
                customFaultException.HelpLink = ex.HelpLink;
                throw new FaultException<CustomFaultException>(customFaultException);
            }
        }



        /// <summary>
        /// Set (add, update, delete) SurveyInfo value.
        /// </summary>
        /// <param name="request">SurveyResponse request message.</param>
        /// <returns>SurveyResponse response message.</returns>
        public SurveyAnswerResponse SetSurveyAnswer(SurveyAnswerRequest request)
        {
            try
            {
                Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);


                SurveyAnswerResponse response = new SurveyAnswerResponse(request.RequestId);

                // Validate client tag, access token, and user credentials
                if (!ValidRequest(request, response, Validate.All))
                {
                    return response;
                }

                // Transform SurveyResponse data transfer object to SurveyResponse business object
                SurveyResponseBO SurveyResponse = Mapper.ToBusinessObject(request.SurveyAnswerList)[0];

                // Validate SurveyResponse business rules

                if (request.Action != "Delete")
                {
                    //if (!SurveyResponse.Validate())
                    //{
                    //    response.Acknowledge = AcknowledgeType.Failure;

                    //    foreach (string error in SurveyResponse.ValidationErrors)
                    //        response.Message += error + Environment.NewLine;

                    //    return response;
                    //}
                }

                // Run within the context of a database transaction. Currently commented out.
                // The Decorator Design Pattern. 
                //using (TransactionDecorator transaction = new TransactionDecorator())
                {
                    if (request.Action.Equals("Create", StringComparison.OrdinalIgnoreCase))
                    {
                        Implementation.InsertSurveyResponse(SurveyResponse);
                        response.SurveyResponseList.Add(Mapper.ToDataTransferObject(SurveyResponse));
                    }
                    else if (request.Action.Equals("Update", StringComparison.OrdinalIgnoreCase))
                    {
                        Implementation.UpdateSurveyResponse(SurveyResponse);
                        response.SurveyResponseList.Add(Mapper.ToDataTransferObject(SurveyResponse));
                    }
                    else if (request.Action.Equals("Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        var criteria = request.Criteria as SurveyAnswerCriteria;
                        var survey = Implementation.GetSurveyResponseById(new List<string> { SurveyResponse.SurveyId },SurveyResponse.UserPublishKey);

                        foreach (SurveyResponseBO surveyResponse in survey)
                        {
                            try
                            {

                                if (Implementation.DeleteSurveyResponse(surveyResponse))
                                {
                                    response.RowsAffected += 1;
                                }

                            }
                            catch
                            {
                                //response.RowsAffected = 0;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                CustomFaultException customFaultException = new CustomFaultException();
                customFaultException.CustomMessage = ex.Message;
                customFaultException.Source = ex.Source;
                customFaultException.StackTrace = ex.StackTrace;
                customFaultException.HelpLink = ex.HelpLink;
                throw new FaultException<CustomFaultException>(customFaultException);
            }
        }

        /// <summary>
        /// Gets unique session based token that is valid for the duration of the session.
        /// </summary>
        /// <param name="request">Token request message.</param>
        /// <returns>Token response message.</returns>
        public TokenResponse GetToken(TokenRequest request)
        {
            var response = new TokenResponse(request.RequestId);

            // Validate client tag only
            if (!ValidRequest(request, response, Validate.ClientTag))
                return response;

            // Note: these are session based and expire when session expires.
            _accessToken = Guid.NewGuid().ToString();
            //_shoppingCart = new ShoppingCart(_defaultShippingMethod);

            response.AccessToken = _accessToken;
            return response;
        }

        /// <summary>
        /// Login to application service.
        /// </summary>
        /// <param name="request">Login request message.</param>
        /// <returns>Login response message.</returns>
        public LoginResponse Login(LoginRequest request)
        {
            var response = new LoginResponse(request.RequestId);

            // Validate client tag and access token
            if (!ValidRequest(request, response, Validate.ClientTag | Validate.AccessToken))
                return response;

            if (! ValidateUser(request.UserName, request.Password))
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = "Invalid username and/or password.";
                return response;
            }

            _userName = request.UserName;

            return response;
        }


        public UserAuthenticationResponse PassCodeLogin(UserAuthenticationRequest request)
        {
            var response = new UserAuthenticationResponse();
            Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);

            UserAuthenticationRequestBO PassCodeBO = Mapper.ToPassCodeBO(request);
            bool result = Implementation.ValidateUser(PassCodeBO);
 
          if (result)
          {

              response.Acknowledge = AcknowledgeType.Failure;
              response.Message = "Invalid Pass Code.";
              response.UserIsValid = true;
              
          }
          else 
          {
              response.UserIsValid = false;
          
          }
           
             
              return response;
        }
        public UserAuthenticationResponse GetAuthenticationResponse(UserAuthenticationRequest pRequest)
        {
            UserAuthenticationResponse response = new UserAuthenticationResponse();
            Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);


            Epi.Web.Common.BusinessObject.UserAuthenticationRequestBO PassCodeBO = Mapper.ToPassCodeBO(pRequest);

            response = Mapper.ToAuthenticationResponse(Implementation.GetAuthenticationResponse(PassCodeBO));

 
            return response;
        
        
        
        }

        public UserAuthenticationResponse SetPassCode(UserAuthenticationRequest request) { 
        
        
            var response = new UserAuthenticationResponse();
            Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);

            Epi.Web.Common.BusinessObject.UserAuthenticationRequestBO PassCodeBO = Mapper.ToPassCodeBO(request);
            Implementation.SavePassCode(PassCodeBO);


            return response;
        
        
        
        }
        /// <summary>
        /// Logout from application service.
        /// </summary>
        /// <param name="request">Logout request message.</param>
        /// <returns>Login request message.</returns>
        public LogoutResponse Logout(LogoutRequest request)
        {
            var response = new LogoutResponse(request.RequestId);

            // Validate client tag and access token
            if (!ValidRequest(request, response, Validate.ClientTag | Validate.AccessToken))
                return response;

            _userName = null;

            return response;
        }

        /// <summary>
        /// Validate 3 security levels for a request: ClientTag, AccessToken, and User Credentials
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="response">The response message.</param>
        /// <param name="validate">The validation that needs to take place.</param>
        /// <returns></returns>
        private bool ValidRequest(RequestBase request, ResponseBase response, Validate validate)
        {
            bool result = true;

            // Validate Client Tag. 
            // Hardcoded here. In production this should query a 'client' table in a database.
            if ((Validate.ClientTag & validate) == Validate.ClientTag)
            {
                if (request.ClientTag != "ABC123")
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Unknown Client Tag";
                    //return false;
                }
            }


            // Validate access token
            if ((Validate.AccessToken & validate) == Validate.AccessToken)
            {
                if (request.AccessToken != _accessToken)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Invalid or expired AccessToken. Call GetToken()";
                    //return false;
                }
            }

            // Validate user credentials
            if ((Validate.UserCredentials & validate) == Validate.UserCredentials)
            {
                if (_userName == null)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Please login and provide user credentials before accessing these methods.";
                    //return false;
                }
            }


            return result;
        }

        /// <summary>
        /// Validation options enum. Used in validation of messages.
        /// </summary>
        [Flags]
        private enum Validate
        {
            ClientTag = 0x0001,
            AccessToken = 0x0002,
            UserCredentials = 0x0004,
            All = ClientTag | AccessToken | UserCredentials
        }


        private bool ValidateUser(string pUserName, string pPassword)
        {
            bool result = true;

            return result;
        }

        public OrganizationAccountResponse CreateAccount(OrganizationAccountRequest pRequest) 
            {
            OrganizationAccountResponse OrgAccountResponse = new OrganizationAccountResponse();
           
            try {
                Epi.Web.Interfaces.DataInterfaces.IAdminDao AdminDao = new EF.EntityAdminDao();
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Account Implementation = new Epi.Web.BLL.Account(AdminDao, OrganizationDao);
                var Organization = Mapper.ToBusinessObject(pRequest.Organization);
                var Admin = Mapper.ToBusinessObject(pRequest.Admin);
                OrgAccountResponse.Message = Implementation.CreateAccount(pRequest.AccountType, Admin, Organization);

            }
            catch (Exception ex) 
                { 
                throw ex; 
                }
            return OrgAccountResponse;
            
            
            }

        public OrganizationAccountResponse GetUserOrgId(OrganizationAccountRequest Request)
        {
            OrganizationAccountResponse  Response = new OrganizationAccountResponse();

            try
            {
                Epi.Web.Interfaces.DataInterfaces.IAdminDao AdminDao = new EF.EntityAdminDao();
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Account Implementation = new Epi.Web.BLL.Account(AdminDao, OrganizationDao);
                Response.OrganizationDTO = new OrganizationDTO();
                Response.OrganizationDTO.OrganizationKey = Implementation.GetUserOrgId(Request.Admin.AdminEmail);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Response;
        }

        public OrganizationAccountResponse GetStateList(OrganizationAccountRequest Request) 
            {

            OrganizationAccountResponse OrgAccountResponse = new OrganizationAccountResponse();

            try
                {
                
                Epi.Web.Interfaces.DataInterfaces.IStateDao StateDao = new EF.EntityStateDao();
                Epi.Web.BLL.Account Implementation = new Epi.Web.BLL.Account(StateDao);
                 
               OrgAccountResponse.StateList= Mapper.ToStateDTO(Implementation.GetStateList());


                }
            catch (Exception ex)
                {
                throw ex;
                }
            return OrgAccountResponse;
            
            }

        public SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage)
            {

            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();

            try
                {


                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(ISurveyInfoDao);
                SurveyControlsResponse = Implementation.GetSurveyControlList(pRequestMessage.SurveyId);

                }
            catch (Exception ex)
                {
                SurveyControlsResponse.Message = "Error";
                throw ex;
                }






            return SurveyControlsResponse;
            }
        public SourceTablesResponse GetSourceTables(SourceTablesRequest Request) {

            SourceTablesResponse DropDownsResponse = new SourceTablesResponse();
            Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
            Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);

            DropDownsResponse.List = Mapper.ToSourceTableDTO(Implementation.GetSourceTables(Request.SurveyId));
            return DropDownsResponse;
        
        }
    }
}
