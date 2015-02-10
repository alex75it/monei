using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;

namespace Monei.MvcApplication.Controllers
{
	public class MoneiControllerBase : Controller, IController
	{

		public /*static*/ /*readonly*/ ILog logger; // = LogManager.GetLogger(typeof(MoneiControllerBase));

		public const string SESSION_RETURN_URL = "returnUrl";
		public const int UNDEFINED_ID = -1;

		public IAccountRepository AccountRepository { get; set; }

		//public Account Account { get {
		//	if (account != null)
		//		return account;
		//} }

		public MoneiControllerBase()
		{
			logger = LogManager.GetLogger(this.GetType());
		}

		private Account account = null;
		/// <summary>
		/// Account of current logged user 
		/// </summary>
		/// <returns></returns>
		protected Account GetAccount() {
			if (account != null)
				return account;

			if (User.Identity.IsAuthenticated)
				return AccountRepository.Read(User.Identity.Name);
			else
			{
				logger.ErrorFormat("User is not authenticated");
				throw new Exception("User is not authenticated");
			}
		}

		protected void SetReturnUrl(string returnUrl)
		{
			ClearReturnUrl();
			
			if (!string.IsNullOrWhiteSpace(returnUrl))				
				Session.Add(SESSION_RETURN_URL, returnUrl);			
		}

		protected void ClearReturnUrl()
		{
			Session.Remove(SESSION_RETURN_URL);
		}

		protected string GetReturnUrl()
		{
			return Session[SESSION_RETURN_URL] as string;
		}

		protected bool IsAuthorized(params Monei.Entities.Account.AccountRole[] roles)
		{
			return roles.Contains(GetAccount().Role);
		}

		protected void SetErrorMessage(string message)
		{
			ViewBag.ErrorMessage = message;
		}

		protected void SetSuccessMessage(string message)
		{
			ViewBag.SuccessMessage = message;
		}

		//protected bool IsAuthorized(Monei.Entities.Account.AccountRole role)
		//{
		//	return role == GetAccount().Role;
		//}


	}//class
}
