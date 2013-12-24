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

			routes.MapRoute("Default", "{controller}/{action}/{id}",
				new {controller = "Home", action = "Index", id = UrlParameter.Optional}, namespaces);

			routes.MapRoute("Search", "Search/",
				new {controller = "Search", action = "Index"});
		}
	}
}
