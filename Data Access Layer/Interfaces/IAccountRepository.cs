using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;

namespace Monei.DataAccessLayer.Interfaces
{
    public interface IAccountRepository
    {
        Account Create(string username, string password, Account.AccountRole role, Currency currency);
        Account Create(Account account);
        Account Update(Account account);
        Account Read(string username);
        Account Read(int id);
        Account Read(Guid accountGuid);
        void Delete(int id);

        void UpdateLastLogin(int accountId, DateTime date);
        IList<Account> ListAll();        
    }
}
