using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Monei.Core.Managers;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Core.Managers
{
	[TestFixture]
	public class CategoryManagerTest
	{
		[Test]
		public void Update_WhenCategoryHasATooLongName_Should_ThrowsSpecificException()
		{
			ICategoryRepository categoryRepository = A.Fake<ICategoryRepository>();
			CategoryManager manager = new CategoryManager(categoryRepository);
			Category category = new Category()
			{
				Name = new String('a', 1000)
			};

			Assert.Throws<CategoryTooLongNameException>(() => manager.Update(category));
		}


		[Test]
		public void Update_WhenCategoryHasATooShortName_Should_ThrowsException()
		{
			ICategoryRepository categoryRepository = A.Fake<ICategoryRepository>();
			CategoryManager manager = new CategoryManager(categoryRepository);
			Category category = new Category()
			{
				Name = new String('a', Category.NAME_MIN_LENGTH-1)
			};

			Assert.Throws<Exception>(() => manager.Update(category));
		}

		[Test]
		public void Update_WhenCategoryHasATooLongDescription_Should_ThrowsException()
		{
			ICategoryRepository categoryRepository = A.Fake<ICategoryRepository>();
			CategoryManager manager = new CategoryManager(categoryRepository);
			Category category = new Category()
			{
				Description = new String('a', Category.DESCRIPTION_MAX_LENGTH+1)
			};

			Assert.Throws<Exception>(() => manager.Update(category));
		}
	}
}
