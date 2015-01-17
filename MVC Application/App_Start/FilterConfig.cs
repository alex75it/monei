using System.Web;
using System.Web.Mvc;
using Monei.MvcApplication.Filters;

namespace Monei.MvcApplication
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());			
		}
	}
}