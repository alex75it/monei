using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using FakeItEasy;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Builder;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Api;
using NUnit.Framework;
using Owin;
using Should;


namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
	[TestFixture, Category("Web API"), Category("Category")]
	public class CategorApiControllerTest :ApiControllerTestBase
	{
		private readonly string routePrefix = "api/category/";

		[Test]
		public void Ping_Should_RespondOk()
		{
			using (var client = GetClient())
			using (var request = CreateRequest(routePrefix + "ping", HttpMethod.Get))
			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldBeTrue();
			}
			
		}

		[Test]
		public void List_Should_ReturnAList()
		{
			string url = routePrefix + "list";
			IEnumerable<Category> returnedList;


			returnedList = CallApi<IEnumerable<Category>>(url, HttpMethod.Get);
							
			// Verify
			returnedList.ShouldNotBeNull();
			returnedList.ShouldNotBeEmpty();
		}

		
		
	}
}
