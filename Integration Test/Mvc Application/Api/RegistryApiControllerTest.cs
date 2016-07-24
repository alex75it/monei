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

namespace Monei.Test.IntegrationTest.Mvc_Application.Api
{

	[TestFixture, Category("Web API"), Category("Registry")]
	public class RegistryApiControllerTest : ApiControllerTestBase
	{
		private const string baseUri = "/api/registry/";

		[Test]
		public void Search_Should_ReturnAList()
		{
			RegistrySearchPostData data = new RegistrySearchPostData() { };

			var result = base.CallApi<RegistrySearchPostData, IEnumerable<RegistryRecord>>(baseUri + "search", HttpMethod.Post, data);
			result.ShouldNotBeEmpty();
		}

	}
}
