using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Enter.Common.DTO;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Common.ObjectMapping;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Exception;
using System.Collections;
using Epi.Web.Enter.Interfaces.DataInterface;
using System.Configuration;
namespace Epi.Web.WCF.SurveyService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class EWEDataService : IEWEDataService
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
        public SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
        {
            try
            {
                SurveyInfoResponse result = new SurveyInfoResponse(pRequest.RequestId);
                //Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
                //Epi.Web.BLL.SurveyInfo implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);

                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
                Epi.Web.BLL.SurveyInfo implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);

                // Validate client tag, access token, and user credentials
                if (!ValidRequest(pRequest, result, Validate.All))
                {
                    return result;
                }

                var criteria = pRequest.Criteria as SurveyInfoCriteria;
                string sort = criteria.SortExpression;
                List<string> SurveyIdList = new List<string>();
                foreach (string id in criteria.SurveyIdList)
                {
                    SurveyIdList.Add(id.ToUpper());
                }


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
                result.SurveyInfoList = Mapper.ToDataTransferObject(implementation.GetSurveyInfoById(SurveyIdList));
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
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FormsInfoResponse GetFormsInfo(FormsInfoRequest pRequest)
        {
            FormsInfoResponse result = new FormsInfoResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            IFormInfoDao FormInfoDao = entityDaoFactory.FormInfoDao;
            Epi.Web.BLL.FormInfo implementation = new Epi.Web.BLL.FormInfo(FormInfoDao);
            try
            {
                List<FormInfoBO> FormInfoBOList = implementation.GetFormsInfo(pRequest.Criteria.UserId);
                //  result.SurveyInfoList = FormInfoBOList;

                foreach (FormInfoBO item in FormInfoBOList)
                {
                    result.FormInfoList.Add(Mapper.ToFormInfoDTO(item));
                }


            }
            catch (Exception ex)
            {

            }
            return result;


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
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
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
                        Implementation.InsertSurveyInfo(SurveyInfo);
                        response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
                    }
                    else if (request.Action == "Update")
                    {
                        Implementation.UpdateSurveyInfo(SurveyInfo);
                        response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
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
                //Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao surveyInfoDao = new EF.EntitySurveyResponseDao();
                //Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(surveyInfoDao);

                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);


                // Validate client tag, access token, and user credentials
                if (!ValidRequest(pRequest, result, Validate.All))
                {
                    return result;
                }

                var criteria = pRequest.Criteria as SurveyAnswerCriteria;
                string sort = criteria.SortExpression;

                //if (request.LoadOptions.Contains("SurveyInfos"))
                //    {
                //    IEnumerable<SurveyInfoDTO> SurveyInfos;
                //    if (!criteria.IncludeOrderStatistics)
                //        {
                //        SurveyInfos = Implementation.GetSurveyInfos(sort);
                //        }
                //    else
                //        {
                //        SurveyInfos = Implementation.GetSurveyInfosWithOrderStatistics(sort);
                //        }

                //    response.SurveyInfos = SurveyInfos.Select(c => Mapper.ToDataTransferObject(c)).ToList();
                //    }

                //if (pRequest.LoadOptions.Contains("SurveyInfo"))
                //{
                //result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseById(pRequest.Criteria.SurveyAnswerIdList, pRequest.Criteria.UserPublishKey));
                //}
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyInfo Implementation1 = new Epi.Web.BLL.SurveyInfo(SurveyInfoDao);
                SurveyInfoBO SurveyInfoBO = Implementation1.GetSurveyInfoById(pRequest.Criteria.SurveyId);
                List<SurveyInfoBO> SurveyInfoBOList = new List<SurveyInfoBO>();
                SurveyInfoBOList.Add(SurveyInfoBO);

                // result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseById(pRequest.Criteria.SurveyAnswerIdList, pRequest.Criteria.UserPublishKey, pRequest.Criteria.SurveyId, SurveyInfoBOList));

                result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseById(pRequest.Criteria, SurveyInfoBOList));
                //SurveyResponseBO Request = new SurveyResponseBO();

                //foreach (var item in pRequest.Criteria.SurveyAnswerIdList)
                //    {
                //    Request.ResponseId = item;
                //    Request.UserId = criteria.UserId;

                //    result.SurveyResponseList.Add(Mapper.ToDataTransferObject(Implementation.GetSurveyResponseByUserId(Request)));
                //    }

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
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);


                SurveyAnswerResponse response = new SurveyAnswerResponse(request.RequestId);

                // Validate client tag, access token, and user credentials
                if (!ValidRequest(request, response, Validate.All))
                {
                    return response;
                }

                // Transform SurveyResponse data transfer object to SurveyResponse business object

                SurveyResponseBO SurveyResponse = Mapper.ToBusinessObject(request.SurveyAnswerList, request.Criteria.UserId)[0];

                SurveyResponse.UserId = request.Criteria.UserId;
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
                    else if (request.Action.Equals("CreateMulti", StringComparison.OrdinalIgnoreCase))
                    {

                        Epi.Web.BLL.SurveyResponse Implementation1 = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);
                        List<SurveyResponseBO> SurveyResponseBOList = Implementation1.GetResponsesHierarchyIdsByRootId(request.SurveyAnswerList[0].ParentRecordId);
                        //check if any orphan records exists 
                        foreach (var item in SurveyResponseBOList)
                        {

                            SurveyResponseBO SurveyResponseBO = Implementation.GetResponseXml(item.ResponseId);
                            if (!string.IsNullOrEmpty(SurveyResponseBO.ResponseId))
                            {
                                SurveyResponseBO.UserId = request.Criteria.UserId;
                                ResponseXmlBO ResponseXmlBO = new ResponseXmlBO();
                                ResponseXmlBO.ResponseId = SurveyResponseBO.ResponseId;
                                Implementation.DeleteResponseXml(ResponseXmlBO);

                            }
                        }

                        SurveyResponseBOList = Implementation1.GetResponsesHierarchyIdsByRootId(request.SurveyAnswerList[0].ParentRecordId);

                        response.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.InsertSurveyResponse(SurveyResponseBOList, request.Criteria.UserId));
                    }
                    else if (request.Action.Equals("CreateChild", StringComparison.OrdinalIgnoreCase))
                    {
                        Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao = new EF.EntitySurveyInfoDao();
                        Epi.Web.BLL.SurveyInfo Implementation1 = new Epi.Web.BLL.SurveyInfo(SurveyInfoDao);
                        SurveyInfoBO SurveyInfoBO = Implementation1.GetParentInfoByChildId(SurveyResponse.SurveyId);

                        Implementation.InsertChildSurveyResponse(SurveyResponse, SurveyInfoBO, request.SurveyAnswerList[0].RelateParentId);
                        response.SurveyResponseList.Add(Mapper.ToDataTransferObject(SurveyResponse));

                        List<SurveyResponseBO> List = new List<SurveyResponseBO>();
                        List.Add(SurveyResponse);
                        Implementation.InsertSurveyResponse(List, request.Criteria.UserId, true);

                    }
                    else if (request.Action.Equals("Update", StringComparison.OrdinalIgnoreCase))
                    {
                        Implementation.UpdateSurveyResponse(SurveyResponse);
                        response.SurveyResponseList.Add(Mapper.ToDataTransferObject(SurveyResponse));
                    }
                    else if (request.Action.Equals("UpdateMulti", StringComparison.OrdinalIgnoreCase))
                    {
                        Implementation.UpdateSurveyResponse(SurveyResponse);
                        response.SurveyResponseList.Add(Mapper.ToDataTransferObject(SurveyResponse));

                        Epi.Web.BLL.SurveyResponse Implementation1 = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);
                        List<SurveyResponseBO> SurveyResponseBOList = Implementation1.GetResponsesHierarchyIdsByRootId(request.SurveyAnswerList[0].ResponseId);


                        List<SurveyResponseBO> ResultList = Implementation.UpdateSurveyResponse(SurveyResponseBOList, SurveyResponse.Status);
                        foreach (var Obj in ResultList)
                        {

                            response.SurveyResponseList.Add(Mapper.ToDataTransferObject(Obj));
                        }
                    }
                    else if (request.Action.Equals("Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        var criteria = request.Criteria as SurveyAnswerCriteria;
                        criteria.SurveyAnswerIdList = new List<string> { SurveyResponse.SurveyId };
                        criteria.UserPublishKey = SurveyResponse.UserPublishKey;
                        criteria.SurveyId = request.Criteria.SurveyId;
                        var survey = Implementation.GetSurveyResponseById(criteria);

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
                    else if (request.Action.Equals("DeleteResponseXml", StringComparison.OrdinalIgnoreCase))
                    {

                        foreach (var item in request.SurveyAnswerList)
                        {
                            try
                            {
                                ResponseXmlBO ResponseXmlBO = new ResponseXmlBO();
                                ResponseXmlBO.ResponseId = item.ResponseId;
                                Implementation.DeleteResponseXml(ResponseXmlBO);

                            }
                            catch
                            {

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

            if (!ValidateUser(request.UserName, request.Password))
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
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
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

        public UserAuthenticationResponse UserLogin(UserAuthenticationRequest request)
        {


            var response = new UserAuthenticationResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;
            Epi.Web.BLL.User Implementation = new Epi.Web.BLL.User(IUserDao);

            UserBO UserBO = Mapper.ToUserBO(request.User);

            UserBO result = Implementation.GetUser(UserBO);



            if (result != null)
            {

                //response.Acknowledge = AcknowledgeType.Failure; TBD
                //response.Message = "Invalid Pass Code.";
                response.User = Mapper.ToUserDTO(result);
                response.UserIsValid = true;

            }
            else
            {
                response.UserIsValid = false;

            }


            return response;
        }


        public bool UpdateUser(UserAuthenticationRequest request)
        {
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;
            Epi.Web.BLL.User Implementation = new Epi.Web.BLL.User(IUserDao);

            UserBO UserBO = Mapper.ToUserBO(request.User);
            OrganizationBO OrgBO = new OrganizationBO();
            return Implementation.UpdateUser(UserBO, OrgBO);


        }

        public UserAuthenticationResponse GetAuthenticationResponse(UserAuthenticationRequest pRequest)
        {
            UserAuthenticationResponse response = new UserAuthenticationResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);


            Epi.Web.Enter.Common.BusinessObject.UserAuthenticationRequestBO PassCodeBO = Mapper.ToPassCodeBO(pRequest);

            response = Mapper.ToAuthenticationResponse(Implementation.GetAuthenticationResponse(PassCodeBO));


            return response;



        }

        public UserAuthenticationResponse SetPassCode(UserAuthenticationRequest request)
        {


            var response = new UserAuthenticationResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);

            Epi.Web.Enter.Common.BusinessObject.UserAuthenticationRequestBO PassCodeBO = Mapper.ToPassCodeBO(request);
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

        public FormResponseInfoResponse GetFormResponseInfo(FormResponseInfoRequest pRequest)
        {

            FormResponseInfoResponse FormResponseInfoResponse = new FormResponseInfoResponse();
            return FormResponseInfoResponse;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public SurveyAnswerResponse GetFormResponseList(SurveyAnswerRequest pRequest)
        {
            try
            {
                SurveyAnswerResponse result = new SurveyAnswerResponse(pRequest.RequestId);


                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);

                SurveyAnswerCriteria criteria = pRequest.Criteria;
                //result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetFormResponseListById(pRequest.Criteria.SurveyId, pRequest.Criteria.PageNumber, pRequest.Criteria.IsMobile));
                result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetFormResponseListById(criteria));
                //Query The number of records

                //result.NumberOfPages = Implementation.GetNumberOfPages(pRequest.Criteria.SurveyId, pRequest.Criteria.IsMobile);
                //result.NumberOfResponses = Implementation.GetNumberOfResponses(pRequest.Criteria.SurveyId);

                result.NumberOfPages = Implementation.GetNumberOfPages(pRequest.Criteria);
                result.NumberOfResponses = Implementation.GetNumberOfResponses(pRequest.Criteria);

                //Get form info 
                Epi.Web.Enter.Interfaces.DataInterface.IFormInfoDao surveyInfoDao = new EF.EntityFormInfoDao();
                Epi.Web.BLL.FormInfo ImplementationFormInfo = new Epi.Web.BLL.FormInfo(surveyInfoDao);
                result.FormInfo = Mapper.ToFormInfoDTO(ImplementationFormInfo.GetFormInfoByFormId(pRequest.Criteria.SurveyId, false, pRequest.Criteria.UserId));

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


        public FormSettingResponse GetFormSettings(FormSettingRequest pRequest)
        {


            FormSettingResponse Response = new FormSettingResponse();
            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();


                IFormInfoDao FormInfoDao = entityDaoFactory.FormInfoDao;
                Epi.Web.BLL.FormInfo FormInfoImplementation = new Epi.Web.BLL.FormInfo(FormInfoDao);
                FormInfoBO FormInfoBO = FormInfoImplementation.GetFormInfoByFormId(pRequest.FormInfo.FormId, pRequest.GetXml, pRequest.FormInfo.UserId);
                Response.FormInfo = Mapper.ToFormInfoDTO(FormInfoBO);


                Epi.Web.Enter.Interfaces.DataInterface.IFormSettingDao IFormSettingDao = entityDaoFactory.FormSettingDao;
                Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;
                Epi.Web.Enter.Interfaces.DataInterface.IFormInfoDao IFormInfoDao = entityDaoFactory.FormInfoDao;
                Epi.Web.BLL.FormSetting SettingsImplementation = new Epi.Web.BLL.FormSetting(IFormSettingDao, IUserDao, IFormInfoDao);
                Response.FormSetting = Mapper.ToDataTransferObject(SettingsImplementation.GetFormSettings(pRequest.FormInfo.FormId.ToString(), FormInfoBO.Xml));



                return Response;


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


        public SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest pRequest)
        {

            try
            {
                SurveyAnswerResponse result = new SurveyAnswerResponse(pRequest.RequestId);


                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);
                foreach (var response in pRequest.SurveyAnswerList)
                {
                    if (pRequest.Criteria.IsSqlProject)
                    {
                        if (pRequest.Criteria.IsEditMode)
                        {
                            Implementation.DeleteSurveyResponseInEditMode(Mapper.ToBusinessObject(response, pRequest.Criteria.UserId));
                        }
                        else
                        {
                            if (pRequest.Criteria.IsDeleteMode)
                            {
                                Implementation.DeleteSurveyResponse(Mapper.ToBusinessObject(response, pRequest.Criteria.UserId));
                            }
                            else
                            {
                                //do status Update
                                var obj = Mapper.ToBusinessObject(response, pRequest.Criteria.UserId);
                                obj.SurveyId = pRequest.Criteria.SurveyId;
                                obj.Status = 0;
                                Implementation.UpdateRecordStatus(obj);
                            }

                        }
                    }
                    else
                    {
                        if (pRequest.Criteria.IsEditMode)
                        {
                            Implementation.DeleteSurveyResponseInEditMode(Mapper.ToBusinessObject(response, pRequest.Criteria.UserId));
                        }
                        else
                        {

                            Implementation.DeleteSurveyResponse(Mapper.ToBusinessObject(response, pRequest.Criteria.UserId));
                        }



                    }
                }

                return result;
                //return null;
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
        public UserAuthenticationResponse GetUser(UserAuthenticationRequest request)
        {

            var response = new UserAuthenticationResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;
            Epi.Web.BLL.User Implementation = new Epi.Web.BLL.User(IUserDao);

            UserBO UserBO = Mapper.ToUserBO(request.User);

            UserBO result = Implementation.GetUserByUserId(UserBO);



            if (result != null)
            {


                response.User = Mapper.ToUserDTO(result);

            }


            return response;
        }
        public FormSettingResponse SaveSettings(FormSettingRequest FormSettingReq)
        {

            FormSettingResponse Response = new FormSettingResponse();
            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();


                Epi.Web.Enter.Interfaces.DataInterface.IFormSettingDao IFormSettingDao = entityDaoFactory.FormSettingDao;
                Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;
                Epi.Web.Enter.Interfaces.DataInterface.IFormInfoDao IFormInfoDao = entityDaoFactory.FormInfoDao;
                Epi.Web.BLL.FormSetting SettingsImplementation = new Epi.Web.BLL.FormSetting(IFormSettingDao, IUserDao, IFormInfoDao);

                foreach (var item in FormSettingReq.FormSetting)
                {
                    string Message = SettingsImplementation.SaveSettings(FormSettingReq.FormInfo.IsDraftMode, item.ColumnNameList, item.AssignedUserList, item.FormId);

                }

                return Response;


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

        public SurveyInfoResponse GetFormChildInfo(SurveyInfoRequest pRequest)
        {
            try
            {
                SurveyInfoResponse result = new SurveyInfoResponse(pRequest.RequestId);


                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
                Epi.Web.BLL.SurveyInfo implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);
                Dictionary<string, int> ParentIdList = new Dictionary<string, int>();
                foreach (var item in pRequest.SurveyInfoList)
                {
                    ParentIdList.Add(item.SurveyId, item.ViewId);
                }
                result.SurveyInfoList = Mapper.ToDataTransferObject(implementation.GetChildInfoByParentId(ParentIdList));


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


        public FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest FormsHierarchyRequest)
        {

            FormsHierarchyResponse FormsHierarchyResponse = new FormsHierarchyResponse();
            List<SurveyResponseBO> AllResponsesIDsList = new List<SurveyResponseBO>();
            //1- Get All form  ID's
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
            Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);

            List<FormsHierarchyBO> RelatedFormIDsList = Implementation.GetFormsHierarchyIdsByRootId(FormsHierarchyRequest.SurveyInfo.FormId);



            //2- Get all Responses ID's

            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao ISurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation1 = new Epi.Web.BLL.SurveyResponse(ISurveyResponseDao);
            if (!string.IsNullOrEmpty(FormsHierarchyRequest.SurveyResponseInfo.ResponseId))
            {
                AllResponsesIDsList = Implementation1.GetResponsesHierarchyIdsByRootId(FormsHierarchyRequest.SurveyResponseInfo.ResponseId);

            }
            else
            {
                AllResponsesIDsList = null;
            }
            //3 Combining the lists.

            FormsHierarchyResponse.FormsHierarchy = Mapper.ToFormHierarchyDTO(CombineLists(RelatedFormIDsList, AllResponsesIDsList));

            return FormsHierarchyResponse;



        }

        private List<FormsHierarchyBO> CombineLists(List<FormsHierarchyBO> RelatedFormIDsList, List<SurveyResponseBO> AllResponsesIDsList)
        {

            List<FormsHierarchyBO> List = new List<FormsHierarchyBO>();

            foreach (var Item in RelatedFormIDsList)
            {
                FormsHierarchyBO FormsHierarchyBO = new FormsHierarchyBO();
                FormsHierarchyBO.FormId = Item.FormId;
                FormsHierarchyBO.ViewId = Item.ViewId;
                if (AllResponsesIDsList != null)
                {
                    FormsHierarchyBO.ResponseIds = AllResponsesIDsList.Where(x => x.SurveyId == Item.FormId).ToList();
                }
                List.Add(FormsHierarchyBO);
            }
            return List;

        }

        public SurveyAnswerResponse GetSurveyAnswerHierarchy(SurveyAnswerRequest pRequest)
        {
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            SurveyAnswerResponse SurveyAnswerResponse = new Enter.Common.Message.SurveyAnswerResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);
            List<SurveyResponseBO> SurveyResponseBOList = Implementation.GetResponsesHierarchyIdsByRootId(pRequest.SurveyAnswerList[0].ResponseId);
            SurveyAnswerResponse.SurveyResponseList = Mapper.ToDataTransferObject(SurveyResponseBOList);

            return SurveyAnswerResponse;
        }


        public SurveyAnswerResponse GetAncestorResponseIdsByChildId(SurveyAnswerRequest pRequest)
        {
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            SurveyAnswerResponse SurveyAnswerResponse = new Enter.Common.Message.SurveyAnswerResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);
            List<SurveyResponseBO> SurveyResponseBOList = Implementation.GetAncestorResponseIdsByChildId(pRequest.Criteria.SurveyAnswerIdList[0]);
            SurveyAnswerResponse.SurveyResponseList = Mapper.ToDataTransferObject(SurveyResponseBOList);

            return SurveyAnswerResponse;

        }
        public SurveyAnswerResponse GetResponsesByRelatedFormId(SurveyAnswerRequest pRequest)
        {
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            SurveyAnswerResponse SurveyAnswerResponse = new Enter.Common.Message.SurveyAnswerResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = entityDaoFactory.SurveyResponseDao;
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);

            //List<SurveyResponseBO> SurveyResponseBOList = Implementation.GetResponsesByRelatedFormId(pRequest.Criteria.SurveyAnswerIdList[0], pRequest.Criteria.SurveyId);

            List<SurveyResponseBO> SurveyResponseBOList = Implementation.GetResponsesByRelatedFormId(pRequest.Criteria.SurveyAnswerIdList[0], pRequest.Criteria);

            SurveyAnswerResponse.SurveyResponseList = Mapper.ToDataTransferObject(SurveyResponseBOList);
            //Query The number of records

            //SurveyAnswerResponse.NumberOfPages = Implementation.GetNumberOfPages(pRequest.Criteria.SurveyId, pRequest.Criteria.IsMobile);
            //SurveyAnswerResponse.NumberOfResponses = Implementation.GetNumberOfResponses(pRequest.Criteria);

            //SurveyAnswerResponse.NumberOfPages = Implementation.GetNumberOfPages(pRequest.Criteria);
            //SurveyAnswerResponse.NumberOfResponses = Implementation.GetNumberOfResponses(pRequest.Criteria);

            return SurveyAnswerResponse;
        }

        public OrganizationResponse GetOrganizationsByUserId(OrganizationRequest request)
        {

            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);


                if (!ValidRequest(request, response, Validate.All))
                    return response;

                List<OrganizationBO> ListOrganizationBO = Implementation.GetOrganizationsByUserId(request.UserId);
                response.OrganizationList = new List<OrganizationDTO>();
                foreach (OrganizationBO Item in ListOrganizationBO)
                {
                    (response.OrganizationList).Add(Mapper.ToDataTransferObjects(Item));

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
        public OrganizationResponse GetUserOrganizations(OrganizationRequest request)
        {

            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);


                if (!ValidRequest(request, response, Validate.All))
                    return response;

                List<OrganizationBO> ListOrganizationBO = Implementation.GetOrganizationInfoByUserId(request.UserId, request.UserRole);
                response.OrganizationList = new List<OrganizationDTO>();
                foreach (OrganizationBO Item in ListOrganizationBO)
                {
                    (response.OrganizationList).Add(Mapper.ToDataTransferObjects(Item));

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

        public OrganizationResponse GetAdminOrganizations(OrganizationRequest request)
        {

            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);


                if (!ValidRequest(request, response, Validate.All))
                    return response;

                List<OrganizationBO> ListOrganizationBO = Implementation.GetOrganizationInfoForAdmin(request.UserId, request.UserRole);
                response.OrganizationList = new List<OrganizationDTO>();
                foreach (OrganizationBO Item in ListOrganizationBO)
                {
                    (response.OrganizationList).Add(Mapper.ToDataTransferObjects(Item));

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

        public OrganizationResponse GetOrganizationInfo(OrganizationRequest request)
        {
            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                OrganizationResponse response = new OrganizationResponse(request.RequestId);


                if (!ValidRequest(request, response, Validate.All))
                    return response;
                OrganizationBO ListOrganizationBO = Implementation.GetOrganizationByKey(request.Organization.OrganizationKey);

                response.OrganizationList = new List<OrganizationDTO>();

                response.OrganizationList.Add(Mapper.ToDataTransferObjects(ListOrganizationBO));


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
        public OrganizationResponse SetOrganization(OrganizationRequest request)
        {

            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                var Organization = Mapper.ToOrgBusinessObject(request.Organization);
                var User = Mapper.ToUserBO(request.OrganizationAdminInfo);
                var response = new OrganizationResponse(request.RequestId);

                if (request.Action.ToUpper() == "UPDATE")
                {

                    if (!ValidRequest(request, response, Validate.All))
                    {
                        response.Message = "Error";
                        return response;
                    }

                    //    Implementation.UpdateOrganizationInfo(Organization);
                    // response.Message = "Successfully added organization Key";
                    if (Implementation.OrganizationNameExists(Organization.Organization, Organization.OrganizationKey, "Update"))
                    {
                        response.Message = "Exists";
                    }
                    else
                    {


                        var success = Implementation.UpdateOrganizationInfo(Organization);
                        if (success)
                        {
                            response.Message = "Successfully added organization Key";
                        }
                        else
                        {
                            response.Message = "Error";
                            return response;
                        }

                    }
                }
                else if (request.Action.ToUpper() == "INSERT")
                {
                    Guid OrganizationKey = Guid.NewGuid();
                    Organization.OrganizationKey = OrganizationKey.ToString();
                    if (!ValidRequest(request, response, Validate.All))
                        return response;
                    if (Implementation.OrganizationNameExists(Organization.Organization, Organization.OrganizationKey, "Create"))
                    {
                        response.Message = "Exists";
                    }
                    else
                    {
                        Implementation.InsertOrganizationInfo(Organization, User);

                        response.Message = "Success";
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

        public OrganizationResponse GetOrganizationUsers(OrganizationRequest request)
        {

            try
            {
                var IUserDao = new EF.EntityUserDao();
                Epi.Web.BLL.User Implementation = new Epi.Web.BLL.User(IUserDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);


                if (!ValidRequest(request, response, Validate.All))
                    return response;

                List<UserBO> ListUserBO = Implementation.GetUsersByOrgId(request.Organization.OrganizationId);
                response.OrganizationUsersList = new List<UserDTO>();
                foreach (UserBO Item in ListUserBO)
                {
                    (response.OrganizationUsersList).Add(Mapper.ToDataTransferObject(Item));

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

        public UserResponse GetUserInfo(UserRequest request)
        {

            UserResponse Response = new UserResponse();
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;
            Epi.Web.BLL.User Implementation = new Epi.Web.BLL.User(IUserDao);

            UserBO UserBO = Mapper.ToUserBO(request.User);
            OrganizationBO OrgBO = Mapper.ToOrgBusinessObject(request.Organization);
            UserBO result = Implementation.GetUserByUserIdAndOrgId(UserBO, OrgBO);



            if (result != null)
            {

                Response.User = new List<UserDTO>();
                Response.User.Add(Mapper.ToUserDTO(result));

            }


            return Response;
        }


        public UserResponse SetUserInfo(UserRequest request)
        {

            try
            {
                UserResponse response = new UserResponse();
                Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;

                Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation1 = new Epi.Web.BLL.Organization(IOrganizationDao);

                Epi.Web.BLL.User Implementation = new Epi.Web.BLL.User(IUserDao);
                UserBO UserBO = Mapper.ToUserBO(request.User);
                OrganizationBO OrgBo = Mapper.ToOrgBusinessObject(request.Organization);
                if (request.Action.ToUpper() == "UPDATE")
                {
                    OrganizationBO OrganizationBO = Implementation1.GetOrganizationByOrgId(request.CurrentOrg);
                    UserBO.Operation = Enter.Common.Constants.Constant.OperationMode.UpdateUserInfo;
                    Implementation.UpdateUser(UserBO, OrganizationBO);
                }
                else
                {
                    UserBO ExistingUser = Implementation.GetUserByEmail(UserBO);//Validate if user is in the system. 
                    bool UserExists = false;
                    if (ExistingUser != null)
                    {
                        OrganizationBO OrganizationBO = Implementation1.GetOrganizationByOrgId(request.CurrentOrg);
                        ExistingUser.Role = UserBO.Role;
                        ExistingUser.IsActive = UserBO.IsActive;
                        UserBO = ExistingUser;
                        UserExists = Implementation.IsUserExistsInOrganizaion(UserBO, OrganizationBO); //validate if user is part of the organization already. 
                    }

                    if (!UserExists)
                    {
                        //OrgBo.OrganizationId = request.CurrentOrg; // User is added to the current organization
                        OrganizationBO OrganizationBO = Implementation1.GetOrganizationByOrgId(request.CurrentOrg);
                        Implementation.SetUserInfo(UserBO, OrganizationBO);
                        response.Message = "Success";
                    }
                    else
                    {
                        response.Message = "Exists";
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

        public void UpdateResponseStatus(SurveyAnswerRequest request)
        {
            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);


                //SurveyAnswerResponse response = new SurveyAnswerResponse(request.RequestId);
                SurveyResponseBO SurveyResponse = Mapper.ToBusinessObject(request.SurveyAnswerList, request.Criteria.UserId)[0];

                //SurveyResponse.UserId = request.Criteria.UserId;
                //Implementation.UpdateSurveyResponse(SurveyResponse);
                //response.SurveyResponseList.Add(Mapper.ToDataTransferObject(SurveyResponse));

                Epi.Web.BLL.SurveyResponse Implementation1 = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);
                //List<SurveyResponseBO> SurveyResponseBOList = Implementation1.GetResponsesHierarchyIdsByRootId(request.SurveyAnswerList[0].ResponseId);

                List<SurveyResponseBO> SurveyResponseBOList = Implementation1.GetSurveyResponseById(request.Criteria);

                List<SurveyResponseBO> ResultList = Implementation.UpdateSurveyResponse(SurveyResponseBOList, request.Criteria.StatusId);

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

        public bool HasResponse(string SurveyId, string ResponseId)
        {
            Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
            Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);

            return Implementation.HasResponse(SurveyId, ResponseId);

        }
    }
}
