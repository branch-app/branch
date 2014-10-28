using System;
using System.Web;
using System.Web.Mvc;
using Branch.App.Helpers.Mvc;

namespace Branch.App.Helpers.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
	public class RequiresNoAuthentication : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			var branchIdentity = Authentication.GetAuthenticatedIdentity();
			if (branchIdentity == null) return;

			HttpContext.Current.Response.RedirectToRoute("Welcome");
			FlashMessage.AddFlashMessage(filterContext.HttpContext.Response, 
				FlashMessage.FlashMessageType.Info,
				"What?", 
				"You can't do that while you're authenticated.");
		}
	}
}