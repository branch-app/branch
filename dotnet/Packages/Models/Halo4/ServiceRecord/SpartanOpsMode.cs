using System;
using Branch.Packages.Converters;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class SpartanOpsMode
	{
		public int TotalSinglePlayerMissionsCompleted { get; set; }

		public int TotalCoopMissionsCompleted { get; set; }

		public int TotalMissionsPossible { get; set; }

		public int TotalMedals { get; set; }

		public int TotalGamesWon { get; set; }

		[JsonConverter(typeof(TimespanConverter))]
		public TimeSpan TotalDuration { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public int TotalGamesStarted { get; set; }
	}
}
