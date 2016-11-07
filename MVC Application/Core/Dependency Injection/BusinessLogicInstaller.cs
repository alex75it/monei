using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Monei.Core;
using Monei.Core.BusinessLogic;
using Castle.Core;

namespace Monei.MvcApplication.Core.DependencyInjection
{
    public class BusinessLogicInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IWindsorContainer thisContainer = container.Register(
                Component.For<IAccountManager>().ImplementedBy<AccountManager>().LifestylePerWebRequest(),
                Component.For<SubcategoryManager>().ImplementedBy<SubcategoryManager>().LifestylePerWebRequest(),                
                Component.For<ICategoryManager>().ImplementedBy<CategoryManager>().LifestylePerWebRequest(),
                Component.For<IRegistryManager>().ImplementedBy<RegistryManager>().LifestylePerWebRequest()
            );            
        }
    }
}