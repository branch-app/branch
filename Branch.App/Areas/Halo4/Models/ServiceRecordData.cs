using Branch.Models.Services.Halo4._343.DataModels;
using Branch.Models.Services.Halo4._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class ServiceRecordData : Base
	{
		public ServiceRecordData(ServiceRecord serviceRecord, GameHistory<GameHistoryModel.WarGames> gameHistory)
		{
			ServiceRecord = serviceRecord;
			GameHistory = gameHistory;
		}

		public GameHistory<GameHistoryModel.WarGames> GameHistory { get; set; }
	}
}