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
using Castle.Windsor.Installer;
using log4net;
using Monei.MvcApplication.Api;
using Monei.MvcApplication.Code;
using Monei.MvcApplication.Controllers;
using Monei.MvcApplication.Controllers.Api;
using Monei.MvcApplication.Core.Installers;
using Monei.MvcApplication.DelegatingHandlers;

namespace Monei.MvcApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer container;

        public IWindsorContainer WindsorContainer { get { return container; } }

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

            InitializeWindsorContainer();	 
        }

        protected void Application_End()
        {
            container.Dispose();
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

        public void InitializeWindsorContainer()
        {
            // http://stackoverflow.com/questions/32852440/dependency-injection-in-asp-net-session-start-method

            container = new WindsorContainer();

            container.Install(
                //FromAssembly.This()
                new RepositoriesInstaller(),
                new ControllerInstaller()
                );

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            var httpDependencyResolver = new WindsorDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver =  httpDependencyResolver;
        }
    }
}