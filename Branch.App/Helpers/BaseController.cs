using System.Web;
using System.Web.Mvc;
using Branch.App.Helpers.Mvc;
using Newtonsoft.Json;

namespace Branch.App.Helpers
{
	public class BaseController : Controller
	{
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
#if DEBUG
			if (HttpContext.Request.QueryString.Get("forceFlashMessage") != null)
			{
				HttpContext.Response.Cookies.Add(new HttpCookie("FlashMessage", JsonConvert.SerializeObject(new[]
				{
					FlashMessage.FlashMessageType.Success.ToString().ToLower(), 
					"Example Flash Message", 
					"This is an example flash message bae."
				})));
			}
#endif

			base.OnActionExecuting(filterContext);
		}
	}
}