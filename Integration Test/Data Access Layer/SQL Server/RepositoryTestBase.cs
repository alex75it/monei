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
using NUnit.Framework;
using Monei.MvcApplication.Core.Installers;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel;
using Castle.Core;

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
        	IWindsorContainer container = new WindsorContainer();

            // prevent the error of PerWebREquestStyle module missing because there is not a web request.
            //container.Kernel.ComponentModelBuilder.AddContributor(new SingletonEqualizer());

            container.Install(
        		new RepositoriesInstaller()
        		//FromAssembly.This()
        		);

        	container.Register(
        		Component.For<TestHelper>().ImplementedBy<TestHelper>()
        	);

        	Helper = container.Resolve<TestHelper>();
        }

        public class SingletonEqualizer :IContributeComponentModelConstruction
        {
            public void ProcessModel(IKernel kernel, ComponentModel model)
            {
                model.LifestyleType = LifestyleType.Singleton;
            }
        }

    }
}
