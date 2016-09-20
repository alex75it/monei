using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Api;
using NUnit.Framework;
using Should;
using Monei.MvcApplication.Api.PostDataObjects;

namespace Monei.Test.UnitTest.MvcApplication.Api
{
    [TestFixture, Category("Web API"), Category("Registry")]
    public class RegistryApiControllerTest
    {
        private RegistryApiController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new RegistryApiController();
            IAccountRepository accountRepository = A.Fake<IAccountRepository>();
            Account account = new Account() {
                Id=1,
                Guid = Guid.NewGuid(),
                Username = "test",
            };
            A.CallTo(() => accountRepository.Read(A.Dummy<string>())).Returns(account);
            controller.AccountRepository = accountRepository;
        }

        [Test]
        public void Create_when_DateIsNotDefined_should_ThrowAnException()
        {
            RegistryRecord record = new RegistryRecord();

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Amount = 0,
                Operation = OperationType.Income,
                CategoryId = 1,
                Note = "aaa",
            };

            A.CallTo(controller.Create(postData)).Throws<Exception>();
        }
        
        [Test]
        public void Create_when_OperationIsNotDefined_should_ThrowAnException()
        {
            RegistryRecord record = new RegistryRecord();

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = DateTime.Now,
                Amount = 0,
                CategoryId = 1,
                Note = "aaa",
            };

            A.CallTo(controller.Create(postData)).Throws<Exception>();
        }

        [Test]
        [TestCase(OperationType.Income)]
        [TestCase(OperationType.Outcome)]
        public void Create_when_AmountIsZeroAndOperationTypeIsNotTransfer_should_ThrowAnException(OperationType operation)
        {
            RegistryRecord record = new RegistryRecord();

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = DateTime.Now,
                Operation = operation,
                Amount = 0,                
                CategoryId = 1,
                Note = "aaa",
            };

            A.CallTo(controller.Create(postData)).Throws<Exception>();
        }

        [Test]
        public void Create_when_CategoryIsNotDefined_should_ThrowAnException()
        {
            RegistryRecord record = new RegistryRecord();

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = DateTime.Now,
                Operation = OperationType.Income,
                Amount = 0,
                CategoryId = 1,
                Note = "aaa",
            };

            A.CallTo(controller.Create(postData)).Throws<Exception>();
        }

        [Test]
        public void Create_should_CallRepositoryWithRightData()
        {
            IRegistryRepository registryRepositoryMock = A.Fake<IRegistryRepository>();
            controller.RegistryRepository = registryRepositoryMock;

            RegistryRecord record = new RegistryRecord();

            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {
                Date = DateTime.Now,
                Amount = 1m,
                Operation = OperationType.Income,
                CategoryId = 1,
                Note = "aaa",
            };

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
    }
}
