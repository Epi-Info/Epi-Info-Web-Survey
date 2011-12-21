using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.DataServiceClient;
using Epi.Web.Common.Message;

namespace Epi.Web.MVC.Repositories
{
    public class SurveyInfoRepository : RepositoryBase, ISurveyInfoRepository
    {



        private Epi.Web.DataServiceClient.IDataService _iSurveyManager;

        public SurveyInfoRepository(Epi.Web.DataServiceClient.IDataService iSurveyManager)
        {
            _iSurveyManager = iSurveyManager;
        }
        
        /// <summary>
        /// Calling the proxy client to fetch a SurveyInfoResponse object
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
        {

            //SurveyInfoResponse result = Client.GetSurveyInfo(pRequest);
            SurveyInfoResponse result = _iSurveyManager.GetSurveyInfo(pRequest);
            return result;
            
        }

        #region stubcode
            public List<Common.DTO.SurveyInfoDTO> GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public Common.DTO.SurveyInfoDTO Get(int id)
            {
                throw new NotImplementedException();
            }

            public int GetCount(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public void Insert(Common.DTO.SurveyInfoDTO t)
            {
                throw new NotImplementedException();
            }

            public void Update(Common.DTO.SurveyInfoDTO t)
            {
                throw new NotImplementedException();
            }

            public void Delete(int id)
            {
                throw new NotImplementedException();
            } 
        #endregion


            List<SurveyInfoResponse> IRepository<SurveyInfoResponse>.GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            SurveyInfoResponse IRepository<SurveyInfoResponse>.Get(int id)
            {
                throw new NotImplementedException();
            }

            public void Insert(SurveyInfoResponse t)
            {
                throw new NotImplementedException();
            }

            public void Update(SurveyInfoResponse t)
            {
                throw new NotImplementedException();
            }
    }
}