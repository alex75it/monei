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

        private IAccountRepository accountRepository;

        public Account Account { get; private set; }

        public BaseViewPage()
        {
            MvcApplication application = HttpContext.Current.ApplicationInstance as MvcApplication;
            accountRepository = application.WindsorContainer.Resolve<IAccountRepository>();
        }

        protected override void InitializePage()
        {
            base.InitializePage();
            if (User.Identity.IsAuthenticated)
                Account = accountRepository.Read(User.Identity.Name);
        }

        public override void Execute()
        {
            logger.ErrorFormat("Call to Execute() not implemented");
            throw new NotImplementedException();
        }		
    }
}