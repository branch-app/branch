using System.Web.Mvc;

namespace Branch.App.Areas.Domain
{
	public class DomainAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Domain";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"DomainDefault",
				"Domain/{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}