using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using Epi.Web.MVC;
using Epi.Web.MVC;
using Epi.Web.MVC.Utility;
using System.Configuration;

namespace Epi.Web.MVC
{
    public static class Bootstrapper
    {
        public static bool IsIntegrated = false;

        public static void Initialise()
        {
            string s = ConfigurationManager.AppSettings["INTEGRATED_SERVICE_MODE"];
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Equals("TRUE",System.StringComparison.OrdinalIgnoreCase))
                {
                     IsIntegrated = true;
                }
                else
                {
                     IsIntegrated = false;
                }
            }

            IUnityContainer container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            UnityContainer container = new UnityContainer();

            // register all your components with the container here
            //Configuring constructor injection. 
            //InjectedMembers: A unity container extension that allows you to configure which constructor, property or method gets injected via API
            //ConfigureInjectionFor is the API to configure injection for a particular type e.g. DataServiceClient proxy class
            //InjectionConstructor: creates an instance of Microsoft.Practices.Unity.InjectionConstructor that looks for a constructor with the given set of parameters
            // e.g. container.RegisterType<ITestService, TestService>();            

            container.RegisterType<Epi.Web.Common.Message.SurveyInfoRequest, Epi.Web.Common.Message.SurveyInfoRequest>();
            container.RegisterType<Epi.Web.Common.Message.CacheDependencyRequest, Epi.Web.Common.Message.CacheDependencyRequest>();

            if (IsIntegrated)
            {
                container.RegisterType<Epi.Web.WCF.SurveyService.IDataService, Epi.Web.WCF.SurveyService.DataService>();
                container.RegisterType<Epi.Web.WCF.SurveyService.IManagerServiceV4 , Epi.Web.WCF.SurveyService.ManagerServiceV4>();
                container.RegisterType<SurveyResponseXML, SurveyResponseXML>()
                    .Configure<InjectedMembers>()
                    .ConfigureInjectionFor<SurveyResponseXML>(new InjectionConstructor());

                container.RegisterType<Epi.Web.MVC.Repositories.Core.ISurveyInfoRepository, Epi.Web.MVC.Repositories.IntegratedSurveyInfoRepository>();
                container.RegisterType<Epi.Web.MVC.Repositories.Core.ICacheDependencyRepository, Epi.Web.MVC.Repositories.IntegratedCacheDependencyRepository>();
                container.RegisterType<Epi.Web.MVC.Repositories.Core.IOrganizationAccountRepository, Epi.Web.MVC.Repositories.IntegratedOrganizationAccountRepository>();
                container.RegisterType<Epi.Web.MVC.Repositories.Core.IReportRepository, Epi.Web.MVC.Repositories.IntegratedReportInfoRepository>();
            }
            else
            {
                container.RegisterType<Epi.Web.MVC.DataServiceClient.IDataService, Epi.Web.MVC.DataServiceClient.DataServiceClient>()
                .Configure<InjectedMembers>()
                .ConfigureInjectionFor<Epi.Web.MVC.DataServiceClient.DataServiceClient>(new InjectionConstructor(ConfigurationManager.AppSettings["ENDPOINT_USED"]));
                container.RegisterType<Epi.Web.MVC.Repositories.Core.ISurveyInfoRepository, Epi.Web.MVC.Repositories.SurveyInfoRepository>();
                container.RegisterType<Epi.Web.MVC.Repositories.Core.IReportRepository, Epi.Web.MVC.Repositories.ReportInfoRepository>();
            }

            container.RegisterType<Epi.Web.Common.Message.SurveyAnswerRequest, Epi.Web.Common.Message.SurveyAnswerRequest>();

            if (IsIntegrated)
            {
                container.RegisterType<Epi.Web.MVC.Repositories.Core.ISurveyAnswerRepository, Epi.Web.MVC.Repositories.IntegratedSurveyAnswerRepository>();
            }
            else
            {
                container.RegisterType<Epi.Web.MVC.Repositories.Core.ISurveyAnswerRepository, Epi.Web.MVC.Repositories.SurveyAnswerRepository>();
            }


            container.RegisterType<Common.DTO.SurveyAnswerDTO, Common.DTO.SurveyAnswerDTO>();
            container.RegisterType<Epi.Web.MVC.Facade.ISurveyFacade, Epi.Web.MVC.Facade.SurveyFacade>();
            container.RegisterControllers();

            return container;

        }
    }
}