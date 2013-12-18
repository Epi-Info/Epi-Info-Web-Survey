using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;

namespace Epi.Web.BLL
{
    public class CacheDependencyInfo
    {
        private Epi.Web.Interfaces.DataInterfaces.ICacheDependencyInfoDao _cacheDependencyInfoDao;

        public CacheDependencyInfo(Epi.Web.Interfaces.DataInterfaces.ICacheDependencyInfoDao cacheDependencyInfoDao)
        {
            _cacheDependencyInfoDao = cacheDependencyInfoDao;
        }

        public List<CacheDependencyBO> GetCacheDependencyInfo()
        {
            List<CacheDependencyBO> result = _cacheDependencyInfoDao.GetCacheDependencyInfo();
            return result;
        }
    }
}
