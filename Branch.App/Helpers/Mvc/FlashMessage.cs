using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Branch.App.Helpers.Mvc
{
	public class FlashMessage
	{
		public enum FlashMessageType
		{
			Success,
			Failure,
			Warning,
			Info
		}

		public static ActionResult RedirectAndFlash(HttpResponseBase response, ActionResult redirectResult, FlashMessageType flashMessageType, string title,
			string body)
		{
			AddFlashMessage(response, flashMessageType, title, body);
			return redirectResult;
		}

		public static void AddFlashMessage(HttpResponseBase response, FlashMessageType flashMessageType, string title,
			string body)
		{
			response.Cookies.Add(new HttpCookie("FlashMessage",
				JsonConvert.SerializeObject(new[] { flashMessageType.ToString().ToLower(), title, body }))
			{
				Path = "/"
			});
		}
	}
}