using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	public class RecentCampaignMatch : RecentMatch
	{
		public bool SinglePlayer { get; set; }

		public Difficulty Difficulty { get; set; }

		public int MissionId { get; set; }

		[JsonConverter(typeof(TimespanConverter))]
		public TimeSpan Duration { get; set; }

		public CampaignMap Map { get; set; }

		public int[] SkullIds { get; set; }
	}
}
