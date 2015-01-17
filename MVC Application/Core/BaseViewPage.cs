using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.MongoDB;
using Monei.Entities;

namespace Monei.MvcApplication
{
	public class BaseViewPage<TModel> : WebViewPage<TModel>
	{
		private ILog logger = LogManager.GetLogger(typeof(BaseViewPage<TModel>));
		//private readonly IAccountRepository accountRepository;
		public IAccountRepository AccountRepository { get; set; }

		public Account Account { get; private set; }

		//public BaseViewPage(IAccountRepository accountRepository)
		//{
		//	this.accountRepository = accountRepository;
		//}

		public BaseViewPage()
		{
			MvcApplication application = HttpContext.Current.ApplicationInstance as MvcApplication;

			AccountRepository = application.WindSorContainer.Resolve<IAccountRepository>();
		}


		protected override void InitializePage()
		{
			//logger.InfoFormat("InitializePage");
			//logger.DebugFormat("{0} InitializePage {1}", this.ToString(), Request.Path);
			base.InitializePage();

			Account = GetAccount();
		}

		public bool HasRole(params Account.AccountRole[] roles)
		{
			//logger.DebugFormat("{0} HasRole {1}", this.ToString(), Request.Path);
			if (Account == null)
				return false;
			return roles.Contains(Account.Role);
		}

		private Account GetAccount()
		{
			Account account = null;

			if (User.Identity.IsAuthenticated)
				account = AccountRepository.Read(User.Identity.Name);

			return account;
		}



		//public new CustomPrincipal User
		//{
		//	get
		//	{
		//		return base.User as CustomPrincipal;
		//	}
		//}

		public override void Execute()
		{
			//logger.InfoFormat("Execute");
		}

		
	}
}