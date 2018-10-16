using System;
using Branch.Packages.Converters;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class WarGamesMode
	{
		public int TotalMedals { get; set; }

		public int TotalGamesWon { get; set; }

		public int TotalGamesCompleted { get; set; }

		public int AveragePersonalScore { get; set; }

		public double KdRatio { get; set; }

		public int TotalGameBaseVariantMedals { get; set; }

		public FavoriteVariant FavoriteVariant { get; set; }

		[JsonConverter(typeof(TimespanConverter))]
		public TimeSpan TotalDuration { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public int TotalGamesStarted { get; set; }
	}
}
