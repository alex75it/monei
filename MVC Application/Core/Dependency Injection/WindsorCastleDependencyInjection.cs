using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Dependencies;
using Castle.Core;
using Castle.MicroKernel.ModelBuilder;

namespace Monei.MvcApplication.Core.DependencyInjection
{
    /// <summary>
    /// Manage Dependency Injection using Windsor Castle
    /// </summary>
    public class WindsorCastleDependencyInjection : IDisposable, System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private IWindsorContainer container;

        public WindsorCastleDependencyInjection(IContributeComponentModelConstruction contributor = null)
        {
            container = new WindsorContainer();

            if(contributor != null)
                container.Kernel.ComponentModelBuilder.AddContributor(contributor);

            try
            {
                Initialize();
            }
            catch (Exception exc)
            {
                throw new Exception("WindsorContainer fails to install components.", exc);
            }
        }

        private void Initialize()
        {          
            container.Install(
                new BusinessLogicInstaller(),
                new RepositoriesInstaller(),
                new ControllersInstaller()
            );
                        
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            //var httpDependencyResolver = new WindsorDependencyResolver(container);
            //GlobalConfiguration.Configuration.DependencyResolver = this; // httpDependencyResolver;
        }

        public T Resolve<T>()
        {
            try
            {
                return container.Resolve<T>();
            }
            catch (Exception exc)
            {
                throw new Exception("Fail to resolve \"" + typeof(T) + "\".", exc);
            }
        }

        public void Dispose()
        {
            if (container != null)
                container.Dispose();
        }
        public object GetService(Type serviceType)
        {
            if (container.Kernel.HasComponent(serviceType))
                return container.Resolve(serviceType);
            else
                return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.ResolveAll(serviceType).Cast<object>();
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(container);
        }
    }
}