using System.Web.Mvc;
using Branch.App.Areas.Identity.Filters;
using Branch.App.Helpers.Mvc;
using Branch.Models.Sql;

namespace Branch.App.Areas.Identity.Controllers
{
	public class ViewController : Controller
	{
		// GET: Identity/{slug}/
		[ValidateBranchIdentity]
		public ActionResult Index(string slug, BranchIdentity branchIdentity)
		{
			return SafeJsonContent.Create(branchIdentity);
		}
	}
}