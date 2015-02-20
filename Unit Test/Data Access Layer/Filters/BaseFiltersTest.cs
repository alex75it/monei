using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Filters;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Data_Access_Layer.Filters
{
	[TestFixture]
	public class BaseFiltersTest :TestBase
	{
		[Test]
		public void Normalize_WhenDateIsMinDate()
		{
			BaseFilters filters = new BaseFilters();
			DateTime date = DateTime.MinValue;

			filters.NormalizeDate(date);

			date.ShouldBeValidSqlDate();
		}

		[Test]
		public void Normalize_WhenDateIsMaxDate()
		{
			BaseFilters filters = new BaseFilters();
			DateTime date = DateTime.MaxValue;

			filters.NormalizeDate(date);

			date.ShouldBeValidSqlDate();
		}

	}
}
