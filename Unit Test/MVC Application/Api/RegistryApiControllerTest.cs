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

namespace Monei.Test.UnitTest.MvcApplication.Api
{
    [TestFixture, Category("Web API"), Category("Registry")]
    public class RegistryApiControllerTest :ApiControllerBaseTest
    {
        private RegistryApiController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new RegistryApiController();
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
