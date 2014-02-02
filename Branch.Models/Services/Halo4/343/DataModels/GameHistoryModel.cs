using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class GameHistoryModel
	{
		public abstract class Base
		{
			public int Standing { get; set; }

			public List<int> TopMedalIds { get; set; }

			public TimeSpan Duration { get; set; }

			public int MapId { get; set; }

			public CommonModels.ImageUrl MapImageUrl { get; set; }

			public int PersonalScore { get; set; }

			public string Id { get; set; }

			[JsonProperty("ModeId")]
			public Enums.Mode Mode { get; set; }

			public string ModeName { get; set; }

			public bool Completed { get; set; }

			public Enums.Result Result { get; set; }

			public DateTime EndDateUtc { get; set; }
		}

		public class WarGames : Base
		{
			public int BaseVariantId { get; set; }

			public CommonModels.ImageUrl BaseVariantImageUrl { get; set; }

			public string VariantName { get; set; }

			public string FeaturedStatName { get; set; }

			public int FeaturedStatValue { get; set; }

			public int TotalMedals { get; set; }

			public string MapVariantName { get; set; }

			public int PlaylistId { get; set; }

			public string PlaylistName { get; set; }
		}

		public class Campaign : Base
		{
			public int Difficulty { get; set; }

			public CommonModels.ImageUrl DifficultyImageUrl { get; set; }

			public bool SinglePlayer { get; set; }

			public int[] SkullIds { get; set; }

			public int Mission { get; set; }

			public string MapName { get; set; }
		}

		public class SpartanOps : Base
		{
			public int Difficulty { get; set; }

			public CommonModels.ImageUrl DifficultyImageUrl { get; set; }

			public bool SinglePlayer { get; set; }

			public string EpisodeName { get; set; }

			public string ChapterName { get; set; }

			public int SeasonId { get; set; }

			public int EpisodeId { get; set; }

			public int ChapterId { get; set; }

			public int ChapterNumber { get; set; }
		}
	}
}