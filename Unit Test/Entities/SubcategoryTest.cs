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
	public class SubcategoryTest
	{
		[Test]
		public void Equal()
		{
			Subcategory sub_1 = new Subcategory() { Id = 1 };
			Subcategory sub_2 = new Subcategory() { Id = 2 };
			Subcategory sub_1bis = new Subcategory() { Id = 1 };

			Assert.IsFalse(sub_1.Equals(null));
			sub_1.ShouldNotEqual(sub_2);
			sub_1.ShouldEqual(sub_1bis);
		}
	}
}
