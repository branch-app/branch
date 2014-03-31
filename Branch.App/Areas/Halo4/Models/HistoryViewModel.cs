using Branch.Models.Services.Halo4._343.Responses;
using _343Enums = Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.App.Areas.Halo4.Models
{
	public class HistoryViewModel<T> : Base
		where T : _343Enums.GameHistoryModel.Base
	{
		public HistoryViewModel(ServiceRecord serviceRecord, GameHistory<T> gameHistory, _343Enums.Enums.GameMode gameMode, int page) :
			base(serviceRecord)
		{
			GameHistory = gameHistory;
			GameMode = gameMode;
			Page = page;
		}

		public GameHistory<T> GameHistory { get; set; }

		public int Page { get; set; }

		public _343Enums.Enums.GameMode GameMode { get; set; }
	}
}
