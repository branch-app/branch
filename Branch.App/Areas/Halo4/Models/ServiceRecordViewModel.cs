using Branch.Models.Services.Halo4._343.DataModels;
using Branch.Models.Services.Halo4._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class ServiceRecordViewModel : Base
	{
		public ServiceRecordViewModel(ServiceRecord serviceRecord, GameHistory<GameHistoryModel.WarGames> gameHistory) :
			base (serviceRecord)
		{
			GameHistory = gameHistory;
		}

		public GameHistory<GameHistoryModel.WarGames> GameHistory { get; set; }
	}
}