using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Monei.DataAccessLayer.SqlServer 
{
    public class AccountRepository :RepositoryBase<int, Account>, IAccountRepository
    {
        public AccountRepository(ISessionFactoryProvider sessionFactoryProvider) : base(sessionFactoryProvider)
        {
        }

        public Account Read(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            using (ISession session = OpenSession())
            {
                Account account = session.Query<Account>()
                    .Where(a => a.Username.ToLowerInvariant() == username.ToLowerInvariant())
                    .FirstOrDefault();

                return account;
            }
        }

        public Account Read(Guid accountGuid)
        {
            if (accountGuid == Guid.Empty)
                throw new ArgumentException(nameof(accountGuid));

            using (ISession session = OpenSession())
            {
                Account account = session.Query<Account>()
                    .Where(a => a.Guid == accountGuid)
                    .FirstOrDefault();

                return account;
            }
        }

        public Account Create(string username, string password, Account.AccountRole role, Currency currency)
        {
            Account account = new Account()
            {
                Username = username,
                Password = password,
                Role = Account.AccountRole.User,
                Currency = currency,
                CreationDate = DateTime.UtcNow,
                //CreationAccount = 
                LastChangeDate = null,
                LastUpdateAccount = null,
                LastLogin = null,
            };

            return Create(account);
        }
        
        public void UpdateLastLogin(int accountId, DateTime date)
        {
            using (ISession session = OpenSession())
            {
                Account account = Read(accountId);
                account.LastLogin = date;
                Update(account);
            }
        }

        public new Account Create(Account account)
        {
            if (Read(account.Username) != null)
                throw new EntityAlreadyExistsException("username");

            int accountId = base.Create(account);
            return Read(accountId);
        }        

        public new Account Update(Account account)
        {
            base.Update(account);
            return Read(account.Id);
        }        

        public IList<Account> ListAll()
        {
            using (ISession session = OpenSession())
            {
                return session.Query<Account>().ToList();
            }
        }

        public Guid GetAccountIdByApiToken(Guid apiToken)
        {
            // temporary
            return apiToken;
        }
    }
}
