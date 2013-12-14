using System;
using System.Web.Mvc;

namespace Branch.App.Filters
{
	public class ValidateH4ServiceRecordFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var gamertag = filterContext.ActionParameters["gamertag"].ToString();
			var serviceRecord = GlobalStorage.H4WaypointManager.GetServiceRecord(gamertag);
			if (serviceRecord == null)
			{
				// Set Flash Message
				// Redirect to search page with this string

				throw new ArgumentOutOfRangeException("filterContext", "This gamertag is not within the range of gamertags in our or 343's database. sorry m8.");
			}

			filterContext.Controller.ViewBag["ServiceRecord"] = filterContext.ActionParameters["ServiceRecord"] = serviceRecord;
		}
	}
}