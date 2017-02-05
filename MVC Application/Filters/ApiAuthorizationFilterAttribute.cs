using Monei.MvcApplication.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Monei.MvcApplication.Filters
{
    public class ApiAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            ApiControllerBase controller = actionContext.ControllerContext.Controller as ApiControllerBase;

            if(controller == null)
                throw new Exception($"This controller must inherits from {nameof(ApiControllerBase)}.");

            if (controller.CurrentAccount == null)
                throw new Exception("Account not found");

            //base.OnAuthorization(actionContext);
        }
    }
}