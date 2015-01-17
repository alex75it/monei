using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Monei.DataAccessLayer.Interfaces;

namespace Monei.Tests.Installers
{
	class RepositoriesInstaller : IWindsorInstaller
	{
		public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
		{
			container.Register(			
				//Component.IsInNamespace("Monei.Tests.DataAccessLayer.Repository")
				Component.For<IAccountRepository>().UsingFactoryMethod(() => new Monei.DataAccessLayer.SqlServer.AccountRepository()).LifestyleTransient(),
				Component.For<ICurrencyRepository>().Instance(new Monei.DataAccessLayer.SqlServer.CurrencyRepository()).LifestyleTransient(),
				Component.For<IRegistryRepository>().UsingFactoryMethod(() => new Monei.DataAccessLayer.SqlServer.RegistryRepository()).LifestyleTransient(),				
				Component.For<ICategoryRepository>().Instance(new Monei.DataAccessLayer.SqlServer.CategoryRepository()).LifestyleTransient(),
				Component.For<ISubcategoryRepository>().Instance(new Monei.DataAccessLayer.SqlServer.SubcategoryRepository()).LifestyleTransient()
				);
		}
	}
}
