using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using log4net;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using Monei.MvcApplication.Core;

namespace Monei.MvcApplication.Core
{
    public class WebSecurity 
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(WebSecurity));
        private readonly IAccountRepository accountRepository;
        private readonly IAuthenticationWorker webAuthenticationWorker;


        public enum LoginResult
        {
            Ok = 0, 
            UsernameNotFound = 10, 
            WrongPassword = 20,
            InternalError = 30,
        }

        public WebSecurity(IAccountRepository accountRepository, IAuthenticationWorker webAuthenticationWorker)
        {
            this.accountRepository = accountRepository;
            this.webAuthenticationWorker = webAuthenticationWorker;
        }		

        public LoginResult Login(string username, string password, bool persistCookie = false)
        {
            try
            {
                Account account = accountRepository.Read(username);

                if (account == null)
                {
                    logger.InfoFormat("Login failed, username not found (\"{0}\").", username);
                    return LoginResult.UsernameNotFound;
                }

                if (account.Password != password)
                {
                    logger.InfoFormat("Login failed, wrong password (\"{0}\").", password);
                    return LoginResult.WrongPassword;
                }

                account.LastLogin = DateTime.UtcNow;
                accountRepository.UpdateLastLogin(account.Id, DateTime.UtcNow);

                logger.InfoFormat("Login success. username: \"{0}\".", username);

                // store authentication cookie
                webAuthenticationWorker.SetAuthenticationCookie(username, persistCookie);

                return LoginResult.Ok;
            }
            catch (Exception exc)
            {
                logger.FatalFormat("Login fail. Error: {0}", exc);
                return LoginResult.InternalError;
            }
        }

        public static void Logout()
        { 
            // todo: register action in database
            FormsAuthentication.SignOut();
        }        
    }
}