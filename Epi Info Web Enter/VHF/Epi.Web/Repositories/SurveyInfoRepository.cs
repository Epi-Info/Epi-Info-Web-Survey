using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.MVC.DataServiceClient;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.Exception;
using System.ServiceModel;
using Epi.Web.MVC.DataServiceClient;
using System.Web.Caching;
using System.Configuration;

namespace Epi.Web.MVC.Repositories
{
    public class SurveyInfoRepository : RepositoryBase, ISurveyInfoRepository
    {



        private Epi.Web.MVC.DataServiceClient.IEWEDataService _iDataService;

        public SurveyInfoRepository(Epi.Web.MVC.DataServiceClient.IEWEDataService iDataService)
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
                //SurveyInfoResponse result = _iDataService.GetSurveyInfo(pRequest);
                SurveyInfoResponse result = null;
                string SurveyId = pRequest.Criteria.SurveyIdList[0].ToString();
                var CacheObj = HttpRuntime.Cache.Get(SurveyId);

                int CacheDuration = 0;
                int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);
                string CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"];//false;
                string IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();

                if (CacheIsOn.ToUpper()=="TRUE") 
                {
                    if (CacheObj == null)
                    {
                        result = (SurveyInfoResponse)_iDataService.GetSurveyInfo(pRequest);

                        if (IsCacheSlidingExpiration.ToUpper() == "TRUE")
                        {
                           
                            HttpRuntime.Cache.Insert(SurveyId, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(CacheDuration));
                        }
                        else 
                        {
                            HttpRuntime.Cache.Insert(SurveyId, result, null, DateTime.Now.AddMinutes(CacheDuration), Cache.NoSlidingExpiration);
                           
                        }
                        return result;
                    }
                    else
                    {


                        return (SurveyInfoResponse)CacheObj;

                    }
                }
                else 
                {
                result = (SurveyInfoResponse)_iDataService.GetSurveyInfo(pRequest);
                return result;
                     
                }
 
                // return result;
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
            public List<Enter.Common.DTO.SurveyInfoDTO> GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public Enter.Common.DTO.SurveyInfoDTO Get(int id)
            {
                throw new NotImplementedException();
            }

            public int GetCount(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public void Insert(Enter.Common.DTO.SurveyInfoDTO t)
            {
                throw new NotImplementedException();
            }

            public void Update(Enter.Common.DTO.SurveyInfoDTO t)
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


            public FormsInfoResponse GetFormsInfoList(FormsInfoRequest fRequest)
            {

                //SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
                //throw new NotImplementedException();
                fRequest.Criteria.UserId = 2;//Hard coded user for now
                FormsInfoResponse result;
                result = (FormsInfoResponse)_iDataService.GetFormsInfo(fRequest);
                return result;

            }


            public SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest SARequest)
            {
              return _iDataService.DeleteResponse(SARequest);
            }

            public SurveyInfoResponse GetFormChildInfo(SurveyInfoRequest SurveyInfoRequest)
                {
                   return _iDataService.GetFormChildInfo(SurveyInfoRequest);
 
                }
            public FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest FormsHierarchyRequest)
                {

                      return _iDataService.GetFormsHierarchy(FormsHierarchyRequest);
 
                }
            public SurveyAnswerResponse GetResponseAncestor(SurveyAnswerRequest SARequest)
                {

                return _iDataService.GetAncestorResponseIdsByChildId(SARequest);
                }
       public OrganizationResponse GetUserOrganizations(OrganizationRequest OrgRequest)

        {
        return _iDataService.GetUserOrganizations(OrgRequest);
        }
    }
}