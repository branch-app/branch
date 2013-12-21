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
				"Halo4/{gamertag}",
				new { controller = "ServiceRecord", action = "Index"},
				namespaces
			);

			// Specializations
			context.MapRoute("Halo4_Specializations", "Halo4/{gamertag}/Specializations", new { controller = "Specializations", action = "Index" }, namespaces);

			// Commendations
			context.MapRoute("Halo4_Commendations", "Halo4/{gamertag}/Commendations/{slug}", new { controller = "Commendations", slug = "Weapons", action = "Index" }, namespaces);

			// CSR
			context.MapRoute("Halo4_Csr", "Halo4/{gamertag}/Csr/", new { controller = "Csr", action = "Index" }, namespaces);
			context.MapRoute("Halo4_CsrDetails", "Halo4/{gamertag}/Csr/{id}/{slug}", new { controller = "Csr", action = "Details" }, namespaces);
		}
	}
}
