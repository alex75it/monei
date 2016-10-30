﻿using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Monei.Core;
using Monei.Core.BusinessLogic;

namespace Monei.MvcApplication.Core.WindsorInstallers
{
    public class BusinessLogicInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<SubcategoryManager>().ImplementedBy<SubcategoryManager>().LifestylePerWebRequest()
                //Component.For<IAccountManager>().ImplementedBy<AccountManager>().LifestylePerWebRequest(),
                //Component.For<ICategoryManager>().ImplementedBy<CategoryManager>().LifestylePerWebRequest(),
            );
        }
    }
}