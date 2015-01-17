using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Tests.Installers;


namespace Monei.Tests.DataAccessLayer.Repository
{
	[TestClass]
	public class RepositoryBaseTest
	{		
		
		public TestHelper Helper { get; set; }
		public IAccountRepository AccountRepository { get; set; }			
		public ICurrencyRepository CurrencyRepository { get; set; }
		public IRegistryRepository RegistryRepository { get; set; }
		public ICategoryRepository CategoryRepository { get; set; }
		public ISubcategoryRepository SubcategoryRepository { get; set; }

		[TestInitialize()]
		public void Initialize()
		{ 
			IWindsorContainer container = new WindsorContainer().Install(
				new RepositoriesInstaller(),
				FromAssembly.This()
				);
			
			AccountRepository = container.Resolve<IAccountRepository>();
			CurrencyRepository = container.Resolve<ICurrencyRepository>();
			RegistryRepository = container.Resolve<IRegistryRepository>();
			CategoryRepository = container.Resolve<ICategoryRepository>();
			SubcategoryRepository = container.Resolve<ISubcategoryRepository>();
						

			container.Register(				
				//Component.For<IAccountRepository>().UsingFactoryMethod(() => new Monei.DataAccessLayer.SqlServer.AccountRepository()).LifestyleTransient(),
				//Component.For<IRegistryRepository>().UsingFactoryMethod(() => new Monei.DataAccessLayer.SqlServer.RegistryRepository()).LifestyleTransient(),
				//Component.For<ICurrencyRepository>().Instance(new Monei.DataAccessLayer.SqlServer.CurrencyRepository()).LifestyleTransient(),
				//Component.For<TestHelper>().DependsOn<TestHelper>() // .Instance( typeof(TestHelper))
				Component.For<RepositoryBaseTest>().Instance( new RepositoryBaseTest()),
				Component.For<TestHelper>().ImplementedBy<TestHelper>()
					.DependsOn(Dependency.OnValue<IAccountRepository>(AccountRepository))
					.DependsOn(Dependency.OnValue<ICurrencyRepository>(CurrencyRepository))
					.DependsOn(Dependency.OnValue<ICategoryRepository>(CategoryRepository))
					.DependsOn(Dependency.OnValue<ISubcategoryRepository>(CategoryRepository))
				);

			//Helper = new TestHelper(CurrencyRepository)
			Helper = container.Resolve<TestHelper>();
									
		}

	}
}
