using System;
using System.Web;
using System.Web.Mvc;
using Branch.App.Helpers.Mvc;
using Branch.Models.Sql;

namespace Branch.App.Helpers.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
	public class RequiresAuthentication : AuthorizeAttribute
	{
		private readonly Nullable<RoleType> _roleType;

		public RequiresAuthentication(RoleType roleType)
		{
			_roleType = roleType;
		}

		public RequiresAuthentication()
		{
			_roleType = null;
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			var branchIdentity = Authentication.GetAuthenticatedIdentity();
			if (branchIdentity == null)
			{
				HttpContext.Current.Response.RedirectToRoute("Welcome");
				FlashMessage.AddFlashMessage(filterContext.HttpContext.Response, 
					FlashMessage.FlashMessageType.Info,
					"Authentication Required", 
					"That section of Branch requires authentication.");
				return;
			}

			if (_roleType == null)
				return;

			if (branchIdentity.BranchRole.Type == _roleType) return;

			HttpContext.Current.Response.RedirectToRoute("Welcome");
			FlashMessage.AddFlashMessage(filterContext.HttpContext.Response,
				FlashMessage.FlashMessageType.Info,
				"Invalid Authorization",
				"You do not have authorization to access that. Naughty.");
		}
	}
}