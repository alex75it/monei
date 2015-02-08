using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.MvcApplication;
using Monei.MvcApplication.Api;
using Monei.MvcApplication.Code;
using Monei.MvcApplication.Core.Installers;
using Newtonsoft.Json.Serialization;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
	[TestClass]
	public class ApiControllerTestBase :IDisposable
	{
		protected HttpServer server;
		private string baseUrl = "http://www.apitest.com/";
		private IWindsorContainer container;

		// todo: CLEAN THIS CLASS

		//protected void InitializeController(ApiController controller)
		//{ 
		
		//}

		[TestInitialize]
		public void Initialize()
		{			

			// WindsorCastle
			
			InitializeWindsorContainer();

			server = new HttpServer(GetConfiguration());
			
			//container.Install(
			//	new RepositoriesInstaller(),
			//	FromAssembly.This()
			//	);

			//AccountRepository = container.Resolve<IAccountRepository>();
			//CurrencyRepository = container.Resolve<ICurrencyRepository>();
			//RegistryRepository = container.Resolve<IRegistryRepository>();
			//CategoryRepository = container.Resolve<ICategoryRepository>();
			//SubcategoryRepository = container.Resolve<ISubcategoryRepository>();
		}


		private void InitializeWindsorContainer()
		{
			// WindsorCastle
			container = new WindsorContainer();

			container.Register(
				Component.For<IAccountRepository>().Instance( new AccountRepository())
				);

			IAccountRepository accountREpository = container.Resolve<IAccountRepository>();

			container.Install( new ControllerInstaller());

			//container.Resolve<MoneiControllerBase>();
			ApiController controller = container.Resolve<ApiControllerBase>();

			var controllerFactory = new WindsorControllerFactory(container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
			

			var httpDependencyResolver = new WindsorDependencyResolver(container);
			GlobalConfiguration.Configuration.DependencyResolver = httpDependencyResolver;

			
			//container.Register(
			//	Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient(),
			//	Classes.FromThisAssembly().BasedOn<MoneiControllerBase>().LifestyleTransient(),
			//	Classes.FromThisAssembly().BasedOn<ApiControllerBase>().LifestyleTransient() // .LifestyleScoped()

			//	//Component.For<IAccountRepository>().DependsOn<IAccountRepository>().in
			//	);

			//container.Install(new RepositoryInstaller());
		}

		public ApiControllerTestBase()
		{
			
		}

		protected HttpConfiguration GetConfiguration()
		{
			HttpConfiguration configuration = new HttpConfiguration();		
			var dependencyResolver = new WindsorDependencyResolver(container);
			configuration.DependencyResolver = dependencyResolver;
			WebApiConfig.Register(configuration);

			return configuration;
		}

		protected HttpRequestMessage CreateRequest(string url, HttpMethod method, string mediaType = "application/json")
		{
			var request = new HttpRequestMessage(method, baseUrl + url);			

			// I don't know what this does. Copied from one example.
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			return request;
		}

		protected HttpRequestMessage CreateRequest<T>(string url, HttpMethod method,  T content, string mediaType = "application/json")
		{
			var request = CreateRequest(url, method, mediaType);
			JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
			formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			request.Content = new ObjectContent<T>(content, formatter);
			return request;
		}


		public void Dispose()
		{
			if (server != null)
				server.Dispose();
		}
	}
}
