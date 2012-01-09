using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.BLL
{
    public class SurveyResponse
    {
        private Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao;

        public SurveyResponse(Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao pSurveyResponseDao)
        {
            this.SurveyResponseDao = pSurveyResponseDao;
        }

        public List<SurveyResponseBO> GetSurveyResponseById(List<String> pId)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(pId);
            return result;
        }

        public List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<String> pSurveyIdList)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponseBySurveyId(pSurveyIdList);
            return result;
        }

        public SurveyResponseBO InsertSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.InsertSurveyResponse(pValue);
            return result;
        }


        public SurveyResponseBO UpdateSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.UpdateSurveyResponse(pValue);
            return result;
        }

        public bool DeleteSurveyResponse(SurveyResponseBO pValue)
        {
            bool result = false;

            this.SurveyResponseDao.DeleteSurveyResponse(pValue);
            result = true;

            return result;
        }
    }
}
