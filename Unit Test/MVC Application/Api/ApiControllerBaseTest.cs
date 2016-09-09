using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Monei.Test.UnitTest.MvcApplication.Api
{
    public abstract class ApiControllerBaseTest
    {

        protected HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("account-guid", Guid.Empty.ToString());
            return request;
        }
    }
}