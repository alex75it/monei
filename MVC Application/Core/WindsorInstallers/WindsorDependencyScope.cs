using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.Windsor;


namespace Monei.MvcApplication.Core.WindsorInstallers
{
	public class WindsorDependencyScope :IDependencyScope
	{
		private readonly IWindsorContainer container;
		private readonly IDisposable scope;

		public WindsorDependencyScope(IWindsorContainer container)
		{
			this.container = container;
			//this.scope = container.BeginScope();
			this.scope = new CallContextLifetimeScope(container);
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

		public void Dispose()
		{
			scope.Dispose();
		}
	}
}