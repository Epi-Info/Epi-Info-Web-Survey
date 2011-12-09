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
    public interface ISurveyManager 
    {
        [OperationContract]
        PublishResponse PublishSurvey(PublishRequest pRequestMessage);

        [OperationContract]
        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest);
    }

}
