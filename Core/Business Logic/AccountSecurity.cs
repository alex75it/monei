using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Monei.Core.BusinessLogic
{
    using DataAccessLayer.Interfaces;
    using Entities;
    using Enums;
    using Exceptions;

    public interface IAccountSecurity
    {
        LoginResult Login(string username, string password);
        void ChangePassword(string name, string oldPassword, string newPassword);
        string GeneratePassword();

        /// <summary>
        /// Return the API token assigned to the Account.
        /// Use an existent one or create a new one if not exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Guid GetApiTokenForAccount(int id);
    }

    public class AccountSecurity : IAccountSecurity
    {
        private ILog logger;
        private IAccountRepository accountRepository;
        private IApiTokenRepository apiTokenRepository;

        public AccountSecurity(IAccountRepository accountRepository)
        {
            logger = LogManager.GetLogger(this.GetType().Name);
            this.accountRepository = accountRepository;
        }

        public LoginResult Login(string username, string password)
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

        public Guid GetApiTokenForAccount(int accountId)
        {
            ApiToken token = apiTokenRepository.GetAccountToken(accountId);

            if (token != null && !token.IsExpired)
            {
                return token.Id;                
            }
            else
            {
                if (token != null && token.IsExpired)                
                    apiTokenRepository.Delete(token.Id);

                return apiTokenRepository.CreateNewToken(accountId);                
            }                    
        }
    }
}