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
		public void Create()
		{
			string username = "test";
			string password = "password";
			Account.AccountRole role = Account.AccountRole.User;
			Currency currency = new Currency() { Code = "BTC", Id=1	};

			// Execute
			Account account = Account.Create(username, password, role, currency);

			// Veify
			account.Username.ShouldEqual(username);
			account.Password.ShouldEqual(password);
			account.Role.ShouldEqual(role);
			account.Currency.ShouldEqual(currency);

			account.Guid.ShouldNotEqual(Guid.Empty);
		}
	}
}
