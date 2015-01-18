using System.Web;
using System.Web.Optimization;

namespace Monei.MvcApplication
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/JQuery/jquery-{version}.js"
						//"~/Scripts/jquery-migrate-{version}.js"
						));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
						"~/Scripts/JQuery/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/JQuery/jquery.unobtrusive*",
						"~/Scripts/JQuery/jquery.validate*"));

			// Bootstrap
			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/bootstrap/bootstrap.js",
				"~/Scripts/JQuery/jquery.validate.unobtrusive-custom-for-bootstrap.js",
				"~/Scripts/bootstrap/bootstrap-select.min.js",
				"~/Scripts/bootstrap/bootstrap-datepicker.js"
				));

			bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
				"~/Content/bootstrap.min.css",
				"~/Content/bootstrap-theme_01.css",
				"~/Content/bootstrap-responsive.min.css",
				"~/Content/bootstrap-mvc-validation.css",
				"~/Content/bootstrap-select.min.css",
				"~/Content/bootstrap-datepicker.css"
				));


			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/angular")
				.Include(
				"~/Scripts/angular/angular.min.js",
				"~/Scripts/angular/angular-resource.min.js"
				//"~/Scripts/ng-infinite-scroll.js",
				//"~/Scripts/angular-strap.js"
				));

            
			bundles.Add(new ScriptBundle("~/bundles/monei").Include(
				// moment
				"~/Scripts/moment.min.js",
				"~/Scripts/moment_langs.min.js",

				// noty
                "~/Scripts/noty/jquery.noty.js",
                "~/Scripts/noty/layouts/topCenter.js",
                "~/Scripts/noty/layouts/bottomRight.js",
                "~/Scripts/noty/layouts/top.js",
                // add other layouts...
                "~/Scripts/noty/themes/default.js",

				// Google charts
				"~/Scripts/ng-google-chart.js",

				// Charts
				"~/Scripts/Chart.min.js", // used in Registry page

				// monei
				"~/Scripts/monei/monei.js",
				"~/Scripts/monei/monei.utils.js",
				"~/Scripts/monei/AngularApp.js",
				"~/Scripts/monei/angular controllers/*.js",
				"~/Scripts/monei/angular directives/*.js"
                ));	


			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

			bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
						"~/Content/themes/base/jquery.ui.core.css",
						"~/Content/themes/base/jquery.ui.resizable.css",
						"~/Content/themes/base/jquery.ui.selectable.css",
						"~/Content/themes/base/jquery.ui.accordion.css",
						"~/Content/themes/base/jquery.ui.autocomplete.css",
						"~/Content/themes/base/jquery.ui.button.css",
						"~/Content/themes/base/jquery.ui.dialog.css",
						"~/Content/themes/base/jquery.ui.slider.css",
						"~/Content/themes/base/jquery.ui.tabs.css",
						"~/Content/themes/base/jquery.ui.datepicker.css",
						"~/Content/themes/base/jquery.ui.progressbar.css",
						"~/Content/themes/base/jquery.ui.theme.css"));


		}
	}
}