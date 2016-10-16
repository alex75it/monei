using System;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Entities
{
    [TestFixture]
    public class AccountTest
    {
        [Test]
        [TestCase(Account.AccountRole.Administrator, "EUR")]
        [TestCase(Account.AccountRole.User, "ZAR")]
        public void Create(Account.AccountRole role, string currencyCode)
        {
            string username = "username1";
            string password = "password1";
            Currency currency = new Currency() { Code = currencyCode, Id=1	};

            // Execute
            Account account = Account.Create(username, password, role, currency);

            // Veify
            account.Guid.ShouldNotEqual(Guid.Empty);
            account.Username.ShouldEqual(username);
            account.Password.ShouldEqual(password);
            account.Role.ShouldEqual(role);
            account.Currency.ShouldEqual(currency);			
        }
    }
}
