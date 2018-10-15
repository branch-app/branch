using System;
using Branch.Packages.Converters;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public abstract class GameModeBase
	{
		[JsonConverter(typeof(TimespanConverter))]
		public TimeSpan TotalDuration { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public int TotalGamesStarted { get; set; }
	}
}
