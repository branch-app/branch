using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;
using Branch.App.Areas.Reach.Models;
using Branch.App.Helpers;
using Branch.Core.Game.HaloReach.Models._343.Responces;
using Branch.App.Helpers.Mvc;

namespace Branch.App.Areas.Reach.Controllers
{
	public class GameController : BaseController
	{
		// GET: 360/{gamertag}/Reach/Game/{id}
		[ValidateReachServiceRecordFilter]
		[ValidateReachApiStatus]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord, string id)
		{
			var game = GlobalStorage.HReachManager.GetGameDetails(id);
			if (game == null)
			{
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "ServiceRecord"),
					FlashMessage.FlashMessageType.Failure,
					"Unable to load Game",
					"It seems the specified game you tried to view doesn't exist, is invalid, or has been purged by 343.");
			}

			return View(new GameViewModel(serviceRecord, game));
		}
	}
}