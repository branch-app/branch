using System;
using System.Net.NetworkInformation;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Helpers.Mvc;
using Branch.Models.Services.Halo4._343.Responses;
using _343Enums = Branch.Models.Services.Halo4._343.DataModels.Enums;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class CommendationsController : Controller
	{
		//
		// GET: /Halo4/{gamertag}/Commendations/
		[ValidateH4ServiceRecordFilter]
		[ValidateH4ApiStatus]
		public ActionResult Index(string gamertag, string slug, ServiceRecord serviceRecord)
		{
			var commendations = GlobalStorage.H4WaypointManager.GetPlayerCommendations(serviceRecord.Gamertag);
			if (commendations == null)
				throw new NetworkInformationException();

			_343Enums.CommendationCategory commendationCategory;
			if (!Enum.TryParse(slug, out commendationCategory))
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "Commendations", new { slug = _343Enums.CommendationCategory.Weapons.ToString() }),
					FlashMessage.FlashMessageType.Info, "Couldn't find Commendation Type", 
					"Branch was unable to find that specific commendation type, so we took you to a familiar, and safe place.");

			return View(new CommendationsViewModel(serviceRecord, commendations.Commendations, commendationCategory));
		}
	}
}