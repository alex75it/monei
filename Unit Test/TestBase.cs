using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Test.UnitTest
{
	public class TestBase
	{
		protected DateTime minSqlDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
		protected DateTime maxSqlDate = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;

		protected bool IsValidSqlDate(DateTime date)
		{
			return date >= minSqlDate
			       && date <= maxSqlDate;
		}

	}
}
