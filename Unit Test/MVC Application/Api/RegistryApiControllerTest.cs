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
        }
        
        [Test]
        public void Create/*_should_CallTheRegistryMethodForCreateNewRecord*/ ()
        {
            // Arrange
            RegistryRecord record = new RegistryRecord();
            
            IRegistryRepository registryRepository = A.Fake<IRegistryRepository>();

            var returnedRegistryRecord = new RegistryRecord();
            returnedRegistryRecord.Id = 1;

            var callToAddRecord = A.CallTo(() => registryRepository.AddRecord(record));
            callToAddRecord.Returns(returnedRegistryRecord);
            
            controller.RegistryRepository = registryRepository;

            // Act
            int newId = controller.Create(record);

            // Assert
            callToAddRecord.MustHaveHappened(Repeated.Exactly.Once);
            newId.ShouldEqual(returnedRegistryRecord.Id);
        }  
    }
}
