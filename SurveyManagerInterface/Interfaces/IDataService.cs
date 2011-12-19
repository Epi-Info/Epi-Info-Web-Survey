﻿using System;
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
    public interface IDataService
    {

        [OperationContract]
        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest);

        [OperationContract]
        SurveyInfoResponse SetSurveyInfo(SurveyInfoRequest pRequest);


        [OperationContract]
        SurveyAnswerResponse GetSurveyAnswer(SurveyAnswerRequest pRequest);

        [OperationContract]
        SurveyAnswerResponse SetSurveyAnswer(SurveyAnswerRequest pRequest);

    }

}
