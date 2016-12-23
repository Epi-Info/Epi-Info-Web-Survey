using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;

namespace Epi.Web.WCF.SurveyService
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        CacheDependencyResponse GetCacheDependencyInfo(CacheDependencyRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyAnswerResponse GetSurveyAnswer(SurveyAnswerRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyAnswerResponse SetSurveyAnswer(SurveyAnswerRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse PassCodeLogin(UserAuthenticationRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse SetPassCode(UserAuthenticationRequest pRequest);
        
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse GetAuthenticationResponse(UserAuthenticationRequest pRequest);
        
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationAccountResponse CreateAccount(OrganizationAccountRequest pRequest);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationAccountResponse GetStateList(OrganizationAccountRequest Request);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        OrganizationAccountResponse GetUserOrgId(OrganizationAccountRequest Request);
    }
}
