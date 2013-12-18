using System;
using System.Linq;
using System.Collections.Generic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.EF
{
    public class EntityCacheDependencyInfoDao : ICacheDependencyInfoDao
    {
        public List<CacheDependencyBO> GetCacheDependencyInfo()
        {
            List<CacheDependencyBO> result = new List<CacheDependencyBO>();

            using (var Context = DataObjectFactory.CreateContext())
            {
                List<SurveyMetaData> list = (List<SurveyMetaData>)(Context.SurveyMetaDatas.ToList());
                Mapper.Map(list, out result);
            }

            return result;
        }
    }
}
