using System.Web.Mvc;
using System.Web.Routing;
using Branch.App.Helpers.Mvc;
using Branch.Core.BranchStuff;

namespace Branch.App.Areas.Halo4.Filters
{
	public class ValidateH4ServiceRecordFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var gamertag = GamerIdReplacementManager.GetOriginalGamerId(filterContext.ActionParameters["gamertag"].ToString(), GlobalStorage.AzureStorage);

			var serviceRecord = GlobalStorage.H4WaypointManager.GetServiceRecord(gamertag);
			if (serviceRecord != null)
			{
				filterContext.ActionParameters["ServiceRecord"] = serviceRecord;
				return;
			}

			filterContext.Result = FlashMessage.RedirectAndFlash(filterContext.HttpContext.Response,
				new RedirectToRouteResult("Search",
					new RouteValueDictionary {{"q", gamertag}}),
				FlashMessage.FlashMessageType.Info, "Unknown Halo 4 Player",
				string.Format("The gamertag '{0}' has not played Halo 4.", gamertag));
		}
	}
}
