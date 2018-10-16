using Branch.Apps.ServiceHalo4.Models.Waypoint.Common;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint.RecentMatches
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
