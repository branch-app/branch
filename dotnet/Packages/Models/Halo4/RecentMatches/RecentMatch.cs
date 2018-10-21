using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	[JsonConverter(
		typeof(IdToAbstractConverter<RecentWarGamesMatch, RecentCampaignMatch, RecentSpartanOpsMatch, RecentWarGamesMatch>),
		new object[] { "ModeId", new string[] { "war-games", "campaign", "spartan-ops", "custom-games" } }
	)]
	public abstract class RecentMatch
	{
		public string Id { get; set; }

		public GameMode GameMode { get; set; }

		public int PersonalScore { get; set; }

		public bool Completed { get; set; }

		public MatchResult Result { get; set; }

		public int[] TopMedalIds { get; set; }

		public DateTime EndDate { get; set; }
	}
}
