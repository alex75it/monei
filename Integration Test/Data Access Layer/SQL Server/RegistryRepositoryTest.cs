using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using NUnit.Framework;
using Should;
using Assert = NUnit.Framework.Assert;
using Monei.DataAccessLayer.SqlServer;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
    [TestFixture]
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
        public void List_when_StartDateAndEndDateAreSet_should_ReturnAList()
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

        #region utils methods

        private void DeleteRecordsOfTestAccount()
        {
            IList<RegistryRecord> records = RegistryRepository.ListRecords(new RegistryFilters() { AccountId = testAccountId });

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

        #endregion

    }
}
