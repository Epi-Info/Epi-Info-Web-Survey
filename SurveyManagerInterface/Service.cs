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
            cSurveyRequestResult result = null;

            Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher();
            result = (cSurveyRequestResult) Implementation.PublishSurvey(pRequestMessage);
            return result;

        }
    }
}
