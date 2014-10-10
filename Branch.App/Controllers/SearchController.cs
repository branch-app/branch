using System;
using System.Web;
using System.Web.Mvc;
using Branch.App.Models;

namespace Branch.App.Controllers
{
	public class SearchController : Controller
	{
		//
		// GET: /Search/?q=
		public ActionResult Index(string q)
		{
			if (String.IsNullOrEmpty(q))
				throw new HttpException(404, "yo, query can't be null/empty");

			// find halo 4 players
			var halo4ServiceRecord = GlobalStorage.H4Manager.GetPlayerServiceRecord(q, true);

			// find halo reach players
			var haloReachServiceRecord = GlobalStorage.HReachManager.GetPlayerServiceRecord(q, true);

			// le render le model le
			return View(new SearchViewModel(q, halo4ServiceRecord, haloReachServiceRecord));
		}
	}
}