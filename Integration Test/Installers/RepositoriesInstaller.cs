using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Test.IntegrationTest.DataAccessLayer.SqlServer;

namespace Monei.Test.IntegrationTest.Installers
{
    class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {           
            container.Register(

                Component.For<ISessionFactoryProvider>().ImplementedBy<SessionFactoryProvider>().LifestyleTransient(),

                Component.For<IAccountRepository>().ImplementedBy<AccountRepository>().LifestyleTransient(),
                Component.For<ICurrencyRepository>().ImplementedBy<CurrencyRepository>().LifestyleTransient(),
                Component.For<IRegistryRepository>().ImplementedBy<RegistryRepository>().LifestyleTransient(),
                Component.For<ICategoryRepository>().ImplementedBy<CategoryRepository>().LifestyleTransient(),
                Component.For<ISubcategoryRepository>().ImplementedBy<SubcategoryRepository>().LifestyleTransient()
            );
        }
    }

}
