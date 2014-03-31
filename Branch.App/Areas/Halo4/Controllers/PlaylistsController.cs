using System.Web.Mvc;
using Branch.App.Filters;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class PlaylistsController : Controller
	{
		//
		// GET: /Halo4/Playlists/
		[ValidateH4ApiStatus]
		public ActionResult Index()
		{
			return View();
		}
	}
}