using System;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Helpers.Mvc;
using Branch.Core.Game.Halo4.Enums;
using Branch.Core.Game.Halo4.Models._343.DataModels;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class HistoryController : Controller
	{
		//
		// GET: /360/{gamertag}/Halo4/History/{slug}?{page}
		[ValidateH4ServiceRecordFilter]
		[ValidateH4ApiStatus]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord, string slug)
		{
			GameMode gameMode;
			Enum.TryParse(slug, out gameMode);
			var page = int.Parse(Request.QueryString["page"] ?? "0");
			if (page < 0) page = 0;
			const int count = 25;

			dynamic gameHistory;
			switch (gameMode)
			{
				case GameMode.Customs:
				case GameMode.WarGames:
					gameHistory =
						GlobalStorage.H4Manager.GetPlayerGameHistory<GameHistoryModel.WarGames>(
							serviceRecord.Gamertag, (page * count), count, gameMode);
					return View("WarGames", new HistoryViewModel<GameHistoryModel.WarGames>(
						serviceRecord, gameHistory, gameMode, page));

				case GameMode.Campaign:
					gameHistory =
						GlobalStorage.H4Manager.GetPlayerGameHistory<GameHistoryModel.Campaign>(
							serviceRecord.Gamertag, (page * count), count, gameMode);
					return View("Campaign", new HistoryViewModel<GameHistoryModel.Campaign>(
						serviceRecord, gameHistory, gameMode, page));

				case GameMode.SpartanOps:
					gameHistory =
						GlobalStorage.H4Manager.GetPlayerGameHistory<GameHistoryModel.SpartanOps>(
							serviceRecord.Gamertag, (page * count), count, gameMode);
					return View("SpartanOps", new HistoryViewModel<GameHistoryModel.SpartanOps>(
						serviceRecord, gameHistory, gameMode, page));

				default:
					return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "History", new { slug = GameMode.WarGames.ToString() }),
						FlashMessage.FlashMessageType.Info, "Couldn't find specified Game Mode",
						"Branch was unable to find that specific game mode, so we took you to a familiar, and safe place.");
			}
		}
	}
}
