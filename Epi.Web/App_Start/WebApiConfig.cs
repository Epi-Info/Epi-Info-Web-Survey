using Epi.Web.MVC.Repositories;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.MVC.Resolver;
//using Epi.Web.SurveyAPI.Repository;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Epi.Web.MVC.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
             var container = new UnityContainer();
             container.RegisterType<ISurveyResponseApiRepository, SurveyResponseApiRepository>(new HierarchicalLifetimeManager());
             config.DependencyResolver = new UnityResolver(container);        
        }
    }
}