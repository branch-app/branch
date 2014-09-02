using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Rank
	{
		[JsonProperty("Key")]
		public int Id { get; set; }

		[JsonProperty("Value")]
		public string Name { get; set; }
	}
}
