using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using Should;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
	[TestClass]
	public class CategoryRepositoryTest
	{
		[TestMethod]
		public void List()
		{
			CategoryRepository repository = new CategoryRepository();

			IEnumerable<Category> list = repository.List();

			// Verify
			list.ShouldNotBeEmpty();
			
		}
	}
}
