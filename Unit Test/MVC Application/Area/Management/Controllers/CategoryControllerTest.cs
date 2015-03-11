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

namespace Monei.Test.UnitTest.MVC_Application.Area.Management.Controllers
{
	[TestFixture]
	public class CategoryControllerTest
	{
		
		[Test]
		public void Edit_WhenCategoryHasATooLongName_Should_DoesNotThrowException()
		{
			CategoryController controller = new CategoryController();
			string username = "test";
			controller.AccountRepository = A.Fake<IAccountRepository>();
			Account account = new Account() {Username=username};
			A.CallTo(() => controller.AccountRepository.Read(username)).Returns(account);

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
