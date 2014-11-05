using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Helpers;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class HomeController : BaseController
	{
		//
		// GET: /Halo4/
		[ValidateH4ApiStatus]
		public ActionResult Index()
		{
			var challenges = GlobalStorage.H4Manager.Challenges;
			var playlists = GlobalStorage.H4Manager.Playlists;

			return View(new HomeViewModel(challenges, playlists));
		}
	}
}