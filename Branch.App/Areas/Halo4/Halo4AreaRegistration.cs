using System.Web.Mvc;

namespace Branch.App.Areas.Halo4
{
	public class Halo4AreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Halo4";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			var namespaces = new[] { "Branch.App.Areas.Halo4.Controllers" };

			context.MapRoute(
				"Halo4_Default",
				"Halo4/",
				new { controller = "Home", action = "Index" },
				namespaces
			);

			context.MapRoute(
				"Halo4_ServiceRecord",
				"Halo4/ServiceRecord/{gamertag}",
				new { controller = "ServiceRecord", action = "Index"},
				namespaces
			);
		}
	}
}
