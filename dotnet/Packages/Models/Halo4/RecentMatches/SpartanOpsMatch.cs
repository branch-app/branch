using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	public class RecentSpartanOpsMatch : RecentMatch
	{
		public bool SinglePlayer { get; set; }

		public Difficulty Difficulty { get; set; }

		[JsonConverter(typeof(TimespanConverter))]
		public TimeSpan Duration { get; set; }

		public int SeasonId { get; set; }

		public SpartanOpsEpisode Episode { get; set; }

		public SpartanOpsChapter Chapter { get; set; }
	}
}
