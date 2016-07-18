using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebPages.OAuth;
using System.Web.Mvc;

namespace Monei.MvcApplication.Controllers
{
    public class HomeController : Controller
    {
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

    }//class
}
