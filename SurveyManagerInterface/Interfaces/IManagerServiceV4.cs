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
    }
}
