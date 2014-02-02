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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <param name="redirectResult"></param>
		/// <param name="flashMessageType"></param>
		/// <param name="title"></param>
		/// <param name="body"></param>
		public static ActionResult RedirectAndFlash(HttpResponseBase response, ActionResult redirectResult, FlashMessageType flashMessageType, string title,
			string body)
		{
			response.Cookies.Add(new HttpCookie("FlashMessage", 
				JsonConvert.SerializeObject(new[] { flashMessageType.ToString().ToLower(), title, body}))
			{
				Path = "/"
			});

			return redirectResult;
		}
	}
}