﻿using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace LogAppenderWeb
{
    /// <summary>
    /// https://www.c-sharpcorner.com/blogs/globalasax-events1
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        private Random m_rnd = new Random(DateTime.Now.Millisecond);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            

            var osInfo = System.Environment.OSVersion.VersionString;
            // definice základních globálních proměnných pro logování
            log4net.GlobalContext.Properties["OsInfo"] = osInfo;
            log4net.GlobalContext.Properties["StartServer"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            log4net.GlobalContext.Properties["LogName"] = "MyLog.log";
            log4net.GlobalContext.Properties["AssemblyVersion"] = "18.09.02.1";
            log4net.GlobalContext.Properties["Uziv"] = "???";

            Helper.log.Info("Start application...");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // definice proměnný pro logování aktuálního requestu
            log4net.ThreadContext.Properties["StartRequest"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            log4net.ThreadContext.Properties["URL"] = HttpContext.Current.Request.Url.OriginalString;
            var rn = $"{m_rnd.Next(0,9999)}-{DateTime.Now.Millisecond}";            

            if (log4net.ThreadContext.Stacks["NDC"].Count == 0)
            {
                log4net.ThreadContext.Stacks["NDC"].Push(rn);
            }

            Helper.log.Debug("Global.asax: Application_BeginRequest...");
        }

        protected void Application_EndRequest()
        {
            log4net.ThreadContext.Stacks["NDC"].Pop();
        }

        protected void Application_PostAuthorizeRequest()
        {            
            // vynutíme session
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);            
            // nastavím si uživatele pro logování
            log4net.ThreadContext.Properties["Uziv"] = User.Identity.IsAuthenticated ? User.Identity.Name : "anonymouse";
            Helper.log.Debug("Global.asax: Application_PostAuthorizeRequest...");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                Helper.log.Error("Unhandled exception", exception);
            }
        }
    }
}