using System;
using System.Collections.Generic;
using System.Linq;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using Monei.DataAccessLayer.SqlServer;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using Should;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
    [TestFixture, Category("Repository")]
    public class RegistryRepositoryTest : RepositoryTestBase
    {

        private const string DEFAULT_TEST_DESCRPTION = "$$$ This is a TEST - Delete me. $$$";

        private static int testAccountId;
        private RegistryRepository repository;
        

        [OneTimeSetUp]
        public void TestInitialize()
        {
            var sessionFactoryProvider = new SessionFactoryProvider();
            repository = new RegistryRepository(sessionFactoryProvider);

            testAccountId = Helper.GetTestAccount().Id;
            DeleteRecordsOfTestAccount();
        }

        [TearDown]
        public void TestClenup()
        {
            DeleteRecordsOfTestAccount();
        }
        
        [Test]
        public void DeleteRecord()
        {
            // todo: 14s for run this test?
            RegistryRecord record = CreateTestRecord();

            RegistryRepository.DeleteRecord(record.Id);

            RegistryFilters filters = new RegistryFilters()
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Categories = new int[] { record.Category.Id}
            };

            RegistryRecord searchedRecord = RegistryRepository.ListRecords(filters).FirstOrDefault(r => r.Id == record.Id);
            Assert.IsTrue(searchedRecord == null);
        }

        private RegistryFilters[] registryFiltersForListTest = new RegistryFilters[] {
            new RegistryFilters() { } // empty
            };

        //[TestCaseSource(RegistryFilters]
        [Test]
        public void List_should_ReturnAList()
        {
            RegistryFilters filters = new RegistryFilters();
            var list = repository.ListRecords(filters);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void List_when_FilterStartDateAndEndDate_should_ReturnAList()
        {
            RegistryFilters filters = new RegistryFilters();
            filters.StartDate = DateTime.Today.AddDays(-3);
            filters.EndDate = DateTime.Today;

            RegistryRecord registryRecord = new RegistryRecord() {
                Date = DateTime.Today.AddDays(-3),
            };

            CreateTestRecord(registryRecord);            

            var list = repository.ListRecords(filters);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void List_when_FilterCategory_should_ReturnAList()
        {
            RegistryFilters filters = new RegistryFilters();
            var category = Helper.GetRandomCategory();
            filters.Categories = new int[] { category.Id };

            RegistryRecord registryRecord = new RegistryRecord()
            {
                Date = DateTime.Today,
                Category = category,
            };

            CreateTestRecord(registryRecord);

            var list = repository.ListRecords(filters);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void List_when_AmountIsSet_should_ReturnRightRecords()
        {
            var filters = new RegistryFilters() {
                Amount = 1.11m
            };

            RegistryRecord record = CreateTestRecord( new RegistryRecord()
            {
                Date = DateTime.Today,
                Category = Helper.GetRandomCategory(),
                Amount = 1.11m,
            });            

            try
            {
                var records = RegistryRepository.ListRecords(filters);
                records.Where(r => r.Id == record.Id).ShouldNotBeEmpty();
            }
            finally
            {
                DeleteTestRecord(record.Id);
            }
        }

        [Test]
        public void List_when_IncludeSpecialEventIsSet_should_ReturnRightRecords()
        {
            var filters = new RegistryFilters()
            {
                IncludeSpecialEvent = true,
            };

            RegistryRecord record = CreateTestRecord(new RegistryRecord()
            {
                Date = DateTime.Today,
                Category = Helper.GetRandomCategory(),
                Amount = 1.23m,
                IsSpecialEvent = true,
                Note = "IncludeSpecialEvent"
            });

            try
            {
                var records = RegistryRepository.ListRecords(filters);
                records.Where(r => r.Id == record.Id).ShouldNotBeEmpty();
            }
            finally
            {
                DeleteTestRecord(record.Id);
            }
        }

        [Test]
        public void List()
        {
            // period of 1 month
            DateTime startDate = new DateTime(2005, 1, 1);
            DateTime endDate = new DateTime(2005, 1, 31);

            DeleteRegistryRecordInPeriod(startDate, endDate);

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
            records = RegistryRepository.ListRecords(filters);

            // Verify
            Assert.AreEqual(1, records.Count, "record count");

            RegistryRecord loadedRecord = records.First();

            Assert.AreEqual(date.Date.ToString(), loadedRecord.Date.ToString()); // database write/load can change missiseconds
            Assert.AreEqual(amount, loadedRecord.Amount, "Amount");
            Assert.AreEqual(note, loadedRecord.Note, "Note");
            Assert.AreEqual(isTaxDeductible, loadedRecord.IsTaxDeductible, "Is Tax deductible");
            Assert.AreEqual(isSpecialEvent, loadedRecord.IsSpecialEvent, "Is Special event");
            Assert.AreEqual(account.Id, loadedRecord.Account.Id, "Account Id");

            // different account must not see records 
            Account differentAccount = Helper.GetDemoAccount();
            filters.AccountId = differentAccount.Id;
            records = RegistryRepository.ListRecords(filters);

            Assert.IsTrue(records.Count == 0);

            DeleteRecordsOfTestAccount();

            // filter by Category
            record = Helper.CreateRecord(date, amount, note, isTaxDeductible, isSpecialEvent, account, category);
            record.Category = category;

            RegistryRepository.AddRecord(record);
            filters = new RegistryFilters();
            filters.AccountId = Helper.GetTestAccount().Id;
            filters.Categories = new int[] { category.Id};

            records = RegistryRepository.ListRecords(filters);

            records.Single(r => r.Category.Id == category.Id);

            // todo: add other tests
        }

        [Test]
        public void List_when_AnAccountIsSpecified_should_ReturnTheRightRecords()
        {
            DeleteRecordsOfTestAccount();

            Account account = Helper.GetTestAccount();

            RegistryRecord record = new RegistryRecord() {
                Account = account,
                Amount = 1,
                Category = Helper.GetRandomCategory(),
                CreationAccount = Helper.GetTestAccount(),
                Date = DateTime.UtcNow
            };

            record = this.CreateTestRecord(record);

            // act
            var records = RegistryRepository.ListRecords(new Monei.DataAccessLayer.Filters.RegistryFilters() { AccountId = account.Id });
            
            records.ShouldNotBeEmpty();
            records.Count.ShouldEqual(1);
            records[0].Account.Id.ShouldEqual(account.Id);
        }

        [Test]
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
                OperationType = OperationType.Outcome,
            };

            record = RegistryRepository.AddRecord(record);

            /// Verify
            Assert.IsNotNull(record, "Null");
            record.Date.ShouldEqual(date, Should.Core.DatePrecision.Date);
            Assert.IsTrue(date.Date == record.Date.Date, "Date");
            record.OperationType.ShouldEqual(OperationType.Outcome, "OperationType");
            Assert.AreEqual(amount, record.Amount, "Amount");
            Assert.AreEqual(note, record.Note, "Note");
            Assert.AreEqual(isTaxDeductible, record.IsTaxDeductible, "IsTaxDeductible");
            Assert.AreEqual(isSpecialEvent, record.IsSpecialEvent, "IsSpecialEvent");

            try
            {
                RegistryRepository.DeleteRecord(record.Id);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Fail to delete test Record. " + exc.Message);
            }

        }

        [Test]
        public void IsSubcategoryUsed()
        {
            Category category = CategoryRepository.List().First();
            Subcategory subcategory = SubcategoryRepository.List(category.Id).First();

            var record = Helper.CreateRecord(DateTime.Now, 1.23m, "test", false, false, Helper.GetDemoAccount(), category);
            record.Subcategory = subcategory;
            record = RegistryRepository.AddRecord(record);

            try
            {
                // execute
                bool isUsed = RegistryRepository.IsSubcategoryUsed(subcategory.Id);

                isUsed.ShouldBeTrue();
            }
            finally {
                DeleteTestRecord(record.Id);
            }
        }


        #region utils methods

        private void DeleteRecordsOfTestAccount()
        {
            IList<RegistryRecord> records = RegistryRepository.ListRecords(new RegistryFilters() {
                AccountId = testAccountId,
                IncludeSpecialEvent =  true
                });

            foreach (var r in records)
                RegistryRepository.DeleteRecord(r.Id);
        }
        
        private void DeleteRegistryRecordInPeriod(DateTime startDate, DateTime endDate)
        {
            IList<RegistryRecord> records = RegistryRepository.ListRecords(new RegistryFilters() { StartDate = startDate, EndDate=endDate });

            foreach (var r in records)
                RegistryRepository.DeleteRecord(r.Id);
        }

        private RegistryRecord CreateTestRecord(RegistryRecord record = null)
        {
            record = record ?? new RegistryRecord();
            if (record.Date == default(DateTime)) record.Date = DateTime.Today;
            if (record.Category == null) record.Category = Helper.GetRandomCategory();
            if (record.Account == null) record.Account = Helper.GetTestAccount();
            
            record = RegistryRepository.AddRecord(record);
            return record;
        }

        private void DeleteTestRecord(int recordId)
        {
            RegistryRepository.DeleteRecord(recordId);
        }

        #endregion

    }
}
