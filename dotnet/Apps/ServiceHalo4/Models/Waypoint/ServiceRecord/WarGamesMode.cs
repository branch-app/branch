using Branch.Apps.ServiceHalo4.Models.Waypoint.Common;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint.ServiceRecord
{
	public class WarGamesMode : GameMode
	{
		public int? TotalMedals { get; set; }

		public int? TotalGamesWon { get; set; }

		public int? TotalGamesCompleted { get; set; }

		public int? AveragePersonalScore { get; set; }

		public double? KdRatio { get; set; }

		public int? TotalGameBaseVariantMedals { get; set; }

		public FavoriteVariant FavoriteVariant { get; set; }
	}
}
