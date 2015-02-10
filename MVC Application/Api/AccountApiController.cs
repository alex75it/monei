using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.MvcApplication.Api.ResponseDataObjects;
using Monei.MvcApplication.Helpers;

namespace Monei.MvcApplication.Api
{
	[RoutePrefix("api/account")]
	public class AccountApiController :ApiControllerBase
	{

		public AccountApiController(IAccountRepository accountRepository)
		{
			AccountRepository = accountRepository;
		}

		[HttpGet, Route("ping")]
		public void Ping() {

			//return "pong";
		}

		[HttpPost, Route("login")]
		public LoginResult Login(LoginPostData data)
		{	
			WebSecurity.LoginResult result = new WebSecurity(AccountRepository).Login(data.Username, data.Password, persistCookie: data.RememberMe);

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