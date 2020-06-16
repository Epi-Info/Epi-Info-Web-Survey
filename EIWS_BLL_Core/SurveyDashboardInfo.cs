using Epi.Web.Common;
using Epi.Web.Common.Message;
using Epi.Web.Common.ObjectMapping;
using Epi.Web.Interfaces.DataInterfaces;

namespace Epi.Web.BLL
{
    public class SurveyDashboardInfo
    {
        private ISurveyResponseDao SurveyResponseDao;
        private ISurveyInfoDao SurveyInfoDao;
        public SurveyDashboardInfo(Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao pSurveyResponseDao, ISurveyInfoDao pSurveyInfoDao)
        {

            this.SurveyResponseDao = pSurveyResponseDao;
            this.SurveyInfoDao = pSurveyInfoDao;
        }

        public DashboardResponse GetSurveyDashboardInfo(string surveyid)
        {
            DashboardResponse DashboardResponse = new DashboardResponse();
            SurveyDashboardBO SurveyDashboardBO = new SurveyDashboardBO();
            SurveyDashboardBO = SurveyResponseDao.GetSurveyDashboardCounts(surveyid);


            DashboardResponse.SurveyInfo = Mapper.ToSurveyInfoDTO(SurveyInfoDao.GetDashboardSurveyInfo(surveyid));
            DashboardResponse.SavedRecordCount = SurveyDashboardBO.SavedRecordCount;
            DashboardResponse.StartedRecordCount = SurveyDashboardBO.StartedRecordCount;
            DashboardResponse.SubmitedRecordCount = SurveyDashboardBO.SubmitedRecordCount;
            DashboardResponse.RecordCount = SurveyDashboardBO.RecordCount;
            DashboardResponse.RecordCountPerDate = SurveyDashboardBO.RecordCountPerDate;
            DashboardResponse.DownloadedRecordCount = SurveyDashboardBO.DownloadedRecordCount;

            return DashboardResponse;
        }
    }
}
