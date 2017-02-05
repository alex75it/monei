using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using Monei.MvcApplication.DependencyInjection;
using Monei.Entities;
using Monei.DataAccessLayer.Interfaces;

namespace Monei.MvcApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {       
        public static WindsorCastleDependencyInjection DependencyInjectionManager { get; private set; }
              
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register); // Web API routing must be configured before pages
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);            
            BundleConfig.RegisterBundles(BundleTable.Bundles);            

            log4net.Config.XmlConfigurator.Configure();
            
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

            if (Request.Path.StartsWith("/api"))
                return;

            if (exception is HttpException)
            {
                HttpStatusCode statusCode = (System.Net.HttpStatusCode)(exception as HttpException).GetHttpCode();
                Server.TransferRequest("Error/" + (int)statusCode, false);            
            }
            
            Server.TransferRequest("Error", false);
        }             
    }    
}