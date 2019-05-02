using System;

namespace Branch.Packages.Models.Halo2
{
	public class ServiceRecordResponse
	{
		public string Gamertag { get; set; }

		public string EmblemUrl { get; set; }

		public string ClanName { get; set; }

		public int TotalGames { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public int TotalAssists { get; set; }

		public DateTime LastPlayed { get; set; }
	}
}
