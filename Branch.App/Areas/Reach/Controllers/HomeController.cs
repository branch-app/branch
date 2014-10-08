using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;

namespace Branch.App.Areas.Reach.Controllers
{
	public class HomeController : Controller
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