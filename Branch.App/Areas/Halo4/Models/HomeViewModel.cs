using Branch.Models.Services.Branch;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class HomeViewModel
	{
		public HomeViewModel(Challenge challenges, Playlist playlists, Halo4StatsEntity weeklyStats, Halo4StatsEntity allTimeStats)
		{
			Challenges = challenges;
			Playlists = playlists;

			WeeklyStats = weeklyStats;
			AllTimeStats = allTimeStats;
		}

		public Challenge Challenges { get; set; }

		public Playlist Playlists { get; set; }

		public Halo4StatsEntity WeeklyStats { get; set; }

		public Halo4StatsEntity AllTimeStats { get; set; }
	}
}