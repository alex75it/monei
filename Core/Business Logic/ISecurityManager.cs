using Monei.Core.Enums;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Core.BusinessLogic
{
    public interface ISecurityManager
    {
        LoginResult Login(string username, string password);

        Account GetAccountByUsername(string username);

        /// <summary>
        /// Return the API token assigned to the Account.
        /// Use an existent one or create a new one if not exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Guid GetApiTokenForAccount(int id);

        Account GetAccountByApiToken(Guid apiToken);
    }
}
