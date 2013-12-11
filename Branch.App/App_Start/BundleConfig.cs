using System.Web.Optimization;

namespace Branch.App.App_Start
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));

			// le bootstrap
			bundles.Add(new StyleBundle("~/bundles/bootstrap-custom").Include(new [] {
				"~/Content/Styles/bootstrap-custom.min.css",
				"~/Content/Styles/bootstrap-branch.css"
			}));

			// branch specific styles
			bundles.Add(new StyleBundle("~/bundles/branch-specific").Include(
				"~/Content/Styles/application.css"
			));
		}
	}
}