using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Extensions
{
	public static class DateTimeExtensions
	{
		public static DateTime NormalizeForSql(this DateTime date)
		{
			if (date < System.Data.SqlTypes.SqlDateTime.MinValue.Value)
				date = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
			else if (date > System.Data.SqlTypes.SqlDateTime.MaxValue.Value)
				date = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;

			return date;
		}
	}
}
