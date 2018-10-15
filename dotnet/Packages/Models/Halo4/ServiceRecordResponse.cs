using System;
using System.Linq;
using Branch.Packages.Converters;
using Branch.Packages.Models.External.Halo4;
using Branch.Packages.Models.Halo4.Common;
using Branch.Packages.Models.Halo4.ServiceRecord;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4
{
	public class ServiceRecordResponse
	{
		public int DateFidelity { get; set; }

		public DateTime LastPlayed { get; set; }

		public DateTime FirstPlayed { get; set; }

		public Identity Identity { get; set; }

		public FavoriteWeapon FavoriteWeapon { get; set; }

		public Specialization[] Specializations { get; set; }

		public GameModes GameModes { get; set; }

		public Rank CurrentRank { get; set; }

		public Rank NextRank { get; set; }

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
