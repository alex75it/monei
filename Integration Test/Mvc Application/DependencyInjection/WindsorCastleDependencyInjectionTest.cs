using Monei.Core.BusinessLogic;
using Monei.MvcApplication.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Should;
using Monei.MvcApplication.Core;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Controllers;
using Monei.MvcApplication.Api;
using Monei.DataAccessLayer.SqlServer;
using Castle.MicroKernel.ModelBuilder;
using Castle.Core;
using Castle.MicroKernel;

namespace Monei.Test.IntegrationTest.MvcApplication.DependencyInjection
{
    /// Thgese tests cannto be Unit tests because the SessionProvider require a real working configuration

    [TestFixture, Category("Dependency Injection")]
    public class WindsorCastleDependencyInjectionTest
    {
        private LifestyleSingletonComponentModelContruction lifestyleSingletonComponentModelConstruction = new LifestyleSingletonComponentModelContruction();

        [Test]
        public void Constructor_should_MakeBusinessLogicComponentsResolvable()
        {
            using (WindsorCastleDependencyInjection dependencyInjection = new WindsorCastleDependencyInjection(lifestyleSingletonComponentModelConstruction))
            {
                IAccountManager accountManager = dependencyInjection.Resolve<IAccountManager>();
                IWebAuthenticationWorker webAuthenticationWorker = dependencyInjection.Resolve<IWebAuthenticationWorker>();
                IAccountSecurity accountSecurity = dependencyInjection.Resolve<IAccountSecurity>();
                accountManager.ShouldNotBeNull();
                webAuthenticationWorker.ShouldNotBeNull();
            }
        }

        [Test]
        public void Constructor_should_MakeRepositoyComponentsResolvable()
        {
            using (WindsorCastleDependencyInjection dependencyInjection = new WindsorCastleDependencyInjection(lifestyleSingletonComponentModelConstruction))
            {
                IAccountRepository accountRepository = dependencyInjection.Resolve<IAccountRepository>();
                ISessionFactoryProvider sessionFActoryProvider = dependencyInjection.Resolve<ISessionFactoryProvider>();

                accountRepository.ShouldNotBeNull();
                sessionFActoryProvider.ShouldNotBeNull();
            }
        }

        [Test]
        public void Constructor_should_MakeControllersResolvable()
        {
            using (WindsorCastleDependencyInjection dependencyInjection = new WindsorCastleDependencyInjection(lifestyleSingletonComponentModelConstruction))
            {
                AccountController accountController = dependencyInjection.Resolve<AccountController>();
                RegistryController registryController = dependencyInjection.Resolve<RegistryController>();

                accountController.ShouldNotBeNull();
                registryController.ShouldNotBeNull();
            }
        }

        [Test]
        public void Constructor_should_MakeApiControllersResolvable()
        {
            using (WindsorCastleDependencyInjection dependencyInjection = new WindsorCastleDependencyInjection(lifestyleSingletonComponentModelConstruction))
            {
                AccountApiController accountController = dependencyInjection.Resolve<AccountApiController>();
                RegistryApiController registryController = dependencyInjection.Resolve<RegistryApiController>();

                accountController.ShouldNotBeNull();
                registryController.ShouldNotBeNull();
            }
        }
    }

    internal class LifestyleSingletonComponentModelContruction : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            model.LifestyleType = LifestyleType.Singleton;
        }
    }
}