using System;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Filters;
using Branch.Models.Services.Halo4._343.DataModels;
using Branch.Models.Services.Halo4._343.Responses;
using Enums = Branch.Models.Services.Halo4._343.DataModels.Enums;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class HistoryController : Controller
	{
		//
		// GET: /360/{gamertag}/Halo4/History/{slug}?{page}
		[ValidateH4ServiceRecordFilter]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord, string slug)
		{
			Enums.Mode gameMode;
			Enum.TryParse(slug, out gameMode);
			var page = int.Parse(Request.QueryString["page"] ?? "0");
			if (page < 0) page = 0;
			const int count = 25;

			dynamic gameHistory;
			switch (gameMode)
			{
				case Enums.Mode.Customs:
				case Enums.Mode.WarGames:
					gameHistory =
						GlobalStorage.H4WaypointManager.GetPlayerGameHistory<GameHistoryModel.WarGames>(
							serviceRecord.Gamertag, (page * count), count, gameMode);
					return View("WarGames", new HistoryViewModel<GameHistoryModel.WarGames>(
						serviceRecord, gameHistory, gameMode, page));

				case Enums.Mode.Campaign:
					gameHistory =
						GlobalStorage.H4WaypointManager.GetPlayerGameHistory<GameHistoryModel.Campaign>(
							serviceRecord.Gamertag, (page * count), count, gameMode);
					return View("Campaign", new HistoryViewModel<GameHistoryModel.Campaign>(
						serviceRecord, gameHistory, gameMode, page));

				case Enums.Mode.SpartanOps:
					gameHistory =
						GlobalStorage.H4WaypointManager.GetPlayerGameHistory<GameHistoryModel.SpartanOps>(
							serviceRecord.Gamertag, (page * count), count, gameMode);
					return View("SpartanOps", new HistoryViewModel<GameHistoryModel.SpartanOps>(
						serviceRecord, gameHistory, gameMode, page));

				default:
					throw new ArgumentException("Invalid game mode, m8.");
			}
		}
	}
}
