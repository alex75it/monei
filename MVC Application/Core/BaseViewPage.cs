using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;

namespace Monei.MvcApplication
{
    public class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        private ILog logger = LogManager.GetLogger(typeof(BaseViewPage<TModel>));

        public Account Account { get; private set; }

        public BaseViewPage()
        {

        }

        protected override void InitializePage()
        {
            base.InitializePage();

            Account = Session["Account"] as Account;

            // Session can be expired
            if (Account == null && User.Identity.IsAuthenticated)
            {
                logger.WarnFormat("Session aCcount is null but User is authenticated");
                MvcApplication application = (MvcApplication)HttpContext.Current.ApplicationInstance;
                IAccountRepository accountRepository = application.WindsorContainer.Resolve<IAccountRepository>();
                Account = accountRepository.Read(User.Identity.Name);
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