using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Monei.MvcApplication.Core.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	/// <see cref="http://blog.kerbyyoung.com/2013/01/setting-up-castle-windsor-for-aspnet.html"/>
	//public class WindsorDependencyResolver :IDependencyResolver
	//{
	//	private readonly IWindsorContainer container;

	//	public WindsorDependencyResolver(WindsorCastleDependencyInjection dependencyInjection)
	//	{
	//		this.container = container;
	//	}

	//	public IDependencyScope BeginScope()
	//	{
	//		return new WindsorDependencyScope(container);
	//	}

	//	public object GetService(Type serviceType)
	//	{
	//		if (container.Kernel.HasComponent(serviceType))
	//			return container.Resolve(serviceType);
	//		else
	//			return null;
	//	}

	//	public IEnumerable<object> GetServices(Type serviceType)
	//	{
	//		return container.ResolveAll(serviceType).Cast<object>();
	//	}

	//	public void Dispose()
	//	{
	//		container.Dispose();
	//	}
	//}
}