using Branch.Apps.ServiceHalo4.Models.Waypoint.Common;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint.RecentMatches
{
	public class SpartanOpsRecentMatch : RecentMatch
	{
		public int Difficulty { get; set; }

		public ImageUrl DifficultyImageUrl { get; set; }

		public bool SinglePlayer { get; set; }

		public string EpisodeName { get; set; }

		public string ChapterName { get; set; }

		public int SeasonId { get; set; }

		public int EpisodeId { get; set; }

		public int ChapterId { get; set; }

		public int ChapterNumber { get; set; }
	}
}
