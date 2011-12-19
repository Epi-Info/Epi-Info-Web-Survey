using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using Epi.Web.Models;
using Epi.Web.Repositories;


namespace Epi.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            //Configuring constructor injection. 
            //InjectedMembers: A unity container extension that allows you to configure which constructor, property or method gets injected via API
            //ConfigureInjectionFor is the API to configure injection for a particular type e.g. DataServiceClient proxy class
            //InjectionConstructor: creates an instance of Microsoft.Practices.Unity.InjectionConstructor that looks for a constructor with the given set of parameters
            // e.g. container.RegisterType<ITestService, TestService>();            
            container
            .RegisterType<Epi.Web.DataServiceClient.IDataService, Epi.Web.DataServiceClient.DataServiceClient>()
           .Configure<InjectedMembers>()
            .ConfigureInjectionFor<Epi.Web.DataServiceClient.DataServiceClient>(new InjectionConstructor("WSHttpBinding_IDataService"));
           
            container.RegisterType<Epi.Web.Common.Message.SurveyInfoRequest, Epi.Web.Common.Message.SurveyInfoRequest>();
            container.RegisterType<Epi.Web.Repositories.Core.ISurveyInfoRepository, Epi.Web.Repositories.SurveyInfoRepository>();


            container.RegisterType<Epi.Web.Common.Message.SurveyAnswerRequest, Epi.Web.Common.Message.SurveyAnswerRequest>();
            container.RegisterType<Epi.Web.Repositories.Core.ISurveyAnswerRepository, Epi.Web.Repositories.SurveyResponseRepository>();


            container.RegisterControllers();

            return container;

        }
    }
}