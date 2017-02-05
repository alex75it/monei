using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using log4net;

namespace Monei.MvcApplication.Filters
{
    /// <summary>
    /// This way does not capture 
    /// 1) Exceptions thrown from controller constructors.
    /// 2) Exceptions thrown from message handlers.
    /// 3) Exceptions thrown during routing.
    /// 4) Exceptions thrown during response content serialization.
    /// ref: http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling
    /// </summary>
    public class ApiControllerExceptionFilterAttribute :ExceptionFilterAttribute
    {
        public readonly ILog logger = LogManager.GetLogger(typeof(ApiControllerExceptionFilterAttribute));
        
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string error;
            if (actionExecutedContext.Exception is HttpResponseException)
            {
                error = ((HttpResponseException)actionExecutedContext.Exception).Response.ToString();

                error += "\r\nRequest: " + actionExecutedContext.Request.Content.ToString();
            }
            else
            {
                error = actionExecutedContext.Exception.ToString();
            }

            logger.ErrorFormat("Unhandled API controller error. {0}", error);

            base.OnException(actionExecutedContext);
        }

    }

}