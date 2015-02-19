using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;

namespace Should
{
	public static class ShouldExtensions
	{
		public static void ShouldBeValidSqlDate(this DateTime date)
		{
			date.ShouldBeInRange(System.Data.SqlTypes.SqlDateTime.MinValue.Value, System.Data.SqlTypes.SqlDateTime.MaxValue.Value);
		}

	}
}
