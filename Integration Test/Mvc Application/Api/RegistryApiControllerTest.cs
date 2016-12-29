using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.Test.IntegrationTest.MvcApplication.Api;
using NUnit.Framework;
using Should;
using Monei.DataAccessLayer.SqlServer;
using System.Reflection;
using System.IO;
using Monei.MvcApplication.Api;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{

    [TestFixture, Category("Web API"), Category("Registry")]
    public class RegistryApiControllerTest : ApiControllerTestBase<RegistryApiController>
    {
        private const string baseUri = "/api/registry";
        
        [Test]
        public void Search_should_ReturnAList()
        {
            RegistrySearchPostData data = new RegistrySearchPostData() { };

            var result = base.CallApi<RegistrySearchPostData, IEnumerable<RegistryRecord>>(baseUri + "/search", data);
            result.ShouldNotBeEmpty();
        }

        [Test]
        public void PostNewRecord_should_CreateANewRecord()
        {
            int categoryId = testDataProvider.GetTestCategory().Id;
            int subcategoryId = testDataProvider.GetTestSubcategory(categoryId).Id;

            RegistryNewRecordPostData data = new RegistryNewRecordPostData()
            {
                Date = DateTime.Today,
                Operation = OperationType.Income,
                Amount = 1.23m,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId,                
                Note = "Note",                
            };
            
            // Execute
            var newId = base.CallApi<RegistryNewRecordPostData, int>(baseUri, data);

            RegistryRecord record = GetRegistryRecord(newId);
            record.ShouldNotBeNull();
            record.Date.ShouldEqual(data.Date);
            record.OperationType.ShouldEqual(data.Operation);
            record.Amount.ShouldEqual(data.Amount);
            record.Category.Id.ShouldEqual(data.CategoryId);
            record.Subcategory.Id.ShouldEqual(data.SubcategoryId);
            record.Note.ShouldEqual(data.Note);            
        }

        #region utility methods
        private RegistryRecord GetRegistryRecord(int newId)
        {
            var repository = new RegistryRepository(sessionFactoryProvider);

            var record = repository.Read(newId);

            return record;
        }

        #endregion
    }
}
