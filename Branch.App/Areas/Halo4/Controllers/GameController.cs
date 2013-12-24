using System.Web.Mvc;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Filters;
using Branch.App.Helpers.Mvc;
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
			var game = GlobalStorage.H4WaypointManager.GetPlayerGame(serviceRecord.Gamertag, id);
			if (game == null)
			{
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "ServiceRecord"),
					FlashMessage.FlashMessageType.Failure,
					"Unable to load Game",
					"It seems the specified game you tried to view doesn't exist, is invalid, or has been purged by 343.");
			}

			return
				View(new GameViewModel(serviceRecord, game));
		}
	}
}