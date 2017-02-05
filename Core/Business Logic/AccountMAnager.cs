using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Core.BusinessLogic
{
    public class AccountManager : IAccountManager

    {
        private readonly IAccountRepository accountRepository;
        private readonly IApiTokenRepository apiTokenRepository;

        public AccountManager(IAccountRepository accountRepository, IApiTokenRepository apiTokenRepository)
        {
            this.accountRepository = accountRepository;
            this.apiTokenRepository = apiTokenRepository;
        }

        public Account CreateAccount(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Account Read(string username)
        {
            return accountRepository.Read(username);
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
