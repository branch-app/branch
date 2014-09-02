using Branch.Core.BranchStuff;
using Branch.Core.Game.Halo4.Models._343.DataModels;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class Base
	{
		public Base(ServiceRecord serviceRecord, GameHistory<GameHistoryModel.WarGames> recentWarGamesHistory = null)
		{
			ServiceRecord = serviceRecord;
			RecentWarGamesHistory = recentWarGamesHistory ??
										GlobalStorage.H4WaypointManager.GetPlayerGameHistory<GameHistoryModel.WarGames>(
											serviceRecord.Gamertag, 0, 20);

			PublicGamertag = GamerIdReplacementManager.GetReplacementGamerId(ServiceRecord.Gamertag, GlobalStorage.AzureStorage);
		}

		public GameHistory<GameHistoryModel.WarGames> RecentWarGamesHistory { get; set; }

		public string PublicGamertag { get; set; }

		public ServiceRecord ServiceRecord { get; set; }
	}
}