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

        public Account Account { get; private set; }
        
        public BaseViewPage()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        protected override void InitializePage()
        {
            base.InitializePage();

            Account = Session["Account"] as Account;

            // Session can be expired
            if (Account == null && User.Identity.IsAuthenticated)
            {
                logger.WarnFormat("Session Account is null but User is authenticated");
                //MvcApplication application = (MvcApplication)HttpContext.Current.ApplicationInstance;
                IAccountManager accountManager = MvcApplication.DependencyInjectionManager.Resolve<IAccountManager>();
                Account = accountManager.Read(User.Identity.Name);
                Session["Account"] = Account;
            }
        }

        public override void Execute()
        {
            logger.ErrorFormat("Call to Execute() not implemented");
            throw new NotImplementedException();
        }		
    }
}