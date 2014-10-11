using System.Web.Http;

namespace Branch.App.App_Start
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				"DefaultApi", 
				"Api/{controller}/{id}", 
				new {id = RouteParameter.Optional}
			);
		}
	}
}