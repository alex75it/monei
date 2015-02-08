using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.MvcApplication.Api.ResponseDataObjects;

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
		public LoginResult Login(LoginPostData data) {




			var account = AccountRepository.Read(data.Username);

			if (account == null)
			{
				return LoginResult.CreateUserNotFound();
			}

			if (account.Password.ToUpper() != data.Password.ToUpper())
			{
				return LoginResult.CreateWrogPassword();
			}

			return new LoginResult()
			{
				IsOk = true,
			};


			//Monei.MvcApplication.Helpers.WebSecurity.LoginResult result = new WebSecurity(accountRepository).Login(model.username, model.Password, persistCookie: model.RememberMe);
			//switch (result)
			//{
			//	case WebSecurity.LoginResult.Ok:
			//		return RedirectToLocal(returnUrl);

			//	case WebSecurity.LoginResult.UsernameNotFound:
			//		ModelState.AddModelError("", "The username provided was not found");	//l10n
			//		break;
			//	case WebSecurity.LoginResult.WrongPassword:
			//		ModelState.AddModelError("", "The password provided was incorrect");	//l10n
			//		break;

			//	default:
			//		throw new Exception("Unmanaged result: " + result);
			//}				

		}

	}
}