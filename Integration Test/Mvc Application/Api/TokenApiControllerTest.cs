using Monei.MvcApplication.Api;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.Test.IntegrationTest.MvcApplication.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Should;
using System.Net;
using Monei.Entities;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
    [TestFixture, Category("Web API"), Category("Security")]
    internal class TokenApiControllerTest : ApiControllerTestBase<TokenApiController>
    {
        private const string BASE_URL = "api/token";

        [Test]
        public void Ping()
        {
            string response = CallApi<string>(BASE_URL + "/ping", HttpMethod.Get);
            response.ShouldEqual("pong");
        }

        [Test]
        public void New_when_CredentialsAreWrong_should_ReturnAnError()
        {          
            NewApiTokenPostData data = new NewApiTokenPostData() { Username = "username", Password = "password" };
            CallApiExpectingError<NewApiTokenPostData>(BASE_URL + "/new", data, HttpStatusCode.BadRequest);
        }

        [Test]
        public void New_should_ReturnAGuid()
        {
            Account account = testDataProvider.GetTestAccount();
            Guid newToken = default(Guid);
            string username = account.Username;
            string password = account.Password;

            NewApiTokenPostData data = new NewApiTokenPostData() { Username = username, Password = password };
            newToken = CallApi<NewApiTokenPostData, Guid>(BASE_URL + "/new", data);

            newToken.ShouldNotEqual(default(Guid));
        }
    }
}
