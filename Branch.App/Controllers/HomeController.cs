using System.Web.Mvc;

namespace Branch.App.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}