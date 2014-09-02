using System;
using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models._343.DataModels;
using Newtonsoft.Json;

namespace Branch.Core.Game.Halo4.Models._343.Responses
{
	public class ServiceRecord : Response
	{
		public int DateFidelity { get; set; }

		public DateTime LastPlayedUtc { get; set; }

		public DateTime FirstPlayedUtc { get; set; }

		public int SpartanPoints { get; set; }

		public int TotalGamesStarted { get; set; }

		public int TotalMedalsEarned { get; set; }

		public TimeSpan TotalGameplay { get; set; }

		public int TotalChallengesCompleted { get; set; }

		public IList<ServiceRecordModels.Promotion> Promotions { get; set; }

		public int TotalLoadoutItemsPurchased { get; set; }

		public double TotalCommendationProgress { get; set; }
		
		public string Gamertag { get; set; }

		public string ServiceTag { get; set; }

		public CommonModels.ImageUrl EmblemImageUrl { get; set; }

		public CommonModels.ImageUrl BackgroundImageUrl { get; set; }

		public int FavoriteWeaponId { get; set; }

		public string FavoriteWeaponName { get; set; }

		public string FavoriteWeaponDescription { get; set; }

		public CommonModels.ImageUrl FavoriteWeaponImageUrl { get; set; }

		public int FavoriteWeaponTotalKills { get; set; }

		public int RankId { get; set; }

		public string RankName { get; set; }

		public CommonModels.ImageUrl RankImageUrl { get; set; }

		[JsonProperty("RankStartXP")]
		public int RankStartXp { get; set; }

		[JsonProperty("NextRankStartXP")]
		public Int64 NextRankStartXp { get; set; }

		[JsonProperty("XP")]
		public int Xp { get; set; }

		public CommonModels.CurrentSkillRank TopSkillRank { get; set; }

		public IList<CommonModels.CurrentSkillRank> SkillRanks { get; set; } 

		public int NextRankId { get; set; }

		public string NextRankName { get; set; }

		public CommonModels.ImageUrl NextRankImageUrl { get; set; }

		public IList<CommonModels.TopMedal> TopMedals { get; set; }

		public IList<CommonModels.Specialization> Specializations { get; set; }

		public IList<CommonModels.GameMode> GameModes { get; set; }

	}
}
