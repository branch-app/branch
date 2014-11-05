using System.Web.Optimization;
using Branch.App.Extentions;

namespace Branch.App.App_Start
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
#if !DEBUG
			bundles.UseCdn = true;
#endif

			// good!

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new AzureScriptBundle("~/bundles/modernizr", "cdn", "https://az673231.vo.msecnd.net").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new AzureScriptBundle("~/bundles/flash", "cdn", "https://az673231.vo.msecnd.net").Include(
				"~/Scripts/flash-cookie.js"));

			bundles.Add(new AzureScriptBundle("~/bundles/jbclock", "cdn", "https://az673231.vo.msecnd.net").Include(
				"~/Scripts/jbclock.js"));
			bundles.Add(new AzureStyleBundle("~/bundles/jbclock-styles", "cdn", "https://az673231.vo.msecnd.net").Include(
				"~/Content/Styles/jbclock.min.css"));


			// Bootstrap Stuff	
			bundles.Add(new AzureStyleBundle("~/bundles/bootstrap-custom", "cdn", "https://az673231.vo.msecnd.net").Include(new[] {
				"~/Content/Styles/bootstrap-custom.min.css",
				"~/Content/Styles/bootstrap-branch.min.css",
				"~/Content/Styles/bootstrap-extras.min.css"
			}));
			bundles.Add(new AzureScriptBundle("~/bundles/bootstrap-scripts", "cdn", "https://az673231.vo.msecnd.net").Include(
				"~/Scripts/bootstrap-3.0.0.js"
			));

			// Branch Typescript
			bundles.Add(new AzureScriptBundle("~/bundles/branch-js", "cdn", "https://az673231.vo.msecnd.net").Include(
				"~/Scripts/NotFuckingLibraries/search-autocomplete.js",
				"~/Scripts/NotFuckingLibraries/app.js"
			));

			// branch specific styles
			bundles.Add(new AzureStyleBundle("~/bundles/branch-specific", "cdn", "https://az673231.vo.msecnd.net").Include(
				"~/Content/Styles/application.min.css"
			));
		}
	}
}