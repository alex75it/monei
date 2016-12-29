using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Api;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.Test.UnitTest.MVC_Application.Api;
using NUnit.Framework;
using Should;
using Monei.MvcApplication.Api.PostDataObjects;
using System.Net.Http;

namespace Monei.Test.UnitTest.MvcApplication.Api
{
    [TestFixture, Category("Web API"), Category("Registry")]
    internal class RegistryApiControllerTest : ApiControllerTestBase<RegistryApiController>
    {
        private RegistryApiController controller;

        [SetUp]
        public void SetUp()
        {
            controller = CreateController();

            Account account = new Account() {
                Id=1,
                Guid = Guid.NewGuid(),
                Username = "test",
            };
            A.CallTo(() => controller.AccountRepository.Read(A.Dummy<string>())).Returns(account);


            //registryRepository = A.Fake<IRegistryRepository>();
        }

        [Test]
        public void Create_when_DateIsNotDefined_should_ThrowAnException()
        [TestCase(OperationType.Outcome, 123.45, 1, 0, null, true, false)]
        [TestCase(OperationType.Transfer, 123.45, 1, 0, null, true, true)]
        public void Create_should_CallTheRegistryMethodForCreateNewRecord(
            OperationType operationType, decimal amount, 
             int categoryId, int subcategoryId, string note,
             bool isSpecialEvent, bool isTaxDEductible)
        {
            DateTime date = DateTime.Now;
               
            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = date,
                OperationType = operationType,
                Amount = amount,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId,
                Note = note,                
            };

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Amount = 1,
                Operation = OperationType.Income,
                CategoryId = 1,
                Note = "aaa",
            };

            //A.CallTo(controller.Create(postData)).Throws<Exception>();
            AssertExceptionIsRaised(() => controller.Create(postData), new Exception("Date is not defined"));
        }
        
        [Test]
        [TestCase(OperationType.Income)]
        [TestCase(OperationType.Outcome)]
        public void Create_when_AmountIsZeroAndOperationTypeIsNotTransfer_should_ThrowAnException(OperationType operation)
        {
            RegistryRecord record = new RegistryRecord();
                    && r.Subcategory.Id == postData.SubcategoryId
                    && r.IsSpecialEvent == isSpecialEvent
                    && r.IsTaxDeductible == isTaxDEductible
                    )                
                ));

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = DateTime.Now,
                Operation = operation,
                Amount = 0,                
                CategoryId = 1,
                Note = "aaa",
            };

            //A.CallTo(() => controller.Create(postData)).Throws(new Exception("Amount is zero"));
            // System.ArgumentException : The specified object is not recognized as a fake object.

            AssertExceptionIsRaised(() => controller.Create(postData), new Exception("Amount is zero"));
        }

        [Test]
        public void Create_when_CategoryIsNotDefined_should_ThrowAnException()
        {
            RegistryRecord record = new RegistryRecord();

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = DateTime.Now,
                Operation = OperationType.Income,
                Amount = 1,
                CategoryId = 0,
                Note = "aaa",
            };

            //A.CallTo(controller.Create(postData)).Throws<Exception>();
            AssertExceptionIsRaised(() => controller.Create(postData), new Exception("Category is not defined"));            
        }

        [Test]
        public void Create_should_CallRepositoryWithRightData()
        {
            RegistryRecord record = new RegistryRecord();

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = DateTime.Now,
                Amount = 1m,
                Operation = OperationType.Income,
                CategoryId = 1,
                Note = "aaa",
            };

            // execute
            controller.Create(postData);

            A.CallTo( () => controller.RegistryRepository.AddRecord(
                A<RegistryRecord>
                .That.Matches(r =>
                r.Date == postData.Date
                && r.OperationType == postData.Operation
                && r.Amount == postData.Amount
                && r.Category.Id == postData.CategoryId
            ))).MustHaveHappened();            
        }
        
          [Test]
        [TestCase(OperationType.Income, 123.45, 1, 2, "note", false, false)]
        [TestCase(OperationType.Outcome, 123.45, 1, 0, null, true, false)]
        [TestCase(OperationType.Transfer, 123.45, 1, 0, null, true, true)]
        public void Create_should_CallTheRegistryMethodForCreateNewRecord(
            OperationType operationType, decimal amount, 
             int categoryId, int subcategoryId, string note,
             bool isSpecialEvent, bool isTaxDEductible)
        {
            // Arrange         
            DateTime date = DateTime.Now;
               
            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = date,
                OperationType = operationType,
                Amount = amount,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId,
                Note = note,                
            };

            IRegistryRepository registryRepository = A.Fake<IRegistryRepository>();
            IAccountRepository accountRepository = A.Fake<IAccountRepository>();

            var returnedRegistryRecord = new RegistryRecord();
            returnedRegistryRecord.Id = 1;

            var callToAddRecord = A.CallTo(() => registryRepository.AddRecord(
                A<RegistryRecord>.That.Matches(r => 
                    r.Date == postData.Date
                    && r.OperationType == operationType
                    && r.Amount == postData.Amount
                    && r.Category.Id == postData.CategoryId
                    && r.Subcategory.Id == postData.SubcategoryId
                    && r.IsSpecialEvent == isSpecialEvent
                    && r.IsTaxDeductible == isTaxDEductible
                    )                
                ));
            callToAddRecord.Returns(returnedRegistryRecord);

            controller.RegistryRepository = registryRepository;
            controller.AccountRepository = accountRepository;
            controller.Request = CreateRequest();

            // Act
            int newId = controller.Create(postData);

            // Assert
            callToAddRecord.MustHaveHappened(Repeated.Exactly.Once);
            newId.ShouldEqual(returnedRegistryRecord.Id);
        }
    }
}
