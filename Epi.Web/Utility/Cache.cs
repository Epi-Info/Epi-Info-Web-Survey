using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epi.Web.MVC.Utility
{
    public static class Cache
    {
        public static object Get(string cacheKey)
        {
            object cachedObject = null;
            cachedObject = (ActionResult)HttpRuntime.Cache.Get(cacheKey);
            return cachedObject;
        }

        public static void Insert(string cacheKey, object cachedObject, string dependencyKey)
        {
            ProxyDependency proxyDependency = new ProxyDependency(dependencyKey);
            HttpRuntime.Cache.Insert(cacheKey, cachedObject, proxyDependency);
        }
    }
}