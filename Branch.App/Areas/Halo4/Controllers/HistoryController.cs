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
		// GET: /Halo4/History/
		[ValidateH4ServiceRecordFilter]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord, string slug)
		{
			Enums.Mode gameMode;
			Enum.TryParse(slug, out gameMode);

			dynamic gameHistory;
			switch (gameMode)
			{
				case Enums.Mode.Customs:
				case Enums.Mode.WarGames:
					gameHistory =
						GlobalStorage.H4WaypointManager.GetPlayerGameHistory<GameHistoryModel.WarGames>(serviceRecord.Gamertag, 0, 25,
							gameMode);
					return View("WarGames", new HistoryData<GameHistoryModel.WarGames>(serviceRecord, gameHistory, gameMode));

				case Enums.Mode.Campaign:
					gameHistory =
						GlobalStorage.H4WaypointManager.GetPlayerGameHistory<GameHistoryModel.Campaign>(serviceRecord.Gamertag, 0, 25,
							gameMode);
					return View("Campaign", new HistoryData<GameHistoryModel.Campaign>(serviceRecord, gameHistory, gameMode));

				case Enums.Mode.SpartanOps:
					gameHistory =
						GlobalStorage.H4WaypointManager.GetPlayerGameHistory<GameHistoryModel.SpartanOps>(serviceRecord.Gamertag, 0, 25,
							gameMode);
					return View("SpartanOps", new HistoryData<GameHistoryModel.SpartanOps>(serviceRecord, gameHistory, gameMode));

				default:
					throw new Exception();
			}
		}
	}
}
