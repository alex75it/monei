using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Monei.Entities;
using Monei.Core.BusinessLogic;
using System.Web.Routing;

namespace Monei.MvcApplication.Controllers
{
    public abstract class MoneiControllerBase : Controller, IController
    {
        public const string SESSION_RETURN_URL = "returnUrl";
        public const int UNDEFINED_ID = -1;
        protected ILog logger;
        private ISecurityManager securityManager;
        private Account account = null;

        public MoneiControllerBase(ISecurityManager securityManager)
        {
            logger = LogManager.GetLogger(this.GetType());
            this.securityManager = securityManager;
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewBag.ApiToken = securityManager.GetApiTokenForAccount(GetAccount().Id);
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Account of current logged user 
        /// </summary>
        /// <returns></returns>
        protected Account GetAccount() {
            if (account != null)
                return account;

            if (User.Identity.IsAuthenticated)
            {
                account = securityManager.GetAccountByUsername(User.Identity.Name);
                return account;
            }
            else
            {
                logger.ErrorFormat("User is not authenticated");
                throw new Exception("User is not authenticated");
            }
        }

        protected void SetReturnUrl(string returnUrl)
        {
            ClearReturnUrl();
            
            if (!string.IsNullOrWhiteSpace(returnUrl))				
                Session.Add(SESSION_RETURN_URL, returnUrl);			
        }

        protected void ClearReturnUrl()
        {
            Session.Remove(SESSION_RETURN_URL);
        }

        protected string GetReturnUrl()
        {
            return Session[SESSION_RETURN_URL] as string;
        }

        protected bool IsAuthorized(params Monei.Entities.Account.AccountRole[] roles)
        {
            return roles.Contains(GetAccount().Role);
        }

        protected void SetErrorMessage(string message)
        {
            ViewBag.ErrorMessage = message;
        }

        protected void SetSuccessMessage(string message)
        {
            ViewBag.SuccessMessage = message;
        }

    }
}
