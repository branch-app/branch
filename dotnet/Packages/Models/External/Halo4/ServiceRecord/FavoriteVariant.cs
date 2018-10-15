using Branch.Packages.Models.External.Halo4.Common;

namespace Branch.Packages.Models.External.Halo4.ServiceRecord
{
	public class FavoriteVariant
	{
		public ImageUrl ImageUrl { get; set; }

		public string TotalDuration { get; set; }

		public int TotalGamesStarted { get; set; }

		public int TotalGamesCompleted { get; set; }

		public int TotalGamesWon { get; set; }

		public int TotalMedals { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public double KdRatio { get; set; }

		public double AveragePersonalScore { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }
	}
}
