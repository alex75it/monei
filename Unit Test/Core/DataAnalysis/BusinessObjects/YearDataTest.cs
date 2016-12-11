using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Core.DataAnalysis.BusinessObjects;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Core.DataAnalysis.BusinessObjects
{
	[TestFixture, Category("Data analysis")]
	public class YearDataTest
	{
		[Test]
		public void Ctor()
		{
			int year = DateTime.Today.Year;
			var data = new YearData(year);

			data.Year.ShouldEqual(year);
			data.Months.ShouldBeEmpty();
			data.Categories.ShouldBeNull();
			data.Income.ShouldEqual(0m);
			data.Outcome.ShouldEqual(0m);
			data.Result.ShouldEqual(0m);
		}
	}
}
