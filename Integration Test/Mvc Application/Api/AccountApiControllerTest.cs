using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.MvcApplication.Api.ResponseDataObjects;
using Newtonsoft.Json;
using NUnit.Framework;
using Should;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
	[TestFixture, Category("API"), Category("Account")]
	public class AccountApiControllerTest :ApiControllerTestBase
	{

		[Test]
		public void Ping_Should_RespondOk()
		{
			using (var client = new HttpClient(InitializeServer()))
			using (var request = CreateRequest("api/account/ping", HttpMethod.Get))
			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldBeTrue();
			}
		}

		[Test]
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
						

			var client = new HttpClient(InitializeServer());

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
			}

			request.Dispose();
		}

		[Test]
		public void Login_Should_ReturnUsernameNotFound()
		{
			var client = new HttpClient(base.InitializeServer());

			var data = new LoginPostData()
			{
				Username = "none",
				Password = "pluto"
			};
			var request = CreateRequest<LoginPostData>("api/account/login", HttpMethod.Post, data);

			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldEqual(true);
				response.Content.ShouldNotBeNull();
				response.Content.ShouldNotBeType<HttpError>();

				LoginResult result = response.Content.ReadAsAsync<LoginResult>().Result;
				result.ShouldEqual(LoginResult.UsernameNotFound);
			}

			request.Dispose();
		}


		[Test]
		public void Login_Should_ReturnWrongPassword()
		{
			var client = new HttpClient(base.InitializeServer());

			var data = new LoginPostData()
			{
				Username = "demo",
				Password = "wrong"
			};
			var request = CreateRequest<LoginPostData>("api/account/login", HttpMethod.Post, data);

			using (var response = client.SendAsync(request).Result)
			{
				response.IsSuccessStatusCode.ShouldEqual(true);
				response.Content.ShouldNotBeNull();
				response.Content.ShouldNotBeType<HttpError>();

				LoginResult result = response.Content.ReadAsAsync<LoginResult>().Result;
				result.ShouldEqual(LoginResult.WrongPassword);
			}

			request.Dispose();
		}


	}
}
