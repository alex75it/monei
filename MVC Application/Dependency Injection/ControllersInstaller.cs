using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Monei.DataAccessLayer.Interfaces;
using Monei.MvcApplication.Api;
using Monei.MvcApplication.Controllers;
using Monei.MvcApplication.Core;

namespace Monei.MvcApplication.DependencyInjection
{
    public class ControllersInstaller :IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                // lifeStyleTransient, lifeStylePerWebRequest, lifeStyleScoped... why one or the other?
                // LifestylePerWebRequest to prevent memory leak caused by the non-release of managed components 

                Classes.FromThisAssembly().BasedOn<IController>().LifestylePerWebRequest(),
                Classes.FromThisAssembly().BasedOn<MoneiControllerBase>().LifestylePerWebRequest(),
                Classes.FromThisAssembly().BasedOn<ApiControllerBase>().LifestylePerWebRequest(),			
                
                Component.For<IWebAuthenticationWorker>().ImplementedBy<WebAuthenticationWorker>().LifestylePerWebRequest()
            );
        }

    }
}