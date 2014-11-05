using System.Web.Mvc;
using Branch.App.Areas.Identity.Filters;
using Branch.App.Areas.Identity.Models;
using Branch.App.Helpers;
using Branch.Models.Sql;

namespace Branch.App.Areas.Identity.Controllers
{
	public class HomeController : BaseController
	{
		// GET: Identity/{slug}/
		[ValidateBranchIdentity]
		public ActionResult Index(string slug, BranchIdentity branchIdentity)
		{
			return View(new HomeIdentityViewModel(branchIdentity));
		}
	}
}