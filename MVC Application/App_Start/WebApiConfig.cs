using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Monei.MvcApplication.Core;
using Monei.MvcApplication.DelegatingHandlers;
using Monei.MvcApplication.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Monei.MvcApplication
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// enable RouteAttribute
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			//config.Routes.MapHttpRoute(
			//	name: "DefaultApiWithAction",
			//	routeTemplate: "api/{controller}/{action}/{p1}/{p2}",
			//	defaults: new { p1 = RouteParameter.Optional, p2 = RouteParameter.Optional }
			//);

			config.Routes.MapHttpRoute(
				name: "DefaultApiWithAction",
				routeTemplate: "api/{controller}/{action}/"
				//defaults: new { p1 = RouteParameter.Optional, p2 = RouteParameter.Optional }
			);


			config.Routes.MapHttpRoute(
				name: "DefaultApiWithAction 2",
				routeTemplate: "api/action/{controller}/{action}"
				//defaults: new { p1 = RouteParameter.Optional, p2 = RouteParameter.Optional }
			);

			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();

			config.Filters.Add(new ApiControllerExceptionFilterAttribute());

			
			//config.MessageHandlers.Add(new CultureDelegatingHandler());
			config.Formatters.Add(new CsvFormatter());



			// lower JSON property name
			//var formatters = GlobalConfiguration.Configuration.Formatters;
			//var jsonFormatter = formatters.JsonFormatter;
			var jsonFormatter = config.Formatters.JsonFormatter;
			var settings = jsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			
			jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			//jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
						
			//config.Formatters.Remove(config.Formatters.XmlFormatter);

		}

	}//class
}
