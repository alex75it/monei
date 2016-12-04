using FakeItEasy;
using Monei.Core.BusinessLogic;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Controllers;
using Monei.MvcApplication.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Monei.Test.UnitTest.MVC_Application.Controllers
{
    [TestFixture]
    public class AccountControllerTest //: ControllerTestBase
    {
        private AccountController accountController;

        [SetUp]
        public void SetUp()
        {
            IAccountSecurity accountSecurity = A.Fake<IAccountSecurity>();
            IAccountManager accountManager = A.Fake<IAccountManager>();
            ICurrencyRepository currencyRepository = A.Fake<ICurrencyRepository>();
            IAuthenticationWorker webAuthenticationWorker = A.Fake<IAuthenticationWorker>();

            accountController = new AccountController(accountSecurity, accountManager, currencyRepository, webAuthenticationWorker);
        }

        public void IsInitialized()
        {
            string returnUrl = "returnUrl";
            ActionResult result = accountController.Login(returnUrl);

            Assert.IsNotNull(result);
        }
        
    }  
}
