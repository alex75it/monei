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
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.MvcApplication;
using Monei.MvcApplication.Api;
using Monei.MvcApplication.Code;
using Monei.MvcApplication.Core.Installers;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
	[TestFixture]
	public class ApiControllerTestBase :IDisposable
	{
		protected HttpServer server;
		private string baseUrl = "http://www.apitest.com/";
		private IWindsorContainer container;

		// todo: CLEAN THIS CLASS

		public ApiControllerTestBase()
		{

		}

		[TestFixtureSetUp]
		public void Initialize()
		{			
			InitializeWindsorContainer();
			server = new HttpServer(GetConfiguration());
		}


		protected HttpClient GetClient()
		{
			// initialize a new Server
			var server = new HttpServer(GetConfiguration());
			return new HttpClient(server);
		}


		private void InitializeWindsorContainer()
		{

			container = new WindsorContainer();

			container.Register(
				Component.For(typeof(IAccountRepository)).ImplementedBy(typeof(AccountRepository)), //.LifestylePerWebRequest(),
				Component.For(typeof(IRegistryRepository)).ImplementedBy(typeof(RegistryRepository)), //.LifestylePerWebRequest(),
				Component.For(typeof(ICurrencyRepository)).ImplementedBy(typeof(CurrencyRepository)), //.LifestylePerWebRequest(),
				Component.For(typeof(ICategoryRepository)).ImplementedBy(typeof(CategoryRepository)), //.LifestylePerWebRequest(),
				Component.For(typeof(ISubcategoryRepository)).ImplementedBy(typeof(SubcategoryRepository)) //.LifestylePerWebRequest(),

				//Component.For(typeof(SubcategoryManager)).ImplementedBy(typeof(SubcategoryManager)), //.LifestylePerWebRequest()
			);
			
			// todo: try to use this...
			//container.Install(new RepositoriesInstaller());
			// give this error:
			/*
			An exception of type 'Castle.MicroKernel.ComponentResolutionException' occurred in Castle.Windsor.dll but was not handled in user code
			Additional information: Looks like you forgot to register the http module Castle.MicroKernel.Lifestyle.PerWebRequestLifestyleModule
			*/

			// for check...
			IAccountRepository accountREpository = container.Resolve<IAccountRepository>();

			container.Install( new ControllerInstaller());

			//container.Resolve<MoneiControllerBase>();
			//ApiController controller = container.Resolve<ApiControllerBase>();

			var controllerFactory = new WindsorControllerFactory(container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
			

			var httpDependencyResolver = new WindsorDependencyResolver(container);
			GlobalConfiguration.Configuration.DependencyResolver = httpDependencyResolver;
		}



		protected HttpConfiguration GetConfiguration()
		{
			HttpConfiguration configuration = new HttpConfiguration();		
			var dependencyResolver = new WindsorDependencyResolver(container);
			configuration.DependencyResolver = dependencyResolver;
			WebApiConfig.Register(configuration);

			return configuration;
		}

		/// <summary>
		/// Create a HttpRequestMessage that can be used with HttpClient.SendAsync() method.		/// 
		/// </summary>
		/// <param name="url">Partial URL of the API <example>api/mycontroller/param</example></param>
		/// <param name="method">GET, POST ...</param>
		/// <param name="content"></param>
		/// <param name="mediaType">HTTP "Header Accept" value, default is for JSON data</param>
		/// <returns></returns>
		protected HttpRequestMessage CreateRequest(string url, HttpMethod method, string mediaType = "application/json")
		{
			var request = new HttpRequestMessage(method, baseUrl + url);			

			// I don't know what this does. Copied from one example.
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			return request;
		}

		/// <summary>
		/// Create a HttpRequestMessage that can be used with HttpClient.SendAsync() method.		/// 
		/// </summary>
		/// <typeparam name="T">Type of the class passed in Content of the request</typeparam>
		/// <param name="url">Partial URL of the API <example>api/mycontroller/param</example></param>
		/// <param name="method">GET, POST ...</param>
		/// <param name="content">The object data passed in the request</param>
		/// <param name="mediaType">HTTP "Header Accept" value, default is for JSON data</param>
		/// <returns></returns>
		protected HttpRequestMessage CreateRequest<T>(string url, HttpMethod method,  T content, string mediaType = "application/json")
		{
			var request = CreateRequest(url, method, mediaType);
			// todo: is this needed? (CamelCasePropertyNameContractResolver)
			JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
			formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			request.Content = new ObjectContent<T>(content, formatter);
			return request;
		}

		protected T CallApi<T>(string url, HttpMethod httpMethod)
			where T : class
		{
			T returnValue;
			using (var client = GetClient())
			using (var result = client.SendAsync(CreateRequest(url, HttpMethod.Get)).Result)
			{
				if (!result.IsSuccessStatusCode)
				{
					Assert.Fail("Server error. Url: " + url + ".\r\n" + result.ToString());
				}
				returnValue = result.Content.ReadAsAsync<T>().Result;
			}
			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T1"></typeparam>
		/// <param name="url"></param>
		/// <param name="httpMethod"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		protected R CallApi<T, R>(string url, HttpMethod httpMethod, T data)
			where T : class
			where R : class
		{
			R returnValue;
			using (var client = GetClient())
			using (var result = client.SendAsync(CreateRequest<T>(url, httpMethod, data)).Result)
			{
				if (!result.IsSuccessStatusCode)
					Assert.Fail("Server error. " + result.ToString());
				returnValue = result.Content.ReadAsAsync<R>().Result;
			}
			return returnValue;
		}


		public void Dispose()
		{
			if (server != null)
				server.Dispose();
		}

	}
}
