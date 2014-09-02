using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.JsonConverters;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Map
	{
		[JsonProperty("Key")]
		public int Id { get; set; }

		[JsonProperty("Value")]
		public MapDetails MapDetails { get; set; }
	}

	public class MapDetails
	{
		public int Id { get; set; }

		[JsonConverter(typeof(MapTypeConverter))]
		public MapType MapType { get; set; }

		public string ImageName { get; set; }

		public string Name { get; set; }
	}
}
