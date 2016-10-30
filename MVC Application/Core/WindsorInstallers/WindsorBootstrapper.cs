using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Monei.MvcApplication.Core.WindsorInstallers
{
    public class WindsorBootstrapper :IDisposable
    {
        private IWindsorContainer container;

        public void Initialize()
        {
            container = new WindsorContainer();

            container.Install(
                new RepositoriesInstaller(),
                new ControllersInstaller()
            );
                        
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            var httpDependencyResolver = new WindsorDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = httpDependencyResolver;
        }

        public void Dispose()
        {
            if (container != null)
                container.Dispose();
        }
    }
}