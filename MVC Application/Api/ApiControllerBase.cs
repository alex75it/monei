using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using log4net;
using Monei.Core;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Core;
using Monei.MvcApplication.Helpers;

namespace Monei.MvcApplication.Api
{

    public interface IAuthenticationModule {
        bool IsAuthenticated();
        string GEtUserName();
    }

    public class ApiControllerBase :ApiController
    {
        // injected properties
        public IAccountRepository AccountRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IRegistryRepository RegistryRepository { get; set; }
        public ISubcategoryRepository SubcategoryRepository { get; set; }
        public ICurrencyRepository CurrencyRepository { get; set; }


        public SubcategoryManager SubcategoryManager { get; set; }

        protected readonly ILog logger;
        private Account currentAccount;
        public Account CurrentAccount
        {
            get
            {
                if (currentAccount == null)
                {


                    // do not use the User.Identity. It is set by ASP cookie, so does not work with calls from outside the web site.
                    // and if you use it you are exposed to CSRF

                    //if (User.Identity.IsAuthenticated)
                    //{
                    //    logger.DebugFormat("User.Identity ({0}): {1}", User.Identity.AuthenticationType, User.Identity.Name);
                    //}
                    //else if (!Request.Headers.Contains("api token"))
                    //    throw new Exception("Missing \"api token\" header");

                    //Guid accountGuid = Guid.Parse(Request.Headers.GetValues("api token").First());

                    //currentAccount = AccountRepository.Read(accountGuid);


                    // todo: create a ApiTokenManager or use IWebAuthenticationWorker

                    if (User.Identity.IsAuthenticated)
                    {
                        logger.DebugFormat("User.Identity ({0}): {1}", User.Identity.AuthenticationType, User.Identity.Name);
                        currentAccount = AccountRepository.Read(User.Identity.Name);
                    }
                    else
                    {
                        logger.ErrorFormat("User is not authenticated");
                        throw new Exception("User is not authenticated");
                    }
                }
                return currentAccount;
            }
        }
                

        //public ApiControllerBase(IAuthenticationModule)
        //{
        //    logger = LogManager.GetLogger(this.GetType());
        //}

    }
}