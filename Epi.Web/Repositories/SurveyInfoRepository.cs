using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.MVC.DataServiceClient;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;
using System.ServiceModel;
//using Epi.Web.WCF.SurveyService;
using System.Web.Caching;
using System.Configuration;

namespace Epi.Web.MVC.Repositories
{
    public class SurveyInfoRepository : RepositoryBase, ISurveyInfoRepository
    {



        private   IDataService _iDataService;
        private Epi.Web.WCF.SurveyService.IManagerServiceV4 _iManagerService;
        private string CacheIsOn = "";
        private string IsCacheSlidingExpiration = "";
        private int CacheDuration = 0;
        public SurveyInfoRepository(IDataService iDataService, Epi.Web.WCF.SurveyService.IManagerServiceV4 iManagerService)
        {
            _iDataService = iDataService;
            _iManagerService = iManagerService;
            int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);
            CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"];//false;
            IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();
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
        public SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequest)
            {
            try
                {
                SurveyControlsResponse ControlListObj = _iDataService.GetSurveyControlList(pRequest);
                return ControlListObj;
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
        public SurveyInfoResponse PublishExcelSurvey(SurveyInfoRequest pRequest)
        {
            try
            {
                SurveyInfoResponse ControlListObj = _iManagerService.SetSurveyInfo(pRequest);
                return ControlListObj;
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
        public bool ValidateOrganization(OrganizationRequest Request)
        {
            try
            {
                bool IsValid =  _iManagerService.ValidateOrganization(Request);
                return IsValid;
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

        public SurveyInfoResponse GetAllSurveysByOrgKey(string OrgKey)
        {
            try
            {
                SurveyInfoResponse SurveyInfoResponse =  _iManagerService.GetAllSurveysByOrgKey(OrgKey);
                return SurveyInfoResponse;
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
       public SourceTablesResponse GetSourceTables(SourceTablesRequest pRequest)
       {

           try
           {
               string SurveyId = pRequest.SurveyId + "_SourceTables";
               var CacheObj = HttpRuntime.Cache.Get(SurveyId);
               SourceTablesResponse result = new SourceTablesResponse();
               string CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"]; ;
               string IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();
               int CacheDuration = 0;
               int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);

               if (CacheIsOn.ToUpper() == "TRUE")
               {
                   if (CacheObj == null)
                   {
                       result = (SourceTablesResponse)_iDataService.GetSourceTables(pRequest);

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


                       return (SourceTablesResponse)CacheObj;

                   }
               }
               else
               {
                   result = (SourceTablesResponse)_iDataService.GetSourceTables(pRequest);
                   return result;

               }

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

        public SurveyInfoResponse GetFormChildInfo(SurveyInfoRequest SurveyInfoRequest)
        {
            return _iDataService.GetFormChildInfo(SurveyInfoRequest);

        }
        public FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest pRequest)
        {


            string SurveyId = pRequest.SurveyInfo.SurveyId;
            var CacheObj = HttpRuntime.Cache.Get(SurveyId);
            FormsHierarchyResponse result = new FormsHierarchyResponse();


            if (CacheIsOn.ToUpper() == "TRUE")
            {
                if (CacheObj == null)
                {
                    result = (FormsHierarchyResponse)_iDataService.GetFormsHierarchy(pRequest);

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


                    return (FormsHierarchyResponse)CacheObj;

                }
            }
            else
            {
                result = (FormsHierarchyResponse)_iDataService.GetFormsHierarchy(pRequest);
                return result;

            }
        }
        public bool HasResponse(string SurveyId, string ResponseId)
        {
            try
            {

                return _iDataService.HasResponse(SurveyId, ResponseId);

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
        public SurveyAnswerResponse GetResponseAncestor(SurveyAnswerRequest SARequest)
        {

            return _iDataService.GetAncestorResponseIdsByChildId(SARequest);
        }
    }
}