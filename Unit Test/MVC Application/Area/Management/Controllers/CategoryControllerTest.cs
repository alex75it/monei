using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FakeItEasy;
using Monei.DataAccessLayer.Exceptions;
using Monei.Entities;
using Monei.MvcApplication.Areas.Management.Controllers;
using NUnit.Framework;
using Should;
using System.Security;
using System.Security.Principal;
using Monei.DataAccessLayer.Interfaces;
using Monei.Core.BusinessLogic;

namespace Monei.Test.UnitTest.MccApplication.Area.Management.Controllers
{
    [TestFixture]
    public class CategoryControllerTest
    {		
        [Test]
        public void Edit_when_CategoryHasATooLongName_should_NotThrowException()
        {
            IAccountManager accountManager = A.Fake<IAccountManager>();
            ICategoryManager categoryManager = A.Fake<ICategoryManager>();
            CategoryController controller = new CategoryController(accountManager, categoryManager);
            string username = "test";
            Account account = new Account() {Username=username};
            A.CallTo(() => accountManager.Read(username)).Returns(account);

            SetAuthenticationOnController(controller, username);

            //controller.CategoryRepository = A.Fake<

            Category category = new Category()
            {
                Name = new String('a', 1000)
            };
            FormCollection form = new FormCollection();
            form.Add("id", "1");

            ActionResult result = controller.Edit(category, form);
        }

        private static void SetAuthenticationOnController(Controller controller, string username)
        {
            ControllerContext controllerContext = A.Fake<ControllerContext>();
            HttpContextBase httpContext = A.Fake<HttpContextBase>();
            A.CallTo(() => controllerContext.HttpContext).Returns(httpContext);
            IPrincipal principal = A.Fake<IPrincipal>();
            IIdentity identity = A.Fake<IIdentity>();
            A.CallTo(() => identity.IsAuthenticated).Returns(true);
            A.CallTo(() => identity.Name).Returns(username); // username
            A.CallTo(() => principal.Identity).Returns(identity);
            A.CallTo(() => httpContext.User).Returns<IPrincipal>(principal);
            
            controller.ControllerContext = controllerContext;
        }
    }
}
