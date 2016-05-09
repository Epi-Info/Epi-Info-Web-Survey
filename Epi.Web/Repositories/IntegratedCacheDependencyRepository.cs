using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
//using Epi.Web.DataServiceClient;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;
using System.ServiceModel;

using System.Web.Caching;
using System.Configuration;
namespace Epi.Web.MVC.Repositories
{
    public class IntegratedCacheDependencyRepository : RepositoryBase, ICacheDependencyRepository
    {
        private Epi.Web.WCF.SurveyService.IDataService _iDataService;

        public IntegratedCacheDependencyRepository(Epi.Web.WCF.SurveyService.IDataService iDataService)
        {
            _iDataService = iDataService;
        }
        
        public CacheDependencyResponse GetCacheDependencyInfo(CacheDependencyRequest pRequest)
        {
            try
            {
                CacheDependencyResponse surveyInfo = null;
                surveyInfo = (CacheDependencyResponse)_iDataService.GetCacheDependencyInfo(pRequest);
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

        public List<Common.DTO.CacheDependencyDTO> GetList(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public Common.DTO.CacheDependencyDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(Common.DTO.CacheDependencyDTO t)
        {
            throw new NotImplementedException();
        }

        public void Update(Common.DTO.CacheDependencyDTO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        } 

        List<CacheDependencyResponse> IRepository<CacheDependencyResponse>.GetList(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        CacheDependencyResponse IRepository<CacheDependencyResponse>.Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(CacheDependencyResponse t)
        {
            throw new NotImplementedException();
        }

        public void Update(CacheDependencyResponse t)
        {
            throw new NotImplementedException();
        }
    }
}