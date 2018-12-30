using System;
using System.Linq;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Models.Halo4.Common;
using Branch.Packages.Models.Halo4.ServiceRecord;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4
{
	public class PlayerOverviewResponse
	{
		public Identity Identity { get; set; }

		public FavoriteWeapon FavoriteWeapon { get; set; }

		public Rank CurrentRank { get; set; }

		public MedalRecord[] TopMedals { get; set; }

		public int XP { get; set; }

		public int SpartanPoints { get; set; }

		public int TotalGamesStarted { get; set; }

		public int TotalMedalsEarned { get; set; }

		[JsonConverter(typeof(TimespanConverter))]
		public TimeSpan TotalGameplay { get; set; }

		public int TotalChallengesCompleted { get; set; }

		public int TotalLoadoutItemsPurchased { get; set; }

		public double TotalCommendationProgress { get; set; }
	}
}
