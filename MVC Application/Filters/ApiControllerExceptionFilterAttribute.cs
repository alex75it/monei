using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using log4net;

namespace Monei.MvcApplication.Filters
{
	public class ApiControllerExceptionFilterAttribute :ExceptionFilterAttribute
	{
		public /*static*/ /*readonly*/ ILog logger = LogManager.GetLogger(typeof(ApiControllerExceptionFilterAttribute));

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			logger.ErrorFormat("Not handled error. {0}", actionExecutedContext.Exception.ToString()); 
 
			base.OnException(actionExecutedContext);
		}

	}
}