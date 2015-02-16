using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Extensions;

namespace Monei.DataAccessLayer.Filters
{
	public class BaseFilters
	{

		public void NormalizeDate(DateTime date)
		{
			date.NormalizeForSql();
		}

		public void NormalizeDates(IEnumerable<DateTime> dates)
		{
			foreach(var date in dates)
				date.NormalizeForSql();
		}

	}
}
