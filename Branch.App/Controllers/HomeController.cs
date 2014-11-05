using System.Web.Mvc;
using Branch.App.Helpers;

namespace Branch.App.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}