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
using Monei.MvcApplication.Controllers.Api;

namespace Monei.MvcApplication.Core.Installers
{
    public class ControllerInstaller :IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                // lifeStyleTransient, lifeStylePerWebRequest, lifeStyleScoped... why one or the other?

                Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient(),
                Classes.FromThisAssembly().BasedOn<MoneiControllerBase>().LifestyleTransient(),
                Classes.FromThisAssembly().BasedOn<ApiControllerBase>().LifestyleTransient(), // .LifestyleScoped()			
                
                Component.For<IWebAuthenticationWorker>().ImplementedBy<WebAuthenticationWorker>()
            );
        }

    }
}