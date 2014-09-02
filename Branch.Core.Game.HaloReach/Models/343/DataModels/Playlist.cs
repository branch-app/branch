using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Playlist
	{
		[JsonProperty("Key")]
		public int Id { get; set; }

		[JsonProperty("Value")]
		public PlaylistDetails PlaylistDetails { get; set; }
	}

	public class PlaylistDetails
	{
		public int Id { get; set; }

		[JsonProperty("ImagePathLrg")]
		public string ImagePathLarge { get; set; }

		[JsonProperty("ImagePathMed")]
		public string ImagePathMedium { get; set; }

		[JsonProperty("ImagePathSm")]
		public string ImagePathSmall { get; set; }

		public string Name { get; set; }

		public int[] VariantClasses { get; set; }
	}
}
