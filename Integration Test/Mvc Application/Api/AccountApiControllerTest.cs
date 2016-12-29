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
using Monei.MvcApplication.Api;
using System.Diagnostics;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
    [TestFixture, Category("Web API"), Category("Account")]
    public class AccountApiControllerTest :ApiControllerTestBase<AccountApiController>
    {
        [Test]
        public void Ping_Should_RespondOk()
        {
            using (var request = CreateRequest("api/account/ping", HttpMethod.Get))
            using (var response = GetClient().SendAsync(request).Result)
            {
                response.IsSuccessStatusCode.ShouldBeTrue(@"Status code is not ""Success"".");
            }
        }

        [Test]
        public void Login_Should_ReturnResult()
        {           
            string url = "api/account/login";
            var data = new LoginPostData() { 
                Username = "username",
                Password = "password"
            };

            // execute
            using (var request = CreateRequest<LoginPostData>(url, HttpMethod.Post, data))
            using (var response = GetClient().SendAsync(request).Result)
            {
                response.IsSuccessStatusCode.ShouldEqual(true, @"Status code is not ""Success"".");
                response.Content.ShouldNotBeNull();
                response.Content.ShouldNotBeType<HttpError>();
            }
        }

        [Test]
        public void Login_Should_ReturnUsernameNotFound()
        {
            /* if test fail the only way to find the error is to debug the controller, if the error is in there ... */

            string url = "api/account/login";
            var data = new LoginPostData()
            {
                Username = "username",
                Password = "password"
            };

            // Execute
            using (var request = CreateRequest<LoginPostData>(url, HttpMethod.Post, data))
            using (var response = GetClient().SendAsync(request).Result)
            {
                response.IsSuccessStatusCode.ShouldEqual(true, @"Status code is not ""Success"". " + response.ReasonPhrase);
                response.Content.ShouldNotBeNull();
                response.Content.ShouldNotBeType<HttpError>();

                LoginResult result = response.Content.ReadAsAsync<LoginResult>().Result;
                result.ShouldEqual(LoginResult.UsernameNotFound);
            }         
        }

        [Test]
        public void Login_Should_ReturnWrongPassword()
        {
            string url = "api/account/login";
            var data = new LoginPostData()
            {
                Username = "demo",
                Password = "wrong"
            };

            // Execute
            using (var request = CreateRequest<LoginPostData>(url, HttpMethod.Post, data))
            using (var response = GetClient().SendAsync(request).Result)
            {
                response.IsSuccessStatusCode.ShouldEqual(true, @"Status code is not ""Success"".");
                response.Content.ShouldNotBeNull("Content is null.");
                response.Content.ShouldNotBeType<HttpError>();

                LoginResult result = response.Content.ReadAsAsync<LoginResult>().Result;

                result.ShouldEqual(LoginResult.WrongPassword);
            }
        }

    }
}
