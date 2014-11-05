using System.Web.Mvc;
using Branch.App.Helpers;

namespace Branch.App.Controllers
{
	public class AboutController : BaseController
	{
		//
		// GET: /About/
		public ActionResult Index()
		{
			return View();
		}
	}
}