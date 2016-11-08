﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using log4net;
using Monei.Core;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Core;

namespace Monei.MvcApplication.Api
{
    public class ApiControllerBase :ApiController
    {
        // injected properties
        // Inject properties permits to not override the constructor on every derived API controller
        public IWebAuthenticationWorker WebAuthenticationWorker { get; set; }
        public SubcategoryManager SubcategoryManager { get; set; }
        public IAccountRepository AccountRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IRegistryRepository RegistryRepository { get; set; }
        public ISubcategoryRepository SubcategoryRepository { get; set; }
        public ICurrencyRepository CurrencyRepository { get; set; }


        protected readonly ILog logger;
        private Account currentAccount;
        

        public ApiControllerBase()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        public Account CurrentAccount
        {
            get
            {
                if (currentAccount == null)
                {
                    currentAccount = WebAuthenticationWorker.GetAccount(Request);


                    // http://stackoverflow.com/questions/19793845/authenticating-asp-net-web-api?rq=1

                    // do not use the User.Identity. It is set by ASP using cookie, so does not work with calls from outside the web site.
                    // and if you use it you are exposed to CSRF

                    // (commented)  retrieve user from ASP Token 
                    //if (User.Identity.IsAuthenticated)
                    //{
                    //    logger.DebugFormat("User.Identity ({0}): {1}", User.Identity.AuthenticationType, User.Identity.Name);
                    //}
                    //else if (!Request.Headers.Contains("api token"))
                    //    throw new Exception("Missing \"api token\" header");
                                        

                    //if (User.Identity.IsAuthenticated)
                    //{
                    //    logger.DebugFormat("User.Identity ({0}): {1}", User.Identity.AuthenticationType, User.Identity.Name);
                    //    currentAccount = AccountRepository.Read(User.Identity.Name);
                    //}
                    //else
                    //{
                    //    logger.ErrorFormat("User is not authenticated");
                    //    throw new Exception("User is not authenticated");
                    //}
                }
                return currentAccount;
            }
        } 

    }
}