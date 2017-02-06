using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Monei.Core.BusinessLogic
{
    using DataAccessLayer.Interfaces;
    using Entities;
    using Enums;

    public class SecurityManager : ISecurityManager
    {
        private const int TokenLifeInMinutes = 120;

        private ILog logger;
        private IAccountRepository accountRepository;
        private IApiTokenRepository apiTokenRepository;

        public SecurityManager(IAccountRepository accountRepository, IApiTokenRepository apiTokenRepository)
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

        public Account GetAccountByUsername(string username)
        {
            return accountRepository.Read(username);
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

                ApiToken newToken = GenerateNewToken(accountId, TimeSpan.FromMinutes(TokenLifeInMinutes));

                apiTokenRepository.Create(newToken);

                return newToken.Id;
            }                    
        }

        private ApiToken GenerateNewToken(int accountId, TimeSpan duration)
        {
            return ApiToken.Create(accountId, duration);
        }

        public Account GetAccountByApiToken(Guid apiToken)
        {
            if (apiToken == Guid.Empty)
                throw new ArgumentException(nameof(apiToken));

            int accountId = apiTokenRepository.GetAccountId(apiToken);

            return accountRepository.Read(accountId);
        }
    }
}