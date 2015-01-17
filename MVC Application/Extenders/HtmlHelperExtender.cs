using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using Monei.MvcApplication.Helpers;

// "Extenders" was removed from namespace for comodity
namespace Monei.MvcApplication
{
	public static class HtmlHelperExtender
	{

		//public static string ActionLink(this HtmlHelper helper, string imagePath, string text)
		//{
		//	return helper.ActionLink(linkText, 
		//}

		public static string ShowDate(this HtmlHelper helper, DateTime dateTime)
		{
			return ViewHelper.ShowDate(dateTime);
		}


		public static string ShowDate(this HtmlHelper helper, DateTime? dateTime)
		{
			return ViewHelper.ShowDate(dateTime);
		}

	}
}