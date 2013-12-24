using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class PlaylistModel
	{
		public int Id { get; set; }

		public bool IsCurrent { get; set; }

		public int PopulationCount { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		[JsonProperty("ModeId")]
		public Enums.Mode Mode { get; set; }

		public string ModeName { get; set; }

		public int? MaxPartySize { get; set; }

		public int? MaxLocalPlayers { get; set; }

		public bool IsFreeForAll { get; set; }

		public CommonModels.ImageUrl ImageUrl { get; set; }

		public IList<CommonModels.GameVariant> GameVariants { get; set; }

		public IList<CommonModels.MapVariant> MapVariants { get; set; }
	}
}
