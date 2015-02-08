using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using FakeItEasy;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Api;
using Owin;
using Should;


namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
	[TestClass]
	public class CategoryControllerTest :ApiControllerTestBase
	{

		private HttpClient client;

		[TestInitialize]
		public void TestInitialize()
		{
			client = new HttpClient();
		}


		[TestMethod]
		public void Ping_Should_RespondOk()
		{
			var client = new HttpClient(server);

			using (var request = CreateRequest("api/category/ping", HttpMethod.Get))
			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldBeTrue();
			}
		}

		[TestMethod, TestCategory("Web")]
		public void Get()
		{
			// Arrange
			CategoryController controller = new CategoryController();
			ICategoryRepository categoryRepository = A.Fake<ICategoryRepository>();

			IList<Category> data = new List<Category>() { };
			data.Add(new Category() { Id = 3, Name="AAA"});

			A.CallTo(() => categoryRepository.List()).Returns(data);

			controller.CategoryRepository = categoryRepository;

			// Act
			IEnumerable<Category> list = controller.Get();

			// Assert
			list.ShouldNotBeEmpty();
		}


		[TestMethod]
		public void AlexTest()
		{
			string baseAddress = "http://localhost:9000/";


			// create the in memory server
			HttpConfiguration configuration = GetConfiguration();
			configuration.Routes.MapHttpRoute("API test", routeTemplate: "api/category/list");
			HttpServer server = new HttpServer(configuration);

			HttpClient client = new HttpClient(server);	

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseAddress +  "api/category/list");



			HttpResponseMessage response = client.SendAsync(request).Result;

			response.IsSuccessStatusCode.ShouldBeTrue();


			// create the client
			//using(HttpMessageHandler handler = new)
  			{
				//HttpClient client = new HttpClient(handler);	
				

			}		

		}


		private HttpConfiguration GetConfiguration() {
			HttpConfiguration configuration = new HttpConfiguration();

			return configuration;
		}


		[TestMethod]
		public void Get_Should_SerializeData()
		{
			// Arrange
			string testAddress = "http://localhost:9000/";
			//HttpClient client = new HttpClient();

			
			//HttpSelfHostConfiguration
			HttpConfiguration configuration = new HttpConfiguration();
				
			configuration.Routes.MapHttpRoute("test", "api/category/list");		
			//configuration.Routes.MapHttpRoute(
			//		name: "DefaultApi",
			//		routeTemplate: "api/{controller}/{id}",
			//		defaults: new { id = RouteParameter.Optional }
			//	);

			//configuration.MessageHandlers.Add();

			IAppBuilder appBuilder = new AppBuilderFactory().Create();
			
			appBuilder.UseWebApi(configuration);



			HttpResponseMessage response;

			using (WebApp.Start<Startup>( url:testAddress))
			{
				HttpClient client = new HttpClient();
				response = client.GetAsync(testAddress + "api/category/list").Result;		
				Console.WriteLine(response.Content.ReadAsStringAsync().Result); 
			}

			// Assert
			response.IsSuccessStatusCode.ShouldBeTrue();

			//Console.ReadLine();
		}

		public class Startup
		{
			// This code configures Web API. The Startup class is specified as a type
			// parameter in the WebApp.Start method.
			public void Configuration(IAppBuilder appBuilder)
			{
				// Configure Web API for self-host. 
				HttpConfiguration config = new HttpConfiguration();
				//config.Routes.MapHttpRoute(
				//	name: "DefaultApi",
				//	routeTemplate: "api/{controller}/{id}",
				//	defaults: new { id = RouteParameter.Optional }
				//);

				//config.

				appBuilder.UseWebApi(config);
			} 	


		}

	}
}
