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

    public class AccountSecurity : IAccountSecurity
    {
        private ILog logger;
        private IAccountRepository accountRepository;
        private IApiTokenRepository apiTokenRepository;

        public AccountSecurity(IAccountRepository accountRepository, IApiTokenRepository apiTokenRepository)
        {
            this.accountRepository = accountRepository;
            this.apiTokenRepository = apiTokenRepository;

            logger = LogManager.GetLogger(this.GetType().Name);            
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

                ApiToken newToken = GenerateNewToken(accountId);

                apiTokenRepository.Create(newToken);

                return newToken.Id;
            }                    
        }

        private ApiToken GenerateNewToken(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}