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
using Epi.Web.BLL;

using System.Configuration;
using Epi.Web.Common.Security;
 
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
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao = new EF.EntityOrganizationDao();


                Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher(SurveyInfoDao,OrganizationDao);
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

                //Validate UserPublishKey exists
                if (pRequest.Criteria.UserPublishKey == null)
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


                

                
                List<SurveyInfoBO> SurveyBOList = new List<SurveyInfoBO>();
              //  int ResponseMaxSize = 16384;   
                int ResponseMaxSize =   Int32.Parse(ConfigurationManager.AppSettings["maxBytesPerRead"]);
                int BandwidthUsageFactor = Int32.Parse(ConfigurationManager.AppSettings["BandwidthUsageFactor"]);


                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao entityDaoFactory1 = new EF.EntityOrganizationDao();
                //Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao surveyInfoDao1 = entityDaoFactory1;

                Epi.Web.BLL.Organization implementation1 = new Epi.Web.BLL.Organization(surveyInfoDao1);
                bool ISValidOrg = implementation1.ValidateOrganization(pRequest.Criteria.OrganizationKey.ToString());

           if (ISValidOrg)
                {
                    if (pRequest.Criteria.ReturnSizeInfoOnly == true)
                    {
                        // add BandwidthUsageFactor
                        PageInfoBO PageInfoBO = implementation.GetSurveySizeInfo(SurveyIdList, criteria.ClosingDate, criteria.OrganizationKey.ToString(), BandwidthUsageFactor, criteria.SurveyType, criteria.PageNumber, criteria.PageSize, ResponseMaxSize);
                        result.PageSize = PageInfoBO.PageSize;
                        result.NumberOfPages = PageInfoBO.NumberOfPages;
                    }
                    else
                    {
                        SurveyBOList = implementation.GetSurveyInfo(SurveyIdList, criteria.ClosingDate, criteria.OrganizationKey.ToString(), criteria.SurveyType, criteria.PageNumber, criteria.PageSize);//Default 
                        foreach (SurveyInfoBO surveyInfoBO in SurveyBOList)
                        {

                            result.SurveyInfoList.Add(Mapper.ToDataTransferObject(surveyInfoBO));

                        }
                    }
                }
                else {

                    result.Message = "Organization Key not found";
                }

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
        /// <param name="request">SurveyInfoRequest request message.</param>
        /// <returns>SurveyInfoRequest response message.</returns>
        public SurveyInfoResponse SetSurveyInfo(SurveyInfoRequest request)
        {
            try
            {
                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.Interfaces.DataInterfaces.IAdminDao AdminDao = new EF.EntityAdminDao();
                Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);
                Epi.Web.BLL.Admin ImplementationAdmin = new Epi.Web.BLL.Admin(AdminDao);


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
                    
                    bool validSurvey = false;
                    //GetSurveyInfoByOrgKey
                    //validSurvey = Implementation.IsSurveyInfoValidByOrgKeyAndPublishKey(SurveyInfo.SurveyId, SurveyInfo.OrganizationKey.ToString(), SurveyInfo.UserPublishKey);
                    validSurvey = Implementation.IsSurveyInfoValidByOrgKey(SurveyInfo.SurveyId, SurveyInfo.OrganizationKey.ToString());

                     Epi.Web.Interfaces.DataInterfaces.IOrganizationDao entityDaoFactory1 = new EF.EntityOrganizationDao();
                     Epi.Web.Interfaces.DataInterfaces.IOrganizationDao surveyInfoDao1 = entityDaoFactory1;
                     Epi.Web.BLL.Organization implementation1 = new Epi.Web.BLL.Organization(surveyInfoDao1);
                     bool ISValidOrg = implementation1.ValidateOrganization(SurveyInfo.OrganizationKey.ToString());

                     if (ISValidOrg )
                    {
                        if (request.Criteria.FileInputStream != null)
                        {
                            validSurvey = true;
                        }

                                 if (validSurvey)
                                {


                                    if (request.Action == "Create")
                                    {
                                        if (request.Criteria.FileInputStream == null)
                                        {
                                            Implementation.InsertSurveyInfo(SurveyInfo);
                                            response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
                                        }
                                        else
                                        {
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
                                        response.Message = SurveyInfo.StatusText;
                                         
                                    }
                                    else if (request.Action == "UpdateMode")
                                        {
                                        Implementation.UpdateSurveyInfo(SurveyInfo);
                                        response.SurveyInfoList.Add(Mapper.ToDataTransferObject(SurveyInfo));
                                        response.Message = SurveyInfo.StatusText;
                                        ImplementationAdmin.SendEmailToAdmins(SurveyInfo);
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
                                else
                                {

                                    response.Message = "SurveyId And/or Publish Key are invalid.";
                                }
                    }
                     else
                     {

                         response.Message = "Organization Key is invalid.";
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
               


                //Get Organization Info
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao entityDaoFactory1 = new EF.EntityOrganizationDao();
                //Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao surveyInfoDao1 = entityDaoFactory1;

                Epi.Web.BLL.Organization implementation1 = new Epi.Web.BLL.Organization(surveyInfoDao1);
                
                bool ISValidOrg = implementation1.ValidateOrganization(pRequest.Criteria.OrganizationKey.ToString());


                
                if (ISValidOrg)
             
                {

                    // Validate client tag, access token, and user credentials
                    if (!ValidRequest(pRequest, result, Validate.All))
                    {
                        return result;
                    }

                    SurveyAnswerCriteria criteria = pRequest.Criteria;


                    if (criteria.UserPublishKey == null)
                    {
                        return result;
                    }


                    Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
                    Epi.Web.BLL.SurveyInfo SurveyInfo = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);





                    //List<string> SurveyIdList = new List<string>();

                    //SurveyIdList.Add(criteria.SurveyId);
                   

                    bool validSurvey = false;
                   // if (string.IsNullOrEmpty(criteria.SurveyId.ToString()))
                   // {
                    validSurvey = SurveyInfo.IsSurveyInfoValidByOrgKeyAndPublishKey(criteria.SurveyId, criteria.OrganizationKey.ToString(), criteria.UserPublishKey);

                    //}

                    if (validSurvey == true)
                    {

                        //Guid UserPublishKey = SurveyInfo.GetSurveyInfoById(criteria.SurveyId).UserPublishKey;

                        //if (criteria.UserPublishKey != UserPublishKey)
                        //{
                        //    return result;
                        //}


                        List<string> IdList = new List<string>();
                        if (criteria.SurveyAnswerIdList != null)
                        {
                            foreach (string id in criteria.SurveyAnswerIdList)
                            {
                                IdList.Add(id.ToUpper());
                            }
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

                        int ResponseMaxSize = Int32.Parse(ConfigurationManager.AppSettings["maxBytesPerRead"]);

                        int BandwidthUsageFactor = Int32.Parse(ConfigurationManager.AppSettings["BandwidthUsageFactor"]);

                        if (pRequest.Criteria.ReturnSizeInfoOnly == true)
                        {
                            // call BLL with a list of records 

                            PageInfoBO PageInfoBO = Implementation.GetResponseSurveySize(IdList, criteria.SurveyId,  criteria.DateCompleted,BandwidthUsageFactor,criteria.IsDraftMode, criteria.StatusId, -1, -1, ResponseMaxSize);

                           

                            result.PageSize = PageInfoBO.PageSize;
                            result.NumberOfPages = PageInfoBO.NumberOfPages;
                        }
                        else
                        {

                            List<SurveyResponseBO> SurveyResponseBOList = Implementation.GetSurveyResponse
                                    (
                                        IdList,
                                        criteria.SurveyId,
                                        criteria.DateCompleted,
                                        criteria.IsDraftMode,
                                        criteria.StatusId, 
                                        pRequest.Criteria.PageNumber,
                                        pRequest.Criteria.PageSize
                                    );

 



                            foreach (SurveyResponseBO surveyResponseBo in SurveyResponseBOList)
                            {
                                // if (surveyResponseBo.UserPublishKey == criteria.UserPublishKey)
                                //if (UserPublishKey == criteria.UserPublishKey)
                                //{
                                    result.SurveyResponseList.Add(Mapper.ToDataTransferObject(surveyResponseBo));
                              //  }
                            }
                        }
                        /*
                        if (string.IsNullOrEmpty(pRequest.Criteria.SurveyId))
                        {
                            result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseById(pRequest.Criteria.SurveyAnswerIdList));
                        }
                        else
                        {
                            result.SurveyResponseList = Mapper.ToDataTransferObject(Implementation.GetSurveyResponseBySurveyId(pRequest.Criteria.SurveyAnswerIdList));
                        }*/
                    }
                    else
                    {
                        result.Message = "SurveyId or Organization Key or Publish Key are invalid.";
                    }
                }
                else
                {
                  result.Message = "Organization Key not found";
                }
               
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
                //response.Message = "Invalid username and/or password.";
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
                   // response.Message = "Unknown Client Tag";
                    //return false;
                }
            }


            // Validate access token
            if ((Validate.AccessToken & validate) == Validate.AccessToken)
            {
                if (request.AccessToken != _accessToken)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    //response.Message = "Invalid or expired AccessToken. Call GetToken()";
                    //return false;
                }
            }

            // Validate user credentials
            if ((Validate.UserCredentials & validate) == Validate.UserCredentials)
            {
                if (_userName == null)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    //response.Message = "Please login and provide user credentials before accessing these methods.";
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
            UserPublishKey = 0x0008,
            All = ClientTag | AccessToken | UserCredentials | UserPublishKey
        }


        private bool ValidateUser(string pUserName, string pPassword)
        {
            bool result = true;

            return result;
        }


        public OrganizationResponse GetOrganization(OrganizationRequest request)
        {
           
            try
            {
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                 // Transform SurveyInfo data transfer object to SurveyInfo business object
                 OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                 var response = new OrganizationResponse(request.RequestId);

                 if (Epi.Web.BLL.Common.ValidateAdmin(request.AdminSecurityKey.ToString()))
                 {

                     // Validate client tag, access token, and user credentials

                     if (!ValidRequest(request, response, Validate.All))
                         return response;

                     List<OrganizationBO> ListOrganizationBO = Implementation.GetOrganizationKey(Organization.Organization);
                     response.OrganizationList = new List<OrganizationDTO>();
                     foreach (OrganizationBO Item in ListOrganizationBO)
                     {
                         (response.OrganizationList).Add(Mapper.ToDataTransferObjects(Item));

                     }
                     return response;
                 }
                 else {
                     response.Message = "Invalid Admin Key";
                     return response;
                 }
              
                 
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
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);

                if (Epi.Web.BLL.Common.ValidateAdmin(request.AdminSecurityKey.ToString()))
                {

                    // Validate client tag, access token, and user credentials

                    if (!ValidRequest(request, response, Validate.All))
                        return response;

                    List<OrganizationBO> ListOrganizationBO = Implementation.GetOrganizationInfo();
                    response.OrganizationList = new List<OrganizationDTO>();
                    foreach (OrganizationBO Item in ListOrganizationBO)
                    {
                        (response.OrganizationList).Add(Mapper.ToDataTransferObjects(Item));

                    }
                    return response;
                }
                else
                {
                    response.Message = "Invalid Admin Key";

                    return response ;
                }


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


        public OrganizationResponse GetOrganizationNames(OrganizationRequest request)
        {
           try
            {
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);

                if (Epi.Web.BLL.Common.ValidateAdmin(request.AdminSecurityKey.ToString()))
                {

                    // Validate client tag, access token, and user credentials

                    if (!ValidRequest(request, response, Validate.All))
                        return response;

                    List<OrganizationBO> ListOrganizationBO = Implementation.GetOrganizationNames(); 
                    response.OrganizationList = new List<OrganizationDTO>();
                    foreach (OrganizationBO Item in ListOrganizationBO)
                    {
                        (response.OrganizationList).Add(Mapper.ToDataTransferObjects(Item));

                    }
                    return response;
                }
                else
                {
                    response.Message = "Invalid Admin Key";

                    return response;
                }


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
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                var Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);
                // Validate client tag, access token, and user credentials
                if (Epi.Web.BLL.Common.ValidateAdmin(request.AdminSecurityKey.ToString()))
                {
                    

                    if (!ValidRequest(request, response, Validate.All))
                        return response;
                    if (Implementation.OrganizationNameExists(Organization.Organization, Organization.OrganizationKey, "Create"))
                    {
                        response.Message = "Organization name exists";
                    }
                    else
                    {
                        Implementation.InsertOrganizationInfo(Organization);

                        response.Message = "Successfully added organization Key";
                    }
                       
                    return response;
                }
                else {
                    response.Message = "Invalid Admin Key";
                    return response;
                }
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

        public OrganizationResponse GetOrganizationByKey(OrganizationRequest request)
        {
           try
            {
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
               
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                OrganizationBO Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);

                if (Epi.Web.BLL.Common.ValidateAdmin(request.AdminSecurityKey.ToString()))
                {

                    // Validate client tag, access token, and user credentials

                    if (!ValidRequest(request, response, Validate.All))
                        return response;

                    OrganizationBO OrganizationBO = Implementation.GetOrganizationByKey( Organization.OrganizationKey .ToString());
                    response.OrganizationList = new List<OrganizationDTO>();

                    (response.OrganizationList).Add(Mapper.ToDataTransferObjects(OrganizationBO));

                   
                    return response;
                }
                else
                {
                    response.Message = "Invalid Admin Key";

                    return response;
                }


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

        public OrganizationResponse UpdateOrganizationInfo(OrganizationRequest request)
        {

            try
            {
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao IOrganizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(IOrganizationDao);
                // Transform SurveyInfo data transfer object to SurveyInfo business object
                var Organization = Mapper.ToBusinessObject(request.Organization);
                var response = new OrganizationResponse(request.RequestId);
                // Validate client tag, access token, and user credentials
                if (Epi.Web.BLL.Common.ValidateAdmin(request.AdminSecurityKey.ToString()))
                {


                    if (!ValidRequest(request, response, Validate.All))
                        return response;

                    if (Implementation.OrganizationNameExists(Organization.Organization, Organization.OrganizationKey, "Update"))
                    {
                        response.Message = "Organization name exists";
                    }

                    else
                    {
                        Implementation.UpdateOrganizationInfo(Organization);
                        response.Message = "Successfully Updated organization Key";
                    }
                    
                    return response;
                }
                else
                {
                    response.Message = "Invalid Admin Key";
                    return response;
                }
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

         public  bool IsValidOrgKey(SurveyInfoRequest request){

              try
            {
             
             bool validSurvey = false;
             //var SurveyInfo = Mapper.ToBusinessObject(request.SurveyInfoList[0]);
             if (request.Criteria.SurveyIdList.Count == 0)
             {
                Epi.Web.Interfaces.DataInterfaces.IOrganizationDao organizationDao = new EF.EntityOrganizationDao();
                Epi.Web.BLL.Organization Implementation = new Epi.Web.BLL.Organization(organizationDao);
                Epi.Web.Common.BusinessObject.OrganizationBO ogranizationBO = Implementation.GetOrganizationByKey(request.Criteria.OrganizationKey.ToString());
                if (ogranizationBO == null)
                {
                    validSurvey = false;
                }
                else
                {
                    validSurvey = true;
                }


             }
             else
             {
                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);
                validSurvey = Implementation.IsSurveyInfoValidByOrgKey(request.Criteria.SurveyIdList[0].ToString(), request.Criteria.OrganizationKey.ToString());
             }
             return validSurvey;
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
         /// <param name="pRequestMessage"></param>
         /// <returns></returns>
         public PublishResponse RePublishSurvey(PublishRequest pRequest)
             {
             try
                 {
                 PublishResponse result = new PublishResponse(pRequest.RequestId);
                 Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao = new EF.EntitySurveyInfoDao();
                 Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao = new EF.EntityOrganizationDao();


                 Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher(SurveyInfoDao, OrganizationDao);
                 SurveyInfoBO surveyInfoBO = Mapper.ToBusinessObject(pRequest.SurveyInfo);
                 SurveyRequestResultBO surveyRequestResultBO = Implementation.RePublishSurvey(surveyInfoBO);
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

    }
}
