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
  public  interface IManagerServiceV2 : IManagerService
        {
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        PreFilledAnswerResponse SetSurveyAnswer(PreFilledAnswerRequest pRequestMessage);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage);
        }
    }
