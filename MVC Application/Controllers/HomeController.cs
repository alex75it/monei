using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebPages.OAuth;
using System.Web.Mvc;
using Monei.Core.BusinessLogic;
using Monei.MvcApplication.Core;
using Monei.Entities;

namespace Monei.MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        private ISecurityManager securityManager;

        public HomeController(ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
        }

        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.Title = "Home page"; // l10n
            ViewBag.Message = "This is the home page of \"monei\"";	// l10n

            return View();
        }

        // GET: /Home/About
        public ActionResult About()
        {
            ViewBag.Title = "About";	//l10n
            ViewBag.Message = "Your app description page.";

            return View();
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        { 
            //ViewBag.Title = 
            ViewBag.Message = "Contact of the site."; //l10n
            
            return View();
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                Account account = Session["Account"] as Account;
                var user = filterContext.HttpContext.User;
                account = securityManager.GetAccountByUsername(user.Identity.Name);
                ViewBag.ApiToken = securityManager.GetApiTokenForAccount(account.Id);
            }
            base.OnResultExecuting(filterContext);
        }

    }
}
