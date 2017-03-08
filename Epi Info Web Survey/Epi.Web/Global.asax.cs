using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Diagnostics;
using System.Configuration;
 
using ImranB.ModelBindingFix;
 
using System.Web.WebPages;
 


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
    
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Android") { ContextCondition = (context => context.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0) });
            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Opera") { ContextCondition = (context => context.Request.UserAgent.IndexOf("Opera Mobi", StringComparison.OrdinalIgnoreCase) >= 0) });

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile") { ContextCondition = (context => context.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0) });
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile") { ContextCondition = (context => context.Request.UserAgent.IndexOf("Opera Mobi", StringComparison.OrdinalIgnoreCase) >= 0) });
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile") { ContextCondition = (context => context.Request.UserAgent.IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0) });

            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("iPhone") { ContextCondition = (context => context.Request.UserAgent.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0) });
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Initialise();

           // DisplayModes.Modes.Insert(0, new  DefaultDisplayMode("Mobile") 
            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile") 
            //{ 
            //    ContextCondition = (ctx => ctx.Request.UserAgent != null 
            //                                && (ctx.Request.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0 
            //                                    || ctx.Request.UserAgent.IndexOf("Mobile", StringComparison.OrdinalIgnoreCase) >= 0 
            //                                    || ctx.Request.UserAgent.IndexOf("Opera Mobi", StringComparison.OrdinalIgnoreCase) >= 0
            //                                    || ctx.Request.UserAgent.IndexOf("Opera", StringComparison.OrdinalIgnoreCase) >= 0
            //                                    || ctx.Request.UserAgent.IndexOf("opera", StringComparison.OrdinalIgnoreCase) >= 0 
            //                                    || ctx.Request.UserAgent.IndexOf("Opera Mini", StringComparison.OrdinalIgnoreCase) >= 0)) 
            //});

            ProxyDependencyReference.Initialise();
            Fixer.FixModelBindingIssue();

        }


        /// <summary>
        ///  HKLM\SYSTEM\CurrentControlSet\services\eventlog needs to be set 
        ///  in order to use the event log
        /// </summary>
        protected void Application_Error()
        {
            Exception exc = Server.GetLastError();

            try
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage(exc);
            }
            catch { }

            this.Response.Redirect("/", true);
        }
    }
}

