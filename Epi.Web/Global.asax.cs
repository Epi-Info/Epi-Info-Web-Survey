using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Epi.Web.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.MapRoute
            (
                null, // Route name
                "Home/{surveyid}", // URL with parameters
                new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional }
            ); // Parameter defaults
            routes.MapRoute
                (
                    null, // Route name
                    "Survey/{responseid}/{PageNumber}", // URL with parameters
                    new { controller = "Survey", action = "Index", responseid = UrlParameter.Optional, PageNumber = UrlParameter.Optional }
                ); // Parameter defaults
            routes.MapRoute
                 (
                     null, // Route name
                     "Home/{responseid}/{StatusId}", // URL with parameters
                     new { controller = "Home", action = "Response", responseid = UrlParameter.Optional, StatusId = UrlParameter.Optional }
                 );

            routes.MapRoute
           (
               null, // Route name
               "Final/{surveyid}", // URL with parameters
               new { controller = "Final", action = "Index", surveyid = UrlParameter.Optional }
           ); // Parameter defaults

            routes.MapRoute
            (
              null,                                              // Route name
              "Post/Notify/{id}",                           // URL with parameters
              new { controller = "Post", action = "Notify", id = "" }
              );  // Parameter defaults


            routes.MapRoute(
               "Default", // Route name
               "{*url}", // URL with parameters
               new { controller = "Home", action = "Splash", id = UrlParameter.Optional }
              );

            //routes.MapRoute
            //(
            //    null, // Route name
            //    "Survey/{surveyid}", // URL with parameters
            //    new { controller = "Survey", action = "ListSurvey", surveyid = UrlParameter.Optional }
            //); // Parameter defaults

            //routes.MapRoute
            //  (
            //      null, // Route name
            //      "Survey/{surveyid}", // URL with parameters
            //      new { controller = "Home", action = "PostSubmit", surveyid = UrlParameter.Optional }
            //  ); // 

        
           

            //routes.MapRoute(
            //   null, // Route name
            //   "{*url}", // URL with parameters
            //   new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Bootstrapper.Initialise();
        }
    }
}