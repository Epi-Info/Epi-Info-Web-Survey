using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.DataServiceClient;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;
using Epi.Web.Utility;
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
        
        public SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
        {
            try
            {
                string surveyKey = pRequest.Criteria.SurveyIdList[0].ToString();

                SurveyInfoResponse surveyInfo = CacheUtility.Get(surveyKey) as SurveyInfoResponse;

                if (surveyInfo != null)
                {
                    return surveyInfo;
                }

                surveyInfo = (SurveyInfoResponse)_iDataService.GetSurveyInfo(pRequest);

                CacheUtility.Insert(surveyKey, surveyInfo);

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