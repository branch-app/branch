using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Branch.App.App_Start;
using Branch.App_Start;
using Branch.Core.Storage;
using H4Api = Branch.Core.Game.Halo4.Api;
using HReachApi = Branch.Core.Game.HaloReach.Api;

#if !RELEASE
using System;
using Branch.App.Controllers;
#endif

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
			GlobalStorage.H4Manager = new H4Api.Manager(GlobalStorage.AzureStorage);
			GlobalStorage.HReachManager = new HReachApi.Manager(GlobalStorage.AzureStorage);
		}

#if !RELEASE
		protected void Application_Error()
		{
			var exception = Server.GetLastError();
			Server.ClearError();

			var routeData = new RouteData();
			routeData.Values.Add("controller", "Error");
			routeData.Values.Add("action", "Index");
			routeData.Values.Add("exception", exception);
			routeData.Values.Add("loggedExceptionGuid", Guid.NewGuid());
			routeData.Values.Add("statusCode", 
				exception.GetType() == typeof(HttpException) 
					? ((HttpException)exception).GetHttpCode() 
					: 500);

			// TODO: Add fail-safe exception logging here - maybe

			IController errorController = new ErrorController();
			errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
			Response.End();
		}
#endif
	}

	public static class GlobalStorage
	{
		public static AzureStorage AzureStorage { get; set; }

		public static H4Api.Manager H4Manager { get; set; }

		public static HReachApi.Manager HReachManager { get; set; }

		public static readonly string AzureCdnEndpoint = "//az673231.vo.msecnd.net/cdn/";
	}
}