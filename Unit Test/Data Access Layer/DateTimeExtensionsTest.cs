using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Extensions;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.DataAccessLayer
{
	[TestFixture]
	public class DateTimeExtensionsTest
	{

		[Test]
		public void NormalizeForSql_WithMinDate_Should_Change_Date_As_MinSqlDate()
		{
			DateTime date = DateTime.MinValue;

			date = date.NormalizeForSql();

			date.ShouldEqual(System.Data.SqlTypes.SqlDateTime.MinValue.Value);
		}

		[Test]
		public void NormalizeForSql_WithMaxDate_Should_Change_Date_As_MaxSqlDate()
		{
			DateTime date = DateTime.MaxValue;

			date = date.NormalizeForSql();

			date.ShouldEqual(System.Data.SqlTypes.SqlDateTime.MaxValue.Value);
		}

	}
}
