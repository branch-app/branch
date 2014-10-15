using System.Web.Mvc;
using System.Web.Routing;

namespace Branch.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			var namespaces = new[] { "Branch.App.Controllers" };
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			AreaRegistration.RegisterAllAreas();

			// Blog
			routes.MapRoute("Blog", "Blog/View/{slug}",
				new { controller = "Blog", action = "View", slug = "welcome" }, namespaces);

			// Search
			routes.MapRoute("SearchIdentity", "Search/Identity/{ident}",
				new { controller = "Search", action = "Identity", ident = UrlParameter.Optional }, namespaces);

			routes.MapRoute("Search", "Search/",
				new { controller = "Search", action = "Index" }, namespaces);

			// Catch All
			routes.MapRoute("Default", "{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces);

		}
	}
}
