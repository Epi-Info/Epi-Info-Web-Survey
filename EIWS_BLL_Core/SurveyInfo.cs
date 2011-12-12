using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;

namespace Epi.Web.BLL
{

  public  class SurveyInfo
    {
      private Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao;


        public SurveyInfo(Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao pSurveyInfoDao)
        {
            this.SurveyInfoDao = pSurveyInfoDao;
        }

        public SurveyInfoBO GetSurveyInfoById(String pId)
        {
            SurveyInfoBO result = this.SurveyInfoDao.GetSurveyInfo(pId);
            return result;
        }

        public SurveyInfoBO InsertSurveyInfo(SurveyInfoBO pValue)
        {
            SurveyInfoBO result = pValue;
            this.SurveyInfoDao.InsertSurveyInfo(pValue);
            return result;
        }



        public SurveyInfoBO UpdateSurveyInfo(SurveyInfoBO pValue)
        {
            SurveyInfoBO result = pValue;
            this.SurveyInfoDao.UpdateSurveyInfo(pValue);
            return result;
        }

        public bool DeleteSurveyInfo(SurveyInfoBO pValue)
        {
            bool result = false;

            this.SurveyInfoDao.DeleteSurveyInfo(pValue);
            result = true;

            return result;
        }

    }
}
