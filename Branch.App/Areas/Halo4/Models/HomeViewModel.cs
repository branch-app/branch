using Branch.Models.Services.Branch;

namespace Branch.App.Areas.Halo4.Models
{
	public class HomeViewModel
	{
		public HomeViewModel(Halo4StatsEntity weeklyStats, Halo4StatsEntity allTimeStats)
		{
			WeeklyStats = weeklyStats;
			AllTimeStats = allTimeStats;
		}

		public Halo4StatsEntity WeeklyStats { get; set; }

		public Halo4StatsEntity AllTimeStats { get; set; }
	}
}