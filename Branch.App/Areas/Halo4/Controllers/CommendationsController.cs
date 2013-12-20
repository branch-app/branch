using System;
using System.Net.NetworkInformation;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Filters;
using Branch.App.Helpers.Razor;
using Branch.Models.Services.Halo4._343.Responses;
using _343Enums = Branch.Models.Services.Halo4._343.DataModels.Enums;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class CommendationsController : Controller
	{
		//
		// GET: /Halo4/{gamertag}/Commendations/
		[ValidateH4ServiceRecordFilter]
		public ActionResult Index(string gamertag, string slug, ServiceRecord serviceRecord)
		{
			var commendations = GlobalStorage.H4WaypointManager.GetPlayerCommendations(serviceRecord.Gamertag);
			if (commendations == null)
				throw new NetworkInformationException();

			_343Enums.CommendationCategory commendationCategory;
			if (!Enum.TryParse(slug, out commendationCategory))
				RedirectToAction("Index", "Commendations", new { gamertag = BranchHelpers.CheckGamertagPrivacy(serviceRecord.Gamertag), slug = _343Enums.CommendationCategory.Weapons.ToString() });

			return View(new CommendationsData(serviceRecord, commendations.Commendations, commendationCategory));
		}
	}
}