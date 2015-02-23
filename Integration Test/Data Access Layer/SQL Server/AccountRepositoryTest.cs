using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using NUnit.Framework;
using Should;



namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
	[TestFixture]
	public class AccountRepositoryTest : RepositoryTestBase
	{


		[TestFixtureSetUp]
		public void ClassInitialize()
		{
			//Helper.RemoveTestAccount();
		}

		[SetUp]
		public void TestInizialize()
		{
			try
			{
				Helper.RemoveTestAccount();
			}
			catch (Exception exc)
			{
				Assert.Inconclusive("FAIL to delete Account test. " + exc.Message);
			}
		}

		[TearDown]
		public void TestCleanup()
		{
			Helper.RemoveTestAccount();
		}



		[Test]
		public void CreateUserAndAccount()
		{
			//todo: 6s for run this test?
			//todo: this test fail (the creation date day is wrong) when executed between 24:00 and 01:00

			string username = Helper.TEST_USERNAME;
			string password = "password_test";
			Account.AccountRole role = Account.AccountRole.User;
			Currency currency = Helper.GetEuroCurrency();

			Account account = AccountRepository.Create(username, password, role, currency);

			Assert.IsTrue(true);
			Assert.AreEqual(username, account.Username, "Username");
			Assert.AreEqual(password, account.Password);
			Assert.AreEqual(role, account.Role);
			Assert.AreEqual(currency.Id, account.Currency.Id);
			Assert.AreEqual(DateTime.Today, account.CreationDate.Date, "CreationDate");

		}


		[Test]
		public void Create()
		{
			string username = "TEST";
			string password = "test";
			Account.AccountRole role = Account.AccountRole.User ;
			Currency currency = Helper.GetEuroCurrency();

			Account account = Account.Create(username, password, role, currency); 
				

			account = AccountRepository.Create(account);


			Account loadAccount = AccountRepository.Read(account.Username);

			Assert.AreEqual(username, loadAccount.Username);

			// username exists

			Account account_2 = Account.Create(username, password, role, currency);
						

			//Action action = a => AccountRepository.Create(a);
			//Action<Account> action = a => AccountRepository.Create(a);
			//Should.ActionAssertionExtensions
			//action.ShouldThrow();

			try
			{
				AccountRepository.Create(account_2);
				Assert.Fail("Duplicate username haven't tot be permitted.");
			}
			catch(EntityAlreadyExistsException)
			{
				// expected
			}


			// clean 
			AccountRepository.Delete(account.Id);
		}


		[Test]
		public void Read()
		{
			string username = "Test";
			string password = "test";
			Account.AccountRole role = Account.AccountRole.User;
			Currency currency = Helper.GetEuroCurrency();

			Account account = Account.Create(username, password, role, currency);

			account = AccountRepository.Create(account);

			Account loadedAccount = AccountRepository.Read(account.Id);
			loadedAccount = AccountRepository.Read(account.Username);

			Assert.AreEqual(username, loadedAccount.Username);
			Assert.AreEqual(password, loadedAccount.Password);
			Assert.AreEqual(role, loadedAccount.Role);
			Assert.AreEqual(currency.Id, loadedAccount.Currency.Id);
			
			// clean 
			AccountRepository.Delete(account.Id);
		}

		[Test]
		public void Update()
		{

			string username = "Test";
			string password = "test";
			Account.AccountRole role = Account.AccountRole.User;
			Currency currency = Helper.GetEuroCurrency();

			Account account = Account.Create(username, password, role, currency);

			// create
			account = AccountRepository.Create(account);

			// change some properties
			string newPassword = "aaaa123";
			Account.AccountRole newRole =  Account.AccountRole.Administrator;
			//Currency			
			account.Password = newPassword;
			account.Role = newRole;

			// update
			account = AccountRepository.Update(account);

			// test
			account = AccountRepository.Read(account.Id);

			Assert.AreEqual(newPassword, account.Password);
			Assert.AreEqual(newRole, account.Role);


			// clean 
			AccountRepository.Delete(account.Id);
		}


		[Test]
		public void SetLastLoginDate()
		{
			Account account = Helper.GetTestAccount();

			int accountId = account.Id;
			DateTime lastLogin = DateTime.Now.AddHours(-2);
			AccountRepository.UpdateLastLogin(accountId, lastLogin);

		}

		[Test]
		public void GelAllAccounts()
		{
			IList<Account> accounts = AccountRepository.ListAll();

			Assert.IsTrue(accounts.Count > 0);
			Assert.IsTrue(accounts.Where(a => a.Username.ToUpper() == "ALEX").Single() != null );
		}

	}
}
