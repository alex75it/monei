using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Core.DataAnalysis.BusinessObjects;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Core.DataAnalysis.BusinessObjects
{
	[TestFixture]
	public class CategoryDataTest
	{
		[Test]
		public void Ctor()
		{
			Category category = new Category();
			category.Name = "Cat A";
			decimal amount = 123m;

			var data = new CategoryData(category, amount);

			data.Amount.ShouldEqual(amount);
			data.Name.ShouldEqual(category.Name);
			data.Id.ShouldEqual(0);
		}
	}
}
