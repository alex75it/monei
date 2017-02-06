using Monei.Core.BusinessLogic;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Security;
using System.Web.Routing;

namespace Monei.MvcApplication.Core
{
    public class AuthenticationWorker : IAuthenticationWorker
    {
        public const string API_TOKEN = "api_token";
        public ISecurityManager SecurityManager { get; set; } // injected

        public void SetAuthenticationCookie(string username, bool persistCookie)
        {
            FormsAuthentication.SetAuthCookie(username, persistCookie);
        }

        public Account GetAccount(HttpRequestMessage request)
        {
            Guid apiToken = GetApiToken(request);

            return SecurityManager.GetAccountByApiToken(apiToken);
        }

        public Guid GetApiToken(HttpRequestMessage request)
        {
            if (!request.Headers.Contains(API_TOKEN))
                throw new Exception("Missing API token header");

            string tokenString = request.Headers.GetValues(API_TOKEN).First();
            if (string.IsNullOrWhiteSpace(tokenString))
                throw new Exception("API token is empty");

            Guid apiToken = Guid.Parse(tokenString);

            return apiToken;
        }
    }
}
