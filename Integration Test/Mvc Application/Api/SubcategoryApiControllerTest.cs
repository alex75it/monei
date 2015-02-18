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
			const string url = ROUTE_PREFIX + "category/" + "-666";
			var returnedList = CallApi<IEnumerable<Subcategory>>(url, HttpMethod.Get);
							
			// Verify
			returnedList.ShouldNotBeNull("fail for url \"" + url +"\"");
			returnedList.ShouldBeEmpty();
		}

		[Test]
		public void Search_Should_ReturnAList()
		{
			//Arrange
			var categoryId = new CategoryRepository().List().First().Id;

			// Act
			string url = ROUTE_PREFIX + "category/" + categoryId;
			var returnedList = CallApi<IEnumerable<Subcategory>>(url, HttpMethod.Get);

			// Assert
			returnedList.ShouldNotBeNull();
			returnedList.ShouldNotBeEmpty();
		}
		
		
	}
}
