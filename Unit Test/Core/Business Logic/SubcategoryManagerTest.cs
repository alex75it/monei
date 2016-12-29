using System;
using FakeItEasy;
using Monei.Core;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using NUnit.Framework;
using Should;
using Monei.Core.BusinessLogic;

namespace Monei.Test.UnitTest.Core.BusinessLogic
{
	[TestFixture, Category("Core")]
	public class SubcategoryManagerTest
	{
		[Test]
		public void IsUsed_ShouldREturnFalse()
		{
			int userId = 999;
			int subcategoryId = -98;
			SubcategoryManager manager = new SubcategoryManager();
			IRegistryRepository repository = A.Fake<IRegistryRepository>();
			manager.RegistryRepository = repository;

			bool result = manager.IsUsed(userId, subcategoryId);

			result.ShouldBeFalse();
		}
	}
}
