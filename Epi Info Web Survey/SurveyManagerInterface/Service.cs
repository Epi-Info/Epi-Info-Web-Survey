using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SurveyManagerInterface
{
    public class Service : ISurveyManager
    {

        ISurveyRequestResult PublishSurvey(ISurveyRequest pRequestMessage)
        {
            ISurveyRequestResult result = null;

            Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher();
            result = (ISurveyRequestResult) Implementation.PublishSurvey(pRequestMessage);
            return result;

        }
    }
}
