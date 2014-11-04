using System.Web.Mvc;

namespace Branch.App.Areas.Identity
{
	public class IdentityAreaRegistration : AreaRegistration
	{
		public override string AreaName { get { return "Identity"; } }

		public override void RegisterArea(AreaRegistrationContext context)
		{
			var namespaces = new[] { "Branch.App.Areas.Identity.Controllers" };

			// Wildcard
			context.MapRoute(
				"Identity_default",
				"Identity/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }, new { controller = "Create" }, namespaces);

			// View Profile
			context.MapRoute(
				"BranchIdentityView",
				"Identity/{slug}/{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces);
		}
	}
}