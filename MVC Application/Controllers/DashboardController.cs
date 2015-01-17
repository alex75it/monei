using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monei.MvcApplication.Controllers
{
	public class DashboardController : MoneiControllerBase
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}