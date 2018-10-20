using System;
using Branch.Packages.Converters;
using Branch.Packages.Models.Halo4.Common;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class CampaignMode
	{

		public Mission[] SinglePlayerMissions { get; set; }

		public Mission[] CoopMissions { get; set; }

		public int TotalTerminalsVisited { get; set; }

		public long NarrativeFlags { get; set; }

		public int? SinglePlayerDASO { get; set; }

		public int? SinglePlayerDifficulty { get; set; }

		public int? CoopDASO { get; set; }

		public int? CoopDifficulty { get; set; }

		[JsonConverter(typeof(TimespanConverter))]
		public TimeSpan TotalDuration { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public int TotalGamesStarted { get; set; }
	}
}
