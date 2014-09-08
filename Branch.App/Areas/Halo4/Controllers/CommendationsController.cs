using System;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Helpers.Mvc;
using Branch.Core.Game.Halo4.Enums;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class CommendationsController : Controller
	{
		//
		// GET: /360/{gamertag}/Halo4/Commendations/
		[ValidateH4ServiceRecordFilter]
		[ValidateH4ApiStatus]
		public ActionResult Index(string gamertag, string slug, ServiceRecord serviceRecord)
		{
			var commendations = GlobalStorage.H4Manager.GetPlayerCommendations(serviceRecord.Gamertag);
			if (commendations == null)
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "ServiceRecord", new { gamertag }),
					FlashMessage.FlashMessageType.Failure, "No cached player commendation",
					"Branch hasn't cached the commendations for this player, and can't load any new data right now.");

			CommendationCategory commendationCategory;
			if (!Enum.TryParse(slug, out commendationCategory))
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "Commendations", new { slug = CommendationCategory.Weapons.ToString() }),
					FlashMessage.FlashMessageType.Info, "Couldn't find Commendation Type", 
					"Branch was unable to find that specific commendation type, so we took you to a familiar, and safe place.");

			return View(new CommendationsViewModel(serviceRecord, commendations.Commendations, commendationCategory));
		}
	}
}