using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;

namespace Monei.Test.IntegrationTest.Installers
{

	class RepositoriesInstaller : IWindsorInstaller
	{
		public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
		{
			container.Register(			
				//Component.IsInNamespace("Monei.Tests.DataAccessLayer.Repository")
				Component.For<IAccountRepository>().Instance(new AccountRepository()).LifestyleTransient(),
				Component.For<ICurrencyRepository>().Instance(new CurrencyRepository()).LifestyleTransient(),
				Component.For<IRegistryRepository>().UsingFactoryMethod(() => new RegistryRepository()).LifestyleTransient(),				
				Component.For<ICategoryRepository>().Instance(new CategoryRepository()).LifestyleTransient(),
				Component.For<ISubcategoryRepository>().Instance(new SubcategoryRepository()).LifestyleTransient()
				);
		}
	}

}
