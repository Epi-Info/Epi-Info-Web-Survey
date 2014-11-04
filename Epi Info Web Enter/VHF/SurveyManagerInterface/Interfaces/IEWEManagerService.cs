using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Enter.Common.DTO;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.Exception;
namespace Epi.Web.WCF.SurveyService
{
    [ServiceContract]
    public interface IEWEManagerService
    {
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        PublishResponse PublishSurvey(PublishRequest pRequestMessage);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationResponse GetOrganization(OrganizationRequest pRequest);


        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationResponse GetOrganizationInfo(OrganizationRequest pRequest);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationResponse GetOrganizationNames(OrganizationRequest pRequest);
        
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationResponse GetOrganizationByKey(OrganizationRequest pRequest);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyInfoResponse SetSurveyInfo(SurveyInfoRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyAnswerResponse GetSurveyAnswer(SurveyAnswerRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationResponse SetOrganization(OrganizationRequest pRequest);
         
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationResponse UpdateOrganizationInfo(OrganizationRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool IsValidOrgKey(SurveyInfoRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        PublishResponse RePublishSurvey(PublishRequest pRequestMessage);
        //[OperationContract]
        //[FaultContract(typeof(CustomFaultException))]
        //AdminResponse GetOrganizationAdmins(AdminRequest request); 

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse UserLogin(UserAuthenticationRequest request);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool UpdateUser(UserAuthenticationRequest request);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse GetUser(UserAuthenticationRequest request);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        PreFilledAnswerResponse SetSurveyAnswer(PreFilledAnswerRequest pRequestMessage);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool PingManagerService();
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        void SetSurveyAnswerStatus(SurveyAnswerRequest pRequestMessage);
    }
}
