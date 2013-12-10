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

		public class GameVariant
		{
			public string GameVariantName { get; set; }

			public string GameVariantDescription { get; set; }

			public int GameBaseVariantId { get; set; }

			public string GameBaseVariantName { get; set; }

			public string GameBaseVariantDescription { get; set; }

			public ImageUrl GameBaseVariantImageUrl { get; set; }

		}

		public class MapVariant
		{
			[JsonProperty("MapID")]
			public int MapId { get; set; }

			public string MapVariantName { get; set; }
		}

	}
}
