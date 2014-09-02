using System;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Player
	{
		public string CampaignProgressCoop { get; set; }

		public string CampaignProgressSp { get; set; }

		public bool Initialized { get; set; }

		public bool IsGuest { get; set; }

		public string LastGameVariantClassPlayed { get; set; }

		public Emblem ReachEmblem { get; set; }

		[JsonProperty("armor_completion_percentage")]
		public double ArmorCompletionPercentage { get; set; }

		public int Bpr { get; set; }

		public int CampaignDeaths { get; set; }

		[JsonProperty("campaign_games")]
		public int CampaignGames { get; set; }

		[JsonProperty("campaign_kills")]
		public int CampaignKills { get; set; }

		[JsonProperty("campaign_level_progress_coop")]
		public int CampaignLevelProgressCoop { get; set; }

		[JsonProperty("campaign_level_progress_sp")]
		public int CampaignLevelProgressSp { get; set; }

		[JsonProperty("current_rank_image")]
		public string CurrentRankImage { get; set; }

		[JsonProperty("current_rank_name")]
		public string CurrentRankName { get; set; }

		[JsonProperty("current_rank_progression_points")]
		public int CurrentRankProgressionPoints { get; set; }

		[JsonProperty("daily_challenges_completed")]
		public int DailyChallangesCompleted { get; set; }

		[JsonProperty("firefight_highscore")]
		public int FirefightHighscore { get; set; }

		[JsonProperty("firefight_highscore_easy")]
		public int FirefightHighscoreEasy { get; set; }

		[JsonProperty("firefight_highscore_normal")]
		public int FirefightHighscoreNormal { get; set; }

		[JsonProperty("firefight_highscore_heroic")]
		public int FirefightHighscoreHeroic { get; set; }

		[JsonProperty("firefight_highscore_legendary")]
		public int FirefightHighscoreLegendary { get; set; }

		[JsonProperty("first_active")]
		public DateTime FirstActive { get; set; }

		[JsonProperty("gamertag")]
		public string Gamertag { get; set; }

		[JsonProperty("games_total")]
		public int GamesTotal { get; set; }

		[JsonProperty("last_active")]
		public DateTime LastActive { get; set; }

		[JsonProperty("multiplayer_deaths")]
		public int MultiplayerDeaths { get; set; }

		[JsonProperty("multiplayer_games")]
		public int MultiplayerGames { get; set; }

		[JsonProperty("multiplayer_kills")]
		public int MultiplayerKills { get; set; }

		[JsonProperty("multiplayer_time")]
		public TimeSpan MultiplayerTime { get; set; }

		[JsonProperty("multiplayer_wins")]
		public int MultiplayerWins { get; set; }

		[JsonProperty("next_rank_name")]
		public string NextRankName { get; set; }

		[JsonProperty("next_rank_points")]
		public int NextRankPoints { get; set; }

		[JsonProperty("service_tag")]
		public string ServiceTag { get; set; }

		[JsonProperty("weekly_challenges_completed")]
		public int WeeklyChallengesCompleted { get; set; }

		public CommendationState[] CommendationState { get; set; }

		[JsonProperty("commendation_completion_percentage")]
		public double CommendationCompletionPercentage { get; set; }
	}
}
