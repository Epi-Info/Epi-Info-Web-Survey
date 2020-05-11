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
 public  interface IManagerServiceV4:IManagerServiceV3
    {
      
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool ValidateOrganization(OrganizationRequest Request);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyInfoResponse GetAllSurveysByOrgKey(string OrgKey);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        string SetJsonColumn(List<string> SurveyIds, string OrgId);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        string SetJsonColumnAll(string AdminKey);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserResponse GetLoginToken(UserRequest UserInfo);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool SetUserInfo(UserRequest UserInfo);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool GetExistingUser(UserRequest UserInfo);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserResponse GetUserListByOrganization(UserRequest UserInfo);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool DeleteUser(UserRequest UserInfo);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool UpdateUserInfo(UserRequest UserInfo);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserResponse GetUserByUserId(UserRequest UserInfo);
    }
}
