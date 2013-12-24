using System;
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
				throw new ArgumentException("yo, query can't be null/empty");

			// find halo 4 players
			var halo4ServiceRecord = GlobalStorage.H4WaypointManager.GetServiceRecord(q);

			// le render le model le
			return View(new SearchViewModel(q, halo4ServiceRecord));
		}
	}
}