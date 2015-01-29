using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
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
	public class CategoryControllerTest
	{

		private HttpClient client;

		[TestInitialize]
		public void TestInitialize()
		{
			client = new HttpClient();
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
		public void Get_Should_SerializeData()
		{
			// Arrange
			string testAddress = "http://localhost:9000/";
			//HttpClient client = new HttpClient();

			
			//HttpSelfHostConfiguration
			HttpConfiguration configuration = new HttpConfiguration();
			//configuration.Routes.MapHttpRoute("route name", "api/category/list");

			//configuration.MessageHandlers.Add();

			IAppBuilder appBuilder = new AppBuilderFactory().Create();
			appBuilder.UseWebApi(configuration);

			using (WebApp.Start<Startup>(url:testAddress))
			{
				HttpClient client = new HttpClient();

				var response = client.GetAsync(testAddress + "api/category/list").Result;

				Console.WriteLine("end");
				Console.WriteLine(response.Content.ReadAsStringAsync().Result); 
			}

			Console.ReadLine();
		}

		public class Startup
		{
			// This code configures Web API. The Startup class is specified as a type
			// parameter in the WebApp.Start method.
			public void Configuration(IAppBuilder appBuilder)
			{
				// Configure Web API for self-host. 
				HttpConfiguration config = new HttpConfiguration();
				config.Routes.MapHttpRoute(
					name: "DefaultApi",
					routeTemplate: "api/{controller}/{id}",
					defaults: new { id = RouteParameter.Optional }
				);

				appBuilder.UseWebApi(config);
			} 	


		}

	}
}
