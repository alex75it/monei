using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using FakeItEasy;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Builder;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using Monei.MvcApplication.Api;
using NUnit.Framework;
using Owin;
using Should;


namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
	[TestFixture, Category("API"), Category("Subcategory")]
	public class SubcategorApiyControllerTest :ApiControllerTestBase
	{
		private const string ROUTE_PREFIX = "api/subcategory/";

		[Test]
		public void Ping_Should_RespondOk()
		{
			using (var client = GetClient())
			using (var request = CreateRequest(ROUTE_PREFIX + "ping", HttpMethod.Get))
			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldBeTrue();
			}
		}

		[Test]
		public void SearchWithUnexistentCategory_Should_ReturnAEmptyList()
		{
			//Arrange
			const string url = ROUTE_PREFIX + "category/" + "-666";

			// Act
			var returnedList = CallApi<IEnumerable<Subcategory>>(url, HttpMethod.Get);
							
			// Assert
			returnedList.ShouldNotBeNull("fail for url \"" + url +"\"");
			returnedList.ShouldBeEmpty();
		}

		[Test]
		public void Search_Should_ReturnAList()
		{
			//Arrange
			var categoryId = new CategoryRepository().List().First().Id;
			string url = ROUTE_PREFIX + "category/" + categoryId;

			// Act
			var returnedList = CallApi<IEnumerable<Subcategory>>(url, HttpMethod.Get);

			// Assert
			returnedList.ShouldNotBeNull();
			returnedList.ShouldNotBeEmpty();
		}

		[Test]
		public void Create_Should_ReturnOk()
		{
			// Arrange
			Subcategory subcategory = new Subcategory()
			{
				Name = "Test " + RandomInt(),
				Description = "Description test"
			};

			//Act
			int newId = CallApi<Subcategory, int>(ROUTE_PREFIX, POST, subcategory);

			// Assert
			ISubcategoryRepository repository = new SubcategoryRepository();
			Subcategory loadedSubcategory = repository.Read(newId);

			Assert.IsNotNull(loadedSubcategory);
			//loadedSubcategory.Name.Sould
			Assert.AreEqual(subcategory.Name, loadedSubcategory.Name);
			Assert.AreEqual(subcategory.Description, loadedSubcategory.Description);
		}


	}
}
