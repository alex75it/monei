using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using log4net;
using Monei.MvcApplication.Core.WindsorInstallers;

namespace Monei.MvcApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private WindsorBootstrapper windsorBootstrapper;

        public MvcApplication()
        {

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CultureDelegatingHandler());
            //config.MessageHandlers.Add(new CultureDelegatingHandler());

            // http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();

            // Dependency Injection with Windsor Castle
            windsorBootstrapper = new WindsorBootstrapper();
            windsorBootstrapper.Initialize();
        }        

        protected void Application_End()
        {
            windsorBootstrapper.Dispose();
            //container.Dispose();
        }
        
        protected void Application_Error()
        {
            ILog logger = LogManager.GetLogger(this.GetType());
            Exception exception = Server.GetLastError();
            logger.Error("Applicaton Error.", exception);

            if (exception is HttpException)
            {
                HttpStatusCode statusCode = (System.Net.HttpStatusCode)(exception as HttpException).GetHttpCode();
                Server.TransferRequest("Error/" + (int)statusCode, false);            
            }
            
            Server.TransferRequest("Error", false);
        }

        
    }
}