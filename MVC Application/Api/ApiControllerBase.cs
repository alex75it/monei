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
        public IAccountRepository AccountRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IRegistryRepository RegistryRepository { get; set; }
        public ISubcategoryRepository SubcategoryRepository { get; set; }

        public SubcategoryManager SubcategoryManager { get; set; }

        public const string ACCOUNT_HEADER = "account-guid";

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
                    if (!Request.Headers.Contains("account-guid"))
                        throw new Exception("Missing \""+ ACCOUNT_HEADER + "\" header");
                    Guid accountGuid = Guid.Parse(Request.Headers.GetValues(ACCOUNT_HEADER).First());

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
    }
}