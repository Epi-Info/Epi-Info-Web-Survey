using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;

namespace Epi.Web.SurveyManager
{
    public class Service : ISurveyManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        public SurveyRequestResponse PublishSurvey(SurveyRequest pRequestMessage)
        {
            SurveyRequestResponse result = null;
            Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao = null;

            Epi.Web.BLL.Publisher Implementation = new Epi.Web.BLL.Publisher(SurveyInfoDao);
            result = Mapper.BusinessObjectToDataTransfer(Implementation.PublishSurvey(Mapper.DataTransferToBusinessObject(pRequestMessage)));

            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public SurveyInfoDTO GetSurveyInfoById(string pId)
        {
            SurveyInfoDTO result = null;
            Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao = null;
            Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(SurveyInfoDao);
            result = Mapper.BusinessObjectToDataTransfer(Implementation.GetSurveyInfoById(pId));
   
            return result;

        }
    }
}
