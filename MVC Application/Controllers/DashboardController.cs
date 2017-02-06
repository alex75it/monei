using Monei.Core.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monei.MvcApplication.Controllers
{
    public class DashboardController : MoneiControllerBase
    {
        public DashboardController(ISecurityManager securityManager) :base(securityManager)
        {

        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}