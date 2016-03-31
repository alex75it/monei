using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Monei.MvcApplication.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                HttpStatusCode statusCodeValue;
                if (Enum.TryParse<HttpStatusCode>(statusCode.Value.ToString(), out statusCodeValue))
                {
                    string message = "HTTP status code: " + statusCodeValue;
                    switch (statusCodeValue)
                    {
                        case HttpStatusCode.NotFound:
                            message = "URL not found";
                            break;
                        default:
                            break;
                    }
                    ViewBag.Message = message;
                }                
            }

            return View();
        }
    }
}