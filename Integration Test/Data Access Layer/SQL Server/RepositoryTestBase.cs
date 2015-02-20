using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Test.IntegrationTest.Installers;
using NUnit.Framework;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
	[TestFixture]
	public class RepositoryTestBase
	{		
		protected TestHelper Helper { get; set; }

		public IAccountRepository AccountRepository { get; set; }
		public ICurrencyRepository CurrencyRepository { get; set; }
		public IRegistryRepository RegistryRepository { get; set; }
		public ICategoryRepository CategoryRepository { get; set; }
		public ISubcategoryRepository SubcategoryRepository { get; set; }


		[TestFixtureSetUp]
		public void Initialize()
		{
			Console.WriteLine("Initialize");

			try
			{
				IWindsorContainer container = new WindsorContainer();
				container.Install(
					new RepositoriesInstaller(),
					FromAssembly.This()
					);
				
				AccountRepository = container.Resolve<IAccountRepository>();
				CurrencyRepository = container.Resolve<ICurrencyRepository>();
				RegistryRepository = container.Resolve<IRegistryRepository>();
				CategoryRepository = container.Resolve<ICategoryRepository>();
				SubcategoryRepository = container.Resolve<ISubcategoryRepository>();


				container.Register(
					Component.For<TestHelper>().ImplementedBy<TestHelper>()
				);

				Helper = container.Resolve<TestHelper>();

			}
			catch (Exception exc)
			{
				Console.WriteLine("Error: " + exc);
				throw;
			}

		}

	}
}
