using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using FakeItEasy;
using Monei.Core.BusinessLogic;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Api;
using Monei.MvcApplication.Core;

namespace Monei.Test.UnitTest.MvcApplication.Api
{
    internal abstract class ApiControllerTestBase<T> : TestBase 
        where T: ApiControllerBase, new()
    {
        protected IAccountRepository accountRepository;
        protected IRegistryRepository registryRepository;
        protected ICategoryRepository categoryRepository;

        internal T CreateController(bool setAuthentication = true) //where T: ApiControllerBase, new()
        {
            T controller = new T();

            controller.AccountRepository = A.Fake<IAccountRepository>();
            controller.AuthenticationWorker = A.Fake<IAuthenticationWorker>();
            controller.RegistryManager = A.Fake<IRegistryManager>();
            controller.CategoryRepository = A.Fake<ICategoryRepository>();            

            SetRequestUser(controller);

            return controller;
        }

        protected void SetRequestUser(ApiController controller)
        {
            Thread.CurrentPrincipal = new GenericPrincipal
            (
               new GenericIdentity(TEST_USERNAME),
               new string[] { /*"managers", "executives"*/ }
            );

            /*
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.IsInRole("admin").Returns(true);
            userMock.SetupGet(p => p.Identity.Name).Returns("tester");
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);

            var requestContext = new Mock<HttpRequestContext>();
            requestContext.Setup(x => x.Principal).Returns(userMock.Object);

            var controller = new ControllerToTest()
            {
                RequestContext = requestContext.Object,
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            */
        }
    }
}
