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

namespace Monei.MvcApplication.Api
{
    
    public class ApiControllerBase :ApiController
    {
        // injected properties
        public IAccountRepository AccountRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IRegistryRepository RegistryRepository { get; set; }
        public ISubcategoryRepository SubcategoryRepository { get; set; }

        public SubcategoryManager SubcategoryManager { get; set; }

        protected readonly ILog logger;
        private Account currentAccount;
        public Account CurrentAccount
        {
            get
            {
                if (currentAccount == null)
                {
                    if (User.Identity.IsAuthenticated)
                        currentAccount = AccountRepository.Read(User.Identity.Name);
                    else
                    {
                        logger.ErrorFormat("User is not authenticated");
                        throw new Exception("User is not authenticated");
                    }
                }
                return currentAccount;
            }
        }
                

        public ApiControllerBase()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

    }
}