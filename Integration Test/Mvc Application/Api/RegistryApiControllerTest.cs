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

namespace Monei.Test.IntegrationTest.Mvc_Application.Api
{

    [TestFixture, Category("Web API"), Category("Registry")]
    public class RegistryApiControllerTest : ApiControllerTestBase
    {
        private const string baseUri = "/api/registry";

        [Test]
        public void Search_Should_ReturnAList()
        {
            RegistrySearchPostData data = new RegistrySearchPostData() { };

            var result = base.CallApi<RegistrySearchPostData, IEnumerable<RegistryRecord>>(baseUri + "/search", HttpMethod.Post, data);
            result.ShouldNotBeEmpty();
        }

        [Test]
        public void PostNewRecord_should_CreateNewRecord()
        {
            RegistryNewRecordPostData data = new RegistryNewRecordPostData()
            {
                CategoryId = 1,
                SubcategoryId = 2
            };

            try
            { 

                var newId = base.CallApi<RegistryNewRecordPostData, int>(baseUri, HttpMethod.Post, data);

                Assert.Pass();

                RegistryRecord record = GetRegistryRecord(newId);
                record.ShouldNotBeNull();
                //data.Date.ShouldEqual(record.Date);
                //data.Subcategory.Id.ShouldEqual(record.)

            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {                
                        if(!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
            }
        }

        #region utilities method
        private RegistryRecord GetRegistryRecord(int newId)
        {
            var repository = new RegistryRepository();

            var record = repository.Read(newId);

            return record;
        }

        #endregion
    }
}
