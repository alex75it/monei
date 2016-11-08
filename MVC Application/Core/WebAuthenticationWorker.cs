using Monei.Core.BusinessLogic;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Security;

namespace Monei.MvcApplication.Core
{
    public class WebAuthenticationWorker : IWebAuthenticationWorker
    {
        public const string API_TOKEN = "api_token";

        public IAccountManager AccountManager { get; set; } // injected
        public IAccountSecurity AccountSecurity { get; set; } // injected

        public void SetAuthenticationCookie(string username, bool persistCookie)
        {
            FormsAuthentication.SetAuthCookie(username, persistCookie);
        }


        public Account GetAccount(HttpRequestMessage request)
        {
            Guid apiToken = GetApiToken(request);

            //AccountSecurity.GET
            return AccountManager.GetAccountByApiToken(apiToken);
        }

        public Guid GetApiToken(HttpRequestMessage request)
        {
            if (!request.Headers.Contains(API_TOKEN))
            {
                throw new Exception("Missing API token header");
            }

            Guid apiToken = Guid.Parse(request.Headers.GetValues(API_TOKEN).First());

            return apiToken;
        }
    }
}
