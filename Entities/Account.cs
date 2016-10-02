using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monei.Entities
{
    public class Account :EntityBase<int>
    {
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
    
    }
}
