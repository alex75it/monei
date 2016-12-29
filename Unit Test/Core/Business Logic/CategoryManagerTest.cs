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

namespace Monei.Test.UnitTest.Core.BusinessLogic
{
    [TestFixture, Category("Business logic"), Category("Category")]
    internal class CategoryManagerTest
    {
        CategoryManager categoryManager;

        [SetUp]
        public void SetUp()
        {
            ICategoryRepository categoryRepository = A.Fake<ICategoryRepository>();
            categoryManager = new CategoryManager(categoryRepository);
        }

        [Test]
        public void Create_when_NameIsTooLong_should_RaiseASpecificException()
        {                        
            int maxLength = Category.NAME_MAX_LENGTH;
            string name = new String('a', maxLength + 1);

            Category category = new Category()
            {
                Name = name
            };

            Assert.Throws<TooLongCategoryNameException>(() => categoryManager.Create(category));
        }

        [Test]
        public void Edit_when_CategoryNameIsTooLong_shouldRaiseASepcificException()
        {
            int maxLength = Category.NAME_MAX_LENGTH;
            string name = new String('a', maxLength + 1);

            Category category = new Category()
            {
                Name = name
            };

            Assert.Throws<TooLongCategoryNameException>(() => categoryManager.Update(category));
        }

        [Test]
        public void Update_when_CategoryHasATooLongName_should_ThrowsSpecificException()
        {
            Category category = new Category()
            {
                Name = new String('a', 1000)
            };

            Assert.Throws<TooLongCategoryNameException>(() => categoryManager.Update(category));
        }


        [Test]
        public void Update_WhenCategoryHasATooShortName_Should_ThrowsException()
        {            
            Category category = new Category()
            {
                Name = new String('a', Category.NAME_MIN_LENGTH - 1)
            };

            Assert.Throws<Exception>(() => categoryManager.Update(category));
        }

        [Test]
        public void Update_WhenCategoryHasATooLongDescription_Should_ThrowsException()
        {
            Category category = new Category()
            {
                Name = "aaa",
                Description = new String('a', Category.DESCRIPTION_MAX_LENGTH + 1)
            };

            Assert.Throws<Exception>(() => categoryManager.Update(category));
        }

    }
}
