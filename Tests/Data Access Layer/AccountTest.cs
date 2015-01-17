using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;

namespace Monei.Tests.DataAccessLayer
{
	[TestClass]
	public class AccountTest
	{
		public TestHelper TestHelper { get; set; }
		public IAccountRepository AccountRepository { get; set; }



		[TestMethod]
		public void Login()
		{
			//string username = "username_test";
			//string password = "password_test";

			//accountRepository.CreateAccount(username, password);

			//Assert.IsTrue(true);
		}

	}//class
}
