using Branch.Packages.Models.External.Halo4.Common;

namespace Branch.Packages.Models.External.Halo4.RecentMatches
{
	public class WarGamesRecentMatch : RecentMatch
	{
		public int BaseVariantId { get; set; }

		public ImageUrl BaseVariantImageUrl { get; set; }

		public string VariantName { get; set; }

		public string FeaturedStatName { get; set; }

		public int FeaturedStatValue { get; set; }

		public int TotalMedals { get; set; }

		public string MapVariantName { get; set; }

		public int PlaylistId { get; set; }

		public string PlaylistName { get; set; }
	}
}
