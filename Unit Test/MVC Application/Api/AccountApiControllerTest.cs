using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Api;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.MvcApplication.Api.ResponseDataObjects;
using Monei.MvcApplication.Core;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.MVC_Application.Api
{
    [TestFixture, Category("Web API"), Category("Account")]
    public class AccountApiControllerTest
    {

        [Test]
        public void Login_Should_ReturnOkResult()
        {
            IAccountRepository accountRepository = A.Fake<IAccountRepository>();
            ICurrencyRepository currencyRepository = A.Fake<ICurrencyRepository>();
            IWebAuthenticationWorker webAuthenticationWorker = A.Fake<IWebAuthenticationWorker>();

            var account = A.Dummy<Account>();
            account.Username = "aaa";
            account.Password = "bbb";
            A.CallTo(() => accountRepository.Read(account.Username)).Returns(account);

            AccountApiController controller = new AccountApiController(accountRepository, currencyRepository, webAuthenticationWorker);
            var data = A.Fake<LoginPostData>();
            data.Username = account.Username;
            data.Password = account.Password;
            data.RememberMe = false;

            // Execute
            var result = controller.Login(data);

            // Verify
            result.ShouldEqual(LoginResult.Ok);
        }

    }
}
