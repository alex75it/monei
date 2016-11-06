using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using log4net;
using Monei.MvcApplication.Core.DependencyInjection;
using Monei.Entities;
using Monei.DataAccessLayer.Interfaces;

namespace Monei.MvcApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {       

        public static WindsorCastleDependencyInjection DependencyInjectionManager { get; private set; }
              
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CultureDelegatingHandler());
            //config.MessageHandlers.Add(new CultureDelegatingHandler());

            // http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();

            // http://stackoverflow.com/questions/32852440/dependency-injection-in-asp-net-session-start-method   
            
            DependencyInjectionManager = new WindsorCastleDependencyInjection();
            GlobalConfiguration.Configuration.DependencyResolver = DependencyInjectionManager;
        }        

        private void Session_Start(object sender, EventArgs e)
        {
            Session["Account"] = null;
        }

        private void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                IAccountRepository accountRepository = DependencyInjectionManager.Resolve<IAccountRepository>();
                Account account = accountRepository.Read(Request.LogonUserIdentity.Name);
                Session["Account"] = account;
            }
            catch (Exception exc)
            {
                throw new Exception("Fail to obtain Account from authenticated user.", exc);
            }
        }

        protected void Application_End()
        {
            if(DependencyInjectionManager != null)
                DependencyInjectionManager.Dispose();
        }
        
        protected void Application_Error()
        {
            ILog logger = LogManager.GetLogger(this.GetType());
            Exception exception = Server.GetLastError();
            logger.Error("Applicaton Error.", exception);

            if (exception is HttpException)
            {
                HttpStatusCode statusCode = (System.Net.HttpStatusCode)(exception as HttpException).GetHttpCode();
                Server.TransferRequest("Error/" + (int)statusCode, false);            
            }
            
            Server.TransferRequest("Error", false);
        }             
    }    
}