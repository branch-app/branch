using Branch.Packages.Models.External.Halo4.Common;

namespace Branch.Packages.Models.External.Halo4.RecentMatches
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
