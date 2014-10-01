using System;
using Branch.Core.Game.HaloReach.Enums;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class GameHistoryEntry
	{
		public string CampaignDifficulty { get; set; }

		public int CampaignGlobalScore { get; set; }

		public bool CampaignMetagameEnabled { get; set; }

		public int GameDuration { get; set; }

		public string GameId { get; set; }

		public DateTime GameTimestamp { get; set; }

		public VariantClass GameVariantClass { get; set; }

		public long GameVariantHash { get; set; }

		public GameVariantIcon GameVariantIconIndex { get; set; }

		public string GameVariantName { get; set; }

		public bool HasDetails { get; set; }

		public bool IsTeamGame { get; set; }

		public string MapName { get; set; }

		public long MapVariantHash { get; set; }

		public int PlayerCount { get; set; }

		public string PlaylistName { get; set; }

		public int RequestedPlayerAssists { get; set; }

		public int RequestedPlayerDeaths { get; set; }

		public int RequestedPlayerKills { get; set; }

		public int RequestedPlayerRating { get; set; }

		public int RequestedPlayerScore { get; set; }

		public int RequestedPlayerStanding { get; set; }
	}
}
