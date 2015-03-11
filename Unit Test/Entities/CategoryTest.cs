using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Entities
{
	[TestFixture]
	public class CategoryTest
	{
		[Test]
		public void Equal()
		{
			Category cat_1 = new Category() { Id = 1 };
			Category cat_2 = new Category() { Id = 2 };
			Category cat_1bis = new Category() { Id = 1 };

			Assert.IsFalse(cat_1.Equals(null));
			cat_1.ShouldNotEqual(cat_2);
			cat_1.ShouldEqual(cat_1bis);
		}
	}
}
