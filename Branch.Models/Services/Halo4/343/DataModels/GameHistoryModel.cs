using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class GameHistoryModel
	{
		public int Standing { get; set; }

		public int BaseVariantId { get; set; }

		public CommonModels.ImageUrl BaseVariantImageUrl { get; set; }

		public string VariantName { get; set; }

		public string FeaturedStatName { get; set; }

		public int FeaturedStatValue { get; set; }

		public List<object> PromotionIds { get; set; }

		public int TotalMedals { get; set; }

		public List<int> TopMedalIds { get; set; }

		public int MapId { get; set; }

		public CommonModels.ImageUrl MapImageUrl { get; set; }

		public string MapVariantName { get; set; }

		public int PlayListId { get; set; }

		public string PlayListName { get; set; }

		public int PersonalScore { get; set; }

		public string Id { get; set; }

		[JsonProperty("ModeId")]
		public Enums.Mode Mode { get; set; }

		public string ModeName { get; set; }

		public bool Completed { get; set; }

		public Enums.Result Result { get; set; }

		public DateTime EndDateUtc { get; set; }
	}
}