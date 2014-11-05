using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;
using Branch.App.Helpers;

namespace Branch.App.Areas.Reach.Controllers
{
	public class HomeController : BaseController
	{
		//
		// GET: /Game/Reach/
		[ValidateReachApiStatus]
		public ActionResult Index()
		{
			return View();
		}
	}
}