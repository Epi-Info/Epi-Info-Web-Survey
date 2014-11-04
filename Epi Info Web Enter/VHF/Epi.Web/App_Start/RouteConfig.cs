using System;
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
                new { staticfile = @".*\.(jpg|gif|jpeg|png|js|css|htm|html|htc|php)$" }
            );


            //routes.MapRoute
            //(
            //   "ListForms", // Route name
            //    "Home/ListForms", // URL with parameters
            //    new { controller = "Home", action = "ListForms" }
            //); // Parameter defaults

            routes.MapRoute
          (
              null, // Route name
              "Home/ReadSortedResponseInfo", ///{formid}/{page}/{sort}/{sortfield} URL with parameters
              new { controller = "Home", action = "ReadSortedResponseInfo", formid = UrlParameter.Optional, page = UrlParameter.Optional, sort = UrlParameter.Optional, sortfield = UrlParameter.Optional }
          ); // Parameter defaults


            routes.MapRoute
           (
               null, // Route name
               "Home/ReadResponseInfo", // URL with parameters
               new { controller = "Home", action = "ReadResponseInfo" }
           ); // Parameter defaults
         
           
            routes.MapRoute
         (
             null, // Route name
             "Home/GetSettings", // URL with parameters
             new { controller = "Home", action = "GetSettings", formid = UrlParameter.Optional }
         ); // Parameter defaults

             
            routes.MapRoute
      (
          null, // Route name
          "Home/SaveSettings", // URL with parameters
          new { controller = "Home", action = "SaveSettings", formid = UrlParameter.Optional }
      ); // Param
            routes.MapRoute
               (
                   null, // Route name
                   "Home/Edit", // URL with parameters
                   new { controller = "Home", action = "Edit", ResId = UrlParameter.Optional }
               );
            routes.MapRoute
              (
                  null, // Route name
                  "Home/LogOut", // URL with parameters
                  new { controller = "Home", action = "LogOut", ResId = UrlParameter.Optional }
              ); 
            routes.MapRoute
            (
                null, // Route name
                "Home/Delete/{ResponseId}", // URL with parameters
                new { controller = "Home", action = "Delete", ResponseId = UrlParameter.Optional }
            );
            routes.MapRoute
            (
                null, // Route name
                "FormResponse/Delete/{ResponseId}", // URL with parameters
                new { controller = "FormResponse", action = "Delete", ResponseId = UrlParameter.Optional }
            );
            routes.MapRoute
           (
               null, // Route name
               "FormResponse/DeleteBranch/{ResponseId}", // URL with parameters
               new { controller = "FormResponse", action = "DeleteBranch", ResponseId = UrlParameter.Optional }
           );
            routes.MapRoute
            (
                null, // Route name
                "Home/{surveyid}", // URL with parameters
                new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional}
            ); // Parameter defaults

            routes.MapRoute
            (
                null, // Route name
                "Home/Index/{surveyid}/{AddNewFormId}", // URL with parameters
                new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional, AddNewFormId = UrlParameter.Optional }
            ); // Parameter defaults
            routes.MapRoute
          (
              null, // Route name
              "Home/Index/{surveyid}/{EditForm}", // URL with parameters
              new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional, EditForm = UrlParameter.Optional }
          ); // Parameter defaults
            routes.MapRoute
            (
                null, // Route name
                "FormResponse/LogOut", // URL with parameters
                new { controller = "FormResponse", action = "LogOut", ResId = UrlParameter.Optional }
            );
            routes.MapRoute
           (
               null, // Route name
               "FormResponse/ReadResponseInfo", // URL with parameters
               new { controller = "FormResponse", action = "ReadResponseInfo", ResId = UrlParameter.Optional }
           );
           
            routes.MapRoute
            (
                null, // Route name
                "FormResponse/{formid}/{responseid}", // URL with parameters
                new { controller = "FormResponse", action = "Index", formid = UrlParameter.Optional, responseid = UrlParameter.Optional }
            ); // Parameter defaults
            routes.MapRoute
                      (
                          null, // Route name
                          "FormResponse/{formid}", // URL with parameters
                          new { controller = "FormResponse", action = "Index", formid = UrlParameter.Optional }
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

            routes.MapRoute(
                   null,                                              // Route name
                   "Survey/SaveSurvey/{id}",                           // URL with parameters
                   new { controller = "Survey", action = "SaveSurvey", id = "" }
                   );
            routes.MapRoute
               (
                   null, // Route name
                   "Survey/Delete/{responseid}", // URL with parameters
                   new { controller = "Survey", action = "Delete", responseid = UrlParameter.Optional, PageNumber = UrlParameter.Optional }
               );
            routes.MapRoute
               (
                   null, // Route name
                   "Survey/DeleteBranch/{responseid}", // URL with parameters
                   new { controller = "Survey", action = "DeleteBranch", responseid = UrlParameter.Optional, PageNumber = UrlParameter.Optional }
               );
            
            routes.MapRoute
               (
                   null, // Route name
                   "Survey/AddChild", // URL with parameters
                   new { controller = "Survey", action = "AddChild" }
               );
          
            routes.MapRoute
              (
                  null, // Route name
                  "Survey/HasResponse", // URL with parameters
                  new { controller = "Survey", action = "HasResponse" }
              );
            routes.MapRoute
             (
                 null, // Route name
                 "Survey/UpDateGrid", // URL with parameters
                 new { controller = "Survey", action = "UpDateGrid" }
             );
            routes.MapRoute
            (
                null, // Route name
                "Survey/ReadResponseInfo", // URL with parameters
                new { controller = "Survey", action = "ReadResponseInfo" }
            );
            routes.MapRoute
             (
                 null, // Route name
                 "Survey/LogOut", // URL with parameters
                 new { controller = "Survey", action = "LogOut", ResId = UrlParameter.Optional }
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
                 "Login/ForgotPassword", // URL with parameters
                 new { controller = "Login", action = "ForgotPassword" }
             ); // Parameter defaults

            routes.MapRoute
            (
                null, // Route name
                "Login/ResetPassword", // URL with parameters
                new { controller = "Login", action = "ResetPassword" }
            ); // Parameter defaults

            routes.MapRoute
              (
                  null, // Route name
                  "Login/Index", // URL with parameters
                  new { controller = "Login", action = "Index" }
              ); // Parameter defaults

            routes.MapRoute
              (
                  null, // Route name
                  "Login", // URL with parameters
                  new { controller = "Login", action = "Index" }
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
            routes.MapRoute
          (
              null, // Route name
              "AdminOrganization/AutoComplete", // URL with parameters
              new { controller = "AdminOrganization", action = "AutoComplete" }
          ); // Parameter defaults
            routes.MapRoute
                 (
                     null, // Route name
                     "AdminOrganization/OrgList", // URL with parameters
                     new { controller = "AdminOrganization", action = "OrgList" }
                 ); // Parameter defaults
            routes.MapRoute
                (
                    null, // Route name
                    "AdminOrganization/{orgkey}/{iseditmode}", // URL with parameters
                    new { controller = "AdminOrganization", action = "OrgInfo", orgkey = UrlParameter.Optional, iseditmode = UrlParameter.Optional }
                ); // Parameter defaults
            routes.MapRoute
                (
                    null, // Route name
                    "AdminOrganization/Cancel", // URL with parameters
                    new { controller = "AdminOrganization", action = "Cancel"}
                ); // Parameter defaults



           
            routes.MapRoute
            (
                null, // Route name
                "AdminUser/GetUserList", // URL with parameters
                new { controller = "AdminUser", action = "GetUserList", orgid = UrlParameter.Optional }
            ); // Parameter defaults
          
            routes.MapRoute
              (
                  null, // Route name
                  "AdminUser/UserList", // URL with parameters
                  new { controller = "AdminUser", action = "UserList" }
              ); // Parameter defaults
            routes.MapRoute
              (
                  null, // Route name
                  "AdminUser/{userid}/{iseditmode}/{orgid}", // URL with parameters
                  new { controller = "AdminUser", action = "UserInfo", userid = UrlParameter.Optional, iseditmode = UrlParameter.Optional, orgid = UrlParameter.Optional }
              ); // Parameter defaults

            //routes.MapRoute(
            //   "Default", // Route name
            //   "{*url}", // URL with parameters
            //   new { controller = "Home", action = "Default", id = UrlParameter.Optional }
            //  );
        }
    }
}
