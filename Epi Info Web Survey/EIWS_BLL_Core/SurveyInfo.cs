using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;

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
    }
}
