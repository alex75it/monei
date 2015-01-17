using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.MongoDB;
using Monei.Entities;

namespace Monei.Tests.Data_Access_Layer.MongoDB
{
	[TestClass]
	public class CategoryRepositoryTest
	{
		[TestMethod]
		public void AddCategory()
		{

			ICategoryRepository repository = CategoryRepository.Instance;

			string name = "Airplane, train, bus, highway, parking";
			string description = "Airplane, train and bus ticket, highway toll, parking fee.";

			var item = new Category() { 
				Name = name,
				Description = description,
				ImageName = null,
			}; 


			item = repository.Create(TestHelper.GetMockAccount(), item);

			Assert.IsNotNull(item);

			Assert.AreEqual(item.Name, name, "Name is not equal.");
			Assert.AreEqual(item.Description, description, "Description is not equal.");

		}

		[TestMethod]
		public void UpdateCategory()
		{
			Account account = TestHelper.GetMockAccount();
			Category category = new Category() { 
				Name = "Test-a",
				Description = "aaa bbb",
			};


			CategoryRepository.Instance.Create(account, category);

			category.Name = "Test-b";


			category = CategoryRepository.Instance.Update(account, category);


			Assert.AreEqual(category.Name, "Test-b");

		}



	}//class
}
