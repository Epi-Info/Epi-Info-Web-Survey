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
        public SurveyResponseResponse GetSurveyResponse(SurveyResponseRequest pRequest)
        {

            //SurveyResponseResponse result = Client.GetSurveyResponse(pRequest);
            SurveyResponseResponse result = _iSurveyManager.GetSurveyResponse(pRequest);
            return result;
            
        }


        public SurveyResponseResponse SaveSurveyResponse(SurveyResponseRequest pRequest)
        {
            SurveyResponseResponse result = _iSurveyManager.SetSurveyResponse(pRequest);
            return result;
        }

        #region stubcode
            public List<Common.DTO.SurveyResponseDTO> GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public Common.DTO.SurveyResponseDTO Get(int id)
            {
                throw new NotImplementedException();
            }

            public int GetCount(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public void Insert(Common.DTO.SurveyResponseDTO t)
            {
                throw new NotImplementedException();
            }

            public void Update(Common.DTO.SurveyResponseDTO t)
            {
                throw new NotImplementedException();
            }

            public void Delete(int id)
            {
                throw new NotImplementedException();
            } 
        #endregion


            List<SurveyResponseResponse> IRepository<SurveyResponseResponse>.GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            SurveyResponseResponse IRepository<SurveyResponseResponse>.Get(int id)
            {
                throw new NotImplementedException();
            }

            public void Insert(SurveyResponseResponse t)
            {
                throw new NotImplementedException();
            }

            public void Update(SurveyResponseResponse t)
            {
                throw new NotImplementedException();
            }
    }
}