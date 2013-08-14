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
    public interface IManagerServicev2
    {
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        PublishResponse RePublishSurvey(PublishRequest pRequestMessage);
    }
}
