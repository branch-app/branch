using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Branch.App.App_Start;
using Branch.App_Start;
using Branch.Core.Api.Halo4;
using Branch.Core.Storage;

namespace Branch.App
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// le
			GlobalStorage.AzureStorage = new AzureStorage();
			GlobalStorage.H4WaypointManager = new WaypointManager(GlobalStorage.AzureStorage);
		}
	}

	public static class GlobalStorage
	{
		public static AzureStorage AzureStorage { get; set; }

		public static WaypointManager H4WaypointManager { get; set; }
	}
}