using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Monei.DataAccessLayer.MongoDB
{
    public class AccountRepository : MongoDBRepositoryBase<Account>, IAccountRepository
    {
		private const string COLLECTION_NAME = "Account";

		public static string CollectionName { get { return COLLECTION_NAME; } }


		private static IDictionary<int, Account> accounts = new Dictionary<int, Account>();

		private static AccountRepository instance = new AccountRepository();

		public static AccountRepository Instance { get {
			return instance;
		} }

		public AccountRepository()
		{
			if (accounts.Count == 0)
			{ 
				accounts.Add(1, new Account() { Username="Alex", Password="******"});
				accounts.Add(2, new Account() { Username="Mario", Password="******"});
				accounts.Add(3, new Account() { Username = "Peppe", Password = "******" });
			}
		}

		public Account Read(int userId)
		{ 
			// todo
			return new Account() { Username="Alex", Password="******"  };
		}

		public Account Read(string username)
		{
			Account account = GetCollection().FindOne( Query.Matches(Account.FIELD_USERNAME, username)); 

			//return new Account() { username = "Alex", Password = "aaa" };
			return account;
		}

		public Account Read(Guid accountGuid)
		{
			Account account = GetCollection().FindOne(Query.EQ(Account.FIELD_GUID, accountGuid.ToString()));

			//return new Account() { username = "Alex", Password = "aaa" };
			return account;
		}

		public Account Create(string username, string password, Account.AccountRole role, Currency currency)
		{
			Account account = Account.Create(username, password, role, currency);

			account.CreationDate = DateTime.UtcNow;
			account.CreationAccount = null;

			GetCollection().Save(account);

			return account;
		}


		public void UpdateLastLogin(int id, DateTime lastLogin) {
			GetCollection().Update( 
				Query.EQ(CollectionName, id),
				//Update<Account>.Set(Account.FIELD_LAST_LOGIN, lastLogin),
				global::MongoDB.Driver.Builders.Update.Set(Account.FIELD_LAST_LOGIN, lastLogin),
				UpdateFlags.Upsert
				);
		}




		public void Delete(int id)
		{
			throw new NotImplementedException();
		}


		public Account Create(Account account)
		{
			throw new NotImplementedException();
		}



		public Account Update(Account account)
		{
			throw new NotImplementedException();
		}


		public IList<Account> ListAll()
		{
			return GetCollection().Find(Query.Null).ToList();
		}


	}//class
}
