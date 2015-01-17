using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.Entities;

namespace Monei.Tests.DataAccessLayer.Repository
{
	[TestClass]
	public class SubcategoryRepositoryTest :RepositoryBaseTest
	{
		[TestMethod]
		public void List()
		{

			int categoryId;

			Category category = Helper.GetRandomCategory();
			categoryId = category.Id;

			List<Subcategory> list = SubcategoryRepository.List(categoryId);

			if(list.Count > 0)
			{
				Assert.IsTrue(list.All(s => s.Category.Equals(category)), "Category");
			}
			else
			{
				Assert.Inconclusive("None subcategory found for category " + category.Name);
			}
		}


		[TestMethod]
		public void Create()
		{
			Account account = Helper.GetTestAccount();
			Category category = Helper.GetRandomCategory();

			string name = "ainjefgnq";
			string description = "aSG òLERF PLIKMERF";

			Subcategory subcategory = new Subcategory() { 
				CreationAccount = account,
				CreationDate = DateTime.Now,
				Name = name,
				Category = category,
				Description = description,				
			};

			subcategory.Id = SubcategoryRepository.Create(subcategory);

			subcategory = SubcategoryRepository.Read(subcategory.Id);

			Assert.IsNotNull(subcategory);
			Assert.AreEqual(name, subcategory.Name);
			Assert.AreEqual(description, subcategory.Description);
			

			// cleanup
			SubcategoryRepository.Delete(subcategory.Id);
		}

		[TestMethod]
		public void Read()
		{
			string name ="to be deleted";
			string description = "<fghe e e egeavb4wt h";
			Category category = Helper.GetRandomCategory();

			Subcategory subcategory = new Subcategory();
			subcategory.Name = name;
			subcategory.Description = description;
			subcategory.Category = category;
			//subcategory.CreationAccount


			subcategory.Id = SubcategoryRepository.Create(subcategory);

			int id = subcategory.Id;
			subcategory = SubcategoryRepository.Read(id);


			Assert.AreEqual(name, subcategory.Name);
			Assert.AreEqual(description, subcategory.Description);

		}

		[TestMethod]
		public void Update()
		{
			string name = "Initial name";
			string description = "Initial Description";

			Account account = Helper.GetTestAccount();
			Category category = Helper.GetRandomCategory();

			Subcategory subcategory = new Subcategory() { 
				Name = name,
				Description = description,
				CreationAccount = account,
				Category = category,
				
			};
			
			// create
			subcategory.Id = SubcategoryRepository.Create(subcategory);

			// change properties
			name = "Update name  - ojear a";
			description = "Update Description  - poear òokeokff 35448re";

			subcategory.Name = name;
			subcategory.Description = description;


			// update
			SubcategoryRepository.Update(subcategory);
			
			Assert.AreEqual(name, subcategory.Name);
			Assert.AreEqual(description, subcategory.Description);
			Assert.AreEqual(category.Id, subcategory.Category.Id);
			Assert.AreEqual(account, subcategory.CreationAccount);

			// cleanup
			SubcategoryRepository.Delete(subcategory.Id);
		}

		[TestMethod]
		public void Delete()
		{
			Subcategory subcategory = Helper.CreateRandomSubcategory();
			int subcategoryId = SubcategoryRepository.Create(subcategory);
						
			SubcategoryRepository.Delete(subcategoryId);

			string sqlQuery = "Select * from Subcategory where Id = " + SubcategoryRepository;

			subcategory = SubcategoryRepository.Read(subcategoryId);

			Assert.IsNull(subcategory);
		}

	}
}
