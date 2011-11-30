using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;

namespace Epi.Web.SurveyManager
{
    [ServiceContract]
    public interface ISurveyManager 
    {
        [OperationContract]
        SurveyRequestResultDTO PublishSurvey(SurveyRequestDTO pRequestMessage);

        [OperationContract]
        SurveyInfoDTO GetSurveyInfoById(String pIid);
    }

}
