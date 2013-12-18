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
using System.Web.Caching;
using System.Configuration;
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
                SurveyInfoResponse surveyInfo = null;
                string SurveyId = pRequest.Criteria.SurveyIdList[0].ToString();

                SurveyInfoResponse cachedSurveyInfo = (SurveyInfoResponse)HttpRuntime.Cache.Get(SurveyId);

                int CacheDuration = 0;
                int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);
                string CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"];//false;
                string IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();

                if (CacheIsOn.ToUpper() == "TRUE")
                {
                    if (cachedSurveyInfo == null)
                    {
                        surveyInfo = (SurveyInfoResponse)_iDataService.GetSurveyInfo(pRequest);
                        ProxyDependency proxyDependency = new ProxyDependency(SurveyId);
                        
                        if (IsCacheSlidingExpiration.ToUpper() == "TRUE")
                        {

                            HttpRuntime.Cache.Insert(SurveyId, surveyInfo, proxyDependency, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(CacheDuration));
                        }
                        else
                        {
                            HttpRuntime.Cache.Insert(SurveyId, surveyInfo, proxyDependency, DateTime.Now.AddMinutes(CacheDuration), Cache.NoSlidingExpiration);
                        }
                    }
                    else
                    {
                        surveyInfo = (SurveyInfoResponse)cachedSurveyInfo;
                    }
                }
                else
                {
                    surveyInfo = (SurveyInfoResponse)_iDataService.GetSurveyInfo(pRequest);
                }

                return surveyInfo;
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