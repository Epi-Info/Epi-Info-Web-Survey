using Epi.Web.Common.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;

namespace Epi.Web.WCF.SurveyService
{
    [ServiceContract]
    public interface IManagerServiceV5 : IManagerServiceV4
    {
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        string SetJsonColumn(List<string> SurveyIds,string OrgId);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        string SetJsonColumnAll(string AdminKey);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserResponse GetLoginToken(UserRequest UserInfo);
    }
}
