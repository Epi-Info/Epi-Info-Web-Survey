using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;

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
            result = Implementation.PublishSurvey(pRequestMessage);
            //result.IsPulished = r.IsPulished;
            //result.StatusText = r.StatusText;
            //result.URL = r.URL;

            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        public cSurveyInfo GetSurveyInfoById(string pId)
        {
            cSurveyInfo result = null;

            //Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher();
            //result = Implementation.PublishSurvey(pRequestMessage);
            //result.IsPulished = r.IsPulished;
            //result.StatusText = r.StatusText;
            //result.URL = r.URL;

            return result;

        }
    }
}
