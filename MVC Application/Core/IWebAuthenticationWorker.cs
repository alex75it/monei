using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Monei.MvcApplication.Core
{
    public interface IWebAuthenticationWorker
    {
        /// <param name="username"></param>
        /// <param name="persistCookie">If true create a cookie without expiration, otherwise it is a session one.</param>
        void SetAuthenticationCookie(string username, bool persistCookie);

        /// <summary>
        /// Return the Account associated to the API token stored in the request.
        /// </summary>
        /// <param name="request">The HTTP request</param>
        Account GetAccount(HttpRequestMessage request);
    }
}
