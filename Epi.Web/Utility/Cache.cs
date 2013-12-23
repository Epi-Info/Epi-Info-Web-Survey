using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Epi.Web.MVC.Utility
{
    public static class Cache
    {
        static bool _cacheIsOn = false;
        static bool _useSlidingExpiration = false;
        static int _cacheDuration = 0;

        static bool initialized = false;

        static private void Initialize()
        {
            if (initialized) return;
            
            int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out _cacheDuration);

            string cacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"];

            if (cacheIsOn.ToUpper() == "TRUE")
            {
                _cacheIsOn = true;
            }

            string useSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();
            
            if (useSlidingExpiration.ToUpper() == "TRUE")
            {
                _useSlidingExpiration = true;
            }

            initialized = true;
        }
        
        public static object Get(string cacheKey)
        {
            Initialize();
            object cachedObject = null;

            if (_cacheIsOn)
            {
                cachedObject = HttpRuntime.Cache.Get(cacheKey);
            }

            return cachedObject;
        }

        public static void Insert(string cacheKey, object cachedObject)
        {
            Insert(cacheKey, cachedObject, cacheKey);
        }
            
        public static void Insert(string cacheKey, object cachedObject, string dependencyKey)
        {
            Initialize();

            if (_cacheIsOn)
            {
                ProxyDependency proxyDependency = new ProxyDependency(dependencyKey);

                if (_useSlidingExpiration)
                {
                    HttpRuntime.Cache.Insert(cacheKey, cachedObject, proxyDependency, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(_cacheDuration));

                }
                else
                {
                    HttpRuntime.Cache.Insert(cacheKey, cachedObject, proxyDependency, DateTime.Now.AddMinutes(_cacheDuration), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
        }
    }
}