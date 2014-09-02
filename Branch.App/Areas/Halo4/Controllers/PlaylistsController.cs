using System;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using Branch.Core.Game.Halo4.Enums;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class PlaylistsController : Controller
	{
		//
		// GET: /Halo4/Playlists/
		[ValidateH4ApiStatus]
		public ActionResult Index(string slug)
		{
			GameMode selectedMode;

			if (!Enum.TryParse(slug, true, out selectedMode))
				selectedMode = GameMode.WarGames;

			return View(new PlaylistsViewModel(selectedMode, GlobalStorage.H4Manager.Playlists));
		}
	}
}