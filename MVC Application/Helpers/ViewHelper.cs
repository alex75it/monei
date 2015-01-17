using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Monei.MvcApplication.Helpers
{
	public static class ViewHelper
	{

		public static string GetDateFormat()
		{
			// {0:dd/MM/yyyy}

			return "{0:" + Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + "}";
		}

		public static string GetDatePlaceHolder()
		{
			// dd/MM/yyyy
			return Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
		}

		public static string ShowDate( DateTime dateTime)
		{
			return dateTime.ToShortDateString();
		}

		public static string ShowDate( DateTime? dateTime)
		{
			if (dateTime.HasValue)
				return dateTime.Value.ToShortDateString();
			else
				return "";

		}


	}
}