using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.MvcApplication.Api.ResponseDataObjects;
using Monei.MvcApplication.Core;
using Monei.MvcApplication.Helpers;

namespace Monei.MvcApplication.Api
{
    [RoutePrefix("api/account")]
    public class AccountApiController :ApiControllerBase
    {

        private readonly IWebAuthenticationWorker webAuthenticationWorker;

        public AccountApiController(IAccountRepository accountRepository, ICurrencyRepository currencyRepository, IWebAuthenticationWorker webAuthenticationWorker)
        {
            AccountRepository = accountRepository;
            CurrencyRepository = currencyRepository;
            this.webAuthenticationWorker = webAuthenticationWorker;
        }

        [HttpGet, Route("ping")]
        public void Ping() {

            //return "pong";
        }

        [HttpPost, Route("login")]
        public LoginResult Login(LoginPostData data)
        {	
            
            WebSecurity.LoginResult result = new WebSecurity(AccountRepository, webAuthenticationWorker).Login(data.Username, data.Password, persistCookie: data.RememberMe);

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