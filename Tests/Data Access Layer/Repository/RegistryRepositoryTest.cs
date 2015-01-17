using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Filters;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;


namespace Monei.Tests.DataAccessLayer.Repository
{
	[TestClass]
	public class RegistryRepositoryTest :RepositoryBaseTest
	{

		private const string DEFAULT_TEST_DESCRPTION = "$$$ This is a TEST - Delete me. $$$";

		private static int testAccountId;

		[TestInitialize]
		public void TestInitialize()
		{
			//IList<RegistryRecord> records = RegistryRepository.ListRecods(new RegistryFilters() { EndDate = new DateTime(2005, 1, 1) });

			testAccountId = Helper.GetTestAccount().Id;

			DeleteRecordsOfTestAccount();
		}

		[TestCleanup()]
		public void TestClenup()
		{

			DeleteRecordsOfTestAccount();
		}

		private void DeleteRecordsOfTestAccount()
		{
			IList<RegistryRecord> records = RegistryRepository.ListRecods(new RegistryFilters() { AccountId = testAccountId });

			foreach (var r in records)
				RegistryRepository.DeleteRecord(r.Id);

		}


		[TestMethod]
		public void DeleteRecord()
		{
			RegistryRecord record = CreateTestRecord();

			RegistryRepository.DeleteRecord(record.Id);

			RegistryFilters filters = new RegistryFilters() { 
				StartDate = DateTime.Today, EndDate = DateTime.Today, CategoryId = record.Category.Id
			};

			RegistryRecord searchedRecord = RegistryRepository.ListRecods(filters).FirstOrDefault(r => r.Id == record.Id);
			Assert.IsTrue(searchedRecord == null);
		}

		private RegistryRecord CreateTestRecord()
		{
			DateTime date = DateTime.Now;
			decimal amount = 123.45m;
			string note = "DEFAULT_TEST_DESCRPTION";
			Account account = Helper.GetTestAccount();
			Category category = Helper.GetRandomCategory();
			Subcategory subcategory = null;

			RegistryRecord record = new RegistryRecord() { 
				Date = date,
				Amount = amount,
				Note = note,
				Category = category,
				Subcategory = subcategory,
				Account = account,
			};

			RegistryRecord recod = RegistryRepository.AddRecord(record);
			return record;
		}


		[TestMethod]
		public void List()
		{
			// 1 month
			DateTime startDate = new DateTime(2005, 1, 1);
			DateTime endDate = new DateTime(2005, 1, 31);

			RegistryFilters filters;

			RegistryRecord record;
			IList<RegistryRecord> records;

			DateTime date = new DateTime(2005, 1, 1);
			decimal amount = 123.45m;
			string note = DEFAULT_TEST_DESCRPTION;
			bool isTaxDeductible = false;
			bool isSpecialEvent = false;
			Account account = Helper.GetTestAccount();
			Category category = Helper.GetRandomCategory();

			record = Helper.CreateRecord(date, amount, note, isTaxDeductible, isSpecialEvent, account, category);
			RegistryRepository.AddRecord(record);

			filters = new RegistryFilters() { StartDate = startDate, EndDate = endDate };
			records = RegistryRepository.ListRecods(filters);

			Assert.AreEqual(1, records.Count);

			RegistryRecord loadedRecord = records.First();

			Assert.AreEqual(date.ToString(), loadedRecord.Date.ToString());
			Assert.AreEqual(amount, loadedRecord.Amount);
			Assert.AreEqual(note, loadedRecord.Note);
			Assert.AreEqual(isTaxDeductible, loadedRecord.IsTaxDeductible);
			Assert.AreEqual(isSpecialEvent, loadedRecord.IsSpecialEvent);
			Assert.AreEqual(account.Id, loadedRecord.Account.Id);

			// different account must not see records 
			Account differentAccount = Helper.GetDemoAccount();			
			filters.AccountId = differentAccount.Id;
			records = RegistryRepository.ListRecods(filters);

			Assert.IsTrue(records.Count == 0);

			DeleteRecordsOfTestAccount();

			// filter by Category
			record = Helper.CreateRecord(date, amount, note, isTaxDeductible, isSpecialEvent, account, category);
			record.Category = category;

			RegistryRepository.AddRecord(record);
			filters = new RegistryFilters();
			filters.AccountId = Helper.GetTestAccount().Id;
			filters.CategoryId = category.Id;

			records = RegistryRepository.ListRecods(filters);

			records.Single(r => r.Category.Id == category.Id);

			
			// todo: add other tests


		}

		[TestMethod]
		public void AddRecord()
		{

			DateTime date = DateTime.Now;
			decimal amount = 123.45m;
			string note = "This is a test description";
			bool isTaxDeductible = false;
			bool isSpecialEvent = false;
			Account account = Helper.GetTestAccount();
			Category category = Helper.GetRandomCategory();
			Subcategory subcategory = null;

			RegistryRecord record = new RegistryRecord()
			{
				Date = date,
				Account = account,
				Amount = 123.45m,
				Category = category,
				Subcategory = subcategory,
				Note = note,
				IsTaxDeductible = isTaxDeductible,
				IsSpecialEvent = isSpecialEvent,
			};

			record = RegistryRepository.AddRecord(record);

			Assert.IsNotNull(record, "Null");
			Assert.IsTrue(date.ToString() == record.Date.ToString(), "Date");
			Assert.AreEqual(amount, record.Amount, "Amount");
			Assert.AreEqual(note, record.Note, "Note");
			Assert.AreEqual(isTaxDeductible, record.IsTaxDeductible, "IsTaxDeductible");
			Assert.AreEqual(isSpecialEvent, record.IsSpecialEvent, "IsSpecialEvent");

			try
			{
				RegistryRepository.DeleteRecord(record.Id);
			}
			catch(Exception exc)
			{
				Console.WriteLine("Fail to delete test Record. " + exc.Message);
			}

		}


	}//class
}
