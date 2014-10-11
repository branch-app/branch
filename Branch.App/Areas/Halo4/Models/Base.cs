using Branch.Core.Game.Halo4.Models._343.DataModels;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class Base
	{
		public Base(ServiceRecord serviceRecord)
		{
			ServiceRecord = serviceRecord;
			RecentWarGamesHistory =
				GlobalStorage.H4Manager.GetPlayerGameHistory<GameHistoryModel.WarGames>(serviceRecord.Gamertag, 0, 20);
			PublicGamertag = ServiceRecord.Gamertag;
		}

		public GameHistory<GameHistoryModel.WarGames> RecentWarGamesHistory { get; set; }

		public string PublicGamertag { get; set; }

		public ServiceRecord ServiceRecord { get; set; }
	}
}