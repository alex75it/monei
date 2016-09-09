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
        public void Create/*_should_CallTheRegistryMethodForCreateNewRecord*/ ()
        {
            // Arrange            
            RegistryNewRecordPostData postData = new RegistryNewRecordPostData()
            {

            };

            IRegistryRepository registryRepository = A.Fake<IRegistryRepository>();
            IAccountRepository accountRepository = A.Fake<IAccountRepository>();

            var returnedRegistryRecord = new RegistryRecord();
            returnedRegistryRecord.Id = 1;

            var callToAddRecord = A.CallTo(() => registryRepository.AddRecord(A.Fake<RegistryRecord>()));
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
