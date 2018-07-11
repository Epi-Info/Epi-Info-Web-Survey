using Epi.Web.MVC.Resolver;
using Epi.Web.SurveyAPI.Repository;
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
        public static void Register(HttpConfiguration configuration)
        {
            var container = new UnityContainer();
            container.RegisterType<ISurveyResponseRepository, SurveyResponseRepository>(new HierarchicalLifetimeManager());
            configuration.DependencyResolver = new UnityResolver(container);
        }
    }
}