using FakeItEasy;
using Monei.Core.BusinessLogic;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Test.UnitTest.Core.Business_Logic
{
    [TestFixture, Category("Business logic"), Category("Category")]
    internal class CategoryManagerTest
    {
        [Test]
        public void Create_when_NameIsTooLong_should_RaiseASpecificException()
        {
            ICategoryRepository categoryRepository = A.Fake<ICategoryRepository>();
            CategoryManager categoryManager = new CategoryManager(categoryRepository);
                        
            int maxLength = Category.NAME_MAX_LENGTH;
            string name = new String('a', maxLength + 1);

            Category category = new Category()
            {
                Name = name
            };

            Assert.Throws<CategoryTooLongNameException>(() => categoryManager.Create(category));
        }
    }
}
