using System;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;

namespace Monei.Core.BusinessLogic
{
    public class AccountManager : IAccountManager
    {
        private readonly IAccountRepository accountRepository;

        public AccountManager(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        
        public Account Read(string username)
        {
            return accountRepository.Read(username);
        }

        public Account CreateAccount(string username, string password, Currency currency)
        {
            Account account = accountRepository.Read(username.ToLowerInvariant());
            if (account != null)
                throw new Exception("Another account with this username already exists");

            account = Account.Create(username, password, Account.AccountRole.User, currency);
            
            account = accountRepository.Create(account);

            return account;
        }
    }
}
