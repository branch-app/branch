using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Branch.App.Helpers.Mvc;
using Branch.Core.Storage;
using System.Data.Entity;

namespace Branch.App.Areas.Identity.Filters
{
	public class ValidateBranchIdentity : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var username = (filterContext.ActionParameters["slug"] ?? "").ToString().Trim().ToLower();

			using (var sqlStorage = new SqlStorage())
			{
				sqlStorage.Configuration.LazyLoadingEnabled = false;

				var branchIdentity = sqlStorage.BranchIdentities
					.Include(i => i.BranchRole)
					.Include(i => i.GamerIdentity)
					.Include(i => i.BranchIdentitySessions)
					.Include(i => i.GamerIdentity.Halo4Identities)
					.Include(i => i.GamerIdentity.ReachIdentities)
					.FirstOrDefault(i => i.Username.ToLower() == username.ToLower());

				if (branchIdentity != null)
				{
					filterContext.ActionParameters["BranchIdentity"] = branchIdentity;
					return;
				}
			}

			filterContext.Result = FlashMessage.RedirectAndFlash(filterContext.HttpContext.Response,
				new RedirectToRouteResult("Search",
					new RouteValueDictionary { { "q", username } }),
				FlashMessage.FlashMessageType.Info, "Unknown Branch Identity",
				string.Format("The identity '{0}' does not exist.", username));
		}
	}
}
