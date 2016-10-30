using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Monei.Core;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.MvcApplication.Controllers;

namespace Monei.MvcApplication.Core.WindsorInstallers
{
    public class RepositoriesInstaller: IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                Component.For<ISessionFactoryProvider>().ImplementedBy<SessionFactoryProvider>().LifeStyle.Singleton,
                                
                Component.For<IAccountRepository>().ImplementedBy<AccountRepository>().LifestylePerWebRequest(),
                Component.For<IRegistryRepository>().ImplementedBy<RegistryRepository>().LifestylePerWebRequest(),
                Component.For<ICurrencyRepository>().ImplementedBy<CurrencyRepository>().LifestylePerWebRequest(),
                Component.For<ICategoryRepository>().ImplementedBy<CategoryRepository>().LifestylePerWebRequest(),
                Component.For<ISubcategoryRepository>().ImplementedBy<SubcategoryRepository>().LifestylePerWebRequest()
            );
        }
    }
}