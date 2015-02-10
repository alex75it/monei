using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using log4net;

namespace Monei.MvcApplication.Core
{
	/// <summary>
	/// ref: http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling
	/// </summary>
	public class MoneiExceptionLogger :ExceptionLogger
	{
		private ILog logger = LogManager.GetLogger(typeof(MoneiExceptionLogger));
		public override void Log(ExceptionLoggerContext context)
		{
			logger.ErrorFormat("Unhandled error from Uri " + context.Request.RequestUri, context.Exception);

			base.Log(context);
		}

		public override System.Threading.Tasks.Task LogAsync(ExceptionLoggerContext context, System.Threading.CancellationToken cancellationToken)
		{
			logger.ErrorFormat("Unhandled error from Uri " + context.Request.RequestUri, context.Exception);

			return base.LogAsync(context, cancellationToken);
		}

	}
}