using System;
using Branch.Packages.Models.Halo4.Common;
using Branch.Packages.Models.Halo4.ServiceRecord;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4
{
	public class ServiceRecordResponse : WaypointResponse
	{
		public int DateFidelity { get; set; }

		public DateTime LastPlayedUtc { get; set; }

		public DateTime FirstPlayedUtc { get; set; }

		public int SpartanPoints { get; set; }

		public int TotalGamesStarted { get; set; }

		public int TotalMedalsEarned { get; set; }

		public TimeSpan TotalGameplay { get; set; }

		public int TotalChallengesCompleted { get; set; }

		public Promotion[] Promotions { get; set; }

		public int TotalLoadoutItemsPurchased { get; set; }

		public double TotalCommendationProgress { get; set; }
		
		public string Gamertag { get; set; }

		public string ServiceTag { get; set; }

		public ImageUrl EmblemImageUrl { get; set; }

		public ImageUrl BackgroundImageUrl { get; set; }

		public int FavoriteWeaponId { get; set; }

		public string FavoriteWeaponName { get; set; }

		public string FavoriteWeaponDescription { get; set; }

		public ImageUrl FavoriteWeaponImageUrl { get; set; }

		public int FavoriteWeaponTotalKills { get; set; }

		public int RankId { get; set; }

		public string RankName { get; set; }

		public ImageUrl RankImageUrl { get; set; }

		public int RankStartXP { get; set; }

		public Int64 NextRankStartXP { get; set; }

		public int XP { get; set; }

		public SkillRank TopSkillRank { get; set; }

		public SkillRank[] SkillRanks { get; set; } 

		public int NextRankId { get; set; }

		public string NextRankName { get; set; }

		public ImageUrl NextRankImageUrl { get; set; }

		public TopMedal[] TopMedals { get; set; }

		public Specialization[] Specializations { get; set; }

		public GameMode[] GameModes { get; set; }
	}
}
