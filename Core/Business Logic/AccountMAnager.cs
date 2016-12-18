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

        public AccountManager(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
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
            Guid accountId = accountRepository.GetAccountIdByApiToken(apiToken);

            return accountRepository.Read(accountId);
        }

    }
}
