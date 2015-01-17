using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Helpers
{
	public static class Utils
	{
		/// <summary>
		/// Return the first day of the month.
		/// Default month is current one.
		/// </summary>
		/// <param name="month"></param>
		/// <returns></returns>
		public static DateTime GetFirstDayOfMonth(int month = 0){
			return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 01);
		}

		/// <summary>
		/// Return the date of the last day in month.
		/// Default month is current one.
		/// </summary>
		/// <param name="month"></param>
		/// <returns></returns>
		public static DateTime GetLastDayOfMonth(/*int month = 0*/)
		{
			int year = DateTime.Today.Year;
			int month = DateTime.Today.Month;
			if (month < 12)
			{
				month += 1;
			}
			else
			{
				year += 1;
				month = 1;
			}
			
			return new DateTime(year, month, 01).AddDays(-1);
		}

		public static void ThrowException(string template, params object[] values)
		{
			throw new Exception(string.Format(template, values));
		}

		public static void ThrowException(Exception innerException, string template, params object[] values)
		{
			throw new Exception(string.Format(template, values), innerException);
		}


	}
}