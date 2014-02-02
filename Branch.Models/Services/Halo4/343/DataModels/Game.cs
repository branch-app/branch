using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class Game
	{
		[JsonProperty("$type")]
		public string Type { get; set; }

		public TimeSpan Duration { get; set; }

		#region Campaign

		public int? Mission { get; set; }

		public int[] SkullIds { get; set; }

		public bool? CampaignScoringEnabled { get; set; }

		public int? CampaignGlobalScore { get; set; }
		#endregion

		#region Campaign/Spartan Ops

		public int? Difficulty { get; set; }

		#endregion
		
		#region Spartan Ops

		public int? ChapterId { get; set; }

		#endregion

		public IList<Player> Players { get; set; } 

		public IList<Team> Teams { get; set; }

		public int TotalPlayers { get; set; }

		public int MapId { get; set; }

		public string MapName { get; set; }

		public CommonModels.ImageUrl MapImageUrl { get; set; }

		public string MapVariantName { get; set; }

		public int PlaylistId { get; set; }

		public string PlaylistName { get; set; }

		public int GameBaseVariantId { get; set; }

		public string GameBaseVariantName { get; set; }

		public string GameVariantName { get; set; }

		public string Id { get; set; }

		public Enums.Mode ModeId { get; set; }

		public string ModeName { get; set; }

		public bool Completed { get; set; }

		public int Result { get; set; }

		public DateTime EndDateUtc { get; set; }

		public class Team
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public CommonModels.ImageUrl EmblemImageUrl { get; set; }

			[JsonProperty("PrimaryRGB")]
			public string PrimaryRgb { get; set; }

			[JsonProperty("PrimaryRGBA")]
			public int PrimaryRgba { get; set; }

			[JsonProperty("SecondaryRGB")]
			public string SecondaryRgb { get; set; }

			[JsonProperty("SecondaryRGBA")]
			public int SecondaryRgba { get; set; }

			public int Standing { get; set; }

			public int Score { get; set; }

			public int Kills { get; set; }

			public int Deaths { get; set; }

			public int Assists { get; set; }

			public int Betrayals { get; set; }

			public int Suicides { get; set; }

			public IList<CommonModels.TickEvent> DeathsOverTime { get; set; }

			public IList<CommonModels.TickEvent> KillsOverTime { get; set; }

			public IList<CommonModels.TickEvent> MedalsOverTime { get; set; }

			public int TotalMedals { get; set; }

			public IList<CommonModels.MedalStat> MedalStats { get; set; }
		}

		public class Player
		{
			[JsonProperty("$type")]
			public string Type { get; set; }

			public IList<CommonModels.TickEvent> DeathsOverTime { get; set; }

			public IList<CommonModels.TickEvent> KillsOverTime { get; set; }

			public IList<CommonModels.TickEvent> MedalsOverTime { get; set; }

			public IList<CommonModels.MedalStat> MedalStats { get; set; }

			public IList<CommonModels.DamageStat> DamageTypeStats { get; set; }

			public IList<CommonModels.EnemyStat> EnemyStats { get; set; }

			public CommonModels.GameSkillRank SkillRank { get; set; }

			public int TeamId { get; set; }

			public bool IsCompleted { get; set; }

			public string ServiceTag { get; set; }

			public bool IsGuest { get; set; }

			public bool JoinedInProgress { get; set; }

			public int Standing { get; set; }

			public int Result { get; set; }

			public int PersonalScore { get; set; }

			public string FeaturedStatName { get; set; }

			public int FeaturedStatValue { get; set; }

			public int StandingInTeam { get; set; }

			public int Kills { get; set; }

			public int Deaths { get; set; }

			public int Assists { get; set; }

			public int Headshots { get; set; }

			public int Betrayals { get; set; }

			public int Suicides { get; set; }

			public string KilledMostGamertag { get; set; }

			public int KilledMostCount { get; set; }

			public string KilledByMostGamertag { get; set; }

			public int KilledByMostCount { get; set; }

			public int RankId { get; set; }

			public string RankName { get; set; }

			public CommonModels.ImageUrl RankUrl { get; set; }

			public CommonModels.ImageUrl EmblemImageUrl { get; set; }

			public string FirstPlayedUtc { get; set; }

			public string LastPlayedUtc { get; set; }

			public float AverageDeathDistance { get; set; }

			public float AverageKillDistance { get; set; }

			public int TotalMedals { get; set; }

			public int TotalKillMedals { get; set; }

			public int TotalBonusMedals { get; set; }

			public int TotalAssistMedals { get; set; }

			public int TotalSpreeMedals { get; set; }

			public int TotalModeMedals { get; set; }

			public int[] TopMedalIds { get; set; }

			public string Gamertag { get; set; }

		}
	}
}
