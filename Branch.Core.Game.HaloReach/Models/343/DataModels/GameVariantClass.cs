using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class GameVariantClass
	{
		[JsonProperty("Value")]
		public int Id { get; set; }

		[JsonProperty("Key")]
		public string Name { get; set; }
	}
}
