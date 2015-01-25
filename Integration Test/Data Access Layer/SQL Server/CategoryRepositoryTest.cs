using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using Monei.Test.IntegrationTest.DataAccessLayer.SqlServer;
using Should;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
	[TestClass]
	public class CategoryRepositoryTest :RepositoryTestBase
	{
		[TestMethod]
		public void List()
		{
			ICategoryRepository repository = new CategoryRepository();

			IEnumerable<Category> list = repository.List();

			// Verify
			list.ShouldNotBeEmpty();
			
		}

		[TestMethod]
		public void AddCategory()
		{
			string name = "Transport and Parking";
			string description = "Airplane, train and bus tickets, highway tolls, parking fees.";

			var category = new Category()
			{
				Name = name,
				Description = description,
				ImageName = null,
			};

			Helper.GetTestAccount();

			// create
			category = CategoryRepository.Create(category);

			Assert.IsNotNull(category);

			Assert.AreEqual(name, category.Name, "Names are not equal.");
			Assert.AreEqual(description, category.Description, "Descriptions are not equal.");

			// cleanup
			CategoryRepository.Delete(category.Id);


			// test max length for name
			int maxLength = Category.NAME_MAX_LENGTH;

			name = new String('a', maxLength + 1);
			category.Name = name;
			try
			{
				CategoryRepository.Create(category);
			}
			catch (Exception)
			{
				//Assert.();
			}

			CategoryRepository.Delete(category.Id);
		}

		[TestMethod]
		public void UpdateCategory()
		{
			Account account = Helper.GetTestAccount();
			Category category = new Category()
			{
				Name = "Test-a",
				Description = "aaa bbb",
			};

			category.CreationAccount = account;
			CategoryRepository.Create(category);

			category.Name = "Test-b";

			category.CreationAccount = account;
			category = CategoryRepository.Update(category);


			Assert.AreEqual(category.Name, "Test-b");

		}

	}
}
