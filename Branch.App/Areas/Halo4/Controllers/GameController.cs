using System.Web.Mvc;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Filters;
using Branch.Models.Services.Halo4._343.Responses;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class GameController : Controller
	{
		//
		// GET: /Halo4/Game/
		[ValidateH4ServiceRecordFilter]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord, string id)
		{
			return
				View(new GameViewModel(serviceRecord, GlobalStorage.H4WaypointManager.GetPlayerGame(serviceRecord.Gamertag, id)));
		}
	}
}