using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel;
using Castle.Windsor;

namespace Monei.MvcApplication.DependencyInjection
{
    // ref: http://docs.castleproject.org/Windsor.Windsor-tutorial-part-two-plugging-Windsor-in.ashx

    public class WindsorControllerFactory :DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                string requestedPath = requestContext.HttpContext.Request.Path;
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestedPath));                
            }
            return (IController)kernel.Resolve(controllerType);
        }
    }
}