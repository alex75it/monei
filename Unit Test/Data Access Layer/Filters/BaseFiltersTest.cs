﻿using System;
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
		public void NormalizeDate_WhenDateIsMinDate()
		{
			BaseFilters filters = new BaseFilters();
			DateTime date = DateTime.MinValue;

			date = filters.NormalizeDate(date);

			date.ShouldBeValidSqlDate();
		}

		[Test]
		public void NormalizeDate_WhenDateIsMaxDate()
		{
			BaseFilters filters = new BaseFilters();
			DateTime date = DateTime.MaxValue;

			date = filters.NormalizeDate(date);

			date.ShouldBeValidSqlDate();
		}

	}
}
