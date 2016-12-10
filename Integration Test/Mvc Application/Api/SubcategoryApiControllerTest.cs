using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using FakeItEasy;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using Monei.MvcApplication.Api;
using NUnit.Framework;
using Should;
using Monei.MvcApplication.Controllers.Api.PostDataObjects;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
    [TestFixture, Category("Web API"), Category("Subcategory")]
    public class SubcategoryApiControllerTest : ApiControllerTestBase<SubcategoryApiController>
    {
        private const string ROUTE_PREFIX = "api/subcategory/";

        [Test]
        public void Ping_Should_RespondOk()
        {
            base.CallApi(ROUTE_PREFIX + "ping", HttpMethod.Get);
        }

        [Test]
        public void SearchWithUnexistentCategory_Should_ReturnAnEmptyList()
        {
            //Arrange
            const string url = ROUTE_PREFIX + "category/" + "-666";

            // Act
            var returnedList = CallApi<IEnumerable<Subcategory>>(url, HttpMethod.Get);

            // Assert
            returnedList.ShouldNotBeNull("fail for url \"" + url + "\"");
            returnedList.ShouldBeEmpty();
        }

        [Test]
        public void Search_Should_ReturnAList()
        {
            //Arrange
            var categoryId = new CategoryRepository(sessionFactoryProvider).List().First().Id;
            string url = ROUTE_PREFIX + "category/" + categoryId;

            // Act
            var returnedList = CallApi<IEnumerable<Subcategory>>(url, HttpMethod.Get);

            // Assert
            returnedList.ShouldNotBeNull();
            returnedList.ShouldNotBeEmpty();
        }

        [Test]
        public void Create_Should_ReturnOk()
        {
            // Arrange
            int categoryId = new CategoryRepository(sessionFactoryProvider).List().First().Id;
            ISubcategoryRepository repository = new SubcategoryRepository(sessionFactoryProvider);

            SubcategoryPostData postData = new SubcategoryPostData()
            {
                Name = "Test " + RandomInt(),
                Description = "Description test",
                CategoryId = categoryId
            };

            // todo: create post data from JSON

            //Act
            int newId = 0;
            try
            {
                newId = CallApi<SubcategoryPostData, int>(ROUTE_PREFIX, postData);

                // Assert

                Subcategory loadedSubcategory = repository.Read(newId);

                Assert.IsNotNull(loadedSubcategory);
                //loadedSubcategory.Name.Sould
                Assert.AreEqual(postData.Name, loadedSubcategory.Name);
                Assert.AreEqual(postData.Description, loadedSubcategory.Description);
            }
            finally
            {
                if (newId != 0)
                    repository.Delete(newId);
            }
        }


        [Test]
        public void Delete_Should_DeleteTheSubcategory()
        {
            Assert.Inconclusive("Not implemented");

            int id = 0;
            string url = ROUTE_PREFIX + "/" + id;
            CallApi(ROUTE_PREFIX, DELETE);
        }
    }
}
