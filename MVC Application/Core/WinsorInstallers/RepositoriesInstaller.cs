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

namespace Monei.MvcApplication.Core.Installers
{
	public class RepositoriesInstaller: IWindsorInstaller
	{
		public void Install(Castle.Windsor.IWindsorContainer container, IConfigurationStore store)
		{
			//typeof(BaseViewPage<T>

			container.Register(

				//Nhibernate session factory
				//Component.For<ISessionFactory>().UsingFactoryMethod(CreateNhSessionFactory).LifeStyle.Singleton,

				// Repositories
				Component.For(typeof(IAccountRepository)).ImplementedBy(typeof(AccountRepository)).LifestylePerWebRequest(),
				Component.For(typeof(IRegistryRepository)).ImplementedBy(typeof(RegistryRepository)).LifestylePerWebRequest(),
				Component.For(typeof(ICurrencyRepository)).ImplementedBy(typeof(CurrencyRepository)).LifestylePerWebRequest(),
				Component.For(typeof(ICategoryRepository)).ImplementedBy(typeof(CategoryRepository)).LifestylePerWebRequest(),
				Component.For(typeof(ISubcategoryRepository)).ImplementedBy(typeof(SubcategoryRepository)).LifestylePerWebRequest(),

				Component.For(typeof(SubcategoryManager)).ImplementedBy(typeof(SubcategoryManager)).LifestylePerWebRequest()

				//Component.For(typeof(BaseViewPage<T>)).Instance( new BaseViewPage())

				);


			//var a = container.Resolve<MoneiControllerBase>();

			//container.Resolve(typeof(BaseViewPage<T>));
		}
	}
}