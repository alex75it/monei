using System;
using System.Transactions;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using Monei.MvcApplication.Filters;
using Monei.MvcApplication.Models;
using System.Collections.Generic;
using Monei.DataAccessLayer;
using Monei.MvcApplication.Helpers;
using Monei.Entities;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Core;

namespace Monei.MvcApplication.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : MoneiControllerBase
    {

        private IAccountRepository accountRepository;
        private ICurrencyRepository currencyRepository;
        private IWebAuthenticationWorker webAuthenticationWorker;

        public AccountController(IAccountRepository accountRepository, ICurrencyRepository currencyRepository, IWebAuthenticationWorker webAuthenticationWorker)
        {
            this.accountRepository = accountRepository;
            this.currencyRepository = currencyRepository;
            this.webAuthenticationWorker = webAuthenticationWorker;
        }

        // GET: /Account/
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Monei.MvcApplication.Helpers.WebSecurity.LoginResult result = new WebSecurity(accountRepository, webAuthenticationWorker).Login(model.username, model.Password, persistCookie: model.RememberMe);
                switch (result)
                { 
                    case WebSecurity.LoginResult.Ok:
                        return RedirectToLocal(returnUrl);

                    case WebSecurity.LoginResult.UsernameNotFound:
                        ModelState.AddModelError("", "The username provided was not found");	//l10n
                        break;
                    case WebSecurity.LoginResult.WrongPassword:
                        ModelState.AddModelError("", "The password provided was incorrect");	//l10n
                        break;

                    default:
                        throw new Exception("Unmanaged result: " + result);
                }				
            }

            // If we got this far, somnething goes wrong, redisplay form
            return View(model);
        }


        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {	
            return View();
        }
        
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            logger.Debug("Register");

            if (ModelState.IsValid)
            {
                // attempt to register the user
                try
                {
                    WebSecurity ws = new WebSecurity(accountRepository, webAuthenticationWorker);
                    ws.CreateUserAndAccount(model.username, model.Password);
                    ws.Login(model.username, model.Password);

                    //http://msdn.microsoft.com/en-us/magazine/dn201748.aspx

                    //AccountRepository.CreateUserAndAccount(model.username, model.Password);
                    //AccountDal.Login(model.username, model.Password);

                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException exc)
                {
                    logger.ErrorFormat("Fail to register user. {0}", exc);
                    ModelState.AddModelError("", ErrorCodeToString(exc.StatusCode));
                }
            }

            // if we got this far, something goeas wrong, redisplay form
            return View(model);			
        }


        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate account if the current logged in user is the owner
            if(ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using(var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable}))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                    
                }
            }

            return RedirectToAction("Manage", new { Message = message});
        }

        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        { 
            ViewBag.StatusMessage = 
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                    : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                        : "";

            // to work MembershipProvider must extend WebMAtrix.WebData.ExtendedMembershipProvider
            //ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = true; // todo: what info is this???

            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            // todo use a MembershipProvider that extends ExtendedMembershipProvider or make work in a different way

            //bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            bool hasLocalAccount = true;
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");

            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will thrown an exception rather than return false in ceertain failure scenario
                    bool changePasswordSuccedeed;
                    try
                    {
                        changePasswordSuccedeed = new WebSecurity(accountRepository, webAuthenticationWorker).ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSuccedeed = false;
                    }

                    if (changePasswordSuccedeed)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            { 
                // User does not have a local password so remove any validation errors caused by missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                    state.Errors.Clear();

                if (ModelState.IsValid)
                {
                    try
                    {
                        new WebSecurity(accountRepository, webAuthenticationWorker).CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account, An account with the name \"{0}\" may already exists.", User.Identity.Name));
                    }
                }
            }

            // If we got so far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl}));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
                return RedirectToAction("ExternalLoginFailure");

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie:false)) //todo: remember me?
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new Account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            { 
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { username = result.UserName, ExternalLoginData = loginData });
            }
        }

        // POST: /Accout/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                var account = accountRepository.Read(model.username.ToLowerInvariant());
           
                if (account == null)
                {
                    // todo: move to Coe.AccountManager class
                    account = new Account() {
                        CreationAccount = null,
                        Currency = currencyRepository.Read(Currency.EUR_CODE),
                        Username = model.username,                        
                    };

                    accountRepository.Create(account);
                    //context.UserProfiles.Add(new UserProfile { username = model.username });
                    //context.SaveChanges();

                    OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.username);
                    OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                    return RedirectToAction(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("username", "User name already exists, please enter a different user name.");
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            // todo: add a MembershipProvider that extends ExtendedMembershipProvider

            List<ExternalLogin> externalLogins = new List<ExternalLogin>();

            //ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromusername(User.Identity.Name);
            //foreach (var account in accounts)
            //{
            //	AuthenticationClientData data = OAuthWebSecurity.GetOAuthClientData(account.Provider);
            //	externalLogins.Add(new ExternalLogin
            //		{
            //			Provider = account.Provider,
            //			ProviderDisplayName = data.DisplayName,
            //			ProviderUserId = account.ProviderUserId,
            //		}
            //	);
            //}

            //ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));

            ViewBag.ShowRemoveButton = false;
            return PartialView("RemoveExternalLoginsPartial", externalLogins);
        }


        public ActionResult Index()
        {
            return View();
        }


        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }


        internal class ExternalLoginResult : ActionResult
        {
            public string Provider { get; set; }
            public string ReturnUrl { get; set; }
            

            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }

        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }


        #endregion




    }//class
}
