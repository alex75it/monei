using FakeItEasy;
using Monei.Core.BusinessLogic;
using Monei.DataAccessLayer.Exceptions;
using Monei.Entities;
using Monei.MvcApplication.Areas.Management.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel;

namespace Monei.Test.UnitTest.MVC_Application.Controllers
{
    [TestFixture, NUnit.Framework.Category("Controllers")]
    public class CategoryControllerTest
    {
        private CategoryController categoryController;

        [SetUp]
        public void SetUp()
        {           
            IAccountManager accountManager = A.Fake<IAccountManager>();
            ICategoryManager categoryManager = A.Fake<ICategoryManager>();

            categoryController = new CategoryController(accountManager, categoryManager);
        }

        [Test]
        public void Edit_when_CategoryNameIsTooLong_should_SetViewBagErrorMessage()
        {
            Category category = new Category()
            {

            };

            FormCollection formCollection = new FormCollection() {
                
            };

            // action
            categoryController.Edit(category, formCollection);

            dynamic viewBag = categoryController.ViewBag;
            Assert.IsNotNull(viewBag);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(viewBag.ErrorMessage));
        }
    }
}
