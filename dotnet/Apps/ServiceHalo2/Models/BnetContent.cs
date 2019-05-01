using System;

namespace Branch.Apps.ServiceHalo2.Models
{
	public class BnetServiceRecord
	{
		public string Gamertag { get; set; }

		public string EmblemUrl { get; set; }

		public string ClanName { get; set; }

		public uint TotalGames { get; set; }

		public uint TotalKills { get; set; }

		public uint TotalDeaths { get; set; }

		public uint TotalAssists { get; set; }

		public DateTime LastPlayed { get; set; }
	}
}
