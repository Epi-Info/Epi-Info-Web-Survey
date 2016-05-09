using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.WCF.SurveyService ;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;
using System.ServiceModel;
 
using System.Web.Caching;
using System.Configuration;

namespace Epi.Web.MVC.Repositories
{
    public class CacheDependencyRepository : RepositoryBase, ICacheDependencyRepository
    {
        private  IDataService _IDataService;

        public CacheDependencyRepository(IDataService iDataService)
        {
            _IDataService = iDataService;
        }
        
        public CacheDependencyResponse GetCacheDependencyInfo(CacheDependencyRequest request)
        {
            try
            {
                CacheDependencyResponse response = null;//(CacheDependencyResponse)_IDataService.GetCacheDependencyInfo(request);
                return response;
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
        #endregion


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