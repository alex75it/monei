using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Monei.MvcApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Error",
                url: "Error/{statusCode}",
                defaults: new { controller = "Error", action = "Index", statusCode = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );

            //routes.MapRoute(
            //	name: "Management",
            //	url: "Management/{controller}/{action}/{id}",
            //	defaults: new { controller="Category", action="List", id=UrlParameter.Optional}
            //	);

        }

    }//class
}