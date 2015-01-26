using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Monei.Entities
{

	public class Account :EntityBase<int>
	{
        public const string FIELD_USERNAME = "Username";
		public const string FIELD_PASSWORD = "Password";
		public const string FIELD_GUID = "Guid";
		public const string FIELD_EMAIL = "Email";
		public const string FIELD_LAST_LOGIN = "LastLogin";

        public enum AccountRole {
            Administrator,
            User,
        }

		public string Username {get; set;}
		public string Password { get; set; }
		public Guid Guid { get; set; }
        public AccountRole Role { get; set; }
		public virtual Currency Currency { get; set; }
		public DateTime? LastLogin { get; set; }


        public static Account Create(string username, string password, AccountRole role, Currency currency)
        {
            Account account = new Account()
            {
				//Id = ObjectId.GenerateNewId(),
				Guid = Guid.NewGuid(),
				Username = username,
				Password = password,
                Role = AccountRole.User,
				Currency = currency,
				LastLogin = null,
				CreationDate = DateTime.UtcNow,
				LastChangeDate = null,					
            };

            return account;
        }

	
	}//class
}
