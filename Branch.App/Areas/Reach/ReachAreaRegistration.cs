using System.Web.Mvc;

namespace Branch.App.Areas.Reach
{
	public class ReachAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Reach";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			var namespaces = new[] { "Branch.App.Areas.Reach.Controllers" };

			// Service Record
			context.MapRoute("Reach_ServiceRecord", "360/{gamertag}/Reach/",
				new { controller = "ServiceRecord", action = "Index" }, namespaces);
		}
	}
}