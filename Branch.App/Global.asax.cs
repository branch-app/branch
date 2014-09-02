using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Branch.App.App_Start;
using Branch.App_Start;
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
			GlobalStorage.H4Manager = new Core.Game.Halo4.Api.Manager(GlobalStorage.AzureStorage);
			GlobalStorage.HReachManager = new Core.Game.HaloReach.Api.Manager(GlobalStorage.AzureStorage);
		}
	}

	public static class GlobalStorage
	{
		public static AzureStorage AzureStorage { get; set; }

		public static Core.Game.Halo4.Api.Manager H4Manager { get; set; }

		public static Core.Game.HaloReach.Api.Manager HReachManager { get; set; }
	}
}