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
        public void New()
        {          
            NewApiTokenPostData data = new NewApiTokenPostData() { Username = "username", Password = "password" };
            Guid newToken = CallApi<NewApiTokenPostData, Guid>(BASE_URL + "/new", data);
        }
    }
}
