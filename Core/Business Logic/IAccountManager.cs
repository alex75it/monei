using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Core.BusinessLogic
{
    public interface IAccountManager
    {
        Account Read(string username);
        Account CreateAccount(string username, string password);
        Account GetAccountByApiToken(Guid apiToken);
    }
}
