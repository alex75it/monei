using System;
using System.Collections.Generic;
using System.Linq;
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
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		private static IWindsorContainer container;

		public IWindsorContainer WindSorContainer { get { return container; } }

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
			logger.Error("Applicaton Error.", Server.GetLastError());

			Response.RedirectToRoute("Error");
		}

		public void InitializeWindsorContainer()
		{
			// WindsorCastle
			container = new WindsorContainer().Install(
				//FromAssembly.This()
				new RepositoriesInstaller(),
				new ControllerInstaller()
				);

			container.Resolve<MoneiControllerBase>();
			container.Resolve<ApiControllerBase>();

			var controllerFactory = new WindsorControllerFactory(container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);

			var httpDependencyResolver = new WindsorDependencyResolver(container);
			GlobalConfiguration.Configuration.DependencyResolver =  httpDependencyResolver;
			
			//container.Install(new RepositoryInstaller());
		}

	}//class
}