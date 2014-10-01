using Branch.Core.Game.HaloReach.Enums;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{


	public class PlaylistStatistics
	{
		public KeyValue<int>[] DeathsByDamageType { get; set; }

		public string HeatmapPlayerDeathsUrl { get; set; }

		public string HeatmapPlayerKillsUrl { get; set; }

		public string HeatmapUrl { get; set; }

		public int? HopperId { get; set; }

		public KeyValue<int>[] KillsByDamageType { get; set; }

		public int? MapId { get; set; }

		public double MedalChestCompletionPercentage { get; set; }

		public KeyValue<int>[] MedalCountsByType { get; set; }

		public int TotalMedals { get; set; }

		public VariantClass VariantClass { get; set; }

		[JsonProperty("game_count")]
		public int GameCount { get; set; }

		[JsonProperty("high_score")]
		public int HighScore { get; set; }

		[JsonProperty("season_id")]
		public int SeasonId { get; set; }

		[JsonProperty("total_assists")]
		public int TotalAssists { get; set; }

		[JsonProperty("total_betrayals")]
		public int TotalBetrayals { get; set; }

		[JsonProperty("total_deaths")]
		public int TotalDeaths { get; set; }

		[JsonProperty("total_first_place")]
		public int TotalFirstPlace { get; set; }

		[JsonProperty("total_kills")]
		public int TotalKills { get; set; }

		[JsonProperty("total_playtime")]
		//[JsonConverter(typeof(PlayTimeConverter))] TODO: Uncomment
		public string TotalPlaytime { get; set; }

		[JsonProperty("total_score")]
		public int TotalScore { get; set; }

		[JsonProperty("total_top_half_place")]
		public int TotalTopHalfPlace { get; set; }

		[JsonProperty("total_top_third_place")]
		public int TotalThirdPlace { get; set; }

		[JsonProperty("total_wins")]
		public int TotalWins { get; set; }
	}

	public class AiPlaylistStatistics
		: PlaylistStatistics
	{

		public KeyValue<int>[] DeathsByEnemyTypeClass { get; set; }

		public KeyValue<int>[] KillsByEnemyTypeClass { get; set; }

		public KeyValue<int>[] PointsByDamageType { get; set; }

		public KeyValue<int>[] PointsByEnemyTypeClass { get; set; }

		[JsonProperty("biggest_kill_points")]
		public int BiggestKillPoints { get; set; }

		[JsonProperty("biggest_kill_streak")]
		public int BiggestKillStreak { get; set; }

		[JsonProperty("game_difficulty")]
		public Difficulty GameDifficulty { get; set; }

		[JsonProperty("high_score_coop")]
		public int HighScoreCoop { get; set; }

		[JsonProperty("high_score_solo")]
		public int HighScoreSolo { get; set; }

		[JsonProperty("highest_game_kills")]
		public int HighestGameKills { get; set; }

		[JsonProperty("highest_set")]
		public int HighestSet { get; set; }

		[JsonProperty("highest_skull_multiplier")]
		public int HighestSkullMultiplier { get; set; }

		[JsonProperty("total_enemy_players_killed")]
		public int TotalEnemyPlayersKilled { get; set; }

		[JsonProperty("total_generators_destroyed")]
		public int TotalGeneratorsDestroyed { get; set; }

		[JsonProperty("total_missions_beating_par")]
		public int TotalMissionsBeatOnPar { get; set; }

		[JsonProperty("total_missions_not_dying")]
		public int TotalMissionsCompletedWithoutDying { get; set; }

		[JsonProperty("total_score_coop")]
		public int TotalScoreCoop { get; set; }

		[JsonProperty("total_score_solo")]
		public int TotalScoreSolo { get; set; }

		[JsonProperty("total_waves_completed")]
		public int TotalWavesCompleted { get; set; }
	}
}
