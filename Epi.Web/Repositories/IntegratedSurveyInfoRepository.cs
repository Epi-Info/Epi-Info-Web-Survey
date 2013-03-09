using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.DataServiceClient;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;
using System.ServiceModel;
using Epi.Web.DataServiceClient;
namespace Epi.Web.MVC.Repositories
{
    public class IntegratedSurveyInfoRepository : RepositoryBase, ISurveyInfoRepository
    {



        private Epi.Web.WCF.SurveyService.IDataService _iDataService;

        public IntegratedSurveyInfoRepository(Epi.Web.WCF.SurveyService.IDataService iDataService)
        {
            _iDataService = iDataService;
        }
        
        /// <summary>
        /// Calling the proxy client to fetch a SurveyInfoResponse object
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
        {

            try
            {
                //SurveyInfoResponse result = Client.GetSurveyInfo(pRequest);
                SurveyInfoResponse result = _iDataService.GetSurveyInfo(pRequest);
                return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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