using Branch.Packages.Models.External.Halo4.Common;

namespace Branch.Packages.Models.External.Halo4.RecentMatches
{
	public class CampaignRecentMatch : RecentMatch
	{
		public int Difficulty { get; set; }

		public ImageUrl DifficultyImageUrl { get; set; }

		public bool SinglePlayer { get; set; }

		public int[] SkullIds { get; set; }

		public int Mission { get; set; }

		public string MapName { get; set; }
	}
}
