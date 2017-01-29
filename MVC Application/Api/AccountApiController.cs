using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Monei.MvcApplication.Api.ResponseDataObjects;
using Monei.MvcApplication.Core;
using Monei.MvcApplication.Helpers;
using Monei.MvcApplication.Api.PostDataObjects;

namespace Monei.MvcApplication.Api
{
    [RoutePrefix("api/account")]
    public class AccountApiController :ApiControllerBase
    {
        [HttpGet, Route("ping")]
        public string Ping() {

            var response = "pong";
            return response;
        }

        [HttpPost, Route("login")]
        public LoginResult Login(LoginPostData data)
        {	            
            WebSecurity.LoginResult result = new WebSecurity(AccountRepository, AuthenticationWorker).Login(data.Username, data.Password, persistCookie: data.RememberMe);

            switch (result)
            {
                case WebSecurity.LoginResult.Ok:
                    return LoginResult.Ok;

                case WebSecurity.LoginResult.UsernameNotFound:
                    return LoginResult.UsernameNotFound;
            
                case WebSecurity.LoginResult.WrongPassword:
                    return LoginResult.WrongPassword;			

                default:
                    throw new Exception("Unmanaged result: " + result);
            }				

        }

    }
}