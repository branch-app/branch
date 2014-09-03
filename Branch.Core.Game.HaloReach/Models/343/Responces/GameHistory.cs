using Branch.Core.Game.HaloReach.Models._343.DataModels;

namespace Branch.Core.Game.HaloReach.Models._343.Responces
{
	public class GameHistory
		: Response
	{
		public bool HasMorePages { get; set; }

		public GameHistoryEntry[] RecentGames { get; set; }
	}
}
