﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Epi.Web.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute
            (
                "{*staticfile}",
                new { staticfile = @".*\.(jpg|gif|jpeg|png|js|css|htm|html|htc)$" }
            );


            routes.MapRoute
            (
                null, // Route name
                "Home/{surveyid}", // URL with parameters
                new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional }
            ); // Parameter defaults

            routes.MapRoute
         (
             null, // Route name
             "EIWST/DataService/{surveyid}", // URL with parameters
             new { controller = "EIWST", action = "Index", surveyid = UrlParameter.Optional }
         ); // Parameter defaults

       //     routes.MapRoute
       //(
       //    null, // Route name
       //    "EIWST/ManagerService", // URL with parameters
       //    new { controller = "EIWST", action = "TestManagerService" }
       //); // Parameter defaults
            routes.MapRoute
     (
       null,                                              // Route name
       "Survey/UpdateResponseXml/{id}",                           // URL with parameters
       new { controller = "Survey", action = "UpdateResponseXml", id = "" }
       );  // Parameter defaults

            routes.MapRoute (
                   null,                                              // Route name
                   "Survey/SaveSurvey/{id}",                           // URL with parameters
                   new { controller = "Survey", action = "SaveSurvey", id = "" }
                   ); 

            routes.MapRoute
                (
                    null, // Route name
                    "Survey/{responseid}/{PageNumber}", // URL with parameters
                    new { controller = "Survey", action = "Index", responseid = UrlParameter.Optional, PageNumber = UrlParameter.Optional }
                ); // Parameter defaults



            routes.MapRoute
              (
                  null, // Route name
                  "Login/{responseid}", // URL with parameters
                  new { controller = "Login", action = "Index", responseid = UrlParameter.Optional }
              ); // Parameter defaults


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




            routes.MapRoute
           (
             null,                                              // Route name
             "Post/SignOut/{id}",                           // URL with parameters
             new { controller = "Post", action = "SignOut", id = "" }
             );  // Parameter defaults



            //routes.MapRoute(
            //   "Default", // Route name
            //   "{*url}", // URL with parameters
            //   new { controller = "Home", action = "Default", id = UrlParameter.Optional }
            //  );
        }
    }
}