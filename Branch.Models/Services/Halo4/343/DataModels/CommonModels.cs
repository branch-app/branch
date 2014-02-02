using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class CommonModels
	{
		public class ImageUrl
		{
			public string BaseUrl { get; set; }

			public string AssetUrl { get; set; }
		}

		public class TopMedal
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public ImageUrl ImageUrl { get; set; }

			public int TotalMedals { get; set; }
		}

		public class MedalStat
		{
			public int ClassId { get; set; }

			public string Name { get; set; }

			public ImageUrl ImageUrl { get; set; }

			public int Id { get; set; }

			public int TotalMedals { get; set; }
		}

		public class DamageStat
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public ImageUrl ImageUrl { get; set; }

			public int Kills { get; set; }

			public int Deaths { get; set; }

			public int Headshots { get; set; }

			public int Betrayals { get; set; }

			public int Suicides { get; set; }
		}

		public class EnemyStat
		{
			public int EnemyId { get; set; }

			public string Name { get; set; }

			public ImageUrl ImageUrl { get; set; }

			public int Kills { get; set; }

			public int Deaths { get; set; }

			public float AverageKillDistance { get; set; }

			public float AverageDeathDistance { get; set; }
		}

		public class TickEvent
		{
			public int Time { get; set; }

			public int Ticks { get; set; }
		}

		#region Multiplayer Specific

		public class FavoriteVariant
		{
			public ImageUrl ImageUrl { get; set; }

			public string TotalDuration { get; set; }

			public int TotalGamesStarted { get; set; }

			public int TotalGamesCompleted { get; set; }

			public int TotalGamesWon { get; set; }

			public int TotalMedals { get; set; }

			public int TotalKills { get; set; }

			public int TotalDeaths { get; set; }

			[JsonProperty("KDRatio")]
			public double KdRatio { get; set; }

			public double AveragePersonalScore { get; set; }

			public int Id { get; set; }

			public string Name { get; set; }
		}

		public class GameVariant
		{
			public string GameVariantName { get; set; }

			public string GameVariantDescription { get; set; }

			public int GameBaseVariantId { get; set; }

			public string GameBaseVariantName { get; set; }

			public string GameBaseVariantDescription { get; set; }

			public ImageUrl GameBaseVariantImageUrl { get; set; }

		}

		public class GameMode
		{
			public string TotalDuration { get; set; }

			public int TotalKills { get; set; }

			public int TotalDeaths { get; set; }

			public IList<DifficultyLevel> DifficultyLevels { get; set; }

			public IList<Mission> SinglePlayerMissions { get; set; }

			public IList<Mission> CoopMissions { get; set; }

			public int TotalTerminalsVisited { get; set; }

			public long NarrativeFlags { get; set; }

			[JsonProperty("SinglePlayerDASO")]
			public object SinglePlayerDaso { get; set; }

			public object SinglePlayerDifficulty { get; set; }

			[JsonProperty("CoopDASO")]
			public object CoopDaso { get; set; }

			public object CoopDifficulty { get; set; }

			public int PresentationId { get; set; }

			public Enums.Mode Id { get; set; }

			public string Name { get; set; }

			public int TotalGamesStarted { get; set; }

			public int? TotalSinglePlayerMissionsCompleted { get; set; }

			public int? TotalCoopMissionsCompleted { get; set; }

			public int? TotalMissionsPossible { get; set; }

			public int? TotalMedals { get; set; }

			public int? TotalGamesWon { get; set; }

			public int? TotalGamesCompleted { get; set; }

			public int? AveragePersonalScore { get; set; }

			[JsonProperty("KDRatio")]
			public double? KdRatio { get; set; }

			public int? TotalGameBaseVariantMedals { get; set; }

			public FavoriteVariant FavoriteVariant { get; set; }
		}

		public class MapVariant
		{
			[JsonProperty("MapID")]
			public int MapId { get; set; }

			public string MapVariantName { get; set; }
		}

		public class Specialization
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public ImageUrl ImageUrl { get; set; }

			public int Level { get; set; }

			public string LevelName { get; set; }

			public double PercentComplete { get; set; }

			public bool IsCurrent { get; set; }

			public bool Completed { get; set; }
		}

		public class CurrentSkillRank
		{
			[JsonProperty("CurrentSkillRank")]
			public int? CompetitiveSkillRank { get; set; }

			public string PlaylistDescription { get; set; }

			public ImageUrl PlaylistImageUrl { get; set; }

			public string PlaylistName { get; set; }

			public int PlaylistId { get; set; }
		}

		public class GameSkillRank
		{
			[JsonProperty("GameSkillRank")]
			public int? CompetitiveSkillRank { get; set; }

			public string PlaylistDescription { get; set; }

			public ImageUrl PlaylistImageUrl { get; set; }

			public string PlaylistName { get; set; }

			public int PlaylistId { get; set; }
		}

		#endregion

		#region Campaign Specific

		public class DifficultyLevel
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public ImageUrl ImageUrl { get; set; }
		}

		public class Mission
		{
			public int MapId { get; set; }

			[JsonProperty("Mission")]
			public int MissionId { get; set; }

			public int Difficulty { get; set; }
		}

		#endregion
	}
}
