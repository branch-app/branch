using System;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using _Enums = Branch.Models.Services.Halo4._343.DataModels.Enums;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class PlaylistsController : Controller
	{
		//
		// GET: /Halo4/Playlists/
		[ValidateH4ApiStatus]
		public ActionResult Index(string slug)
		{
			_Enums.GameMode selectedMode;

			if (!Enum.TryParse(slug, true, out selectedMode))
				selectedMode = _Enums.GameMode.WarGames;

			return View(new PlaylistsViewModel(selectedMode, GlobalStorage.H4WaypointManager.Playlists));
		}
	}
}