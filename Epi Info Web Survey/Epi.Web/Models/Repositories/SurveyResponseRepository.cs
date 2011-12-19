using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.Repositories.Core;
using Epi.Web.DataServiceClient;
using Epi.Web.Common.Message;

namespace Epi.Web.Repositories
{
    public class SurveyResponseRepository : RepositoryBase, ISurveyResponseRepository
    {



        private Epi.Web.DataServiceClient.IDataService _iSurveyManager;

        public SurveyResponseRepository(Epi.Web.DataServiceClient.IDataService iSurveyManager)
        {
            _iSurveyManager = iSurveyManager;
        }
        
        /// <summary>
        /// Calling the proxy client to fetch a SurveyResponseResponse object
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public SurveyAnswerResponse GetSurveyAnswer(SurveyAnswerRequest pRequest)
        {

            //SurveyResponseResponse result = Client.GetSurveyResponse(pRequest);
            SurveyAnswerResponse result = _iSurveyManager.GetSurveyAnswer(pRequest);
            return result;
            
        }


        public SurveyAnswerResponse SaveSurveyAnswer(SurveyAnswerRequest pRequest)
        {
            SurveyAnswerResponse result = _iSurveyManager.SetSurveyAnswer(pRequest);
            return result;
        }

        #region stubcode
            public List<Common.DTO.SurveyAnswerDTO> GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public Common.DTO.SurveyAnswerDTO Get(int id)
            {
                throw new NotImplementedException();
            }

            public int GetCount(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public void Insert(Common.DTO.SurveyAnswerDTO t)
            {
                throw new NotImplementedException();
            }

            public void Update(Common.DTO.SurveyAnswerDTO t)
            {
                throw new NotImplementedException();
            }

            public void Delete(int id)
            {
                throw new NotImplementedException();
            } 
        #endregion


            List<SurveyAnswerResponse> IRepository<SurveyAnswerResponse>.GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            SurveyAnswerResponse IRepository<SurveyAnswerResponse>.Get(int id)
            {
                throw new NotImplementedException();
            }

            public void Insert(SurveyAnswerResponse t)
            {
                throw new NotImplementedException();
            }

            public void Update(SurveyAnswerResponse t)
            {
                throw new NotImplementedException();
            }
    }
}