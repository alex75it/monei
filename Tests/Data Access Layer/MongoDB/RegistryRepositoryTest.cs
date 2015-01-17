using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.MongoDB;
using Monei.Entities;
using MongoDB.Bson;

namespace Monei.Tests.Data_Access_Layer.MongoDB
{
	[TestClass]
	public class RegistryRepositoryTest
	{
		[TestMethod]
		public void InsertRecord()
		{
			// todo: use a Helper class
			AccountRepository accountRepository = new AccountRepository();


			int accountId = 123;
			
			Account account = new AccountRepository().GetAccount( accountId);
			Category category = TestHelper.GetRandomCategory();

			//Subcategory subcategory = new Subcategory() { Category = category, Name = "Meat" };
			Subcategory subcategory = null;

			RegistryRecord record = new RegistryRecord() { Account = account, Date = DateTime.Now, Category = category, Amount = 4.45m, Subcategory = subcategory, Note = "Comprata carne per cena" };
		

			IRegistryRepository repository = new RegistryRepository();

			//repository.

			record = repository.AddRecord(record);

			Assert.IsTrue (record != null, "Registry record is null");

			//Assert.IsTrue(record);

		}


		[TestMethod]
		public void DeleteRecord()
		{
			int id = RegistryRecord.EMPTY_ID;

			RegistryRepository.Instance.Delete(id);

			Assert.IsTrue(true);
		}


	}//class
}
