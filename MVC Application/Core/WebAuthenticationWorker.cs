using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Monei.MvcApplication.Core
{
	public class WebAuthenticationWorker : IWebAuthenticationWorker
	{
		public void SetAuthenticationCookie(string username, bool persistCookie)
		{
			FormsAuthentication.SetAuthCookie(username, persistCookie);
		}        
	}
}
