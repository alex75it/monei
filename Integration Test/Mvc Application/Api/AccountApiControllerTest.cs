using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.MvcApplication.Api.PostDataObjects;
using Should;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
	[TestClass]
	public class AccountApiControllerTest :ApiControllerTestBase
	{

		[TestMethod]
		public void Ping_Should_RespondOk()
		{
			var client = new HttpClient(server);

			using(var request = CreateRequest("api/account/ping", HttpMethod.Get))
			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldBeTrue();
			}			
		}

		[TestMethod]
		public void Login_Should_ReturnResult()
		{

			// ref: http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/
			// this one promise to use ALL the pipeline:
			//– submitting a request
			//– a message handler
			//– custom action filter attribute
			//– controller action
			//– JSON formatter applied to the incoming response
			//– receiving the response
						

			var client = new HttpClient(base.server);

			var data = new LoginPostData() { 
				Username = "pippo",
				Password = "pluto"
			};
			var request = CreateRequest<LoginPostData>("api/account/login", HttpMethod.Post, data);

			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldEqual(true);
				response.Content.ShouldNotBeNull();
				response.Content.ShouldNotBeType<HttpError>();
				//response.Co
				response.IsSuccessStatusCode.ShouldEqual(true);
			}

			request.Dispose();
		}

	}
}
