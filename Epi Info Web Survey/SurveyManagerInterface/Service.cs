using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Epi.Web.SurveyManager
{
    public class Service : ISurveyManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        public cSurveyRequestResult PublishSurvey(cSurveyRequest pRequestMessage)
        {
            cSurveyRequestResult result = new cSurveyRequestResult();

            Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher();
            Epi.Web.Interfaces.ISurveyRequestResult r = Implementation.PublishSurvey(pRequestMessage);
            result.IsPulished = r.IsPulished;
            result.StatusText = r.StatusText;
            result.URL = r.URL;

            return result;

        }
    }
}
