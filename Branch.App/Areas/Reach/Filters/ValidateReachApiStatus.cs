using System.Web.Mvc;
using Branch.App.Helpers.Mvc;

namespace Branch.App.Areas.Reach.Filters
{
	public class ValidateReachApiStatus : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var apiStatus = GlobalStorage.HReachManager.CheckApiValidity();
			if (apiStatus) return;

			FlashMessage.AddFlashMessage(filterContext.HttpContext.Response, FlashMessage.FlashMessageType.Info, "Archive Mode",
				"The Halo Waypoint API seems to be down. The site will continue working, but will only load data that we already have. No new player data can be loaded right now.");
		}
	}
}
