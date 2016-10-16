using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
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

        public IAccountRepository AccountRepository { get { return Helper.AccountRepository; } }
        public ICurrencyRepository CurrencyRepository { get { return Helper.CurrencyRepository; } }
        public IRegistryRepository RegistryRepository { get { return Helper.RegistryRepository; } }
        public ICategoryRepository CategoryRepository { get { return Helper.CategoryRepository; } }
        public ISubcategoryRepository SubcategoryRepository { get { return Helper.SubcategoryRepository; } }

        [OneTimeSetUp]
        public void Initialize()
        {
        //	try
        //	{
        		IWindsorContainer container = new WindsorContainer();
        		container.Install(
        			new RepositoriesInstaller(),
        			FromAssembly.This()
        			);

        		container.Register(
        			Component.For<TestHelper>().ImplementedBy<TestHelper>()
        		);

        		Helper = container.Resolve<TestHelper>();

        //	}
        //	catch (Exception exc)
        //	{
        //		Console.WriteLine("Error: " + exc);
        //		throw;
        //	}

        }

    }
}
