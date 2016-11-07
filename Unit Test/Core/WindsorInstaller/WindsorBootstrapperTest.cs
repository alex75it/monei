using Monei.Core.BusinessLogic;
using Monei.MvcApplication.Controllers;
using Monei.MvcApplication.Core.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Should;

namespace Monei.Test.UnitTest.Core.DependencyInjection
{
    [TestFixture]
    public class WindsorCastleDependencyInjectionTest
    {
        public void Constructor_should_MakeBusinessLogicResolvable()
        {
            using (WindsorCastleDependencyInjection dependencyInjection = new WindsorCastleDependencyInjection())
            {
                IAccountManager accountManager = dependencyInjection.Resolve<IAccountManager>();
                accountManager.ShouldNotBeNull();
            }
        }


        //public void Initialize_should_MakeControllersResolvable()
        //{
        //    WindsorBootstrapper bootstrapper = new WindsorBootstrapper();

        //    bootstrapper.Initialize();

        //    AccountController controller = new WindsorContainer()


        //}
    }
}