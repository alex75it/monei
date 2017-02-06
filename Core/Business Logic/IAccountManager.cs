using System;
using System.Linq;
using Monei.Entities;

namespace Monei.Core.BusinessLogic
{
    public interface IAccountManager
    {
        Account Read(string username);

        Account CreateAccount(string username, string password, Currency currency);
    }
}
