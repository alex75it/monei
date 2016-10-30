using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.Core.BusinessLogic;

namespace Monei.MvcApplication
{
    /// <summary>
    /// Base class for web pages. Set up in .config (system.web.webPages.razor).
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        private ILog logger;
        //private readonly IAccountManager accountManager;

        public Account Account { get; private set; }

        //public BaseViewPage(IAccountManager accountManager)
        //{
        //    logger = LogManager.GetLogger(this.GetType());
        //    this.accountManager = accountManager;
        //}

        public BaseViewPage()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        protected override void InitializePage()
        {
            base.InitializePage();
            Account = GetAccount();
        }

        //public bool HasRole(params Account.AccountRole[] roles)
        //{
        //    if (Account == null)
        //        return false;
        //    return roles.Contains(Account.Role);
        //}

        private Account GetAccount()
        {
            //Account account = null;

            //if (User.Identity.IsAuthenticated)
            //    account = accountManager.Read(User.Identity.Name);            

            Account account = Session["Account"] as Account;

            if (User.Identity.IsAuthenticated && account == null)
                logger.ErrorFormat("REquest authenticated but Account is null");

            return account;
        }

        public override void Execute()
        {
            logger.ErrorFormat("Call to \"Execute\" is not implemented");
            throw new NotImplementedException();
        }
    }
}