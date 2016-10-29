using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Monei.Core.BusinessLogic
{
    using DataAccessLayer.Interfaces;
    using Entities;
    using Enums;
        
    public interface IAccountSecurity
    {
        LoginResult Login(string username, string password, bool caseSensitive = false);
        void ChangePassword(string name, string oldPassword, string newPassword);
        string GeneratePassword();
    }

    public class AccountSecurity : IAccountSecurity
    {
        private ILog logger;
        private IAccountRepository accountRepository;

        public AccountSecurity(IAccountRepository accountRepository)
        {
            logger = LogManager.GetLogger(this.GetType().Name);
            this.accountRepository = accountRepository;
        }

        public LoginResult Login(string username, string password, bool caseSensitive = false)
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

                accountRepository.UpdateLastLogin(account.Id, DateTime.UtcNow);

                return LoginResult.Ok;
            }
            catch (Exception exc)
            {
                logger.FatalFormat("Login fail. Error: {0}", exc);
                return LoginResult.InternalError;
            }
        }
        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public string GeneratePassword()
        {
            return Guid.NewGuid().ToString().Substring(0,8);
        }
    }
}