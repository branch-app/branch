using System.Web.Mvc;
using Branch.App.Helpers.Mvc;

namespace Branch.App.Filters
{
	public class ValidateH4ApiStatus : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var apiStatus = GlobalStorage.H4WaypointManager.CheckApiValidity();
			if (!apiStatus) return;

			FlashMessage.AddFlashMessage(filterContext.HttpContext.Response, FlashMessage.FlashMessageType.Info, "Archive Mode",
				"The Halo Waypoint API seems to be down. The site will continue working, but will only load data that we already have. No new player data can be loaded right now.");
		}
	}
}
