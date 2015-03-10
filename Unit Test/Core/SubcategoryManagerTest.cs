using System;
using Monei.Core;
using NUnit.Framework;
using Should;


namespace Monei.Test.UnitTest.Core
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
			bool result = manager.IsUsed(userId, subcategoryId);

			result.ShouldBeFalse();
		}
	}
}
