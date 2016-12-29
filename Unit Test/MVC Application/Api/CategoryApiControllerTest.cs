using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Api;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.MvcApplication.Api
{
    [TestFixture, Category("Web API"), Category("Category"),]
    internal class CategoryApiControllerTest :ApiControllerTestBase<CategoryApiController>
    {
        [Test]
        public void List_when_OrderByMostUsed_then_ReturnedList_should_BeOrderedByUsage()
        {
            var controller = CreateController();
            
            IList<Category> data = new List<Category>();
            data.Add(new Category() { Id = 1, Name = "Test" });
            A.CallTo(() => controller.CategoryRepository.List()).Returns(data);

            // Act
            string orderBy = "mostUsed";
            IEnumerable<Category> list = controller.Get(orderBy);

            // Assert
            list.ShouldNotBeEmpty();

            var firstItem = list.First();
            var lastItem = list.Last();

            Assert.Inconclusive("because the returned items haven't a parameter for the number of use.");
            //firstItem.?
        }
    }
}
