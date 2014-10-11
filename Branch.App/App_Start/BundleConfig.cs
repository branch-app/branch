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

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new AzureScriptBundle("~/bundles/modernizr", "cdn", "http://cdn.branchapp.co").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new AzureScriptBundle("~/bundles/flash", "cdn", "http://cdn.branchapp.co").Include(
				"~/Scripts/flash-cookie.js"));

			bundles.Add(new AzureScriptBundle("~/bundles/jbclock", "cdn", "http://cdn.branchapp.co").Include(
				"~/Scripts/jbclock.js"));
			bundles.Add(new AzureStyleBundle("~/bundles/jbclock-styles", "cdn", "http://cdn.branchapp.co").Include(
				"~/Content/Styles/jbclock.css"));

			// jquery
			bundles.Add(new AzureScriptBundle("~/bundles/jqueryui-scripts", "cdn", "http://cdn.branchapp.co").Include(
				"~/Scripts/jquery-ui.min.js"
			));
			bundles.Add(new AzureStyleBundle("~/bundles/jqueryui-styles", "cdn", "http://cdn.branchapp.co").Include(
				"~/Content/Styles/jquery-ui.min.css",
				"~/Content/Styles/jquery-ui.structure.min.css"
			));

			// Bootstrap Stuff
			bundles.Add(new AzureStyleBundle("~/bundles/bootstrap-custom", "cdn", "http://cdn.branchapp.co").Include(new[] {
				"~/Content/Styles/bootstrap-custom.min.css",
				"~/Content/Styles/bootstrap-branch.css"
			}));
			bundles.Add(new AzureScriptBundle("~/bundles/bootstrap-scripts", "cdn", "http://cdn.branchapp.co").Include(
				"~/Scripts/bootstrap-3.0.0.js"));

			// branch specific styles
			bundles.Add(new AzureStyleBundle("~/bundles/branch-specific", "cdn", "http://cdn.branchapp.co").Include(
				"~/Content/Styles/application.css"
			));
		}
	}
}