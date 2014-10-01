using System;
using System.Collections.Generic;
using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.JsonConverters;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class GameDetails
	{
		public string BaseMapName { get; set; }

		public Difficulty CampaignDifficulty { get; set; }

		public uint CampaignGlobalScore { get; set; }

		public bool CampaignMetagameEnabled { get; set; }

		[JsonConverter(typeof(SecondsConverter))]
		public TimeSpan GameDuration { get; set; }

		public string GameId { get; set; }

		public DateTime GameTimestamp { get; set; }

		public VariantClass GameVariantClass { get; set; }

		[JsonProperty("GameVariantIconIndex")]
		public GameVariantIcon GameVariantIcon { get; set; }

		public string GameVariantName { get; set; }

		public bool HasDetails { get; set; }

		public bool IsTeamGame { get; set; }

		public string MapName { get; set; }

		public int PlayerCount { get; set; }

		public List<PlayerGameDetails> Players { get; set; }

		public string PlaylistName { get; set; }

		public List<Team> Teams { get; set; }
	}
}
