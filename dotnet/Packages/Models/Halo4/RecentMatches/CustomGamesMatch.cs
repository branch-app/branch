using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	public class RecentCustomGamesMatch : RecentMatch
	{
		public GameVariant GameVariant { get; set; }

		public FeaturedStat FeaturedStat { get; set; }

		public MultiplayerMap Map { get; set; }

		public int TotalMedals { get; set; }
	}
}
