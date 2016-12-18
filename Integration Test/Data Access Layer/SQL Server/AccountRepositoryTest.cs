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
    [TestFixture, Category("Data Access Layer")]
    public class AccountRepositoryTest : RepositoryTestBase
    {
        
        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            //Helper.RemoveTestAccount();
        }

        [SetUp]
        public void SetUp()
        {
            try
            {
                Helper.RemoveTestAccount();
            }
            catch (Exception exc)
            {
                Assert.Inconclusive("FAIL to delete test Account. " + exc.Message);
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
            //todo: this test fail (the creation date day is wrong) when executed in Italy between 24:00 and 01:00

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

        /// Todo: split this test in more tests
        [Test]
        public void Create()
        {
            string username = Helper.TEST_USERNAME;
            string password = "test";
            Account.AccountRole role = Account.AccountRole.User;
            Currency currency = Helper.GetEuroCurrency();

            Account account = Account.Create(username, password, role, currency);
            Account account_2 = Account.Create(username, password, role, currency);

            account = AccountRepository.Create(account);
            
            try
            {
                AccountRepository.Create(account_2);
                Assert.Fail("Duplicate username haven't to be permitted.");
            }
            catch (EntityAlreadyExistsException exc)
            {
                exc.PropertyName.ShouldEqual("username");
            }

            // clean 
            AccountRepository.Delete(account.Id);
        }

        [Test]
        [TestCase(Account.AccountRole.User, Currency.EUR_CODE)]
        [TestCase(Account.AccountRole.Administrator, Currency.USD_CODE)]
        public void Read_using_AccountId(Account.AccountRole role, string currencyCode)
        {
            string username = "Test";
            string password = "test";
            Currency currency = CurrencyRepository.Read(currencyCode);

            Helper.DeleteAccount(username);

            Account account = Account.Create(username, password, role, currency);

            account = AccountRepository.Create(account);

            // execute
            Account loadedAccount = AccountRepository.Read(account.Id);

            Assert.AreEqual(username, loadedAccount.Username);
            Assert.AreEqual(password, loadedAccount.Password);
            Assert.AreEqual(role, loadedAccount.Role);
            Assert.AreEqual(currency.Id, loadedAccount.Currency.Id);

            // clean 
            AccountRepository.Delete(account.Id);
        }

        [Test]
        public void Read_using_Username()
        {
            string username = "Test";
            string password = "test";
            Currency currency = CurrencyRepository.Read(Currency.EUR_CODE);
            Account.AccountRole role = Account.AccountRole.User;

            Helper.DeleteAccount(username);

            Account account = AccountRepository.Create(Account.Create(username, password, role, currency));

            // execute
            Account loadedAccount = AccountRepository.Read(username);

            loadedAccount.ShouldNotBeNull();

            // clean 
            AccountRepository.Delete(account.Id);
        }

        [Test]
        public void Read_using_Guid()
        {
            string username = "Test";
            string password = "test";
            Currency currency = CurrencyRepository.Read(Currency.EUR_CODE);
            Account.AccountRole role = Account.AccountRole.User;

            Helper.DeleteAccount(username);

            Account account = AccountRepository.Create(Account.Create(username, password, role, currency));

            // execute
            Guid accountGuid = account.Guid;
            Account loadedAccount = AccountRepository.Read(accountGuid);

            loadedAccount.ShouldNotBeNull();

            // clean 
            AccountRepository.Delete(account.Id);
        }

        [Test]
        public void Read_when_UsernameIsNull_should_RaiseASpecificException()
        {
            string username = null;

            Assert.Throws<ArgumentNullException>(() =>
                AccountRepository.Read(username)
            );
        }

        [Test]
        public void Read_when_GuidIsEmpty_should_RaiseASpecificException()
        {
            Guid accountId = Guid.Empty;

            Assert.Throws<ArgumentException>(() =>
                AccountRepository.Read(accountId)
            );
        }

        [Test]
        public void Update()
        {
            string username = "Test";
            string password = "test";
            Account.AccountRole role = Account.AccountRole.User;
            Currency currency = Helper.GetEuroCurrency();

            Helper.DeleteAccount(username);
                   
            Account account = Account.Create(username, password, role, currency);

            // create
            account = AccountRepository.Create(account);

            // change some properties
            string newPassword = "aaaa123";
            Account.AccountRole newRole = Account.AccountRole.Administrator;
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
            Assert.IsTrue(accounts.Where(a => a.Username.ToUpper() == "ALEX").Single() != null);
        }

    }
}
