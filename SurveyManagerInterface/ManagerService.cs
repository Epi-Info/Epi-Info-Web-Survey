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
    public class ManagerService : IManagerService
    {

        // Session state variables
        private string _accessToken;
        //private ShoppingCart _shoppingCart;
        private string _userName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        public PublishResponse PublishSurvey(PublishRequest pRequest)
        {
            try
            {
                PublishResponse result = new PublishResponse(pRequest.RequestId);
                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao = new EF.EntitySurveyInfoDao();

                Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher(SurveyInfoDao);
                SurveyInfoBO surveyInfoBO = Mapper.ToBusinessObject(pRequest.SurveyInfo);
                SurveyRequestResultBO surveyRequestResultBO = Implementation.PublishSurvey(surveyInfoBO);
                result.PublishInfo = Mapper.ToDataTransferObject(surveyRequestResultBO);

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
        public SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
        {
            try
            {
                SurveyInfoResponse result = new SurveyInfoResponse(pRequest.RequestId);
                //Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
                //Epi.Web.BLL.SurveyInfo implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);

                Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
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

                result.SurveyInfoList = Mapper.ToDataTransferObject(implementation.GetSurveyInfo(SurveyIdList, criteria.ClosingDate, criteria.SurveyType));
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

                SurveyAnswerCriteria criteria = pRequest.Criteria;

                List<string> IdList = new List<string>();

                foreach (string id in criteria.SurveyAnswerIdList)
                {
                    IdList.Add(id.ToUpper());
                }
                //string sort = criteria.SortExpression;

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


                result.SurveyResponseList = Mapper.ToDataTransferObject
                    (
                        Implementation.GetSurveyResponse
                        (
                            IdList,
                            criteria.SurveyId,
                            criteria.DateCompleted,
                            criteria.StatusId
                        )
                    );
                /*
                if (string.IsNullOrEmpty(pRequest.Criteria.SurveyId))
                {
                    result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseById(pRequest.Criteria.SurveyAnswerIdList));
                }
                else
                {
                    result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseBySurveyId(pRequest.Criteria.SurveyAnswerIdList));
                }*/

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

    }
}
