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
            return result;
        }
        
        public List<CacheDependencyBO> GetCacheDependencyInfo(List<string> surveyKeys)
        {
            List<CacheDependencyBO> result = new List<CacheDependencyBO>();

            if (surveyKeys.Count > 0)
            {
                try
                {
                    foreach (string key in surveyKeys)
                    {
                        Guid guid = new Guid(key);

                        using (var Context = DataObjectFactory.CreateContext())
                        {
                            SurveyMetaData surveyMetaDatas = Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == guid);
                            CacheDependencyBO cacheDependencyBO = Mapper.MapDependency(surveyMetaDatas);
                            result.Add(cacheDependencyBO);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return result;
        }
    }
}
