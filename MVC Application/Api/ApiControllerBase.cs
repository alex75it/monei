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

        public ICurrencyRepository CurrencyRepository { get; set; }


        public SubcategoryManager SubcategoryManager { get; set; }

        public /*static*/ readonly ILog logger; // = LogManager.GetLogger(typeof(ApiControllerBase));

        private Account currentAccount;
        public Account CurrentAccount
        {
            get
            {
                if (currentAccount == null)
                {
                    if (!Request.Headers.Contains("account-guid"))
                        throw new Exception("Missing \"account-guid\" header");
                    Guid accountGuid = Guid.Parse(Request.Headers.GetValues("account-guid").First());

                    currentAccount = AccountRepository.Read(accountGuid);


                    //System.Security.Principal.IPrincipal principal = RequestContext.Principal;
                    //if(principal == null)
                    //	throw new Exception("Principal is null");
                    //if(principal.Identity.IsAuthenticated == false)
                    //	throw new Exception("Principal not authenticated");
                    ////string username = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                    //string username = principal.Identity.Name;
                    
                    //logger.InfoFormat("Thread.CurrentPrincipal: '{0}'.", username);
                    //currentAccount = AccountRepository.Read(username);
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