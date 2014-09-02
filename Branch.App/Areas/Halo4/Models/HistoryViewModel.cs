using Branch.Core.Game.Halo4.Enums;
using Branch.Core.Game.Halo4.Models._343.DataModels;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class HistoryViewModel<T> : Base
		where T : GameHistoryModel.Base
	{
		public HistoryViewModel(ServiceRecord serviceRecord, GameHistory<T> gameHistory, GameMode gameMode, int page) :
			base(serviceRecord)
		{
			GameHistory = gameHistory;
			GameMode = gameMode;
			Page = page;
		}

		public GameHistory<T> GameHistory { get; set; }

		public int Page { get; set; }

		public GameMode GameMode { get; set; }
	}
}
