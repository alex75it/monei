using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Api.ResponseDataObjects
{
	public class LoginResult
	{
		public const int ERROR_USER_NOT_FOUND = 10;
		public const int ERROR_WRONG_PASSWORD = 20;


		public bool IsOk { get; set; }

		public int ErrorCode { get; set; }

		public LoginResult() {
			IsOk = false;
		}

		public static LoginResult CreateUserNotFound()
		{
			return new LoginResult() {
				IsOk = false,
				ErrorCode = ERROR_USER_NOT_FOUND,
			};
		}


		public static LoginResult CreateWrogPassword()
		{
			return new LoginResult()
			{
				IsOk = false,
				ErrorCode = ERROR_WRONG_PASSWORD,
			};
		}
	}
}